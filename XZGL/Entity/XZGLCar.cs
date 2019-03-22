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
    [Table("XZGLCar")]
    public class XZGLCar : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
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
        /// 车身颜色
        /// </summary>
        [DisplayName(@"车身颜色")]
        [MaxLength(5)]
        [Required]
        public string CarColor { get; set; }

        /// <summary>
        /// 排量
        /// </summary>
        [DisplayName(@"排量")]
        [MaxLength(5)]
        [Required]
        public string Amount { get; set; }

        /// <summary>
        /// 变速箱
        /// </summary>
        [DisplayName(@"变速箱")]
        public XZGLCarVariable Variable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 号牌号码
        /// </summary>
        [DisplayName(@"号牌号码")]
        [MaxLength(20)]
        [Required]
        public string Number { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        [DisplayName(@"车辆类型")]
        [MaxLength(20)]
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// 所有人
        /// </summary>
        [DisplayName(@"所有人")]
        [MaxLength(20)]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 使用性质
        /// </summary>
        [DisplayName(@"使用性质")]
        [MaxLength(20)]
        [Required]
        public string UserType { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        [DisplayName(@"住址")]
        [MaxLength(200)]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 行驶证品牌型号
        /// </summary>
        [DisplayName(@"行驶证品牌型号")]
        [MaxLength(20)]
        [Required]
        public string DrivingType { get; set; }

        /// <summary>
        /// 车辆识别代号
        /// </summary>
        [DisplayName(@"车辆识别代号")]
        [MaxLength(20)]
        [Required]
        public string DrivingNumber { get; set; }

        /// <summary>
        /// 发动机号码
        /// </summary>
        [DisplayName(@"发动机号码")]
        [MaxLength(20)]
        [Required]
        public string EngineNumber { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        [DisplayName(@"注册日期")]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 发证日期
        /// </summary>
        [DisplayName(@"发证日期")]
        public DateTime CertificationTime { get; set; }

        /// <summary>
        /// 行驶证备注
        /// </summary>
        [DisplayName(@"行驶证备注")]
        public string DrivingRemark { get; set; } 
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [DisplayName(@"是否启用")]
        public bool IsEnable { get; set; }


        #endregion
    }
}