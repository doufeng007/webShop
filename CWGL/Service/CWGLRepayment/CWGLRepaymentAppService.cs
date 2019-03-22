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
using CWGL.Enums;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Domain.Uow;

namespace CWGL
{
    public class CWGLRepaymentAppService : FRMSCoreAppServiceBase, ICWGLRepaymentAppService
    { 
        private readonly IRepository<CWGLRepayment, Guid> _repository;
		private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        private readonly IRepository<CWGLBorrowMoney, Guid> _borrowMoneyRepository;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public CWGLRepaymentAppService(IRepository<CWGLRepayment, Guid> repository
		,WorkFlowBusinessTaskManager workFlowBusinessTaskManager,IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository, IRepository<CWGLBorrowMoney, Guid> borrowMoneyRepository, IRepository<User, long> usersRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IUnitOfWorkManager unitOfWorkManager
        )
        {
            this._repository = repository;
			_workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
             _projectAuditManager = projectAuditManager;
             _workFlowCacheManager = workFlowCacheManager;
             _workFlowTaskRepository = workFlowTaskRepository;
            _borrowMoneyRepository = borrowMoneyRepository;
            _usersRepository = usersRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<BorrowMoneyOutputDto>> GetRepaymentList(GetRepaymentListInput input)
        {
            var queryBase = _borrowMoneyRepository.GetAll().Where(x => !x.IsDeleted && x.TypeId == BorrowMoney.普通借款);
            if (input.GetMy)
            {
                queryBase = queryBase.Where(r => r.CreatorUserId == AbpSession.UserId.Value);
            }
            else
            {
                queryBase = queryBase.Where(a => a.CreatorUserId.Value == AbpSession.UserId.Value || a.DealWithUsers.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : ""));
            }
            var query = from a in queryBase
                        join b in _usersRepository.GetAll() on a.UserId equals b.Id
                        let c =_repository.GetAll().Where(x=>x.BorrowMoneyId==a.Id && x.Status==-1).Sum(x=>x.Money)
                        select new BorrowMoneyOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            OrgId = a.OrgId,
                            Status = c==a.Money?"已完成":"未完成",
                            Money = a.Money,
                            RepayMentMoney = c,
                            UserName = b.Name,
                            Type = "普通借款"
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.UserName.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                if (ret.Count > 0)
                {
                    foreach (var tmp in ret)
                    {
                        var orgModel = await _organizationUnitRepository.GetAsync(tmp.OrgId);
                        tmp.DepartmentName = orgModel.DisplayName;
                    }
                }
            }
            return new PagedResultDto<BorrowMoneyOutputDto>(toalCount, ret);
        }
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<CWGLRepaymentListOutputDto>> GetList(GetCWGLRepaymentListInput input)
        {
            var strflowid = input.FlowId.ToString();
            var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.BorrowMoneyId==input.BorrowMoneyId)
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                           x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                           x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                         select c).Any()
                        select new CWGLRepaymentListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            BorrowMoneyId = a.BorrowMoneyId,
                            Money = a.Money,
                            Mode = a.Mode,
                            BankName = a.BankName,
                            CardNumber = a.CardNumber,
                            BankOpenName = a.BankOpenName
							,OpenModel = openModel ? 1 : 2,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			foreach (var item in ret){item.InstanceId = item.Id.ToString();_workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);}
            return new PagedResultDto<CWGLRepaymentListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
		public async Task<CWGLRepaymentOutputDto> Get(GetWorkFlowTaskCommentInput input)
		{
			var id = Guid.Parse(input.InstanceId);
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<CWGLRepaymentOutputDto>();
		}
		/// <summary>
        /// 添加一个CWGLRepayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
		public async Task<InitWorkFlowOutput> Create(CreateCWGLRepaymentInput input)
        {
            var borrowMoney = _borrowMoneyRepository.Get(input.BorrowMoneyId);
            var money = _repository.GetAll().Where(x => !x.IsDeleted && x.BorrowMoneyId == input.BorrowMoneyId && x.Status!=-2).Sum(x=>x.Money);
            if(money + input.Money > borrowMoney.Money)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "还款金额不能大于借款金额！");

            var newmodel = new CWGLRepayment()
                {
                    BorrowMoneyId = input.BorrowMoneyId,
                    Money = input.Money,
                    Mode = input.Mode,
                    BankName = input.BankName,
                    CardNumber = input.CardNumber,
                    BankOpenName = input.BankOpenName
		        };
				newmodel.Status = 0; 
                await _repository.InsertAsync(newmodel);
				return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLRepayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLRepaymentInput input)
        {
		    if (input.InStanceId != Guid.Empty)
            {
                var borrowMoney = _borrowMoneyRepository.Get(input.BorrowMoneyId);
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.InStanceId);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
                var money = _repository.GetAll().Where(x => !x.IsDeleted && x.BorrowMoneyId == input.BorrowMoneyId && x.Status == -1).Sum(x => x.Money) ;
                if (money - dbmodel.Money + input.Money > borrowMoney.Money)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "还款金额不能大于借款金额！");

                var logModel = new CWGLRepayment();
			   if (input.IsUpdateForChange)
			   {
					logModel = dbmodel.DeepClone<CWGLRepayment>();
			   }
			   dbmodel.BorrowMoneyId = input.BorrowMoneyId;
			   dbmodel.Money = input.Money;
			   dbmodel.Mode = input.Mode;
			   dbmodel.BankName = input.BankName;
			   dbmodel.CardNumber = input.CardNumber;
			   dbmodel.BankOpenName = input.BankOpenName;
                input.FACData.BusinessId = input.InStanceId.ToString();
               await _repository.UpdateAsync(dbmodel);
                var groupId = Guid.NewGuid();
                input.FACData.GroupId = groupId;
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));                  
                    await _projectAuditManager.InsertAsync(logs, input.InStanceId.ToString(), flowModel.TitleField.Table, groupId);
                }
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		private CWGLRepaymentLogDto GetChangeModel(CWGLRepayment model)
        {
            var ret = model.MapTo<CWGLRepaymentLogDto>();
            ret.Mode = model.Mode.ToString();
            return ret;
        }
    }
}