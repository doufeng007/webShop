using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ZCYX.FRMSCore.Authorization.Users;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapFrom(typeof(User))]
    public class WorkFlowOrganizationUnitUserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public DateTime AddedTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 用户部门信息
        /// </summary>
        public List<SimpleOrganizationDto> Organization { get; set; }
        /// <summary>
        /// 用户岗位信息
        /// </summary>
        public List<UserPostDto> Posts { get; set; }
    }
    public class SimpleOrganizationDto
    {
        public long Id { get; set; }
        public string Code { get; set; }

        public string Title { get; set; }

        public bool IsMain { get; set; }
    }

    public class OrganizationUnitUserInput
    {
        public long Id { get; set; }

        public List<SimpleOrganizationDto> Organization { get; set; }
    }



    public class OrganizationUnitUserOutput: SimpleOrganizationDto

    {

    }
}