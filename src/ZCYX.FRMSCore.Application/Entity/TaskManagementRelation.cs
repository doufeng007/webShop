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

namespace  ZCYX.FRMSCore.Application
{
    [Serializable]
    [Table("TaskManagementRelation")]
    public class TaskManagementRelation : Entity<Guid>
    {
        #region 表字段
        
        /// <summary>
        /// 流程编号
        /// </summary>
        [DisplayName(@"流程编号")]
        public Guid FlowId { get; set; }

        /// <summary>
        /// 关联编号
        /// </summary>
        [DisplayName(@"关联编号")]
        public Guid InStanceId { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        [DisplayName(@"任务编号")]
        public Guid TaskManagementId { get; set; }


        #endregion
    }
}