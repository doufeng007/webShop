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
    public interface IWorkRecordAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<WorkRecordListOutputDto>> GetList(GetWorkRecordListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<WorkRecordOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个WorkRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateWorkRecordInput input);

		/// <summary>
        /// 修改一个WorkRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateWorkRecordInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}