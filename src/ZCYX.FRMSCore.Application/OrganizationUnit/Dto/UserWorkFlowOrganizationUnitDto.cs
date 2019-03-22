using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{

    public class UserWorkFlowOrganizationUnitDto
    {
        public long OrgId { get; set; }

        public string OrgId_Name { get; set; }

        public List<UserPostInfo> UserPosts { get; set; }


        public UserWorkFlowOrganizationUnitDto()
        {
            this.UserPosts = new List<UserPostInfo>();
        }

    }


    public class UserPostInfo
    {
        public Guid UserOrgPostId { get; set; }

        public Guid OrgPostId { get; set; }

        public Guid PostId { get; set; }

        public string PostName { get; set; }

    }


}
