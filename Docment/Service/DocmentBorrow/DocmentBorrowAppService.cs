using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.File;
using Abp.WorkFlow;
using Abp.WorkFlowDictionary;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment
{
    /// <summary>
    /// 档案借阅管理
    /// </summary>
    [AbpAuthorize]
    public class DocmentBorrowAppService : FRMSCoreAppServiceBase, IDocmentBorrowAppService
    {
        private readonly IRepository<DocmentList, Guid> _docmentRepository;
        private readonly IRepository<DocmentBorrow, Guid> _docmentBorrowRepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IRepository<ProjectAudit, Guid> _projectAuditRepository;
        private readonly IRepository<DocmentBorrowSub, Guid> _docmentBorrowSubRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitsRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _orgRepository;
        public DocmentBorrowAppService(IRepository<DocmentList, Guid> docmentRepository,
            WorkFlowBusinessTaskManager workFlowBusinessTaskManager, WorkFlowCacheManager workFlowCacheManager, ProjectAuditManager projectAuditManager, IRepository<ProjectAudit, Guid> projectAuditRepository,
            IRepository<AbpDictionary, Guid> abpDictionaryRepository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<DocmentBorrowSub, Guid> docmentBorrowSubRepository,
            IRepository<DocmentBorrow, Guid> docmentBorrowRepository, IWorkFlowTaskRepository workFlowTaskRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitsRepository,
            IRepository<User, long> userRepository, IRepository<WorkFlowOrganizationUnits, long> orgRepository)
        {
            _workFlowTaskRepository = workFlowTaskRepository;
            _docmentRepository = docmentRepository;
            _projectAuditManager = projectAuditManager;
            _projectAuditRepository = projectAuditRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _abpDictionaryRepository = abpDictionaryRepository;
            _userRepository = userRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _docmentBorrowRepository = docmentBorrowRepository;
            _docmentBorrowSubRepository = docmentBorrowSubRepository;
            _userOrganizationUnitsRepository = userOrganizationUnitsRepository;
            _orgRepository = orgRepository;
        }
        /// <summary>
        /// 创建档案借阅申请(批量申请)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateDocmentBorrowInput input)
        {

            if (input.DocmentIds == null || input.DocmentIds.Count == 0) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请至少选择一个档案。");
            }
            var isleader = false;
            var cuser = _userRepository.Get(AbpSession.UserId.Value);
            var roles = UserManager.GetRolesAsync(cuser).Result;
            if (roles.Any(r => r == "FGLD"))
            {
                isleader = true;
            }

            var model = input.MapTo<DocmentBorrow>();
            model.Count = input.DocmentIds.Count;
            var bid=  _docmentBorrowRepository.InsertAndGetId(model);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var ite in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.外部借阅证明,
                    Files = fileList
                });
            }
            foreach (var docid in input.DocmentIds)
            {
                _docmentBorrowSubRepository.Insert(new DocmentBorrowSub
                {
                    BorrowId =bid,
                     DocmentId=docid,
                      Status=isleader?BorrowSubStatus.同意: BorrowSubStatus.审核中,
                });
                var doc = _docmentRepository.Get(docid);
                if (doc.Status != (int)DocmentStatus.在档)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案状态不是【在档】，借阅失败。");
                }
                if (doc.Attr == DocmentAttr.纸质)
                {
                    doc.Status = (int)DocmentStatus.审批中;//状态改为锁定中，流程完成后改为借阅中，等归还后 改回-1
                }
                _docmentRepository.Update(doc);
            }
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        /// <summary>
        /// 获取档案借阅详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<DocmentBorrowDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var query = from a in _docmentBorrowRepository.GetAll()
                        join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                        join c in _userOrganizationUnitsRepository.GetAll() on new { UserId = a.CreatorUserId.GetValueOrDefault(), IsMain = true } equals new { UserId = c.UserId, IsMain = c.IsMain }
                        join d in _orgRepository.GetAll() on c.OrganizationUnitId equals d.Id
                        where a.Id == id
                        select new DocmentBorrowDto()
                        {
                            CreationTime = a.CreationTime,
                            CreateUserId = a.CreatorUserId,
                            Des = a.Des,
                            OrgId = c.OrganizationUnitId,
                            OrgName = d.DisplayName,
                            Id = a.Id,
                            CreateUserId_Name = b.Name,
                            BackTime = a.BackTime,
                            Status=a.Status
                        };
            var model = await query.FirstOrDefaultAsync();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到数据");
            }
            var sub = (from a in _docmentBorrowSubRepository.GetAll()
                       join b in _docmentRepository.GetAll() on a.DocmentId equals b.Id
                       join c in _abpDictionaryRepository.GetAll() on b.Type equals c.Id
                       where a.BorrowId == id
                       select new DocmentBorrowSubDto()
                       {
                           Attr = b.Attr,
                           Attr_Name = b.Attr.ToString(),
                           BackTime = a.BackTime,
                           BorrowId = a.BorrowId,
                           CreationTime=a.CreationTime,
                           DocmentId = a.DocmentId,
                           GetTime = a.GetTime,
                           Location = b.Location,
                           Name = b.Name,
                           No = b.No,
                           Status = a.Status,
                           StatusTitle = a.Status.ToString(),
                           Id = a.Id,
                           QrCodeId=b.QrCodeId,
                           Type = b.Type,
                           Type_Name = c.Title
                       }).ToList();
            model.Docments = sub;
            model.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.外部借阅证明 });
            return model;
        }
        /// <summary>
        /// 获取档案借阅记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DocmentBorrowListDto>> GetAll(DocmentBorrowSearchInput input)
        {
            var query = from a in _docmentBorrowRepository.GetAll()
                        join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                        join c in _userOrganizationUnitsRepository.GetAll() on new { UserId = a.CreatorUserId.GetValueOrDefault(), IsMain = true } equals new { UserId = c.UserId, IsMain = c.IsMain }
                        join d in _orgRepository.GetAll() on c.OrganizationUnitId equals d.Id
                        where a.CreatorUserId == AbpSession.UserId.Value || a.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "")//只查看我申请的和参与的
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new DocmentBorrowListDto()
                        {
                            CreationTime = a.CreationTime,
                            Id = a.Id,
                            UserId = a.CreatorUserId,
                            UserId_Name = b.Name,
                            OrgId = c.OrganizationUnitId,
                            OrgName = d.DisplayName,
                            Status = a.Status,
                            Count = a.Count.Value,
                            BackTime = a.BackTime,
                            Des = a.Des,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0 || y.Status == -1)) > 0
                                            ? 1
                                            : 2
                        };
            if (input.OrgId.HasValue) {
                query = query.Where(ite => ite.OrgId == input.OrgId);
            }
            if (input.UserId.HasValue) {
                query = query.Where(ite => ite.UserId == input.UserId.Value);
            }
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false) {
                query = query.Where(ite => ite.OrgName.Contains(input.SearchKey) || ite.UserId_Name.Contains(input.SearchKey));
            }
            var count = query.Count();
            var ret = (await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync()).MapTo<List<DocmentBorrowListDto>>();

            foreach (var r in ret)
            {
                r.InstanceId = r.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, r as BusinessWorkFlowListOutput);
                if (r.UserId.HasValue)
                {
                    var u = _userRepository.Get(r.UserId.Value);
                    r.UserId_Name = u.Name;
                }
            }
            return new PagedResultDto<DocmentBorrowListDto>(count, ret);
        }
        /// <summary>
        /// 获取借阅验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetVerify(Guid id)
        {
            var record = _docmentBorrowRepository.GetAll().First(ite=>ite.Id==id&&ite.CreatorUserId==AbpSession.UserId.Value);
            if (record.Status != 2) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "借阅状态不对，当前尚未生成验证码。");
            }
            return record.Verify;
        }
        /// <summary>
        /// 档案管理员验证验证码(接口作废，不需要验证码)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        [Obsolete]
        public bool Verify(Guid id, string verify) {
            var record = _docmentBorrowRepository.Get(id);
            if (record.Verify == verify) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新档案借阅记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Update(UpdateeDocmentBorrowInput input)
        {
            var model = _docmentBorrowRepository.Get(input.Id);
            
            var old_Model = model.DeepClone();
            //if (model.DocmentId != input.DocmentId)//如何修改了档案id则默认更新新老档案的状态
            //{
            //    var old = _docmentRepository.Get(model.DocmentId);
            //    var newd = _docmentRepository.Get(input.DocmentId);
            //    old.Status = (int)DocmentStatus.在档;
            //    if (newd.Attr == DocmentAttr.纸质) {
            //        newd.Status= (int)DocmentStatus.使用中;
            //    }
            //    var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
            //    var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
            //    noticeInput.ProjectId = input.Id;
            //    noticeInput.Content = $"你申请借阅《{old.Name}》已变更为《{newd.Name}》。";
            //    noticeInput.Title = $"借阅《{old.Name}》变更";
            //    noticeInput.NoticeUserIds = model.CreatorUserId.Value.ToString();
            //    noticeInput.NoticeType = 1;
            //    await noticeService.CreateOrUpdateNotice(noticeInput);
            //    await _docmentRepository.UpdateAsync(old);
            //    await _docmentRepository.UpdateAsync(newd);
            //}
            model = input.MapTo(model);
            await _docmentBorrowRepository.UpdateAsync(model);
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(model));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }

        private UpdateeDocmentBorrowInput GetChangeModel(DocmentBorrow model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<UpdateeDocmentBorrowInput>();
            var doc = _docmentRepository.Get(ret.DocmentId);
            ret.DocmentName = doc.Name+"【"+doc.Attr.ToString()+"】";
            return ret;
        }
        /// <summary>
        /// 档案管理员作废借阅申请
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        
        public async Task StopBorrow(Guid id, Guid flowId, string reason)
        {
            var record = _docmentBorrowRepository.Get(id);
            if (flowId == Guid.Empty) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr,"工作流id不能为空。");
            }
            record.Status = (int)WorkFlowBusinessStatus.作废;
            var task = _workFlowTaskRepository.GetAll().Where(ite=>ite.FlowID== flowId && ite.InstanceID==id.ToString()&&ite.Status<2).ToList();
            foreach (var t in task) {
                t.Status = 10;//流程管理员作废申请
                t.Comment = reason;
                await _workFlowTaskRepository.UpdateAsync(t);
            }
            var sub = _docmentBorrowSubRepository.GetAll().Where(ite => ite.BorrowId == id);
            if (sub != null) {
                foreach (var s in sub) {
                    var doc = _docmentRepository.Get(s.DocmentId);
                    doc.Status = (int)DocmentStatus.在档;
                    await _docmentRepository.UpdateAsync(doc);
                }
            }
            await _docmentBorrowRepository.UpdateAsync(record);
        }

        /// <summary>
        /// 领导同意或驳回借阅
        /// </summary>
        /// <param name="docmengId"></param>
        /// <param name="borrowId"></param>
        /// <param name="agree"></param>
        /// <returns></returns>
        public async Task<bool> Agree(AgreeInput input) {
            var borr = _docmentBorrowRepository.Get(input.BorrowId);
            
            var subs = _docmentBorrowSubRepository.GetAll().Where(ite => ite.BorrowId == input.BorrowId);
            foreach (var s in subs) {
                if (s.Status != BorrowSubStatus.审核中)
                    continue;
                if (input.DocmentIds.Contains(s.DocmentId))
                {
                    if (borr.IsOut.HasValue&&borr.IsOut.Value) {
                        s.Status = BorrowSubStatus.待领取;
                    } else {
                        s.Status = BorrowSubStatus.同意;
                    }
                    
                }
                else {
                    s.Status = BorrowSubStatus.驳回;
                }
            }
            return true;
        }
        /// <summary>
        /// 档案管理员借出
        /// </summary>
        /// <param name="docmengId"></param>
        /// <param name="borrowId"></param>
        /// <returns></returns>
        public async Task<bool> SureBorrow(AgreeInput input) {
            var subs = _docmentBorrowSubRepository.GetAll().Where(ite => ite.BorrowId == input.BorrowId).ToList();
            foreach (var s in subs)
            {
                if (s.Status== BorrowSubStatus.同意||s.Status== BorrowSubStatus.审核中)
                {
                    s.Status = BorrowSubStatus.待领取;
                }
            }
            return true;
        }
        /// <summary>
        /// 扫码确认领取
        /// </summary>
        /// <param name="docmengId"></param>
        /// <returns></returns>
        public async Task<bool> SureGet(BorrowBackInput input)
        {
            var subs = _docmentBorrowSubRepository.GetAll().Where(ite => ite.BorrowId==input.BorrowId).ToList();
            if (subs == null) {
                throw new UserFriendlyException("未找到借阅记录。");
            }
            var sub = subs.FirstOrDefault(ite => ite.DocmentId == input.DocmentId);
            sub.Status = BorrowSubStatus.使用中;
            sub.GetTime = DateTime.Now;
            var doc = _docmentRepository.Get(sub.DocmentId);
            if (doc.Attr == DocmentAttr.纸质)
            {
                doc.Status = (int)DocmentStatus.使用中;
            }
            if (subs.Count(ite => ite.Status == BorrowSubStatus.待领取) == 0) {
                //如果没有待领取档案，并自动发送待办到下一步
                return true;
            }
            return false;
        }
        /// <summary>
        /// 顶部扫码领取借阅的档案
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <returns></returns>
        public async Task<DocmentDto> ScanBorrowGet(Guid qrCodeId) {
            var doc = _docmentRepository.GetAll().FirstOrDefault(ite => ite.QrCodeId == qrCodeId);
            if (doc == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案不存在。");
            }
            var sub = _docmentBorrowSubRepository.GetAll().FirstOrDefault(ite => ite.DocmentId == doc.Id&&ite.Status== BorrowSubStatus.待领取);
            if (sub != null)
            {
                sub.Status = BorrowSubStatus.使用中;
                doc.Status = (int)DocmentStatus.使用中;
            }
            else {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案未申请借阅，无法领取。");
            }
            return doc.MapTo<DocmentDto>();
        }
        /// <summary>
        /// 借阅档案扫码确认归还
        /// </summary>
        /// <param name="docmengId"></param>
        /// <returns></returns>
        public async Task<bool> SureBack(Guid qrCodeId)
        {
            var doc = _docmentRepository.GetAll().FirstOrDefault(ite => ite.QrCodeId == qrCodeId);
            if (doc == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案不存在。");
            }
            
            if (doc.Status == (int)DocmentStatus.使用中)
            {
                doc.Status = (int)DocmentStatus.在档;
                var sub = _docmentBorrowSubRepository.GetAll().FirstOrDefault(ite => ite.DocmentId == doc.Id && ite.Status == BorrowSubStatus.使用中);
                if (sub != null)
                {
                    sub.Status = BorrowSubStatus.已归还;
                    sub.BackTime = DateTime.Now;
                }
            }
            else if (doc.Status == (int)DocmentStatus.审批中)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案正在审批中。");
            }
            else {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案状态未借出，无法归还。");
            }
            
            return true;
        }
        /// <summary>
        /// 归档中档案确认入档
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <returns></returns>
        public async Task<bool> SureIn(Guid qrCodeId)
        {
            var doc = _docmentRepository.GetAll().FirstOrDefault(ite => ite.QrCodeId == qrCodeId);
            if (doc == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案不存在。");
            }
            if (doc.Status == (int)DocmentStatus.归档中)
            {
                doc.Status = (int)DocmentStatus.在档;
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前档案状态无法入档。");
            }

            return true;
        }

    }
}
