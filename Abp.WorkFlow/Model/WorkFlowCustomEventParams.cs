using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abp.WorkFlow
{
    /// <summary>
    /// 调用流程事件时的参数实体
    /// </summary>
    public class WorkFlowCustomEventParams
    {

        public Guid FlowID { get; set; }

        public Guid StepID { get; set; }

        public Guid GroupID { get; set; }

        public Guid TaskID { get; set; }

        public string InstanceID { get; set; }

        /// <summary>
        /// 其它参数字符串
        /// </summary>
        public string OtherString { get; set; }

        public long NextRecevieUserId { get; set; }
    }

    public class WorkFlowCustomEventParamsForAfterSubmit : WorkFlowCustomEventParams
    {
        public List<WorkFlowTask> NextTasks { get; set; } = new List<WorkFlowTask>();
    }
}
