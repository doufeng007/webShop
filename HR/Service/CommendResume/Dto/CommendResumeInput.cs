using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(CommendResume))]
    public class CommendResumeInput: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public string Reason { get; set; }

        public string Source { get; set; }

        public string OnlineUrl { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Guid? Job { get; set; }

        public int Status { get; set; }
        public int? TenantId { get; set; }
    }
}
