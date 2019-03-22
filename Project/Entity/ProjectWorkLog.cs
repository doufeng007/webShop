using Abp.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("ProjectWorkLog")]
    public class ProjectWorkLog : IEntity<Guid>
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
        [DisplayName("ProjectId")]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName("Content")]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("CreationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        [DisplayName("WorkTime")]
        public DateTime WorkTime { get; set; }

        public int? LogType { get; set; }

        public string Files { get; set; }

        public Guid? StepId { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }
    }
}