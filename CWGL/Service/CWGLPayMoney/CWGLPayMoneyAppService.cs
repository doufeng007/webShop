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
using Abp.Domain.Uow;
using ZCYX.FRMSCore.Authorization.Users;
using Abp;
using CWGL.Enums;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLPayMoneyAppService : FRMSCoreAppServiceBase, ICWGLPayMoneyAppService
    {
        private readonly IRepository<CWGLPayMoney, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public CWGLPayMoneyAppService(IRepository<CWGLPayMoney, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService ,WorkFlowCacheManager workFlowCacheManager,ProjectAuditManager projectAuditManager,IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> usersRepository, IWorkFlowTaskRepository workFlowTaskRepository
        )
        {
            _workFlowCacheManager = workFlowCacheManager;
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _unitOfWorkManager = unitOfWorkManager;
            _usersRepository = usersRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLPayMoneyListOutputDto>> GetList(GetCWGLPayMoneyListInput input)
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
            var query = (from a in queryBase
                         let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                             x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                             x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                          select c).Any()
                         select new CWGLPayMoneyListOutputDto
                         {
                             Id = a.Id,
                             CreationTime = a.CreationTime,
                             Money = a.Money,
                             UserName = a.UserName,
                             CustomerName = a.CustomerName,
                             Status = a.Status ?? 0,
                             OpenModel = openModel ? 1 : 2,
                         });
            var toalCount = await query.CountAsync();
            var models = await query.OrderBy(r=>r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            List<CWGLPayMoneyListOutputDto> ret = null;
            if (models.Count > 0)
            {
                ret = new List<CWGLPayMoneyListOutputDto>();
                foreach (var tmp in models)
                {
                    tmp.InstanceId = tmp.Id.ToString();
                    _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, tmp as BusinessWorkFlowListOutput);
                    ret.Add(tmp);
                }
            }
            return new PagedResultDto<CWGLPayMoneyListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CWGLPayMoneyOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var tmp = model.MapTo<CWGLPayMoneyOutputDto>();
            tmp.UserId = model.CreatorUserId.Value;
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.付款申请
            });
            return tmp;
        }
        /// <summary>
        /// 添加一个CWGLPayMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLPayMoneyInput input)
        {
            var id = Guid.NewGuid();
            var newmodel = new CWGLPayMoney()
            {
                Id = id,
                UserName = input.UserName,
                CustomerName = input.CustomerName,
                Money = input.Money,
                Mode = input.Mode,
                BankName = input.BankName,
                CardNumber = input.CardNumber,
                BankOpenName = input.BankOpenName,
                Note = input.Note,
                Nummber = input.Nummber,
                ContractNum = input.ContractNum
            };
            newmodel.Status = 0;
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
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.付款申请,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLPayMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLPayMoneyInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new CWGLPayMoney();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<CWGLPayMoney>();
                }
                dbmodel.UserName = input.UserName;
                dbmodel.CustomerName = input.CustomerName;
                dbmodel.Money = input.Money;
                dbmodel.Mode = input.Mode;
                dbmodel.BankName = input.BankName;
                dbmodel.CardNumber = input.CardNumber;
                dbmodel.BankOpenName = input.BankOpenName;
                dbmodel.Note = input.Note;
                dbmodel.Nummber = input.Nummber;
                dbmodel.ContractNum = input.ContractNum;
                dbmodel.FlowNumber = input.FlowNumber;
                input.FACData.BusinessId = input.Id.ToString();

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
                    BusinessType = (int)AbpFileBusinessType.付款申请,
                    Files = fileList
                });
                var groupId = Guid.NewGuid();
                input.FACData.GroupId = groupId;
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table,groupId);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }
        private CWGLPayMoneyLogDto GetChangeModel(CWGLPayMoney model)
        {
            var ret = model.MapTo<CWGLPayMoneyLogDto>();
            ret.Mode_Name = Enum.GetName(typeof(MoneyMode), model.Mode);
            return ret;
        }
 


        public void CreateCredential(Guid flowID, string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.Get(id);
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLCredentialAppService>();
            var input = new CreateCWGLCredentialInput();
            input.IsPay = true;
            input.BusinessType = CredentialType.付款;
            input.BusinessId = id;
            input.Name = model.UserName;
            input.Cause = model.Note;
            input.Money = model.Money;
            input.FlowNumber = model.FlowNumber;
            input.ContractNum = model.ContractNum;
            input.Mode = model.Mode;
            input.BankName = model.BankName;
            input.CardNumber = model.CardNumber;
            input.BankOpenName = model.BankOpenName;
            input.Nummber = model.Nummber;
            var files = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.付款申请
            });
            input.FileList = files;
            service.Create(input);
        }
    }
}