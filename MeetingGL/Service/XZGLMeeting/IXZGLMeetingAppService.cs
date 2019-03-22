using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace MeetingGL
{
    public interface IXZGLMeetingAppService : IApplicationService
    {
        //List<ExpandoObject> GetMeetingType(string value = null, string setEmpty = null);
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<XZGLMeetingListOutputDto>> GetList(GetXZGLMeetingListInput input);

        Task<PagedResultDto<XZGLMeetingListOutputDto>> GetMyList(GetXZGLMeetingListInput input);
        /// <summary>
        /// 周期会议-列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<XZGLMeetingPeriodListOutputDto>> GetPeriodList(GetXZGLMeetingListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<XZGLMeetingOutputDto> Get(GetWorkFlowTaskCommentInput input);



        Task<XZGLMeetingOutputDto> GetForViewAsync(EntityDto<Guid> input);


        XZGLMeetingOutputDto GetForView(EntityDto<Guid> input);
        /// <summary>
        /// 周期会议取消
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdatePeriodClear(PeriodClearInput input);
        /// <summary>
        /// 添加一个XZGLMeeting
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateXZGLMeetingInput input);

        /// <summary>
        /// 修改一个XZGLMeeting
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateXZGLMeetingInput input);

        /// <summary>
        /// 检查当前会议人员权限
        /// </summary>
        /// <returns></returns>
        bool CheckPostFromUser(Guid meetingId, int type);

        /// <summary>
        /// 抄送参会人员
        /// </summary>
        /// <param name="Id"></param>
        void NotifyMeeting(WorkFlowCustomEventParams eventParams);


        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 删除周期会议
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeletePeriodMeeting(EntityDto<Guid> input);

        [RemoteService(false)]
        void CreateMeetingBeforeTask(Guid instanceId);

        /// <summary>
        /// 提交会议记录-获取
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        Task<GetViewForRecodeOutput> GetViewForRecord(Guid instanceId);


        /// <summary>
        /// 提交会议记录-保存
        /// </summary>
        /// <param name="input"></param>
        void SubmitRecord(SubmitRecordInput input);


        Task<InitWorkFlowOutput> CreatePeriod(CreateXZGLMeetingInput input);


        //Task UpdatePeriod(UpdateXZGLMeetingInput input);


        //[RemoteService(IsEnabled = false)]
        void CreatePeriodSelf(Guid id, Guid flowId);

        [RemoteService(IsEnabled = false)]
        void SendMessageForJoinUser(Guid guid);

        [RemoteService(IsEnabled = false)]
        void CreatePeriodSelfForDay(Guid id, Guid flowId);

        [RemoteService(IsEnabled = false)]
        void StopMeeting(Guid instanceId);

        [RemoteService(IsEnabled = false)]
        void CreatePeriodJobForCycleForDay(Guid flowId, DateTime doTime);

        /// <summary>
        /// 会议确认
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        string GetMeetingConfirmUser(Guid instanceId);

        /// <summary>
        /// 会议记录人
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        string GetMeetingRecodeUser(Guid instanceId);

        /// <summary>
        /// 终止会议时，清空会议室的占用
        /// </summary>
        /// <param name="meetingId"></param>
        [RemoteService(IsEnabled = false)]
        void EndMeetingRoomUse(Guid meetingId);


    }
}