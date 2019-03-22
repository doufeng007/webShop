using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Docment
{
    /// <summary>
    /// 档案列表
    /// </summary>
    [Table("Docment")]
    [Serializable]
    public class DocmentList : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 档案编号
        /// </summary>
        [StringLength(18, ErrorMessage = "最大长度为18")]
        public string No { get; set; }
        /// <summary>
        /// 档案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 档案类别
        /// </summary>
        public Guid Type { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        [StringLength(200, ErrorMessage = "最大长度为200")]
        public string Location { get; set; }
        /// <summary>
        /// 档案属性
        /// </summary>
        public DocmentAttr Attr { get; set; }
        /// <summary>
        /// 档案备注
        /// </summary>
        [StringLength(200, ErrorMessage = "最大长度为200")]
        public string Des { get; set; }
        /// <summary>
        /// 状态(0:待收 -1：在档 -2：驳回 -3：借阅中 -4：已移交 -5：已销毁，-6：移交中 ，-8借阅审批种中,10：档案袋)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 是否项目归档
        /// </summary>
        public bool IsProject { get; set; }
        /// <summary>
        /// 归档项目id
        /// </summary>
        public Guid? ArchiveId { get; set; }

        public string DealWithUsers { get; set; }
        /// <summary>
        /// 是否已借到外部
        /// </summary>
        public bool IsOut { get; set; }
        /// <summary>
        /// 是否老档案（导入的档案都是老档案）
        /// </summary>
        public bool IsOld { get; set; }
        /// <summary>
        /// 档案二维码
        /// </summary>
        public Guid? QrCodeId { get; set; }
        /// <summary>
        /// 档案所属流程
        /// </summary>
        public Guid? FlowId { get; set; }
        /// <summary>
        /// 是否需归还
        /// </summary>
        public bool NeedBack { get; set; }
        /// <summary>
        /// 归档申请备注
        /// </summary>
        [StringLength(200, ErrorMessage = "最大长度为200")]
        public string ApplyDes { get; set; }
    }

    public enum DocmentAttr {
        电子=0,
        纸质=1
    }

    /// <summary>
    /// 档案状态
    /// </summary>
    public enum DocmentStatus
    {
        申请入档=0,
        归档中 = 1,
        在档 = -1,
        已驳回=-2,
        使用中 = -3,
        已移交 = -4,
        已销毁 = -5,
        移交中=-6,
        销毁中=-7,
        审批中=-8,
        未归档=-10
    }
}
