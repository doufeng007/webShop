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

namespace CWGL
{
    public class CWGLoanAppService : FRMSCoreAppServiceBase, ICWGLoanAppService
    { 
        private readonly IRepository<CWGLoan, Guid> _repository;
		private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        public CWGLoanAppService(IRepository<CWGLoan, Guid> repository
		,WorkFlowBusinessTaskManager workFlowBusinessTaskManager,IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager
        )
        {
            this._repository = repository;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
             _projectAuditManager = projectAuditManager;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<CWGLoanListOutputDto>> GetList(GetCWGLoanListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new CWGLoanListOutputDto()
                        {
                            Id = a.Id,
                            Money = a.Money,
                            Remark = a.Remark,
                            Status=a.Status,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			foreach (var item in ret){item.InstanceId = item.Id.ToString();_workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);}
            return new PagedResultDto<CWGLoanListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
		public async Task<CWGLoanOutputDto> Get(GetWorkFlowTaskCommentInput input)
		{
			var id = Guid.Parse(input.InstanceId);
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var tmp = model.MapTo<CWGLoanOutputDto>();
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.贷款申请
            });
            return tmp;
        }
        /// <summary>
        /// 添加一个CWGLoan
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLoanInput input)
        {
            var id = Guid.NewGuid();
            var newmodel = new CWGLoan()
            {
                Id = id,
                Money = input.Money,
                Remark = input.Remark
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);

            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLoan
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLoanInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
               }
			   var logModel = new CWGLoan();
			   if (input.IsUpdateForChange)
			   {
					logModel = dbmodel.DeepClone<CWGLoan>();
			   }
			   dbmodel.Money = input.Money;
			   dbmodel.Remark = input.Remark;
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
                    BusinessType = (int)AbpFileBusinessType.贷款申请,
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
		private CWGLoanLogDto GetChangeModel(CWGLoan model)
        {
            var ret = model.MapTo<CWGLoanLogDto>();
            return ret;
        }

    }
}