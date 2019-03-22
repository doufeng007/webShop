using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.WorkFlow;
using Abp.WorkFlowDictionary;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;
using Abp.Application.Services.Dto;
using Docment.Service;
using Abp.Linq.Extensions;
using Abp.File;

namespace Docment
{
    /// <summary>
    /// 档案销毁
    /// </summary>
    [AbpAuthorize]
    public class DocmentDestroyAppService : FRMSCoreAppServiceBase, IDocmentDestroyAppeService
    {
        private readonly IRepository<DocmentList, Guid> _docmentRepository;
        private readonly IRepository<DocmentDestroy, Guid> _docmentDestroyRepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IRepository<ProjectAudit, Guid> _projectAuditRepository;
        public DocmentDestroyAppService(IRepository<DocmentList, Guid> docmentRepository,
            WorkFlowBusinessTaskManager workFlowBusinessTaskManager,
            IRepository<AbpDictionary, Guid> abpDictionaryRepository, IAbpFileRelationAppService abpFileRelationAppService, IWorkFlowTaskRepository workFlowTaskRepository,
            IRepository<DocmentDestroy, Guid> docmentDestroyRepository, WorkFlowCacheManager workFlowCacheManager,
            IRepository<ProjectAudit, Guid> projectAuditRepository,
            ProjectAuditManager projectAuditManager,
            IRepository<User, long> userRepository)
        {
            _docmentRepository = docmentRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _abpDictionaryRepository = abpDictionaryRepository;
            _projectAuditManager = projectAuditManager;
            _projectAuditRepository = projectAuditRepository;
            _userRepository = userRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _docmentDestroyRepository = docmentDestroyRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 创建档案销毁申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateDocmentDestroyInput input)
        {
            var model = input.MapTo<DocmentDestroy>();
            var doc = _docmentRepository.Get(model.DocmentId);
            doc.Status = (int)DocmentStatus.销毁中;
            _docmentRepository.Update(doc);
            _docmentDestroyRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        /// <summary>
        /// 获取档案销毁申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<DocmentDestroyDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var query = from a in _docmentDestroyRepository.GetAll()
                        join b in _docmentRepository.GetAll() on a.DocmentId equals b.Id
                        join c in _abpDictionaryRepository.GetAll() on b.Type equals c.Id
                        where a.Id == id
                        select new DocmentDestroyDto()
                        {
                            CreationTime = a.CreationTime,
                            Attr = b.Attr,
                            Attr_Name = b.Attr.ToString(),
                            Des = a.Des,
                            DocmentId = a.DocmentId,
                            Id = a.Id,
                            Location = b.Location,
                            DocmentId_Name = b.Name,
                            No = b.No,
                            Type = b.Type,
                            UserId = b.UserId,
                            Type_Name = c.Title,
                             CreateUserId=a.CreatorUserId
                        };
            var model = await query.FirstOrDefaultAsync();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到数据");
            }
            if (model.UserId.HasValue && model.UserId.Value != 0)
            {
                var u = _userRepository.Get(model.UserId.Value);
                model.UserId_Name = u.Name;
                var u2 = _userRepository.Get(model.CreateUserId.Value);
                model.CreateUserId_Name = u2.Name;
            }
            model.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = model.DocmentId.ToString(), BusinessType = (int)AbpFileBusinessType.档案附件 });
            model.MoveFileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.销毁证明文件 });
            return model;
        }

        public async Task<PagedResultDto<DocmentDestroyListDto>> GetAll(DocmentDestroySearchDto input)
        {
            var query = from a in _docmentDestroyRepository.GetAll()
                        join c in _docmentRepository.GetAll() on a.DocmentId equals c.Id
                        join b in _abpDictionaryRepository.GetAll() on c.Type equals b.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new DocmentDestroyListDto()
                        {
                            CreationTime = a.CreationTime,
                            Attr = c.Attr,
                            Attr_Name = c.Attr.ToString(),
                            Id = a.Id,
                            Location = c.Location,
                            Name = c.Name,
                            No = c.No,
                            Type = c.Type,
                            Type_Name = b.Title,
                            UserId = a.CreatorUserId,
                            Status = a.Status,
                            DocmentId = a.DocmentId,
                            Des = a.Des,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey) || ite.No.Contains(input.SearchKey));
            }
            if (input.Status.HasValue)
            {
                query = query.Where(ite => ite.Status == (int)input.Status);
            }
            var count = query.Count();
            var ret = (await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync()).MapTo<List<DocmentDestroyListDto>>();

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
            return new PagedResultDto<DocmentDestroyListDto>(count, ret);
        }

        /// <summary>
        /// 更新档案销毁申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Update(UpdateeDocmentDestroyInput input)
        {
            var model = _docmentDestroyRepository.Get(input.Id);
            var old_Model = model.DeepClone();
            if (model.DocmentId != input.DocmentId)//如何修改了档案id则默认更新新老档案的状态
            {
                var old = _docmentRepository.Get(model.DocmentId);
                var newd = _docmentRepository.Get(input.DocmentId);
                old.Status = (int)DocmentStatus.在档;
                newd.Status = (int)DocmentStatus.审批中;
                var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
                var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
                noticeInput.ProjectId = input.Id;
                noticeInput.Content = $"你申请销毁《{old.Name}》已变更为《{newd.Name}》。";
                noticeInput.Title = $"销毁《{old.Name}》变更";
                noticeInput.NoticeUserIds = model.CreatorUserId.Value.ToString();
                noticeInput.NoticeType = 1;
                await noticeService.CreateOrUpdateNoticeAsync(noticeInput);
                await _docmentRepository.UpdateAsync(old);
                await _docmentRepository.UpdateAsync(newd);
            }
            model = input.MapTo(model);
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
                    BusinessType = (int)AbpFileBusinessType.销毁证明文件,
                    Files = fileList
                });
            }
            await _docmentDestroyRepository.UpdateAsync(model);
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(model));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }
        private UpdateeDocmentDestroyInput GetChangeModel(DocmentDestroy model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<UpdateeDocmentDestroyInput>();
            return ret;
        }

        public async Task StopDestroy(Guid id, Guid flowId, string reason)
        {
            var record = _docmentDestroyRepository.Get(id);
            if (flowId == Guid.Empty)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "工作流id不能为空。");
            }
            record.Status = (int)WorkFlowBusinessStatus.作废;
            var task = _workFlowTaskRepository.GetAll().Where(ite => ite.FlowID == flowId && ite.InstanceID == id.ToString()).ToList();
            foreach (var t in task)
            {
                t.Status = 10;//流程管理员作废申请
                t.Comment = reason;
                await _workFlowTaskRepository.UpdateAsync(t);
            }

            var doc = _docmentRepository.Get(record.DocmentId);
            doc.Status = (int)DocmentStatus.在档;
            await _docmentDestroyRepository.UpdateAsync(record);
            await _docmentRepository.UpdateAsync(doc);

        }

        
    }
}
