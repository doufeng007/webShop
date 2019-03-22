using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Project
{
    [Table("OAContract")]
    public class OAContract : FullAuditedEntity<Guid>
    {

        #region 表字段

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
        [DisplayName("ProjectId")]
        public Guid? ProjectId { get; set; }

        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ContractType")]
        public string ContractType { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ContractType")]
        public string ContractTypeCode { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [DisplayName("StartDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EndDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        public string UnitA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SigningUser")]
        public long SigningUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PayType")]
        public string PayType { get; set; }


        public string PayTypeCode { get; set; }

        public string SettlementTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SettlementType")]
        public string SettlementType { get; set; }




        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PreAmount")]
        public decimal PreAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Deposit")]
        public decimal Deposit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SignData")]
        public DateTime SignData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("File")]
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Conditions")]
        public string Conditions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("MainConditions")]
        public string MainConditions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }


        public string UnitAContract { get;set;}

        public string UnitAContractTel { get; set; }


        public string UnitAContractAddress { get; set; }

        public int Status { get; set; }

        #endregion


    }
}
