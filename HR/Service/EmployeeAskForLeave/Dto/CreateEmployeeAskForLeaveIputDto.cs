using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    [AutoMapTo(typeof(EmployeeAskForLeave))]
    public class CreateEmployeeAskForLeaveIputDto : CreateWorkFlowInstance
    {

        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }


        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        public decimal? Hours { get; set; }

        public long? RelationUserId { get; set; }
        public long OrgId { get; set; }

    }


    public class UpdateEmployeeAskForLeaveIputDto : CreateEmployeeAskForLeaveIputDto
    {
        public Guid Id { get; set; }


        public bool IsUpdateForChange { get; set; }
    }
}
