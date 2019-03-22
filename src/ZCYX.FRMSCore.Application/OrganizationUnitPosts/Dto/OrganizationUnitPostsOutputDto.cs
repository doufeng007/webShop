using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapFrom(typeof(OrganizationUnitPosts))]
    public class OrganizationUnitPostsOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// OrganizationUnitId
        /// </summary>
        public long OrganizationUnitId { get; set; }

        public string PostName { get; set; }

        /// <summary>
        /// PostId
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// PrepareNumber
        /// </summary>
        public int PrepareNumber { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 岗位级别，值越小级别越高
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 岗位预设角色
        /// </summary>
        public List<string> RoleNames { get; set; }
        /// <summary>
        /// 岗位角色
        /// </summary>
        public int? RoleId { get; set; }

    }
}
