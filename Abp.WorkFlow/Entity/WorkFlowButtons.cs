using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Abp.WorkFlow.Entity
{
    [Table("WorkFlowButtons")]
    public class WorkFlowButtons : IEntity<Guid>
    {
        public bool IsTransient()
        {
            return false;
        }

        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [DisplayName("图标")]
        public string Ico { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        [DisplayName("脚本")]
        public string Script { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        [DisplayName("备注说明")]
        public string Note { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int Sort { get; set; }

        #endregion

    }
}
