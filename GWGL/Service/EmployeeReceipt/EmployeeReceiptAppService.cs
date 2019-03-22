using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using System.Dynamic;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Threading;
using Abp.Authorization;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Project;
using Abp.Events.Bus;

namespace GWGL
{
    [AbpAuthorize]
    public class EmployeeReceiptAppService : FRMSCoreAppServiceBase, IEmployeeReceiptAppService
    {
        private readonly IRepository<EmployeeReceipt, Guid> _repository;
        private readonly IRepository<GW_DocumentType, Guid> _gW_DocumentTypeRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly QrCodeManager _qrCodeManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public  IEventBus _eventBus { get; set; }
        public EmployeeReceiptAppService(IRepository<EmployeeReceipt, Guid> repository
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager,
            IAbpFileRelationAppService abpFileRelationAppService, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IWorkFlowTaskRepository workFlowTaskRepository, QrCodeManager qrCodeManager, IRepository<GW_DocumentType, Guid> gW_DocumentTypeRepository, WorkFlowTaskManager workFlowTaskManager
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _qrCodeManager = qrCodeManager;
            _gW_DocumentTypeRepository = gW_DocumentTypeRepository;
            _eventBus = NullEventBus.Instance;
            _workFlowTaskManager = workFlowTaskManager;
        }


        public List<ExpandoObject> GetReceiptDocProperty(string value = null, string setEmpty = null)
        {
            return EnumExtensions.GetEnumList<ReceiptDocProperty>(value, setEmpty);
        }
        public List<ExpandoObject> GetReceiptRankProperty(string value = null, string setEmpty = null)
        {
            return EnumExtensions.GetEnumList<RankProperty>(value, setEmpty);
        }

        public List<ExpandoObject> GetEmergencyDegreeProperty(string value = null, string setEmpty = null)
        {
            return EnumExtensions.GetEnumList<EmergencyDegreeProperty>(value, setEmpty);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeReceiptListOutputDto>> GetList(GetEmployeeReceiptListInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var userId = user.Id.ToString();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && (x.CreatorUserId==user.Id || x.DealWithUsers.GetStrContainsArray(userId) || x.CopyForUsers.GetStrContainsArray(userId)))
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new EmployeeReceiptListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            DocReceiveDep = a.DocReceiveDep,
                            DocReceiveNo = a.DocReceiveNo,
                            ReceiptNo = a.ReceiptNo,
                            DocType = a.DocType,
                            Rank = a.Rank,
                            EmergencyDegree = a.EmergencyDegree,
                            CreationTime = a.CreationTime,
                            ReportMatters = a.ReportMatters,
                            DocProperty = a.DocProperty,
                            Remark = a.Remark,
                            Status = a.Status ?? 0,
                            Opinion = a.Opinion,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2,
                            IsMe=openModel.Count(y=>y.Status==-1)>0 && a.CreatorUserId!=AbpSession.UserId.Value,
                            DealWithUsers = a.DealWithUsers,
                            CreateUserId = a.CreatorUserId
                        };
            query = query.Where(x => !x.IsMe);
            if (!input.SearchKey.IsNullOrEmpty())
            {
                query = query.Where(r => r.Title.Contains(input.SearchKey) || r.DocReceiveNo.Contains(input.SearchKey) || r.ReceiptNo.ToString().Contains(input.SearchKey));
            }
            if (!input.DocReceiveNo.IsNullOrEmpty())
            {
                query = query.Where(r => r.DocReceiveNo.Contains(input.DocReceiveNo));
            }
            if (input.DocType.HasValue)
            {
                query = query.Where(r => r.DocType==input.DocType);
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            ret.ForEach(x =>
           {
               x.InstanceId = x.Id.ToString();
               _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, x);
               if (!string.IsNullOrEmpty(x.DealWithUsers))
               {
                   var allusers = x.DealWithUsers.Split(',').Select(long.Parse);
                   if (allusers.Any())
                   {
                       allusers = allusers.Where(y => y != x.CreateUserId).ToList();
                       if (allusers.Any())
                       {
                           x.DealWithUsers = string.Join(",",
                               UserManager.Users.Where(y => allusers.Contains(y.Id)).Select(y => y.Name));
                       }
                       else
                       {
                           x.DealWithUsers = UserManager.Users.FirstOrDefault(y => y.Id == x.CreateUserId)?.Name;
                       }
                   }
                   else
                   {
                       x.DealWithUsers = "";
                   }

               }
               else
               {
                   x.DealWithUsers = "";
               }
               x.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
               {
                   BusinessId = x.Id.ToString(),
                   BusinessType = (int)AbpFileBusinessType.收文附件
               });
               x.DocPropertyName = x.DocProperty.GetLocalizedDescription();
               var type = _gW_DocumentTypeRepository.GetAll().FirstOrDefault(y => y.Id == x.DocType);
               x.DocTypeName = type?.Name;
               x.RankPropertyName = x.Rank.GetLocalizedDescription();
               x.EmergencyDegreePropertyName = x.EmergencyDegree.GetLocalizedDescription();
           });
            return new PagedResultDto<EmployeeReceiptListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<EmployeeReceiptOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var result = model.MapTo<EmployeeReceiptOutputDto>();

            result.DocPropertyName = result.DocProperty.GetLocalizedDescription();
            var type = _gW_DocumentTypeRepository.GetAll().FirstOrDefault(x => x.Id == model.DocType);
            result.DocTypeName =type?.Name;
            result.RankPropertyName = result.Rank.GetLocalizedDescription();
            result.EmergencyDegreePropertyName = result.EmergencyDegree.GetLocalizedDescription();

            result.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
            {
                BusinessId = result.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.收文附件
            });
            return result;
        }

        /// <summary>
        /// 添加一个EmployeeReceipt
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<EmployeeReceiptOutput> Create(CreateEmployeeReceiptInput input)
        {
            if(input.DocProperty==ReceiptDocProperty.Electronic && input.FileList.Count()==0)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请上传正文。");
            var newmodel = new EmployeeReceipt()
            {
                Title = input.Title,
                DocReceiveDep = input.DocReceiveDep,
                DocReceiveNo = input.DocReceiveNo,
                DocType = input.DocType,
                DocProperty = input.DocProperty,
                Rank = input.Rank,
                EmergencyDegree = input.EmergencyDegree,
                ReportMatters = input.ReportMatters,
                Remark = input.Remark,
                IsPrintQrcode = input.IsPrintQrcode,
                QrCodeId = _qrCodeManager.GetCreateId(QrCodeType.公文),
                Opinion = ""//拟办意见为空
            };
            var lastNumber = Convert.ToInt32(_repository.GetAll().OrderByDescending(x => x.ReceiptNo).FirstOrDefault()?.ReceiptNo);
            Interlocked.Increment(ref lastNumber);
            newmodel.ReceiptNo = lastNumber;
            await _repository.InsertAsync(newmodel);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = newmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.收文附件,
                    Files = fileList
                });
            }
            return new EmployeeReceiptOutput() { InStanceId = newmodel.Id.ToString(),QrCodeId=newmodel.QrCodeId.Value };
        }

        /// <summary>
        /// 修改一个EmployeeReceipt
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateEmployeeReceiptInput input)
        {
            if (input.Id != Guid.Empty)
            {
                if (input.DocProperty == ReceiptDocProperty.Electronic && input.FileList.Count() == 0)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请上传正文。");
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new EmployeeReceipt();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<EmployeeReceipt>();
                }
                dbmodel.Opinion = input.Opinion;
                dbmodel.Title = input.Title;
                dbmodel.Remark = input.Remark;
                dbmodel.Rank = input.Rank;
                dbmodel.IsPrintQrcode = input.IsPrintQrcode;
                dbmodel.DocReceiveNo = input.DocReceiveNo;
                dbmodel.DocReceiveDep = input.DocReceiveDep;
                dbmodel.CopyForType = input.CopyForType;
                dbmodel.TaskType = input.TaskType;
                dbmodel.DocType = input.DocType;
                dbmodel.CopyForUsers = input.CopyForUsers;
                dbmodel.EmergencyDegree = input.EmergencyDegree;
                await _repository.UpdateAsync(dbmodel);

                var fileList = new List<AbpFileListInput>();
                if (input.FileList != null)
                {
                    foreach (var item in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                    }
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = input.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.收文附件,
                    Files = fileList,
                });
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var files = input.OldFileList.Select(r => new AbpFileChangeDto { FileName = r.FileName, Id = r.Id }).ToList();
                    var newfiles = input.FileList.Select(r => new AbpFileChangeDto { FileName = r.FileName, Id = r.Id }).ToList();
                    var logs = GetChangeModel(logModel, files).GetColumnAllLogs(GetChangeModel(dbmodel, newfiles));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
                    _workFlowTaskManager.CreateNoticeForTask(input.FlowId, input.Id.ToString(), "收文变更", dbmodel.Title);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

        /// <summary>
        /// 修改一个EmployeeReceipt
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task UpdateCopyFor(UpdateEmployeeReceiptInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                dbmodel.CopyForType = input.CopyForType;
                dbmodel.TaskType = input.TaskType;
                dbmodel.CopyForUsers = input.CopyForUsers;
                await _repository.UpdateAsync(dbmodel);
                if (dbmodel.QrCodeId.HasValue)
                {
                    var qrcodeModel = _qrCodeManager.Get(dbmodel.QrCodeId.Value);
                    if (qrcodeModel.Type == QrCodeType.公文)
                    {
                        qrcodeModel.Type = QrCodeType.档案;
                        _qrCodeManager.UpdateType(qrcodeModel);
                    }
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }
        public async Task<long> GetNextUserId(EmployeeReceiptNextInput input)
        {
            var taskModel = _workFlowTaskRepository.GetAll().Where(x =>x.FlowID==input.FlowId&&  x.InstanceID == input.InstanceId.ToString() && x.ReceiveID==AbpSession.UserId.Value  && x.Type == 7 && x.Status==1).FirstOrDefault();
            if (taskModel == null)
                return 0;
            var task = _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.SenderTime>taskModel.SenderTime&& x.InstanceID == input.InstanceId.ToString() && x.Status != 2 &&x.Type==7).OrderBy(x => x.SenderTime).FirstOrDefault();
            return task!=null?task.ReceiveID:0;
        }
        public async Task<WorkFlowCustomEventParams> EnableTask(EntityDto<Guid> input)
        {
            if (input.Id == Guid.Empty)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            var qrcodeModel = _qrCodeManager.Get(input.Id);
            if (qrcodeModel.Type != QrCodeType.公文)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            var model = _repository.GetAll().FirstOrDefault(x => x.QrCodeId == input.Id && x.DocProperty == ReceiptDocProperty.Paper);
            if(model==null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            var task = _workFlowTaskRepository.GetAll().Where(x => x.ReceiveID == AbpSession.UserId.Value && x.InstanceID == model.Id.ToString() && x.Status!=2 ).OrderByDescending(x=>x.Sort).FirstOrDefault();
            if(task==null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "没有找到待办。");
            if (task.Status == -1) {
                task.Status = 0;
                _workFlowTaskRepository.Update(task);
            }
            //qrcodeModel.Type = QrCodeType.档案;
            //_qrCodeManager.UpdateType(qrcodeModel);

            _eventBus.Trigger(new DocmentQrcode() { QrCode=qrcodeModel.Id });
            return new WorkFlowCustomEventParams() { TaskID=task.Id,FlowID=task.FlowID,GroupID=task.GroupID,StepID=task.StepID,InstanceID=task.InstanceID};
        }
        private EmployeeReceiptLogDto GetChangeModel(EmployeeReceipt model, List<AbpFileChangeDto> files)
        {
            var ret = model.MapTo<EmployeeReceiptLogDto>();
            var type = _gW_DocumentTypeRepository.GetAll().FirstOrDefault(y => y.Id == model.DocType);
            ret.DocType = type?.Name;
            ret.DocProperty = model.DocProperty.GetLocalizedDescription();
            ret.Rank = model.Rank.GetLocalizedDescription();
            ret.EmergencyDegree = model.EmergencyDegree.GetLocalizedDescription();
            if(files.Count()>0)
                ret.Files = files;
            return ret;
        }
        public async Task CreateWrite(EmployeeReceiptAddWriteInput input)
        {
            var employeeReceiptModel = _repository.Get(input.Id);
            switch (employeeReceiptModel.TaskType)
            {
                case TaskTypeProperty.CreateTask:
                    break;
                case TaskTypeProperty.Distribute:
                    var workFlowWorkTaskAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
                    if (!string.IsNullOrEmpty(employeeReceiptModel.CopyForUsers))
                    {
                        if (employeeReceiptModel.CopyForType == CopyForTypeProperty.Order)
                        {
                            workFlowWorkTaskAppService.AddWrite(input.TaskId, 2, 3, employeeReceiptModel.CopyForUsers, "");
                        }
                    }
                    break;
                case TaskTypeProperty.Filing:
                    break;
            }
        }
    }
}