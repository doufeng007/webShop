using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace HR
{
    /// <summary>
    /// [单表映射]
    /// </summary>
    [Table("OrganizationUnitPostPlan")]
    [Serializable]
    public class OrganizationUnitPostPlan : OrganizationUnitPostsBase, IMayHaveTenant
    {
        #region 表字段

        

        #endregion

    }


    [Table("OrganizationUnitPostChangePlan")]
    [Serializable]
    public class OrganizationUnitPostChangePlan : OrganizationUnitPostsBase, IMayHaveTenant
    {
        #region 表字段

        

        #endregion

    }

   
}
