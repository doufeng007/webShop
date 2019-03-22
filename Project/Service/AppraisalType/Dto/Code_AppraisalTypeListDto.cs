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
    [AutoMapFrom(typeof(Code_AppraisalType))]
    public class Code_AppraisalTypeListDto : FullAuditedEntity
    {
        public string Name { get; set; }

        public string ParentName { get; set; }


        public int ParentId { get; set; }

        public int Sort { get; set; }
    }
}
