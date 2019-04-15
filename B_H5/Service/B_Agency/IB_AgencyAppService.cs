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
    public interface IB_AgencyAppService : IApplicationService
    {	
	    /// <summary>
        /// 微信端-渠道列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<B_AgencyListOutputDto>> GetList(GetB_AgencyListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<B_AgencyOutputDto> Get(EntityDto<Guid> input);

		/// <summary>
        /// 添加一个B_Agency
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateB_AgencyInput input);

		/// <summary>
        /// 修改一个B_Agency
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateB_AgencyInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 封停代理
        /// </summary>
        /// <param name="input"></param>
        Task Disable(EntityDto<Guid> input);
    }
}