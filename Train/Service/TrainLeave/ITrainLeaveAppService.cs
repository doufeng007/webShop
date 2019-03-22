using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Train
{
    public interface ITrainLeaveAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<TrainLeaveListOutputDto>> GetList(GetTrainLeaveListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<TrainLeaveOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个TrainLeave
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateTrainLeaveInput input);

		/// <summary>
        /// 修改一个TrainLeave
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateTrainLeaveInput input);


        [RemoteService(false)]
        string TrainLeaveFlowActive(Guid instanceId);
        bool TrainLeaveVerification(Guid instanceId);
        string GetTrainLeaveAuditUser(Guid instanceId);
    }
}