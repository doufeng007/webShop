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
    [AutoMapFrom(typeof(ArchivesManager))]
    public class ArchivesManagerListOutputDto: BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string ArchivesName { get; set; }

        public Guid? ProjectId { get; set; }


        public int ArchivesType { get; set; }

        public string ProjectName { get; set; }

        public string ProjectCode { get; set; }

        public string SingleProjectName { get; set; }


        public string SingleProjectCode { get; set; }

        public int? SecrecyLevel { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

    }
}
