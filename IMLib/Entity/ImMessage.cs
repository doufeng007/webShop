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

namespace IMLib
{
    [Serializable]
    [Table("ImMessage")]
    public class ImMessage : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 组编号
        /// </summary>
        [DisplayName(@"组编号")]
        [MaxLength(50)]
        [Required]
        public string To { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public long UserId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        [MaxLength(20)]
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName(@"内容")]
        public string Msg { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [DisplayName(@"文件名")]
        [MaxLength(300)]
        public string FileName { get; set; }

        /// <summary>
        /// 是否聊天室
        /// </summary>
        [DisplayName(@"是否聊天室")]
        public bool? RoomType { get; set; }

        /// <summary>
        /// 聊天室类型
        /// </summary>
        [DisplayName(@"聊天室类型")]
        [MaxLength(20)]
        public string ChatType { get; set; }


        #endregion
    }
}