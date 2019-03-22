using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapFrom(typeof(PostInfo))]
    public class PostInfoDto : EntityDto<Guid>
    {
        public virtual int? TenantId { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }
    }


    public class UserPostDto
    {
        public Guid Id { get; set; }

        public Guid OrgPostId { get; set; }

        public bool IsMain { get; set; } 
        public Guid PostId { get; set; }

        public string PostName { get; set; }
        public string OrgName { get; set; }
    }


}
