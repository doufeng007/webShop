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
    public interface IPerformanceAppealAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<PerformanceAppealListOutputDto>> GetList(GetPerformanceAppealListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        PerformanceAppealOutputDto Get(GetWorkFlowTaskCommentInput input);

        /// <summary>
        /// 添加一个PerformanceAppeal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreatePerformanceAppealInput input);

        /// <summary>
        /// 修改一个PerformanceAppeal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdatePerformanceAppealInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		void Delete(EntityDto<Guid> input);
    }
}