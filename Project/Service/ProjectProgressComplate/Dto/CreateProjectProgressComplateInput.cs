using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;

namespace Project
{
    [AutoMapTo(typeof(ProjectProgressComplate))]
    public class CreateProjectProgressComplateInput: CreateWorkFlowInstance
    {
        #region 表字段
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
        /// Status
        /// </summary>
        public int Status { get; set; }


        #endregion
    }
}