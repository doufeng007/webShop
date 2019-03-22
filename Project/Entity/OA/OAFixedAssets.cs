using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Table("OAFixedAssets")]
    public class OAFixedAssets : FullAuditedEntity<Guid>
    {
        #region 表字段


        public Guid? PurchaseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Brand")]
        public string Brand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Spec")]
        public string Spec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Number")]
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("DateOfManufacture")]
        public DateTime DateOfManufacture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BuyDate")]
        public DateTime BuyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PostingDate")]
        public DateTime PostingDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BuyType")]
        public string BuyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BuyTypeCode")]
        public string BuyTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FeeSource")]
        public string FeeSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FeeSourceCode")]
        public string FeeSourceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ServiceLife")]
        public int ServiceLife { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OriginalValue")]
        public decimal OriginalValue { get; set; }

        public decimal ReferPrice { get; set; }

        public decimal RealPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("File")]
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }

        #endregion

    }

    public enum OAFixedAssetsStatus
    {
        采购 = 1,
        在库 = 2,
        领用申请 = 3,
        领用 = 4,
        归还申请 = 5,
        报废申请 = 6,
        报废 = 7,
        维修申请 = 8,
        维修 = 9,
    }
}
