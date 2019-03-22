using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    /// <summary>
    /// 档案销毁信息
    /// </summary>
    public class DocmentDestroyDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public Guid DocmentId { get; set; }

        public string Des { get; set; }

        /// <summary>
        /// 档案编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 档案名称
        /// </summary>
        public string DocmentId_Name { get; set; }
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
        /// <summary>
        /// 责任人
        /// </summary>
        public long? UserId { get; set; }

        public string UserId_Name { get; set; }

        public DateTime CreationTime { get; set; }
        public long? CreateUserId { get; set; }
        public string CreateUserId_Name { get; set; }
        /// <summary>
        /// 档案附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
        /// <summary>
        /// 移交证明附件
        /// </summary>
        public List<GetAbpFilesOutput> MoveFileList { get; set; }
    }
}
