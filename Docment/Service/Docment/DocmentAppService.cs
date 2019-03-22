using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using Abp.WorkFlowDictionary;
using ZCYX.FRMSCore;
using System.Linq;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Abp.File;
using Abp.Linq.Extensions;
using Abp.Authorization;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Linq;
using Abp.Events.Bus;

namespace Docment.Service
{
    [AbpAuthorize]
    public class DocmentAppService : FRMSCoreAppServiceBase, IDocmentAppService
    {
        private readonly IRepository<DocmentList, Guid> _docmentRepository;
        private readonly IRepository<DocmentFlowing, Guid> _docmentFlowingRepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly QrCodeManager _qrCodeManager;
        private readonly IRepository<QrCode, Guid> _qrcoderepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _orgRepository;
        private readonly IRepository<AbpFile, Guid> _abpFilerepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitsRepository;
        private readonly IRepository<ProjectAudit, Guid> _projectAuditRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public IEventBus _eventBus { get; set; }
        public DocmentAppService(IRepository<DocmentList, Guid> docmentRepository, IWorkFlowTaskRepository workFlowTaskRepository, IRepository<DocmentFlowing, Guid> docmentFlowingRepository,
            WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IRepository<ProjectAudit, Guid> projectAuditRepository, IRepository<WorkFlowOrganizationUnits, long> orgRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitsRepository,
            IRepository<AbpDictionary, Guid> abpDictionaryRepository, IAbpFileRelationAppService abpFileRelationAppService, WorkFlowCacheManager workFlowCacheManager, ProjectAuditManager projectAuditManager, QrCodeManager qrCodeManager
              ,IRepository<AbpFile, Guid> abpFilerepository,IRepository<User, long> userRepository, IRepository<QrCode, Guid> qrcoderepository) {
            _docmentRepository = docmentRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _abpDictionaryRepository = abpDictionaryRepository;
            _projectAuditManager = projectAuditManager;
            _docmentFlowingRepository = docmentFlowingRepository;
            _userRepository = userRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _projectAuditRepository = projectAuditRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _qrCodeManager = qrCodeManager;
            _abpFilerepository = abpFilerepository;
            _orgRepository = orgRepository;
            _eventBus = NullEventBus.Instance;
            _userOrganizationUnitsRepository = userOrganizationUnitsRepository;
            _qrcoderepository = qrcoderepository;
        }
        /// <summary>
        /// 创建归档申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task< InitWorkFlowOutput> Create(CreateDocmentInput input)
        {
            var model = input.MapTo<DocmentList>();
            model.Id = Guid.NewGuid();
            model.UserId = AbpSession.UserId.Value;
            model.No = DateTime.Now.ToString("yyyyMMddHHmmss");
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
                    BusinessType = (int)AbpFileBusinessType.档案附件,
                    Files = fileList
                });
            }
            if (input.QrCodeId.HasValue)
            {
                var qrcodeModel = new QrCode()
                {
                    Id = input.QrCodeId.Value,
                    Type = QrCodeType.档案
                };
                _qrCodeManager.UpdateType(qrcodeModel);
                model.QrCodeId = input.QrCodeId;
            }
            else {
               model.QrCodeId=_qrCodeManager.GetCreateId(QrCodeType.档案);
            }
            
            _docmentRepository.Insert(model);
            //创建流转记录
            _docmentFlowingRepository.Insert(new DocmentFlowing() {
                DocmentId = model.Id,
                IsOut = false,
                 Des="创建了该资料"
            });
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        /// <summary>
        /// 流程归档-档案进入档案袋
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> CreateDocment(DocmentInput input)
        {
            var task = _workFlowTaskRepository.Get(input.TaskId);
            if (input.QrCodeId.HasValue)
            {
                var qrcodeModel = new QrCode()
                {
                    Id = input.QrCodeId.Value,
                    Type = QrCodeType.档案
                };
                _qrCodeManager.UpdateType(qrcodeModel);
                
            }
            else
            {
                input.QrCodeId = _qrCodeManager.GetCreateId(QrCodeType.档案);
            }
            
            var id=  _docmentRepository.InsertAndGetId(new DocmentList()
            {
                ArchiveId = new Guid(task.InstanceID),
                Attr = input.Attr,//
                Des = task.Title+"归档申请",
                FlowId = task.FlowID,
                Status=(int) DocmentStatus.未归档,
                Name = task.Title + "-归档",
                No = DateTime.Now.ToString("yyyyMMddHHmmss"),
                QrCodeId = input.QrCodeId,
                UserId = AbpSession.UserId.Value,
                Type = new Guid("2B84FB3B-5083-4944-1B45-08D5BBBE5313")
            });
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var ite in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.档案附件,
                    Files = fileList
                });
            }
            //创建流转记录
            _docmentFlowingRepository.Insert(new DocmentFlowing()
            {
                DocmentId = id,
                IsOut = false,
                Des = "创建了该资料"
            });
            return new InitWorkFlowOutput() { InStanceId = id.ToString() };
        }
        /// <summary>
        /// 档案管理员新增档案
        /// </summary>
        /// <param name="inptu"></param>
        /// <returns></returns>
        public Guid Add(CreateDocmentInput input) {

            DocmentList model = null;
            if (input.QrCodeId.HasValue)
            {
                model = _docmentRepository.FirstOrDefault(ite => ite.QrCodeId == input.QrCodeId.Value);
            }
            if (model == null)
            {
                model = input.MapTo<DocmentList>();
                model.Status = -10;
                if (input.Status.HasValue) {
                    model.Status = (int)input.Status.Value;
                }
                model.No = DateTime.Now.ToString("yyyyMMddHHmmss");
                model.Id = Guid.NewGuid();
                if (input.Type == Guid.Empty)
                {
                    model.Type = new Guid("E919D963-F6F7-4491-73A0-08D5C079B809");
                }
                else
                {
                    model.Type = input.Type;
                }
                if (input.QrCodeId.HasValue)
                {
                    model.QrCodeId = input.QrCodeId;
                }
                else {
                    model.QrCodeId = _qrCodeManager.GetCreateId(QrCodeType.档案);
                }
                model.UserId = AbpSession.UserId.Value;
                if (input.FileList != null)
                {
                    var fileList = new List<AbpFileListInput>();
                    foreach (var ite in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                    }
                     _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                    {
                        BusinessId = model.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.档案附件,
                        Files = fileList
                    });
                }
                _docmentRepository.Insert(model);
                //创建流转记录
                _docmentFlowingRepository.Insert(new DocmentFlowing()
                {
                    DocmentId = model.Id,
                    IsOut = false,
                    Des = "创建了该资料"
                });
            }
            else {
                model.Name = input.Name;
                model.Attr = input.Attr;
                model.Des = input.Des;
                if (input.FileList != null)
                {
                    var fileList = new List<AbpFileListInput>();
                    foreach (var ite in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                    }
                    _abpFileRelationAppService.Update(new CreateFileRelationsInput()
                    {
                        BusinessId = model.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.档案附件,
                        Files = fileList
                    });
                }
                _docmentRepository.Update(model);
            }
            return model.Id;
        }
        /// <summary>
        /// 归档申请详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<DocmentDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var query = from a in _docmentRepository.GetAll()
                        join b in _abpDictionaryRepository.GetAll() on a.Type equals b.Id
                        //join c in _userRepository.GetAll() on a.UserId equals c.Id
                        where a.Id==id
                        select new DocmentDto()
                        {
                            Attr = a.Attr,
                            Attr_Name = a.Attr.ToString(),
                            Des = a.Des,
                            Id = a.Id,
                            Location = a.Location,
                            Name = a.Name,
                            No = a.No,
                            Type = a.Type,
                            Type_Name = b.Title,
                            UserId = a.UserId,
                            IsProject=a.IsProject,
                            ArchiveId = a.ArchiveId,
                            QrCodeId=a.QrCodeId,
                            NeedBack=a.NeedBack,
                            FlowId=a.FlowId,
                            //UserId_Name = c.Name
                        };
            var model =await query.FirstOrDefaultAsync();
            if (model == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到数据");
            }
            if (model.UserId.HasValue&&model.UserId.Value!=0) {
                var u = _userRepository.Get(model.UserId.Value);
                model.UserId_Name = u.Name;
            }
            model.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.档案附件 });
            return model;
        }

        public async Task<DocmentDto> GetDetail(Guid qrcodeIid) {
            var query = from a in _docmentRepository.GetAll()
                        join b in _abpDictionaryRepository.GetAll() on a.Type equals b.Id
                        //join c in _userRepository.GetAll() on a.UserId equals c.Id
                        where a.QrCodeId == qrcodeIid
                        select new DocmentDto()
                        {
                            Attr = a.Attr,
                            Attr_Name = a.Attr.ToString(),
                            Des = a.Des,
                            Id = a.Id,
                            Location = a.Location,
                            Name = a.Name,
                            No = a.No,
                            Type = a.Type,
                            Type_Name = b.Title,
                            FlowId = a.FlowId,
                            UserId = a.UserId,
                            IsProject = a.IsProject,
                            ArchiveId = a.ArchiveId,
                            QrCodeId = a.QrCodeId,
                            NeedBack = a.NeedBack,
                            ApplyDes=a.ApplyDes,
                            Status=a.Status
                            //UserId_Name = c.Name
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
            }
            model.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.档案附件 });
            return model;

        }
        /// <summary>
        /// 档案列表(排除-10的)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DocmentListDto>> GetAll(DocmentSearchInput input)
        {
            var query = from a in _docmentRepository.GetAll()
                        join b in _abpDictionaryRepository.GetAll() on a.Type equals b.Id
                        //join c in _userRepository.GetAll() on a.UserId equals c.Id 
                        where a.Status!=-10//不显示档案袋的
                        select new DocmentListDto()
                        {
                            CreationTime = a.CreationTime,
                            Attr = a.Attr,
                            Attr_Name = a.Attr.ToString(),
                            Id = a.Id,
                            QrCodeId=a.QrCodeId,
                            Location = a.Location,
                            Name = a.Name,
                            No = a.No,
                            Type = a.Type,
                            IsOld=a.IsOld,
                            IsOut=a.IsOut,
                            IsProject=a.IsProject,
                            ArchiveId = a.ArchiveId,
                            Type_Name = b.Title,
                            UserId = a.UserId,
                            Des = a.Des,
                            FlowId=a.FlowId,
                            Status = (int)a.Status,
                            NeedBack=a.NeedBack,
                            StatusTitle = ((DocmentStatus)a.Status).ToString()
                        };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false) {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey)||ite.No.Contains(input.SearchKey)||ite.Location.Contains(input.SearchKey));
            }
            if (input.Type.HasValue) {
                query = query.Where(ite => ite.Type == input.Type);
            }
            if (input.Attr.HasValue) {
                query = query.Where(ite => ite.Attr == input.Attr);
            }
            if (input.Status != null && input.Status.Count > 0) {
                query = query.Where(ite => input.Status.Contains((DocmentStatus)ite.Status));
                if (input.Status.Contains( DocmentStatus.在档)){//排除老档案
                    query = query.Where(ite => ite.QrCodeId.HasValue);
                }
            }
            var count = query.Count();
            var ret = (await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync()).MapTo<List<DocmentListDto>>();
            foreach (var r in ret)
            {
                if (r.UserId.HasValue && r.UserId != 0)
                {
                    var u = _userRepository.Get(r.UserId.Value);
                    r.UserId_Name = u.Name;
                }
                r.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.档案附件 });
            }
            return new PagedResultDto<DocmentListDto>(count,ret);
        }

        /// <summary>
        /// 获取待收档案
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DocmentListDto>> GetAllForWait(DocmentSearchInput input)
        {
            var query = from a in _docmentRepository.GetAll()
                        join b in _abpDictionaryRepository.GetAll() on a.Type equals b.Id
                        //join c in _userRepository.GetAll() on a.UserId equals c.Id 
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new DocmentListDto()
                        {
                            CreationTime = a.CreationTime,
                            Attr = a.Attr,
                            Attr_Name = a.Attr.ToString(),
                            Id = a.Id,
                            QrCodeId=a.QrCodeId,
                            Location = a.Location,
                            Name = a.Name,
                            No = a.No,
                            FlowId = a.FlowId,
                            IsOld =a.IsOld,
                             IsOut=a.IsOut,
                            Type = a.Type,
                            Type_Name = b.Title,
                            UserId = a.UserId,
                            Des = a.Des,
                            Status = (int)a.Status,
                            NeedBack=a.NeedBack,
                            StatusTitle = ((DocmentStatus)a.Status).ToString(),
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            query = query.Where(ite => ite.Status == 1);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey));
            }
            if (input.Type.HasValue)
            {
                query = query.Where(ite => ite.Type == input.Type);
            }
            if (input.Attr.HasValue)
            {
                query = query.Where(ite => ite.Attr == input.Attr);
            }
            //query = query.Where(ite => ite.Status == (int)input.Status);
            var count = query.Count();
            var ret = (await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync()).MapTo<List<DocmentListDto>>();
            foreach (var r in ret)
            {
                r.InstanceId = r.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, r as BusinessWorkFlowListOutput);
                if (r.UserId.HasValue && r.UserId != 0)
                {
                    var u = _userRepository.Get(r.UserId.Value);
                    r.UserId_Name = u.Name;
                }

                r.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.档案附件 });
            }
            return new PagedResultDto<DocmentListDto>(count, ret);
        }
        /// <summary>
        /// 获取档案袋列表、资料清单（状态为-10）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DocmentListDto>> GetAllForWorkflow(DocmentSearchInput input)
        {
            var query = from a in _docmentRepository.GetAll()
                        join b in _abpDictionaryRepository.GetAll() on a.Type equals b.Id
                        join c in _qrcoderepository.GetAll() on a.QrCodeId equals c.Id into e
                        from d in e.DefaultIfEmpty()
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        
                        select new DocmentListDto()
                        {
                             QrCodeType= d==null?  QrCodeType.档案 : d.Type,
                            CreationTime = a.CreationTime,
                             CreatorUserId=a.CreatorUserId,
                            Attr = a.Attr,
                            Attr_Name = a.Attr.ToString(),
                            Id = a.Id,
                            QrCodeId=a.QrCodeId,
                            Location = a.Location,
                            Name = a.Name,
                            No = a.No,
                            FlowId = a.FlowId,
                            Type = a.Type,
                            Type_Name = b.Title,
                            UserId = a.UserId,
                            Des = a.Des,
                            Status = (int)a.Status,
                            IsOut=a.IsOut,
                            IsOld=a.IsOld,
                            NeedBack=a.NeedBack,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey));
            }
            if (input.GetOutorIn.HasValue) {
                query = query.Where(ite => ite.IsOut == input.GetOutorIn.Value);
            }
            if (input.Type.HasValue)
            {
                query = query.Where(ite => ite.Type == input.Type);
            }
            
            if (input.GetMy)
            {
                var isorgleader = _orgRepository.GetAll().Where(ite => ite.Leader.Contains("u_" + AbpSession.UserId.Value)).Select(ite => ite.Id).ToList();
                if (isorgleader == null || isorgleader.Count == 0)
                {
                    query = query.Where(ite => ite.UserId == AbpSession.UserId);
                }
                else
                {
                    var userids = _userOrganizationUnitsRepository.GetAll().Where(ite => isorgleader.Contains(ite.OrganizationUnitId)).Select(ite => ite.UserId).Distinct().ToList();
                    query = query.Where(ite => userids.Contains(ite.UserId.Value));
                }
            }
            if (input.Attr.HasValue)
            {
                query = query.Where(ite => ite.Attr == input.Attr);
            }
            if (input.GetCanDocmentIn) {
                query = query.Where(ite => ite.QrCodeType == QrCodeType.档案);
            }
            if (input.Status != null && input.Status.Count > 0) {
                query = query.Where(ite => input.Status.Contains((DocmentStatus)ite.Status));
            }
            var count = query.Count();
            var ret = (await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync()).MapTo<List<DocmentListDto>>();
            foreach (var r in ret)
            {
                r.InstanceId = r.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, r as BusinessWorkFlowListOutput);
                r.StatusTitle = ((DocmentStatus)r.Status).ToString();
                if (r.UserId.HasValue && r.UserId != 0)
                {
                    var u = _userRepository.Get(r.UserId.Value);
                    r.UserId_Name = u.Name;
                }
                r.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = r.Id.ToString(), BusinessType = (int)AbpFileBusinessType.档案附件 });
            }
            return new PagedResultDto<DocmentListDto>(count, ret);
        }
        /// <summary>
        /// 档案袋-申请档案归档
        /// </summary>
        /// <returns></returns>
        public void ApplyStorge(ApplyDocmentInput input) {
            if (input.DocmentIds == null || input.DocmentIds.Count == 0) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请至少选择一个档案。");
            }
            var docs = _docmentRepository.GetAll().Where(ite =>input.DocmentIds.Contains(ite.Id)).ToList();
            if (docs == null || docs.Count == 0) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请至少选择一个档案。");
            }
            foreach (var doc in docs) {
                if (doc.Status == (int)DocmentStatus.未归档)
                {
                    doc.ApplyDes = input.ApplyDes;
                    doc.Status = (int)DocmentStatus.归档中;
                }
            }
            if (input.ReasonTaskId.HasValue)
            {
                _eventBus.Trigger(new TaskManagementData() { Id = input.ReasonTaskId.Value, TaskStatus = TaskManagementStateEnum.Done });
            }
        }
        /// <summary>
        /// 档案袋-办结归档
        /// </summary>
        /// <returns></returns>
        public async Task ApplyStorgeForShouwen(Guid QrCodeId)
        {
            //办结归档改变二维码状态
            var qrcodeModel = _qrCodeManager.Get(QrCodeId);
            qrcodeModel.Type = QrCodeType.档案;
            _qrCodeManager.UpdateType(qrcodeModel);

            var doc = _docmentRepository.FirstOrDefault(ite=>ite.QrCodeId==QrCodeId);
            if (doc == null )
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案袋中不存在该档案。");
            }
                if (doc.Status == (int)DocmentStatus.未归档)
                {
                    doc.Status = (int)DocmentStatus.归档中;
                }

        }
        /// <summary>
        /// 档案管理员接受档案袋归档申请
        /// </summary>
        /// <param name="qrcodeId"></param>
        /// <returns></returns>
        public async Task ApplyStorgeAgree(Guid qrcodeId) {
            var doc = _docmentRepository.GetAll().FirstOrDefault(ite => ite.QrCodeId == qrcodeId);
            if (doc == null) {
                throw new UserFriendlyException("未找到对应档案信息。");
            }
            if (doc.Status == (int)DocmentStatus.归档中)
            {
                doc.Status = (int)DocmentStatus.在档;
            }
            else {
                throw new UserFriendlyException("档案无需再次入档");
            }
        }
        /// <summary>
        /// 档案袋流转-扫码接受档案
        /// </summary>
        /// <param name="docmentid"></param>
        /// <returns></returns>
        public async Task GetFlowingDocment(Guid docmentId) {
            var doc = _docmentRepository.FirstOrDefault(ite=>ite.QrCodeId==docmentId);
            if (doc == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案不存在。");
            }
            doc.UserId = AbpSession.UserId.Value;//更改责任人
            doc.IsOut = false;//扫码后为内部档案
            //创建流转记录
          await  _docmentFlowingRepository.InsertAsync(new DocmentFlowing() {
                Des = "接收到该资料",
                DocmentId=doc.Id,
                IsOut = false
            });

        }
        /// <summary>
        /// 档案袋流转-公文顺序传阅（电子档），
        /// </summary>
        /// <param name="docmentid"></param>
        /// <returns></returns>
        public async Task AutoFlowingDocment(Guid qrCodeId,string userid)
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请指定要传阅的人。");
            }
            var doc = _docmentRepository.FirstOrDefault(ite => ite.QrCodeId == qrCodeId);
            if (doc == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "档案不存在。");
            }
            var id = userid.Replace("u_", "").ToInt();
            doc.UserId = id;//更改责任人
            doc.IsOut = false;//扫码后为内部档案
                              //创建流转记录
            await _docmentFlowingRepository.InsertAsync(new DocmentFlowing()
            {
                Des = "接收到该资料",
                DocmentId = doc.Id,
                IsOut = false
            });
        }
        /// <summary>
        /// 更新归档申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Update(UpdateDocmentInput input)
        {
            var model = _docmentRepository.Get(input.Id);
            var old_Model = model.DeepClone();
            model = input.MapTo(model);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var ite in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.档案附件,
                    Files = fileList
                });
            }
            await _docmentRepository.UpdateAsync(model);
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(model));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }
        private DocmentListDto GetChangeModel(DocmentList model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<DocmentListDto>();
            return ret;
        }
        /// <summary>
        /// 删除归档申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(Guid input)
        {
            var model = _docmentRepository.Get(input);
            if (model.Status != -10) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "只有状态为“未归档”的记录可以删除");
            }
            await _docmentRepository.DeleteAsync(model);
        }

        /// <summary>
        /// 批量销毁档案
        /// </summary>
        /// <param name="docmentIds"></param>
        /// <returns></returns>
        public async Task Destories(List<Guid> docmentIds)
        {
            if (docmentIds == null || docmentIds.Count == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请至少选择一个档案。");
            }
            var docs = await _docmentRepository.GetAll().Where(ite => docmentIds.Contains(ite.Id)).ToListAsync();
            foreach (var doc in docs)
            {
                doc.Status = (int)DocmentStatus.已销毁;
            }
        }

        /// <summary>
        /// 批量移交档案
        /// </summary>
        /// <param name="docmentIds"></param>
        /// <returns></returns>
        public async Task Moves(List<Guid> docmentIds)
        {
            if (docmentIds == null || docmentIds.Count == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请至少选择一个档案。");
            }
            var docs = await _docmentRepository.GetAll().Where(ite => docmentIds.Contains(ite.Id)).ToListAsync();
            foreach (var doc in docs)
            {
                doc.Status = (int)DocmentStatus.已移交;
            }
        }
        /// <summary>
        /// 旧档案导入
        /// </summary>
        /// <param name="fildId"></param>
        /// <returns></returns>
        public async Task<int> Export(Guid fileId)
        {
            var fileModel = _abpFilerepository.Get(fileId);
            var ds = this.LoadXlsx(fileModel.FilePath);
            // var ds = this.LoadXlsx(@"e:\doc.xlsx");
            foreach (var dt in ds) {
               for(var i=1;i<dt.Rows.Count;i++) {
                    var row = dt.Rows[i];
                    var obj = new DocmentList() {
                       
                        IsOld = true,
                        No=DateTime.Now.ToString("yyyyMMddHHmmss")+i,
                        Status=-1,
                        IsOut = false,
                        Type = new Guid("e919d963-f6f7-4491-73a0-08d5c079b809"),//档案袋
                         UserId=AbpSession.UserId.Value
                    };
                    obj.Name = row[0].ToString();
                    if (obj.Name.Length > 20) {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"行{i}：档案名称长度不能大于20");
                    }
                    obj.Location = row[1].ToString();
                    if (obj.Location.Length > 200)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"行{i}：存放位置长度不能大于200");
                    }
                    obj.Des= row[2].ToString();
                    if (obj.Des.Length > 200)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"行{i}：档案备注长度不能大于200");
                    }
                    obj.Attr = DocmentAttr.纸质;
                    var attr = row[2].ToString().Trim();
                    if (attr == "电子") {
                        obj.Attr = DocmentAttr.电子;
                    }
                    if (string.IsNullOrWhiteSpace(obj.Name))
                    {
                        return i;
                    }
                    else {
                        if (_docmentRepository.GetAll().Count(ite => ite.Name == obj.Name) == 0) {
                            _docmentRepository.Insert(obj);
                        }
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 统一二维码扫描
        /// </summary>
        /// <param name="docmentId"></param>
        /// <returns></returns>
        public async Task<string> Scan(Guid qrcodeId) {
            var doc = _docmentRepository.GetAll().FirstOrDefault(ite => ite.QrCodeId == qrcodeId);
            if (doc == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案");
            }
            var msg = "";
            switch ((DocmentStatus)doc.Status) {
                case DocmentStatus.使用中:
                    doc.Status = (int)DocmentStatus.在档;
                    msg = $"档案【{doc.Name}】已经成功归还。";
                    break;
                case DocmentStatus.未归档:
                    doc.UserId = AbpSession.UserId.Value;
                    msg = $"档案【{doc.Name}】已经成功流转到你的档案袋。";
                    break;
                case DocmentStatus.归档中:
                    doc.Status = (int)DocmentStatus.在档;
                    msg = $"档案【{doc.Name}】已经成功归档。";
                    break;
            }
            return "";
        }
        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<System.Data.DataTable> LoadXlsx(string path)
        {
            var dts = new List<System.Data.DataTable>();
            System.Data.DataTable dt = null;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            int sheetCount = 0;
            XSSFWorkbook book;
            try
            {
                book = new XSSFWorkbook(fs);
            }
            catch (Exception ex) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr,"打开excel文件失败，格式不被支持。");
            }
            sheetCount = book.NumberOfSheets;
            for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
            {
                NPOI.SS.UserModel.ISheet sheet = book.GetSheetAt(sheetIndex);
                if (sheet == null) continue;

                NPOI.SS.UserModel.IRow row = sheet.GetRow(0);
                if (row == null) continue;

                int firstCellNum = 0;
                int lastCellNum = row.LastCellNum;
                if (firstCellNum == lastCellNum) continue;

                dt = new System.Data.DataTable(sheet.SheetName);
                for (int i = firstCellNum; i < lastCellNum; i++)
                {
                    if (row.GetCell(i) == null)
                    {
                        dt.Columns.Add();
                        continue;
                    }
                    dt.Columns.Add(row.GetCell(i).StringCellValue, typeof(string));
                }

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    System.Data.DataRow newRow = dt.Rows.Add();
                    for (int j = firstCellNum; j < lastCellNum; j++)
                    {
                        if (sheet.GetRow(i) == null)
                        {
                            continue;
                        }
                        if (sheet.GetRow(i).GetCell(j) == null)
                        {
                            newRow[j] = "";
                        }
                        else
                        {
                            var cell = sheet.GetRow(i).GetCell(j);
                            switch (cell.CellType)
                            {
                                case NPOI.SS.UserModel.CellType.Blank:
                                    newRow[j] = "";
                                    break;
                                case NPOI.SS.UserModel.CellType.Boolean:
                                    newRow[j] = cell.BooleanCellValue;
                                    break;
                                case NPOI.SS.UserModel.CellType.Error:
                                    break;
                                case NPOI.SS.UserModel.CellType.Formula:
                                    newRow[j] = cell.NumericCellValue;
                                    break;
                                case NPOI.SS.UserModel.CellType.Numeric:
                                    //NPOI中数字和日期都是NUMERIC类型的，这里对其进行判断是否是日期类型
                                    if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                                    {
                                        newRow[j] = cell.DateCellValue;
                                    }
                                    else//其他数字类型
                                    {
                                        newRow[j] = cell.NumericCellValue;
                                    }
                                    break;
                                case NPOI.SS.UserModel.CellType.String:
                                    newRow[j] = cell.StringCellValue;
                                    break;
                                case NPOI.SS.UserModel.CellType.Unknown:
                                    break;

                            }
                        }
                    }
                }

                dts.Add(dt);
            }

            return dts;
        }
        /// <summary>
        /// 当前用户是否领导
        /// </summary>
        /// <returns></returns>
        public bool IsLeader()
        {
            var cuser = _userRepository.Get(AbpSession.UserId.Value);
            var roles= UserManager.GetRolesAsync(cuser).Result;
            if (roles.Any(r => r == "ZJL"))
                return true;
            else
                return false;
        }
    }
}
