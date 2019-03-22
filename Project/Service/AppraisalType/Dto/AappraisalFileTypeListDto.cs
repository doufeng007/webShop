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
    [AutoMapFrom(typeof(AappraisalFileType))]
    public class AappraisalFileTypeListDto 
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public string Code_AappTypeName { get; set; }

        public string AuditRoleIds { get; set; }

        public int Sort { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
