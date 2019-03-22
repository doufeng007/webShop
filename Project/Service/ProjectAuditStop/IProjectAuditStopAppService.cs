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
    public interface IProjectAuditStopAppService : IApplicationService
    {

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<GetProjectAuditStopForEidtOutput> GetProjectForAuditStop(GetProjectForAuditStopInput input);


        /// <summary>
        /// 停滞申请发起Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> CreateStopAsync(CreateProjectAuditStopInput input);


        /// <summary>
        /// 编辑api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateStopAsync(UpdateProjectAuditStopInput input);


        Task<SubmitProjectRelieveOutput> SubmitProjectRelieve(EntityDto<Guid> input);


        bool GetStopIsCreateByProjecetLeader(Guid taskId);

        [RemoteService(IsEnabled = false)]
        string GetProjectLeaderForStop(Guid instanceId);


        [RemoteService(IsEnabled = false)]
        void RejectProjectStop(Guid instanceId);


        [RemoteService(IsEnabled = false)]
        void ReturnProjectRelieve(Guid instanceId);


        [RemoteService(IsEnabled = false)]
        void CompleteRejectProject(Guid instanceId);

        [RemoteService(IsEnabled = false)]
        void PassProjectStop(Guid instanceId);


    }

}
