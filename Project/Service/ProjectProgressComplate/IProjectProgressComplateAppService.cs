using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectProgressComplateAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<ProjectProgressComplateListOutputDto>> GetList(GetProjectProgressComplateListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<ProjectProgressComplateOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个ProjectProgressComplate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateProjectProgressComplateInput input);

		/// <summary>
        /// 修改一个ProjectProgressComplate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(ProjectProgressComplateOutputDto input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
        /// <summary>
        /// 时间到后更改待办状态为已读
        /// </summary>
        /// <param name="projectId"></param>
        void TimeOut(Guid instancId, Guid taskId);
        /// <summary>
        /// 创建待办并发送到下一步
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task Send(Guid projectId);

        Task FinishAndSend(FinishAndSendInput input);
    }
}