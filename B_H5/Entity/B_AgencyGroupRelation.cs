using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;

namespace B_H5
{
    [Serializable]
    [Table("B_AgencyGroupRelation")]
    public class B_AgencyGroupRelation : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// AgencyId
        /// </summary>
        [DisplayName(@"AgencyId")]
        public Guid AgencyId { get; set; }

        /// <summary>
        /// GroupId
        /// </summary>
        [DisplayName(@"GroupId")]
        public Guid GroupId { get; set; }

        /// <summary>
        /// IsGroupLeader
        /// </summary>
        [DisplayName(@"IsGroupLeader")]
        public bool IsGroupLeader { get; set; }


        #endregion
    }
}