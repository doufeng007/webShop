using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Authorization.Users;
using System.Collections.Generic;
using System;

namespace ZCYX.FRMSCore.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }
        //[Required]
        public bool? Sex { get; set; }

        public string IdCard { get; set; }

        public DateTime? EnterTime { get; set; }
        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage ="请填写用户工号")]
        public string WorkNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
        /// <summary>
        /// 用户所在部门
        /// </summary>
        public long OrganizationUnitId { get; set; }

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
        /// <summary>
        /// 主岗位
        /// </summary>
        public Guid MainPostId { get; set; }
        public List<Guid> OrgPostIds { get; set; }


        public List<Guid> RelationSystemIds { get; set; }

        public CreateUserDto()
        {
            OrgPostIds = new List<Guid>();
            RelationSystemIds = new List<Guid>();
        }
    }
    public class CreateImUserDto {
        public long Id { get; set; }
        public string PassWord { get; set; }
    }
}
