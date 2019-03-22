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
    [AutoMapFrom(typeof(OABidProject))]
    public class OABidProjectListDto
    {
        public Guid Id { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string BidderName { get; set; }

        public string BuildUnit { get; set; }

        public DateTime WriteDate { get; set; }


        public int Status { get; set; }



        public DateTime CreationTime { get; set; }

        public string StatusTitle { get; set; }

    }
}
