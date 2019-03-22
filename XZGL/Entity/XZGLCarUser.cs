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

namespace XZGL
{
    [Serializable]
    [Table("XZGLCarUser")]
    public class XZGLCarUser : Entity<Guid>
    {
        #region 表字段

        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }
        /// <summary>
        /// 平台名称
        /// </summary>
        [DisplayName(@"平台名称")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 平台地址
        /// </summary>
        [DisplayName(@"平台地址")]
        [MaxLength(200)]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 平台联系方式
        /// </summary>
        [DisplayName(@"平台联系方式")]
        [MaxLength(50)]
        [Required]
        public string Tel { get; set; }

        /// <summary>
        /// 租车费用
        /// </summary>
        [DisplayName(@"租车费用")]
        public decimal CarMoney { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [DisplayName(@"订单编号")]
        [MaxLength(50)]
        public string OrderNum { get; set; }

        /// <summary>
        /// 车辆品牌型号
        /// </summary>
        [DisplayName(@"车辆品牌型号")]
        [MaxLength(50)]
        [Required]
        public string CarType { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DisplayName(@"车牌号")]
        [MaxLength(50)]
        [Required]
        public string CarNum { get; set; }

        /// <summary>
        /// 用车编号
        /// </summary>
        [DisplayName(@"用车编号")]
        public Guid CarBorrowId { get; set; }


        #endregion
    }
}