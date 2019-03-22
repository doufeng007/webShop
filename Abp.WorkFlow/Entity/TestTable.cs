using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Abp.WorkFlow.Entity
{
    [Table("TestTable")]
    public class TestTable : IEntity<Guid>
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
        public string Name { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int Status { get; set; }

        #endregion

    }
}
