using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;

namespace ZCYX.FRMSCore.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string WorkNumber { get; set; }

        public string PhoneNumber { get; set; }
        public bool? Sex { get; set; }

        public string IdCard { get; set; }

        public DateTime? EnterTime { get; set; }
        public bool IsTwoFactorEnabled { get; set; }//用于标识是否第一次登陆

        /// <summary>
        /// 用户岗位信息
        /// </summary>
        public List<OrganizationUnitPostsDto> Posts { get; set; }
    }
}
