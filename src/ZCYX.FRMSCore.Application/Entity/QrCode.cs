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

namespace ZCYX.FRMSCore.Application
{
    [Serializable]
    [Table("QrCode")]
    public class QrCode : Entity<Guid>
    {
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 关注类别
        /// </summary>
        [DisplayName(@"类别")]
        public QrCodeType Type { get; set; }


        #endregion
    }
}