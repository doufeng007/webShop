using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Authorization.Users;

namespace Abp.WorkFlow
{
    public interface IWorkFlowWorkTaskAppService : IApplicationService
    {
        Task GetRelation();
        Task<InitWorkFlowOutput> InitWorkFlowInstanceAsync(InitWorkFlowInput input);

        InitWorkFlowOutput InitWorkFlowInstance(InitWorkFlowInput input);


        void InitWorkFlowInstanceByRole(InitWorkFlowInput input, long userId, int roleId);


        Guid InsertWorkFlowTaskForUserId(Guid flowId, string instanceId, string flowTitle, long reciveUserId);


        /// <summary>
        /// 流程todo处理前的验证
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<ExecuteWorkFlowOutput> VailitionTaskStatus(Guid taskId);
        Task UpdateOpenTaskStatus(Guid taskId);
        Task<ExecuteWorkFlowOutput> ExecuteTask(ExecuteWorkFlowInput input);
        ExecuteWorkFlowOutput ExecuteTaskSync(ExecuteWorkFlowInput input);

        [RemoteService(IsEnabled = false)]
        ExecuteWorkFlowOutput ExecuteTaskWithUser(ExecuteWorkFlowInput input, User doUser);

        Task<ExecuteWorkFlowOutput> FlowCopyForAsync(FlowCopyForInput input);

        Task UpdateTaskFiles(TaskFileInput input);
        /// <summary>
        /// 加签
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="addType"></param>
        /// <param name="writeType"></param>
        /// <param name="writeUsers"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        ExecuteWorkFlowOutput AddWrite(Guid taskID, int addType, int writeType, string writeUsers, string note);


        /// <summary>
        /// 流程自动加签
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        ExecuteWorkFlowOutput AddWriteWithFlow(Guid taskId);


        /// <summary>
        /// 终止一个任务所在的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task EndTask(EndTaskInput input);



        string GetInstanceCreatUserId(Guid flowId, string instanceId, string taskId);

        //ExecuteWorkFlowOutput FlowCopyFor(FlowCopyForInput input);

        /// <summary>
        /// 根据流程获取路由
        /// </summary>
        /// <returns></returns>
        //string GetUrlFromFlow();


        DefaultMemberModel GetDefaultMemberExecuteQuery(string sql);


        Task<PagedResultDto<WorkFlowTodoListDto>> GetTodoList(GetWorkFlowTodoListInput input);


        /// <summary>
        /// 撤回任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task WithdrawTask(Guid taskId);


        Task<PagedResultDto<WorkTaskList>> GetWorkTaskPage(GetWorkTaskListInput input);


        Task<List<WorkFlowTaskCommentDto>> GetInstanceCommentAsync(GetWorkFlowTaskCommentInput input);


        List<WorkFlowTaskCommentDto> GetInstanceComment(GetWorkFlowTaskCommentInput input);
        List<WorkFlowTaskStepFileResult> GetInstanceFiles(GetWorkFlowTaskCommentInput input);
        List<WorkFlowTaskStepComentResult> GetCurrentUserComents(GetWorkFlowTaskCommentInput input);
        /// <summary>
        /// 更新打开时间；点击处理链接后是否改变task状态为1； 项目评审 oa任务执行  不需要更改；其它需要改
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId">步骤id</param>
        Task UpdateOpenTime(Guid taskId, Guid flowId, Guid stepId);

        ExecuteWorkFlowOutput FlowCopyFor(FlowCopyForInput input);
        Task<PagedResultDto<FlowInquiryOutput>> GetFlowInquiryList(GetFlowInquiryInput input);

    }

}
