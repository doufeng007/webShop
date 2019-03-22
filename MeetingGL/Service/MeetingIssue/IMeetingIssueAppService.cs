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
    public interface IMeetingIssueAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<MeetingIssueListOutputDto>> GetList(GetMeetingIssueListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<MeetingIssueOutputDto> Get(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加一个MeetingIssue
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<MeetingIssueOutputDto> Create(CreateMeetingIssueInput input);


        MeetingIssueOutputDto CreateSelf(CreateMeetingIssueInput input);

        /// <summary>
        /// 修改一个MeetingIssue
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateMeetingIssueInput input);


        void UpdateSelf(UpdateMeetingIssueInput input);


        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        [RemoteService(false)]
        void ChangeIssueProjectLeader(Guid singleProjectId, long userId);

        [RemoteService(false)]
        void UpdateNesIssueSatatus(Guid meetingId);
    }
}