using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(AappraisalFileType))]
    public class GetAappraisalFileTypeForEditOutput
    {

        public int? Id { get; set; }
        public string Name { get; set; }


        public int AppraisalTypeId { get; set; }


        public bool IsPaperFile { get; set; }

        public bool IsMust { get; set; }


        public string AuditRoleIds { get; set; }


        public int Sort { get; set; }
    }


}
