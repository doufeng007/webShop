using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Application;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        //public bool IsActive { get; set; }
        public bool? Sex { get; set; }

        public string IdCard { get; set; }

        public DateTime? EnterTime { get; set; }
        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }

        public List<UserPostDto> Posts { get; set; }

        public List<RealationSystem> RealationSystems { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkNumber { get; set; }
        public UserDto()
        {
            Posts = new List<UserPostDto>();
            RealationSystems = new List<RealationSystem>();
        }
    }
}
