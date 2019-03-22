using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;

namespace TaskGL
{
    public interface ITaskManagementAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<TaskManagementListOutputDto>> GetList(GetTaskManagementListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<TaskManagementOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个TaskManagement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateTaskManagementInput input);

		/// <summary>
        /// 修改一个TaskManagement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateTaskManagementInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);


        [RemoteService(IsEnabled = false)]
        void UpdateTaskStatus(Guid id, TaskManagementStateEnum status);

        /// <summary>
        /// 当前登录用户是否是领导
        /// </summary>
        /// <returns></returns>
        bool IsLeader();


        /// <summary>
        /// 更新任务工作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SaveRecore(CreateOrUpdateTaskManagementRecordInput input);

        /// <summary>
        /// 获取任务工作记录
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <returns></returns>
        Task<TaskManagementRecordOutput> GetRecordByTaskId(Guid id);




        /// <summary>
        /// 自动加签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<string> AutoAddWriteForTask(Guid id, Guid taskId);

        [RemoteService(IsEnabled = false)]
        void CreateComplete(Guid id);

        /// <summary>
        /// 查找审批人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        string FindAuditUser(Guid id, int actionType);

    }
}