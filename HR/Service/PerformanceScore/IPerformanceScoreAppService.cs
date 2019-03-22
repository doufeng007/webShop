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
    public interface IPerformanceScoreAppService : IApplicationService
    {
        Task<List<PerformanceScoreTypeListOutput>> GetList();
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<PerformanceScoreOutputDto> Get(NullableIdDto<Guid> input);
		/// <summary>
        /// 修改一个PerformanceScore
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(List<UpdatePerformanceScoreInput> input);
    }
}