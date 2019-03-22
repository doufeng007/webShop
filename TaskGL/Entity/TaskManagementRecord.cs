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
using NPOI.HSSF.Model;
using TaskGL.Enum;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;

namespace TaskGL
{
    [Serializable]
    [Table("TaskManagementRecord")]
    public class TaskManagementRecord : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        public Guid TaskManagementId { get; set; }


        public string Content { get; set; }


        #endregion
    }
}