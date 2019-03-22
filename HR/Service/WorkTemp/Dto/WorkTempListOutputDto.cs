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
    public class WorkTempListOutputDto : BusinessWorkFlowListOutput
    {

        public Guid Id { get; set; }
        public WorkTempType Type { get; set; }
        public string TypeName { get; set; }
        public long CreatorUserId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DepartmentName { get; set; }
        public long OrgId { get; set; }
        public List<WorkTempPost> Post { get; set; } = new List<WorkTempPost>();
        public string Posts { get; set; }
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }
        public decimal Hours { get; set; }
    }
    public class WorkTempPost
    {
        public Guid PostId { get; set; }
        public string PostName { get; set; }
    }
}
