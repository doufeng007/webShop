using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using ZCYX.FRMSCore;

namespace GWGL
{
    public interface IGW_SealAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<GW_SealListOutputDto>> GetList(GetGW_SealListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<GW_SealOutputDto> Get(EntityDto<Guid> input);

		/// <summary>
        /// 添加一个GW_Seal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateGW_SealInput input);

		/// <summary>
        /// 修改一个GW_Seal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateGW_SealInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);


        Task<List<ChangeLog>> GetChangeLogList(EntityDto<Guid> input);
    }
}