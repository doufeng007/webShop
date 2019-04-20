using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    public interface IB_WithdrawalAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<B_WithdrawalListOutputDto>> GetList(GetB_WithdrawalListInput input);


        Task<B_WithdrawalCount> GetCount();


        Task<B_AgencyApplyCount> GetAuditCount();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<B_WithdrawalOutputDto> Get(EntityDto<Guid> input);

		/// <summary>
        /// 添加一个B_Withdrawal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateB_WithdrawalInput input);

		/// <summary>
        /// 修改一个B_Withdrawal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateB_WithdrawalInput input);

        /// <summary>
        /// 审核提现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Audit(AuditB_WithdrawalInput input);


        /// <summary>
        /// 打款
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Remit(RemitInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);
    }
}