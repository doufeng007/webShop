using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;

namespace Train
{
    [Serializable]
    [Table("TrainLogistics")]
    public class TrainLogistics : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 培训编号
        /// </summary>
        [DisplayName(@"培训编号")]
        public Guid 培训编号 { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        [DisplayName(@"类型名")]
        [MaxLength(20)]
        public string 类型名 { get; set; }

        /// <summary>
        /// 类型值
        /// </summary>
        [DisplayName(@"类型值")]
        [MaxLength(20)]
        public string 类型值 { get; set; }


        #endregion
    }
}