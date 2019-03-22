using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZCYX.FRMSCore.Application
{
    [Table("ProjectAudit")]
    public class ProjectAudit : FullAuditedEntity<Guid>
    {
        public bool IsTransient()
        {
            return false;
        }

        [Column("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 工作记录ID
        /// </summary>
        [DisplayName("TaskId")]
        public Guid TaskId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [DisplayName("InstanceId")]
        public string InstanceId { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [DisplayName("FieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// 字段老值
        /// </summary>
        [DisplayName("OldValue")]
        public string OldValue { get; set; }

        /// <summary>
        /// 字段新值
        /// </summary>
        [DisplayName("NewValue")]
        public string NewValue { get; set; }

        


        public int? ChangeType { get; set; }


        public string TableName { get; set; }



        public Guid GroupId { get; set; }
    }
}