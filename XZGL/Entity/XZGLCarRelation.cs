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

namespace XZGL
{
    [Serializable]
    [Table("XZGLCarRelation")]
    public class XZGLCarRelation : Entity<Guid>
    {
        #region 表字段

        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }
        /// <summary>
        /// 用车编号
        /// </summary>
        [DisplayName(@"用车编号")]
        public Guid CarBorrowId { get; set; }

        /// <summary>
        /// 绑定编号
        /// </summary>
        [DisplayName(@"绑定编号")]
        public Guid BusinessId { get; set; }

        /// <summary>
        /// 绑定类型
        /// </summary>
        [DisplayName(@"绑定类型")]
        public CarRelationType BusinessType { get; set; }


        #endregion
    }
}