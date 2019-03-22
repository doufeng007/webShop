using Abp.AutoMapper;
using System;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapTo(typeof(OrganizationUnitPosts))]
    public class CreateOrganizationUnitPostsInput
    {
        #region 表字段
        /// <summary>
        /// OrganizationUnitId
        /// </summary>
        public long OrganizationUnitId { get; set; }

       

        /// <summary>
        /// PrepareNumber
        /// </summary>
        public int PrepareNumber { get; set; }

        /// <summary>
        /// 岗位级别，值越小级别越高
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 岗位预设角色
        /// </summary>
        public List<string> RoleNames { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; }
        #endregion
    }
    public class UpdateOrganizationUnitPostsInput
    {
        #region 表字段

        public Guid Id { get; set; }
        /// <summary>
        /// PrepareNumber
        /// </summary>
        public int PrepareNumber { get; set; }

        /// <summary>
        /// 岗位级别，值越小级别越高
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 岗位预设角色
        /// </summary>
        public List<string> RoleNames { get; set; }


        public string Name { get; set; }
        #endregion
    }

}