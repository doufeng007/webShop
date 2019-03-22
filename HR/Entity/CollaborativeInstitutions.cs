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

namespace HR
{
    [Table("CollaborativeInstitutions")]
    public class CollaborativeInstitutions : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DisplayName(@"Name")]
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// Function
        /// </summary>
        [DisplayName(@"Function")]
        [MaxLength(500)]
        public string Function { get; set; }

        /// <summary>
        /// ScaleNum
        /// </summary>
        [DisplayName(@"ScaleNum")]
        public int ScaleNum { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DisplayName(@"Address")]
        [MaxLength(500)]
        public string Address { get; set; }

        /// <summary>
        /// Head
        /// </summary>
        [DisplayName(@"Head")]
        [MaxLength(50)]
        public string Head { get; set; }

        /// <summary>
        /// Tel
        /// </summary>
        [DisplayName(@"Tel")]
        [MaxLength(20)]
        public string Tel { get; set; }

        /// <summary>
        /// BankNum
        /// </summary>
        [DisplayName(@"BankNum")]
        [MaxLength(30)]
        public string BankNum { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [DisplayName(@"BankName")]
        [MaxLength(30)]
        public string BankName { get; set; }

        /// <summary>
        /// BankDeposit
        /// </summary>
        [DisplayName(@"BankDeposit")]
        [MaxLength(100)]
        public string BankDeposit { get; set; }


        #endregion
    }
}