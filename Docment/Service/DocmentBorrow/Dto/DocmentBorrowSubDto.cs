using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    public class DocmentBorrowSubDto
    {
        public Guid Id{ get; set; }

        /// <summary>
        /// 借阅申请id
        /// </summary>
        public Guid BorrowId { get; set; }
        /// <summary>
        /// 档案id
        /// </summary>
        public Guid DocmentId { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime? GetTime { get; set; }
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? BackTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public BorrowSubStatus Status { get; set; }

        public string StatusTitle { get; set; }

        /// <summary>
        /// 档案编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 档案名称
        /// </summary>
        public string Name { get; set; }
        public Guid? QrCodeId { get; set; }
        /// <summary>
        /// 档案类别
        /// </summary>
        public Guid Type { get; set; }

        public string Type_Name { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 档案属性
        /// </summary>
        public DocmentAttr Attr { get; set; }

        public string Attr_Name { get; set; }
    }
}
