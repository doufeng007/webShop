using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [Table("OrganizationUnitPostsBase")]
    [Serializable]
    public class OrganizationUnitPostsBase : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public long OrganizationUnitId { get; set; }

        public Guid PostId { get; set; }

        //public string Summary { get; set; }
        public int Status { get; set; }



        public int PrepareNumber { get; set; }


        /// <summary>
        /// 岗位级别，值越小级别越高
        /// </summary>
        public Level? Level { get; set; }

    }

    public class OrganizationUnitPosts : OrganizationUnitPostsBase, IMayHaveTenant
    {
        #region 表字段
       


        #endregion

    }
}
