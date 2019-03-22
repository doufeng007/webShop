using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// [单表映射]
    /// </summary>
    [Table("Employee")]
    public class Employee : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        #region 表字段

        public virtual int? TenantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Sex")]
        public int Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Age")]
        public int Age { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Birthday")]
        public DateTime Birthday { get; set; }

        public string Birthday2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Nature")]
        public string Nature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HomeTown")]
        public string HomeTown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Hight")]
        public int? Hight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Blood")]
        public string Blood { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Health")]
        public string Health { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Party")]
        public string Party { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Marray")]
        public int? Marray { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Education")]
        public string Education { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("School")]
        public string School { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OldCompany")]
        public string OldCompany { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OldCompanyType")]
        public string OldCompanyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Professional")]
        public string Professional { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Duty")]
        public string Duty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("StartWork")]
        public DateTime? StartWork { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("IdCard")]
        public string IdCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Address")]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OldCompanyRelation")]
        public string OldCompanyRelation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AddressPost")]
        public string AddressPost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HomeAddress")]
        public string HomeAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HomeAddressPost")]
        public string HomeAddressPost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("DocmentAddress")]
        public string DocmentAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HomeTel")]
        public string HomeTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WarningPeople")]
        public string WarningPeople { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WarningPeoplePhone")]
        public string WarningPeoplePhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ForeignLevel")]
        public string ForeignLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Specialty")]
        public string Specialty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("CanDistribution")]
        public bool CanDistribution { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("InterviewId")]
        public Guid? InterviewId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Sick")]
        public string Sick { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Crime")]
        public string Crime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Insurance")]
        public string Insurance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; } = 0;


        public long? UserId { get; set; }

        /// <summary>
        /// 是否临时工
        /// </summary>
        public bool? IsTemp { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WXchat { get; set; }
        /// <summary>
        /// 兴趣爱好
        /// </summary>
        public string Enjoy { get; set; }
        /// <summary>
        /// 支付宝
        /// </summary>
        public string Alipay { get; set; }
        /// <summary>
        /// 工资卡号
        /// </summary>
        public string BankNo { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public Guid? BankType { get; set; }
        /// <summary>
        /// 开户行地址
        /// </summary>
        public string BankAddress { get; set; }

        
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EnterTime { get; set; }
        #endregion

    }

}
