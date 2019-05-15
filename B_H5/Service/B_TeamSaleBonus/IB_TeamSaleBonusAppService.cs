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
    public interface IB_TeamSaleBonusAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<B_TeamSaleBonusListOutputDto>> GetList(GetB_TeamSaleBonusListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<B_TeamSaleBonusOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个B_TeamSaleBonus
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateB_TeamSaleBonusInput input);



        decimal GetEffectScale(decimal amout);
    }
}