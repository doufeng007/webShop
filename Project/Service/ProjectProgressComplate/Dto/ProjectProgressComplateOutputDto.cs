using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMap(typeof(ProjectProgressComplate))]
    public class ProjectProgressComplateOutputDto
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

        public List<ProjectProgressFaultDto> ProjectProgressFault { get; set; }
    }

    [AutoMap(typeof(ProjectProgressFault))]
    public class ProjectProgressFaultDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        ///项目进度
        /// </summary>
        public Guid ProgressComplate { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus FaultType { get; set; }
        [Required(AllowEmptyStrings =true)]
        [MaxLength(200,ErrorMessage = "输入的内容不能超过200字符")]
        public string Content { get; set; }
    }

    public class FinishAndSendInput
    {
        public Guid FlowId { get; set; }

        public Guid StepId { get; set; }

        public string InstanceId { get; set; }

        public Guid GroupId { get; set; }

        public Guid TaskId { get; set; }
    }
}
