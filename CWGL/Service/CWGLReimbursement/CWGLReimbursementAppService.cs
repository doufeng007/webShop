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
using ZCYX.FRMSCore.Users;

namespace CWGL
{
    public class CWGLReimbursementAppService : FRMSCoreAppServiceBase, ICWGLReimbursementAppService
    { 
        private readonly IRepository<CWGLReimbursement, Guid> _repository;
        private readonly IRepository<CWGLBorrowMoney, Guid> _borrowMoneyRepository;
		private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        public CWGLReimbursementAppService(IRepository<CWGLReimbursement, Guid> repository
		,WorkFlowBusinessTaskManager workFlowBusinessTaskManager,IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> usersRepository, WorkFlowCacheManager workFlowCacheManager, IRepository<CWGLBorrowMoney, Guid> borrowMoneyRepository, IWorkFlowTaskRepository workFlowTaskRepository
        )
        {
            this._repository = repository;
			_workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
             _projectAuditManager = projectAuditManager;
            _borrowMoneyRepository = borrowMoneyRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _usersRepository = usersRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<CWGLReimbursementListOutputDto>> GetList(GetCWGLReimbursementListInput input)
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
                         select new CWGLReimbursementListOutputDto
                         {
                             Id = a.Id,
                             UserName = b.Name,
                             CreationTime = a.CreationTime,
                             Money = a.Money,
                             Note = a.Note,
                             OrgId = a.OrgId,
                             Status = a.Status ?? 0,
                             OpenModel = openModel ? 1 : 2,
                         });
            var toalCount = await query.CountAsync();
            var models = await query.OrderBy(r=>r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            List<CWGLReimbursementListOutputDto> ret = null;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                if (models.Count > 0)
                {
                    ret = new List<CWGLReimbursementListOutputDto>();
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
            return new PagedResultDto<CWGLReimbursementListOutputDto>(toalCount, ret);
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
        public async Task<CWGLReimbursementOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }

            var tmp = model.MapTo<CWGLReimbursementOutputDto>();
            tmp.ModeName = Enum.GetName(typeof(MoneyMode), model.Mode);
            var user = _usersRepository.Get(model.UserId);
            if (!model.BorrowMoneyId.IsEmptyGuid())
            {
                var borrowMoney = _borrowMoneyRepository.Get(model.BorrowMoneyId);
                tmp.BorrowMoney = borrowMoney.Money;
            }
            tmp.UserName = user.Name;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var orgModel = await _organizationUnitRepository.GetAsync(model.OrgId);
                tmp.DepartmentName = orgModel.DisplayName;
            }
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.报销
            });
            return tmp;
        }
        /// <summary>
        /// 添加一个CWGLReimbursement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLReimbursementInput input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var id = Guid.NewGuid();
            var newmodel = new CWGLReimbursement()
            {
                Id = id,
                UserId = AbpSession.UserId.Value,
                OrgId = userOrgModel.OrgId,
                Money = input.Money,
                Mode = input.Mode,
                BankName = input.BankName,
                CardNumber = input.CardNumber,
                BankOpenName = input.BankOpenName,
                Note = input.Note,
                Nummber = input.Nummber,
                BorrowMoneyId = input.BorrowMoneyId
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
                    BusinessType = (int)AbpFileBusinessType.报销,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLReimbursement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLReimbursementInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
               }
			   var logModel = new CWGLReimbursement();
			   if (input.IsUpdateForChange)
			   {
					logModel = dbmodel.DeepClone<CWGLReimbursement>();
			   }
			   dbmodel.Money = input.Money;
			   dbmodel.Mode = input.Mode;
			   dbmodel.BankName = input.BankName;
			   dbmodel.CardNumber = input.CardNumber;
			   dbmodel.BankOpenName = input.BankOpenName;
			   dbmodel.Note = input.Note;
			   dbmodel.Nummber = input.Nummber;
			   dbmodel.BorrowMoneyId = input.BorrowMoneyId;
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
                    BusinessType = (int)AbpFileBusinessType.报销,
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
		private CWGLReimbursementLogDto GetChangeModel(CWGLReimbursement model)
        {
            var ret = model.MapTo<CWGLReimbursementLogDto>();
            ret.Mode_Name = Enum.GetName(typeof(MoneyMode), model.Mode);
            return ret;
        }


        public void UpdateIsPayBack(Guid flowID, string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.Get(id);
            if (!model.BorrowMoneyId.IsEmptyGuid()) {
                var borrowMoney = _borrowMoneyRepository.Get(model.BorrowMoneyId);    
                borrowMoney.IsPayBack = true;
                _borrowMoneyRepository.Update(borrowMoney);
            }
        }
    }
}