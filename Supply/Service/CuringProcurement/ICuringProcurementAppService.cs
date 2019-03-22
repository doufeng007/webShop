using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    public interface ICuringProcurementAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<CuringProcurementListOutputDto>> GetList(GetCuringProcurementListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<CuringProcurementOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个CuringProcurement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateCuringProcurementInput input);

		/// <summary>
        /// 修改一个CuringProcurement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateCuringProcurementInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);

        [RemoteService(IsEnabled = false)]
        void UpdateToChange(Guid id);


        /// <summary>
        /// 固化采购入库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PutIn(RegisterSupplyPurchasePlanInput input);
    }
}