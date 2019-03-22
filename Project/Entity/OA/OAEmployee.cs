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
    [Table("OAEmployee")]
    public class OAEmployee : FullAuditedEntity<Guid>
    {

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WorkNo")]
        public string WorkNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Department")]
        public string Department { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Work")]
        public string Work { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("JoinDate")]
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("LeftDate")]
        public DateTime? LeftDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("CardNo")]
        public string CardNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HomeTown")]
        public string HomeTown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Nation")]
        public string Nation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("IsMarrayed")]
        public bool IsMarrayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("School")]
        public string School { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BankCard")]
        public string BankCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Education")]
        public string Education { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Major")]
        public string Major { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WarningPerson")]
        public string WarningPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WarningPhone")]
        public string WarningPhone { get; set; }

      
        public DateTime? Birthday { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }

        public string AuditUser { get; set; }

    }



}
