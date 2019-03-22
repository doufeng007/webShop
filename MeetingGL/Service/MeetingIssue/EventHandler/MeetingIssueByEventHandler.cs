using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Abp.File;
using Abp.Runtime.Caching;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using ZCYX.FRMSCore;

namespace MeetingGL
{
    public class MeetingIssueByEventHandler : IEventHandler<MeetingIssueByEvent>, ISingletonDependency
    {
        private readonly IMeetingIssueAppService _iDocmentAppServiceRepository;
        public MeetingIssueByEventHandler(IMeetingIssueAppService iDocmentAppServiceRepository)
        {
            _iDocmentAppServiceRepository = iDocmentAppServiceRepository;
        }

        public void HandleEvent(MeetingIssueByEvent model)
        {
            _iDocmentAppServiceRepository.CreateSelf(new CreateMeetingIssueInput() {
                Name = model.Name,
                OrgId = model.OrgId,
                UserId = model.UserId,
                Content = model.Content,
                RelationProposalId = model.RelationProposalId,
                SingleProjectId = model.SingleProjectId,
                IssueType = (IssueType)model.IssueType
            });
        }
    }
}
