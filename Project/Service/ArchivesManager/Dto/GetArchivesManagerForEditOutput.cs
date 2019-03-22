using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMap(typeof(ArchivesManager))]
    public class GetArchivesManagerForEditOutput : CreateOrUpdateArchivesManagerInput
    {
        public string ProjectName { get; set; }

        public string ProjectCode { get; set; }


        public string SingleProjectName { get; set; }


        public string SingleProjectCode { get; set; }




        public int AppraisalTypeId { get; set; }

    }


}
