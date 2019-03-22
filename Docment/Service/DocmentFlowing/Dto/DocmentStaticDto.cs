using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment
{
    /// <summary>
    /// 档案统计信息
    /// </summary>
    public class DocmentStaticDto
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public long OrgId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string OrgName { get; set; }

        public long? ParentId { get; set; }

        public string OrgCode { get; set; }

        public long? UserId { get; set; }

        public string UserName { get; set; }
        /// <summary>
        /// 资料份数
        /// </summary>
        public int DocmentCount { get; set; }
        /// <summary>
        /// 待归档资料份数
        /// </summary>
        public int WaitDocmentCount { get; set; }
        /// <summary>
        /// 在外流转的档案份数
        /// </summary>
        public int OutDocmentCount { get; set; }
        /// <summary>
        /// 待归还档案
        /// </summary>
        public int WaitBackDocmentCount { get; set; }
    }

    public class DocmentStaticSearshDto: PagedInputDto
    {
        /// <summary>
        /// 统计方式
        /// </summary>
         public StaticType StaticType { get; set; }
    }

    public enum StaticType {
        /// <summary>
        /// 按部门统计
        /// </summary>
        Org=0,
        /// <summary>
        /// 按人员统计
        /// </summary>
        User=1
    }
}
