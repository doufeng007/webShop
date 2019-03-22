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
    [Table("XZGLCarInfo")]
    public class XZGLCarInfo :Entity<Guid>
    {
        #region 表字段

        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }
        /// <summary>
        /// 用车编号
        /// </summary>
        [DisplayName(@"用车编号")]
        public Guid CarBorrowId { get; set; }

        /// <summary>
        /// 车辆编号
        /// </summary>
        [DisplayName(@"车辆编号")]
        public Guid CarId { get; set; }

        /// <summary>
        /// 司机
        /// </summary>
        [DisplayName(@"司机")]
        public long UserId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [DisplayName(@"车牌号")]
        [MaxLength(20)]
        [Required]
        public string CarNum { get; set; }

        /// <summary>
        /// 品牌型号
        /// </summary>
        [DisplayName(@"品牌型号")]
        [MaxLength(20)]
        [Required]
        public string CarType { get; set; }

        /// <summary>
        /// 座位数
        /// </summary>
        [DisplayName(@"座位数")]
        [MaxLength(2)]
        [Required]
        public string SeatNum { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        [DisplayName(@"排量")]
        [MaxLength(5)]
        [Required]
        public string Amount { get; set; }


        #endregion
    }
}