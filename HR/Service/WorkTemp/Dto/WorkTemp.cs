using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using HR.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public class WorkTemp
    {

        public Guid Id { get; set; }
        public WorkTempType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PostIds { get; set; }
        public long OrgId { get; set; }
        public long CreatorUserId { get; set; }
        public string DealWithUsers { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public decimal Hours { get; set; }

    }
}
