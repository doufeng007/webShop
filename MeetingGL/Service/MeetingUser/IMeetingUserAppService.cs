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
    public interface IMeetingUserAppService : IApplicationService
    {	

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<MeetingUserOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个MeetingUser
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateMeetingUserInput input);

        InitWorkFlowOutput CreateSelf(CreateMeetingUserInput input);

        /// <summary>
        /// 修改一个MeetingUser
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateMeetingUserInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 查询会议的发起者
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        string GetMeetingCreateUserForReturnReceipt(Guid instanceId);


        string GetMeetingCreateUser(Guid meetingId);


        /// <summary>
        /// 会议确认 管理员作废会议回执的处理中的待办
        /// </summary>
        [RemoteService(IsEnabled = false)]
        void AutoInvalidMeetingUserReturnReceiptTodos(Guid meetingId, bool isSendMessage = false);
    }
}