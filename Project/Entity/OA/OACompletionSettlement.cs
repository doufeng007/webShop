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
    [Table("OACompletionSettlement")]
    public class OACompletionSettlement : FullAuditedEntity<Guid>
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


        public string ProjectAdress { get; set; }

        

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ContractId")]
        public string ContractId { get; set; }


        [DisplayName("ContractName")]
        public string ContractName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ContractAmount")]
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UnitA")]
        public string UnitA { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SettlementAmount")]
        public decimal SettlementAmount { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Debit")]
        public decimal Debit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Fine")]
        public decimal Fine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WriteUser")]
        public long WriteUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WriteData")]
        public DateTime WriteData { get; set; }

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
}
