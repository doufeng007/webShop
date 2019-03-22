using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetNextStepForRunOutput
    {
        public List<GetNextStepOutput> Steps { get; set; } = new List<GetNextStepOutput>();

        /// <summary>
        /// 流转类型 0系统控制 1单选一个分支流转 2多选几个分支流转
        /// </summary>
        public int FlowType { get; set; }

        /// <summary>
        /// 审签类型 0无签批意见栏 1有签批意见(无须签章) 2有签批意见(须签章)
        /// </summary>
        public int SignatureType { get; set; }


        public string SugguestionTitle { get; set; }


        

        public List<RelationUser> Users { get; set; } = new List<RelationUser>();

    }
    public class RelationUser
    {
        public Guid RelationId { get; set; }
        public long UserId { get; set; }
        public long RelationUserId { get; set; }
        public Guid NextStepId { get; set; }
    }
    public class GetNextStepOutput
    {
        /// <summary>
        /// 默认处理人id
        /// </summary>
        public string DefaultUserId { get; set; }


        public string DefaultUserName { get; set; }


        public Guid NextStepId { get; set; }

        public string NextStepName { get; set; }

        /// <summary>
        /// 处理人员是否允许选择
        /// </summary>
        public bool IsAllowChoose { get; set; }

        /// <summary>
        /// 处理人员的选择范围
        /// </summary>
        public string SelectRangeRootId { get; set; }


        public string NextStepReciveUserNames { get; set; }
    }
}
