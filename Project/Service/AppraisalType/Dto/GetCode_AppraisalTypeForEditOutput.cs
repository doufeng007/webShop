using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(Code_AppraisalType))]
    public class GetCode_AppraisalTypeForEditOutput
    {

        public int? Id { get; set; }
        public string Name { get; set; }

        public int ParentId { get; set; }

        public int? Sort { get; set; }
    }


}
