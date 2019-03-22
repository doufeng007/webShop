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

namespace HR
{
    [Serializable]
    [Table("HrSystemRead")]
    public class HrSystemRead :  Entity<Guid>
    {
        #region 表字段
        
        /// <summary>
        /// 制度编号
        /// </summary>
        [DisplayName(@"制度编号")]
        public Guid SystemId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public long UserId { get; set; }


        #endregion
    }
}