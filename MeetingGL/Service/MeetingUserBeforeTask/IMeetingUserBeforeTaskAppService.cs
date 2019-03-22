using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace MeetingGL
{
    public interface IMeetingUserBeforeTaskAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<MeetingUserBeforeTaskListOutputDto>> GetList(GetMeetingUserBeforeTaskListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<MeetingUserBeforeTaskOutputDto> Get(GetWorkFlowTaskCommentInput input);

        /// <summary>
        /// 添加一个MeetingUserBeforeTask
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateMeetingUserBeforeTaskInput input);


        InitWorkFlowOutput CreateSelf(CreateMeetingUserBeforeTaskInput input);

        /// <summary>
        /// 修改一个MeetingUserBeforeTask
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateMeetingUserBeforeTaskInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 会议终止 管理员作废会前任务待办
        /// </summary>
        [RemoteService(IsEnabled = false)]
        void AutoInvalidBeforeTaskTodos(Guid meetingId);
    }
}