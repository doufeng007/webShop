using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    /// <summary>
    /// 档案借阅管理列表
    /// </summary>
    public class DocmentBorrowListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public Guid DocmentId { get; set; }

        public string Des { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? BackTime { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 申请份数
        /// </summary>
        public int Count { get; set; }
        ///// <summary>
        ///// 档案编号
        ///// </summary>
        //public string No { get; set; }
        ///// <summary>
        ///// 档案名称
        ///// </summary>
        //public string Name { get; set; }
        ///// <summary>
        ///// 档案类别 
        ///// </summary>
        //public Guid Type { get; set; }

        //public string Type_Name { get; set; }
        ///// <summary>
        ///// 存放位置
        ///// </summary>
        //public string Location { get; set; }
        ///// <summary>
        ///// 档案属性
        ///// </summary>
        //public DocmentAttr Attr { get; set; }

        //public string Attr_Name { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public long? UserId { get; set; }

        public string UserId_Name { get; set; }
        /// <summary>
        /// 申请人部门
        /// </summary>
        public long OrgId { get; set; }
        /// <summary>
        /// 申请人部门
        /// </summary>
        public string OrgName { get; set; }
    }
}
