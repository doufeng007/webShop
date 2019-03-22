using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace MeetingGL
{
    public class MeetingIssueProjectLeaderHandler : IEventHandler<ProjectLeaderChange>, ISingletonDependency
    {
        private readonly IMeetingIssueAppService _meetingIssueAppServiceRepository;
        public MeetingIssueProjectLeaderHandler(IMeetingIssueAppService meetingIssueAppServiceRepository)
        {
            _meetingIssueAppServiceRepository = meetingIssueAppServiceRepository;
        }

        public void HandleEvent(ProjectLeaderChange eventData)
        {
            _meetingIssueAppServiceRepository.ChangeIssueProjectLeader(eventData.ProjectId, eventData.UserId);

        }
    }
}
