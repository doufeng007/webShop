using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("DispatchMessage")]
    public class DispatchMessage : IEntity<Guid>
    {
        public bool IsTransient()
        {
            return false;
        }

        [Column("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [DisplayName("ProjectId")]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 任务流程ID
        /// </summary>
        [DisplayName("TaskId")]
        public Guid TaskId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName("ProjectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 评审负责人
        /// </summary>
        [DisplayName("ProjectLeader")]
        public string ProjectLeader { get; set; }

        /// <summary>
        /// 评审人员
        /// </summary>
        [DisplayName("ProjectReviewer")]
        public string ProjectReviewer { get; set; }

        /// <summary>
        /// 来文字号
        /// </summary>
        [DisplayName("DispatchCode")]
        public string DispatchCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName("StartDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName("EndDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 审减原因
        /// </summary>
        [DisplayName("Reason")]
        public string Reason { get; set; }

        /// <summary>
        /// 附加说明
        /// </summary>
        [DisplayName("Additional")]
        public string Additional { get; set; }

        /// <summary>
        /// 项目承办单编号
        /// </summary>
        [DisplayName("ProjectUndertakeCode")]
        public string ProjectUndertakeCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("CreationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 建设单位
        /// </summary>
        [DisplayName("SendUnitName")]
        public string SendUnitName { get; set; }

    }
}