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
    [Table("XZGLCarBorrow")]
    public class XZGLCarBorrow : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        [DisplayName(@"DealWithUsers")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
        [DisplayName(@"使用人")]
        public long UserId { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [DisplayName(@"联系方式")]
        [MaxLength(50)]
        public string Tel { get; set; }

        /// <summary>
        /// 用车开始时间
        /// </summary>
        [DisplayName(@"用车开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 用车结束时间
        /// </summary>
        [DisplayName(@"用车结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 用车类型
        /// </summary>
        [DisplayName(@"用车类型")]
        public CarType CarType { get; set; }

        /// <summary>
        /// 用车事由
        /// </summary>
        [DisplayName(@"用车事由")]
        public string Note { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 单位车辆备注
        /// </summary>
        [DisplayName(@"单位车辆备注")]
        public string CompanyRemark { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        [DisplayName(@"供应商编号")]
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// 供应商联系方式
        /// </summary>
        [DisplayName(@"供应商联系方式")]
        [MaxLength(50)]
        public string SupplierTel { get; set; }

        /// <summary>
        /// 供应商备注
        /// </summary>
        [DisplayName(@"供应商备注")]
        public string SupplierRemark { get; set; }

        /// <summary>
        /// 安排用车备注
        /// </summary>
        [DisplayName(@"安排用车备注")]
        public string CarRemark { get; set; }

        /// <summary>
        /// 用油量
        /// </summary>
        [DisplayName(@"用油量")]
        [MaxLength(20)]
        public string Consumption { get; set; }

        /// <summary>
        /// 用车归还备注
        /// </summary>
        [DisplayName(@"用车归还备注")]
        public string CarReturnRemark { get; set; }

        /// <summary>
        /// 其他备注
        /// </summary>
        [DisplayName(@"其他备注")]
        public string OtherRemark { get; set; }

        /// <summary>
        /// 个人用车备注
        /// </summary>
        [DisplayName(@"个人用车备注")]
        public string UserCarRemark { get; set; }

        /// <summary>
        /// 是否单位车辆
        /// </summary>
        [DisplayName(@"是否单位车辆")]
        public bool IsCompanyCar { get; set; }

        /// <summary>
        /// 是否单位租车
        /// </summary>
        [DisplayName(@"是否单位租车")]
        public bool IsCompanyRent { get; set; }

        /// <summary>
        /// 是否个人租车
        /// </summary>
        [DisplayName(@"是否个人租车")]
        public bool IsUserRent { get; set; }


        #endregion
    }
}