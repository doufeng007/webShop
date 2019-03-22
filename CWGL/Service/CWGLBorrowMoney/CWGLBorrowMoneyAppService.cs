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
using Abp.Authorization;
using Abp.Application.Services;
using ZCYX.FRMSCore.Users;
using CWGL.Enums;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLBorrowMoneyAppService : FRMSCoreAppServiceBase, ICWGLBorrowMoneyAppService
    {
        private readonly IRepository<CWGLBorrowMoney, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IDynamicRepository _dynamicRepository;
        public CWGLBorrowMoneyAppService(IRepository<CWGLBorrowMoney, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> usersRepository, IWorkFlowTaskRepository workFlowTaskRepository, IDynamicRepository dynamicRepository
        )
        {
            _dynamicRepository = dynamicRepository;
            _workFlowCacheManager = workFlowCacheManager;
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _unitOfWorkManager = unitOfWorkManager;
            _usersRepository = usersRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }
        public async Task<PagedResultDto<CWGLBorrowMoneyMyListOutputDto>> GetMyList(CWGLBorrowMoneyMyListInput input)
        {
            var query = _repository.GetAll().Where(x => !x.IsDeleted && x.CreatorUserId == AbpSession.UserId.Value && x.Status == -1);

            var toalCount = await query.CountAsync();
            var models = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            List<CWGLBorrowMoneyMyListOutputDto> ret = null;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                if (models.Count > 0)
                {
                    ret = new List<CWGLBorrowMoneyMyListOutputDto>();
                    foreach (var a in models)
                    {
                        var user = _usersRepository.Get(a.UserId);
                        var tmp = new CWGLBorrowMoneyMyListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            IsPayBack = a.IsPayBack,
                            Money = a.Money,
                        };
                        tmp.UserName = user.Name;
                        var orgModel = await _organizationUnitRepository.GetAsync(a.OrgId);
                        tmp.DepartmentName = orgModel.DisplayName;
                        ret.Add(tmp);
                    }
                }

            }
            return new PagedResultDto<CWGLBorrowMoneyMyListOutputDto>(toalCount, ret);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLBorrowMoneyListOutputDto>> GetList(GetCWGLBorrowMoneyListInput input)
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
                         join b in _usersRepository.GetAll() on a.UserId equals b.Id
                         let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                             x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                             x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                          select c).Any()
                         select new CWGLBorrowMoneyListOutputDto
                         {
                             Id = a.Id,
                             UserName = b.Name,
                             CreationTime = a.CreationTime,
                             Money = a.Money,
                             Status = a.Status,
                             OrgId = a.OrgId,
                             IsPayBack = a.IsPayBack,
                             TypeId = a.TypeId,
                             OpenModel = openModel ? 1 : 2,
                         });
            var toalCount = await query.CountAsync();
            var models = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            List<CWGLBorrowMoneyListOutputDto> ret = null;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                if (models.Count > 0)
                {
                    ret = new List<CWGLBorrowMoneyListOutputDto>();
                    foreach (var tmp in models)
                    {
                        var orgModel = await _organizationUnitRepository.GetAsync(tmp.OrgId);
                        tmp.DepartmentName = orgModel.DisplayName;
                        tmp.InstanceId = tmp.Id.ToString();
                        _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, tmp as BusinessWorkFlowListOutput);
                        ret.Add(tmp);
                    }
                }

            }
            return new PagedResultDto<CWGLBorrowMoneyListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 检查用户权限
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckPostFromUser(Guid TaskId, int type)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var taskModel = repository.Get(TaskId);

            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(taskModel.SenderID);
            switch (type)
            {
                case 0:
                    return userRoles.Any(r => r == "CW");
                case 1:
                    return userRoles.Any(r => r == "DLeader");
                case 2:
                    return userRoles.Any(r => r == "ZJL");
                case 3:
                    return userRoles.Any(r => r == "CL");
            }
            return false;
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CWGLBorrowMoneyOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var tmp = model.MapTo<CWGLBorrowMoneyOutputDto>();
            var user = _usersRepository.Get(model.UserId);
            tmp.UserName = user.Name;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var orgModel = await _organizationUnitRepository.GetAsync(model.OrgId);
                tmp.DepartmentName = orgModel.DisplayName;
            }
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.借款申请
            });
            return tmp;

        }
        /// <summary>
        /// 添加一个CWGLBorrowMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLBorrowMoneyInput input)
        {

            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var id = Guid.NewGuid();
            var newmodel = new CWGLBorrowMoney()
            {
                Id = id,
                UserId = AbpSession.UserId.Value,
                OrgId = userOrgModel.OrgId,
                TypeId = input.TypeId,
                Money = input.Money,
                Mode = input.Mode,
                IsPayBack = false,
                BankName = input.BankName,
                CardNumber = input.CardNumber,
                BankOpenName = input.BankOpenName,
                Note = input.Note,
                RepaymentTime = input.RepaymentTime,
                Nummber = input.Nummber
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
                    BusinessType = (int)AbpFileBusinessType.借款申请,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }


        /// <summary>
        /// 添加一个CWGLBorrowMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task<InitWorkFlowOutput> CreateTest()
        {
            var input = new CreateCWGLBorrowMoneyInput()
            {
                BankName = "1",
                BankOpenName = "2",
                CardNumber = "3",
                FlowId = Guid.Parse("384c41b8-9d13-429b-ab62-fcc22e8639ee"),
                FlowTitle = "查询效率测试",
                Mode = MoneyMode.现金,
                Money = 12312,
                Note = "测试",
                Nummber = 1,
                TypeId = BorrowMoney.普通借款,


            };
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var id = Guid.NewGuid();
            var newmodel = new CWGLBorrowMoney()
            {
                Id = id,
                UserId = AbpSession.UserId.Value,
                OrgId = userOrgModel.OrgId,
                TypeId = input.TypeId,
                Money = input.Money,
                Mode = input.Mode,
                IsPayBack = false,
                BankName = input.BankName,
                CardNumber = input.CardNumber,
                BankOpenName = input.BankOpenName,
                Note = input.Note,
                Nummber = input.Nummber
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
                    BusinessType = (int)AbpFileBusinessType.借款申请,
                    Files = fileList
                });
            }
            var service1 = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            service1.InitWorkFlowInstance(new InitWorkFlowInput() { FlowId = input.FlowId, FlowTitle = input.FlowTitle, InStanceId = newmodel.Id.ToString() });
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLBorrowMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLBorrowMoneyInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new CWGLBorrowMoney();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<CWGLBorrowMoney>();
                }
                dbmodel.TypeId = input.TypeId;
                dbmodel.Money = input.Money;
                dbmodel.Mode = input.Mode;
                dbmodel.BankName = input.BankName;
                dbmodel.CardNumber = input.CardNumber;
                dbmodel.BankOpenName = input.BankOpenName;
                dbmodel.Note = input.Note;
                dbmodel.RepaymentTime = input.RepaymentTime;
                dbmodel.Nummber = input.Nummber;
                dbmodel.RepayTime = input.RepayTime;
                dbmodel.RepayMode = input.RepayMode;
                dbmodel.RepayCardNumber = input.RepayCardNumber;
                dbmodel.RepayBankOpenName = input.RepayBankOpenName;
                dbmodel.RepayBankName = input.RepayBankName;
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
                    BusinessType = (int)AbpFileBusinessType.借款申请,
                    Files = fileList
                });
                var groupId = Guid.NewGuid();
                input.FACData.GroupId = groupId;
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table, groupId);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }
        private CWGLBorrowMoneyLogDto GetChangeModel(CWGLBorrowMoney model)
        {
            var ret = model.MapTo<CWGLBorrowMoneyLogDto>();
            ret.TypeId_Name = Enum.GetName(typeof(BorrowMoney), model.TypeId);
            ret.Mode_Name = Enum.GetName(typeof(MoneyMode), model.Mode);
            return ret;
        }
      

        /// <summary>
        /// 是否需要分管领导审核 若行政人员发起的流程 则需要分管领导审核
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns> 
        [RemoteService(IsEnabled = false)]
        [AbpAuthorize]
        public bool IsNeedCWCLAudit(Guid flowID, Guid groupID, string InstanceID)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
            var firstSender = _service.GetFirstSnderID(flowID, groupID);
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(AbpSession.UserId.Value);
            return userRoles.Any(r => r == "CW");
        }

        /// <summary>
        /// 是否需要分管领导审核 若行政人员发起的流程 则需要分管领导审核
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns> 
        //[RemoteService(IsEnabled = false)]
        [AbpAuthorize]
        public bool IsRole(Guid flowID, Guid groupID, string InstanceID, int Type, string Field = "CreatorUserId")
        {
            var id = Guid.Parse(InstanceID);
            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
            if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
            var firstDB = flowModel.DataBases.First();
            var query_Sql = $"select {Field} from {flowModel.TitleField.Table} where  {firstDB.PrimaryKey}=\'{InstanceID}\'";
            var instanceModel = _dynamicRepository.QueryFirst(query_Sql);
            if (instanceModel == null) throw new UserFriendlyException(0, $"缺少{Field}.");
            long userId = 0;
            foreach (var item in instanceModel)
            {
                if (item.Key == Field)
                    userId = Convert.ToInt64(item.Value);
            }
            if(userId==0)
                throw new UserFriendlyException(0, $"未查到{Field}用户.");

            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(userId);


            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            switch (Type)
            {
                case 1://总经理
                    return userRoles.Any(r => r == "ZJL");
                case 2://分管领导
                    //return !userRoles.Any(r => r == "ZJL") && userRoles.Any(r => r == "FGLD");
                    return !userRoles.Any(r => r == "ZJL") && (manager.IsChargerLeader(userId) || userRoles.Any(r => r == "FGLD"));
                case 3:// 部门领导
                    //return !userRoles.Any(r => r == "ZJL") && !userRoles.Any(r => r == "FGLD") && manager.IsChargerLeader(userId);
                    return !userRoles.Any(r => r == "ZJL") && !userRoles.Any(r => r == "FGLD") && !manager.IsChargerLeader(userId)&& manager.IsLeader(userId);
                case 4://普通员工
                    return !userRoles.Any(r => r == "ZJL") && !userRoles.Any(r => r == "FGLD") && !manager.IsChargerLeader(userId) && !manager.IsLeader(userId);
            }
            return false;
        }
        public bool IsCommonLoan(Guid flowID, string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.Get(id);
            return model.TypeId == BorrowMoney.普通借款;
        }
        public void CreateCredential(Guid flowID, string InstanceID, bool isPay)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.Get(id);
            if (model.TypeId == BorrowMoney.备用金)
                return;
            var user = _usersRepository.Get(model.UserId);
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICWGLCredentialAppService>();
            var input = new CreateCWGLCredentialInput();
            input.IsPay = isPay;
            input.BusinessType = CredentialType.借款;
            input.BusinessId = id;
            input.Name = user.Name;
            input.Cause = model.Note;
            input.Money = model.Money;
            if (isPay)
            {
                input.Mode = model.Mode;
                input.BankName = model.BankName;
                input.CardNumber = model.CardNumber;
                input.BankOpenName = model.BankOpenName;
            }
            else
            {
                input.Mode = model.RepayMode ?? model.RepayMode.Value;
                input.BankName = model.RepayBankName;
                input.CardNumber = model.RepayCardNumber;
                input.BankOpenName = model.RepayBankOpenName;
                input.CreationTime = model.RepayTime;
                model.IsPayBack = true;
                _repository.Update(model);
            }
            input.Nummber = model.Nummber;
            var files = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.借款申请
            });
            input.FileList = files;
            service.Create(input);

        }


        public void UpdateIsPayBack(Guid flowID, string InstanceID, bool isPay)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.Get(id);
            if (model.TypeId == BorrowMoney.备用金)
                return;
            var user = _usersRepository.Get(model.UserId);
            if (!isPay)
            {
                model.IsPayBack = true;
                _repository.Update(model);
            }
        }
    }
}