using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.WorkFlow
{
    [Table("WorkFlowForm")]
    public class WorkFlowForm : IEntity<Guid>
    {
        public bool IsTransient()
        {
            return false;
        }

        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [Column("Id")]
        public Guid Id { get; set; }


        /// <summary>
        /// 表单名称
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 表单分类
        /// </summary>
        [DisplayName("Type")]
        public Guid Type { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [DisplayName("CreateUserID")]
        public long CreateUserID { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DisplayName("CreateUserName")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DisplayName("LastModifyTime")]
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// 表单html
        /// </summary>
        [DisplayName("Html")]
        public string Html { get; set; }

        /// <summary>
        /// 从表设置数据
        /// </summary>
        [DisplayName("SubTableJson")]
        public string SubTableJson { get; set; }

        /// <summary>
        /// 事件设置
        /// </summary>
        [DisplayName("EventsJson")]
        public string EventsJson { get; set; }

        /// <summary>
        /// 相关属性
        /// </summary>
        [DisplayName("Attribute")]
        public string Attribute { get; set; }

        /// <summary>
        /// 状态：0 保存 1 编译 2作废
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }

        #endregion



    }
}