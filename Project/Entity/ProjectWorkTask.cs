using Abp.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("ProjectWorkTask")]
    public class ProjectWorkTask : IEntity<Guid>
    {
        public bool IsTransient()
        {
            return false;
        }

        [Column("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 记录类型
        /// 1.工作底稿  2.数据修改（变更） 3.工作联系  4.意见征询 5.发文记录  6 退回审核  7 补充数据
        /// </summary>
        [DisplayName("TaskType")]
        public int TaskType { get; set; }

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
        [MaxLength(300,ErrorMessage ="标题不能超过300字。")]
        public string Title { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("CreationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 阶段ID
        /// </summary>
        [DisplayName("StepId")]
        public Guid? StepId { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        [DisplayName("StepName")]
        public string StepName { get; set; }

        public Guid? InstanceId { get; set; }



    }
}