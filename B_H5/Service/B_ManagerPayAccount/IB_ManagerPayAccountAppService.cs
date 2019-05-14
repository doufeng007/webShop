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
    public interface IB_ManagerPayAccountAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<B_ManagerPayAccountListOutputDto>> GetList(GetB_ManagerPayAccountListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<B_ManagerPayAccountOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个B_ManagerPayAccount
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateB_ManagerPayAccountInput input);

		/// <summary>
        /// 修改一个B_ManagerPayAccount
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateB_ManagerPayAccountInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 账户上线、下线
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpOrDown(EntityDto<Guid> input);
    }
}