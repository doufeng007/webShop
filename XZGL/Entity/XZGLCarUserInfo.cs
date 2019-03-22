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
    [Table("XZGLCarUserInfo")]
    public class XZGLCarUserInfo : Entity<Guid>
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
        /// 车主
        /// </summary>
        [DisplayName(@"车主")]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DisplayName(@"车牌号")]
        [MaxLength(20)]
        public string CarNum { get; set; }

        /// <summary>
        /// 品牌型号
        /// </summary>
        [DisplayName(@"品牌型号")]
        [MaxLength(20)]
        public string CarType { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        [DisplayName(@"排量")]
        [MaxLength(20)]
        public string Amount { get; set; }

        /// <summary>
        /// 座位数
        /// </summary>
        [DisplayName(@"座位数")]
        [MaxLength(20)]
        public string SeatNum { get; set; }

        /// <summary>
        /// 驾驶证号
        /// </summary>
        [DisplayName(@"驾驶证号")]
        [MaxLength(20)]
        public string Number { get; set; }

        /// <summary>
        /// 驾驶证准驾车型
        /// </summary>
        [DisplayName(@"驾驶证准驾车型")]
        [MaxLength(20)]
        public string Type { get; set; }

        /// <summary>
        /// 行驶证车辆识别号
        /// </summary>
        [DisplayName(@"行驶证车辆识别号")]
        [MaxLength(20)]
        public string CarTypeNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }


        #endregion
    }
}