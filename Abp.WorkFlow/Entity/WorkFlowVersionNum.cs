using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.WorkFlow
{
    [Table("WorkFlowVersionNum")]
    public class WorkFlowVersionNum : Entity<Guid>, IMayHaveTenant
    {

        #region 表字段



        public int? TenantId { get; set; }

        /// <summary>
        /// 流程Id
        /// </summary>
        [DisplayName("FlowId")]
        public Guid FlowId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [DisplayName("VersionNum")]
        public int VersionNum { get; set; }

        /// <summary>
        /// 流程内容
        /// </summary>
        [DisplayName("RunJSON")]
        public string RunJSON { get; set; }


        #endregion


    }
}