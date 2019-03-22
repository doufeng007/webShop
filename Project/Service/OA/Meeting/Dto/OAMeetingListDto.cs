using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(OAMeeting))]
    public class OAMeetingListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }

        public string Title { get; set; }


    }
}
