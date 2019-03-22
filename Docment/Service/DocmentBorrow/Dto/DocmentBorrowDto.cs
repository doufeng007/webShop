using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Docment
{
    /// <summary>
    /// 档案借阅信息
    /// </summary>
    public class DocmentBorrowDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        //public Guid DocmentId { get; set; }
        public List<DocmentBorrowSubDto> Docments { get; set; }
        public string Des { get; set; }
        /// <summary>
        /// 申请人部门
        /// </summary>
        public long OrgId { get; set; }
        /// <summary>
        /// 申请人部门
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public long? CreateUserId { get; set; }
        public string CreateUserId_Name { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? BackTime { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
        public int Status { get; set; }
    }
}
