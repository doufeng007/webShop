using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace TaskGL
{
    public interface ITaskManagementChangeAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<TaskManagementChangeListOutputDto>> GetList(GetTaskManagementChangeListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<TaskManagementChangeOutputDto> Get(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加一个TaskManagementChange
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<Guid> Create(CreateTaskManagementChangeInput input);

		/// <summary>
        /// 修改一个TaskManagementChange
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateTaskManagementChangeInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}