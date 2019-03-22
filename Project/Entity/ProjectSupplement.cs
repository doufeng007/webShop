using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("ProjectSupplement")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectSupplement : FullAuditedEntity<Guid>
    {
        #region 表字段
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectBaseId")]
        public Guid ProjectBaseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("TableKey")]
        public string TableKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("TableName")]
        public string TableName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ColumnKey")]
        public string ColumnKey { get; set; }

        [DisplayName("ColumnName")]
        public string ColumnName { get; set; }


        [DisplayName("Value")]
        public string Value { get; set; }


        [DisplayName("HasSupplement")]
        public bool HasSupplement { get; set; }

        public Guid? RelationId { get; set; }



        /// <summary>
        /// 1 为表字段缺少 2为表内容一条数据的某一字段缺少 此时RelationId不为空
        /// </summary>
        public int Type { get; set; }


        #endregion

    }


}
