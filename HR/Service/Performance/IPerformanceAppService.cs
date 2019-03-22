using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    public interface IPerformanceAppService : IApplicationService
    {	

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<PerformanceOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个Performance
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreatePerformanceInput input);
        Task<List<PerformanceDataListOutputDto>> GetDataList(GetPerformanceListInput input);
        Task<PagedResultDto<PerformanceListOutputDto>> GetNoDataList(GetPerformanceListInput input);
    }
}