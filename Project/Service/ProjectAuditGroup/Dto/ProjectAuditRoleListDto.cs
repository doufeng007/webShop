using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(ProjectAuditGroup))]
    public class ProjectAuditGroupListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

    }


   
}
