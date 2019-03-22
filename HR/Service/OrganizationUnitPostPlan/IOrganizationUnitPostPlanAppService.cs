using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IOrganizationUnitPostPlanAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<OrganizationUnitPostPlanListOutputDto>> GetList(GetOrganizationUnitPostPlanListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<OrganizationUnitPostPlanOutputDto> Get(GetOrgPostPlanInput input);

		/// <summary>
        /// 添加一个OrganizationUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateOrganizationUnitPostPlanInput input);

		/// <summary>
        /// 修改一个OrganizationUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateOrganizationUnitPostPlanInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 计划通过后 更新编制
        /// </summary>
        /// <param name="instanceId"></param>
        void CompletaPlan(string instanceId);


        /// <summary>
        /// 整改编制的完成事件
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(IsEnabled = false)]
        void CompletaChangePlan(string instanceId);
    }
}