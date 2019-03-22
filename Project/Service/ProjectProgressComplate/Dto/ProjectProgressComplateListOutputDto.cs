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
    [AutoMapFrom(typeof(ProjectProgressComplate))]
    public class ProjectProgressComplateListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid ProjectBaseId { get; set; }

        /// <summary>
        /// FirstAuditComplateTime
        /// </summary>
        public DateTime? FirstAuditComplateTime { get; set; }

        /// <summary>
        /// FirstAduitDelayHour
        /// </summary>
        public int? FirstAduitDelayHour { get; set; }

        /// <summary>
        /// JiliangComplateTime
        /// </summary>
        public DateTime? JiliangComplateTime { get; set; }

        /// <summary>
        /// JiliangDelayHour
        /// </summary>
        public int? JiliangDelayHour { get; set; }

        /// <summary>
        /// JijiaComplateTime
        /// </summary>
        public DateTime? JijiaComplateTime { get; set; }

        /// <summary>
        /// JijiaDelayHour
        /// </summary>
        public int? JijiaDelayHour { get; set; }

        /// <summary>
        /// SelfAuditComplateTime
        /// </summary>
        public DateTime? SelfAuditComplateTime { get; set; }

        /// <summary>
        /// SelfAuditDelayHour
        /// </summary>
        public int? SelfAuditDelayHour { get; set; }

        /// <summary>
        /// SecondAuditComplateTime
        /// </summary>
        public DateTime? SecondAuditComplateTime { get; set; }

        /// <summary>
        /// SecondAuditDelayHour
        /// </summary>
        public int? SecondAuditDelayHour { get; set; }

        /// <summary>
        /// LastAuditComplateTime
        /// </summary>
        public DateTime? LastAuditComplateTime { get; set; }

        /// <summary>
        /// LastAuditDelayHour
        /// </summary>
        public int? LastAuditDelayHour { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }


    }
}
