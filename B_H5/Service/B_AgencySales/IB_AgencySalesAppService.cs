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
    public interface IB_AgencySalesAppService : IApplicationService
    {

        /// <summary>
        /// 获取类别下的销售额明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<B_AgencySalesCategroyListOutputDto>> GetListByCategroyId(GetB_AgencyCategroySalesListInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<B_AgencySalesListOutputDto>> GetList(GetB_AgencySalesListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<B_AgencySalesOutputDto> Get(GetB_AgencySalesInput input);

        /// <summary>
        /// 添加一个B_AgencySales
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Create(CreateB_AgencySalesInput input);

		/// <summary>
        /// 修改一个B_AgencySales
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateB_AgencySalesInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}