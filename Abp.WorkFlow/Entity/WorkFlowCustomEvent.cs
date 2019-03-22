using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.WorkFlow
{
    [Table("WorkFlowCustomEvent")]
    public class WorkFlowCustomEvent : IEntity<Guid>
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


        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 表单名称
        /// </summary>
        [DisplayName("ProcedureName")]
        public string ProcedureName { get; set; }

        #endregion



    }
}