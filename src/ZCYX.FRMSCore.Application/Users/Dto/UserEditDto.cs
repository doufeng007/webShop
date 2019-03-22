using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZCYX.FRMSCore.Authorization.Users;

namespace ZCYX.FRMSCore.Users.Dto
{
    public class UserEditDto : IPassivable
    {
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// </summary>
        public long? Id { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        //  [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        // Not used "Required" attribute since empty value is used to 'not change password'
        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public virtual bool IsTwoFactorEnabled { get; set; }

        public virtual bool IsLockoutEnabled { get; set; }

        public bool? Sex { get; set; }

        public string IdCard { get; set; }

        public DateTime? EnterTime { get; set; }
    }
}
