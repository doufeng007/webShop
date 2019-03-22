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

namespace Supply
{
    [Table("SupplySupplier")]
    public class SupplySupplier : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 供应商类型
        /// </summary>
        [DisplayName(@"供应商类型")]
        [MaxLength(1,ErrorMessage = "供应商类型最大为1个字符。")]
        public string Type { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [DisplayName(@"供应商名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        [DisplayName(@"主营业务")]
        [MaxLength(500)]
        public string MainBusiness { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        [DisplayName(@"公司地址")]
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        [DisplayName(@"法人")]
        [MaxLength(32)]
        public string LegalPerson { get; set; }

        /// <summary>
        /// 法人联系电话
        /// </summary>
        [DisplayName(@"法人联系电话")]
        [MaxLength(32)]
        public string LegalPersonTel { get; set; }

        /// <summary>
        /// 销售联系人
        /// </summary>
        [DisplayName(@"销售联系人")]
        [MaxLength(32)]
        public string SalesContact { get; set; }

        /// <summary>
        /// 销售联系人电话
        /// </summary>
        [DisplayName(@"销售联系人电话")]
        [MaxLength(32)]
        public string SalesContactTel { get; set; }

        /// <summary>
        /// 评价备注
        /// </summary>
        [DisplayName(@"评价备注")]
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        [DisplayName(@"信箱")]
        [MaxLength(100)]
        public string Email { get; set; }


        #endregion
    }
}