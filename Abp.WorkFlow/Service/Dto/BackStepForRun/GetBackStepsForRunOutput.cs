using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetBackStepsForRunOutput
    {
        /// <summary>
        /// 退回策略 0不能退回 1根据处理策略退回 2一人退回全部退回 3所人有退回才退回 4独立退回（一般用于上一步处理者为多个的单独退回，最匹配的上一步为独立处理）
        /// </summary>
        public int BackModel { get; set; }


        /// <summary>
        /// 退回类型 0退回前一步 1退回第一步 2退回某一步
        /// </summary>
        public int BackType { get; set; }

        /// <summary>

        /// </summary>
        public BackStepResult Result { get; set; }



        /// <summary>
        /// 审签类型 0无签批意见栏 1有签批意见(无须签章) 2有签批意见(须签章)
        /// </summary>
        public int SignatureType { get; set; }


        public string SugguestionTitle { get; set; }


        public List<GetBackStepsOutput> Steps { get; set; }

        public GetBackStepsForRunOutput()
        {
            this.Steps = new List<GetBackStepsOutput>();
        }


    }

    public class GetBackStepsOutput
    {
        /// <summary>
        /// 处理人id
        /// </summary>

        public List<GetBackStepsUserOutput> BackUsers { get; set; }


        public Guid BackStepId { get; set; }

        public string BackStepName { get; set; }

        public GetBackStepsOutput()
        {
            this.BackUsers = new List<GetBackStepsUserOutput>();
        }

    }

    public class GetBackStepsUserOutput
    {
        public string UserIdWithPerfix { get; set; }

        public string UserName { get; set; }
    }
}
