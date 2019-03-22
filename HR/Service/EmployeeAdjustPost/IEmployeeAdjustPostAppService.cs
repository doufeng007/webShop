using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using HR.Service.EmployeeAdjustPost.Dto;

namespace HR
{
    public interface IEmployeeAdjustPostAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<EmployeeAdjustPostListOutputDto>> GetList(GetEmployeeAdjustPostListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<EmployeeAdjustPostOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个EmployeeAdjustPost
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateEmployeeAdjustPostInput input);

		/// <summary>
        /// 修改一个EmployeeAdjustPost
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateEmployeeAdjustPostInput input);
        /// <summary>
        /// 调岗成功后进行调岗事件处理
        /// </summary>
        /// <returns></returns>
        bool AdjustPostRun(Guid Id);
        bool EmployeeAdjustPostIsLeader(Guid guid);
    }
}