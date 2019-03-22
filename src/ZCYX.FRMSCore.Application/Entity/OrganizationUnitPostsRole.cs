using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    /// <summary>
    /// 岗位和角色的关联表
    /// </summary>
    [Table("OrganizationUnitPostsRole")]
    public class OrganizationUnitPostsRole: Entity<Guid>
    {
        /// <summary>
        /// 岗位id
        /// </summary>
        public Guid OrgPostId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleName { get; set; }
    }
}
