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
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp;
using System.Threading;

namespace CWGL
{
    public class CWGLReceivableAppService : FRMSCoreAppServiceBase, ICWGLReceivableAppService
    {
        private readonly IRepository<CWGLReceivable, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        public CWGLReceivableAppService(IRepository<CWGLReceivable, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLReceivableListOutputDto>> GetList(GetCWGLReceivableListInput input)
        {
            var strflowid = input.FlowId.ToString();
            var queryBase = _repository.GetAll().Where(x => !x.IsDeleted);
            if (input.GetMy)
            {
                queryBase = queryBase.Where(r => r.CreatorUserId == AbpSession.UserId.Value);
            }
            else
            {
                queryBase = queryBase.Where(a => a.CreatorUserId.Value == AbpSession.UserId.Value || a.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : ""));
            }
            if (!string.IsNullOrEmpty(input.SearchKey))
                queryBase = queryBase.Where(r => r.UserName.Contains(input.SearchKey) || r.Name.Contains(input.SearchKey));
            var query = from a in queryBase
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                             x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                             x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                         select c).Any()
                        select new CWGLReceivableListOutputDto()
                        {
                            Id = a.Id,
                            UserName = a.UserName,
                            Name = a.Name,
                            Money = a.Money,
                            Mode = a.Mode,
                            BankName = a.BankName,
                            CardNumber = a.CardNumber,
                            BankOpenName = a.BankOpenName,
                            Note = a.Note,
                            Nummber = a.Nummber,
                            CreationTime = a.CreationTime,
                            Status = a.Status
                            ,
                            OpenModel = openModel ? 1 : 2,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<CWGLReceivableListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CWGLReceivableOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var tmp = model.MapTo<CWGLReceivableOutputDto>();
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.收款管理
            });
            return tmp;
        }
        /// <summary>
        /// 添加一个CWGLReceivable
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLReceivableInput input)
        {
            var id = Guid.NewGuid();
            var newmodel = new CWGLReceivable()
            {
                Id = id,
                UserName = input.UserName,
                Name = input.Name,
                Money = input.Money,
                Mode = input.Mode,
                BankName = input.BankName,
                CardNumber = input.CardNumber,
                BankOpenName = input.BankOpenName,
                Note = input.Note,
                Nummber = input.Nummber
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);

            if (input.IsSaveFAC)
            {
                var certificateAppService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IFinancialAccountingCertificateAppService>();

                input.FACData.BusinessId = id.ToString();
                certificateAppService.CreateOrUpdateWithOutNLP(input.FACData);
            }
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.收款管理,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLReceivable
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLReceivableInput input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                var logModel = new CWGLReceivable();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<CWGLReceivable>();
                }
                dbmodel.UserName = input.UserName;
                dbmodel.Name = input.Name;
                dbmodel.Money = input.Money;
                dbmodel.Mode = input.Mode;
                dbmodel.BankName = input.BankName;
                dbmodel.CardNumber = input.CardNumber;
                dbmodel.BankOpenName = input.BankOpenName;
                dbmodel.Note = input.Note;
                dbmodel.Nummber = input.Nummber;

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
                    BusinessType = (int)AbpFileBusinessType.收款管理,
                    Files = fileList
                });
                input.FACData.BusinessId = input.InStanceId.ToString();
                var groupId = Guid.NewGuid();
                input.FACData.GroupId = groupId;
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table, groupId);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
        private CWGLReceivableLogDto GetChangeModel(CWGLReceivable model)
        {
            var ret = model.MapTo<CWGLReceivableLogDto>();
            ret.Mode = model.Mode.ToString();
            return ret;
        }

        public void SendToZjlAsync(Guid flowID, string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.Get(id);
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var defaultMembers = organizeManager.GetAbpUsersByRoleCode("ZJL").Select(x => x.Id).ToList();
            var users = string.Join(",", defaultMembers);
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
            var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
            noticeInput.Content = $"收到({String.Format("{0:N2}", model.Money)}) 时间：" + model.CreationTime.ToString("yyyy/MM/dd HH:mm:ss");
            noticeInput.Title = $"收款：《{model.Name}》事务通知";
            noticeInput.NoticeUserIds = users;
            noticeInput.NoticeType = 1;
            noticeService.CreateOrUpdateNotice(noticeInput);
        }
    }
}