using System;
using System.Collections.Generic;
using System.Text;
using TaskGL.Enum;

namespace TaskGL.Service.TaskManagementStatistics.Dto
{
    public class MyTaskStatisticDetailResponse
    {
        /// <summary>
        /// 任务统计类型
        /// </summary>
        public TaskStatisticDetailTypeEnum TaskType { get; set; }
        /// <summary>
        /// 全部任务
        /// </summary>
        public int All { get; set; }
        /// <summary>
        /// 审批中
        /// </summary>
        public int Approvaling { get; set; }
        /// <summary>
        /// 驳回
        /// </summary>
        public int Reject { get; set; }
        /// <summary>
        /// 待开始
        /// </summary>
        public int BefreStart { get; set; }
        /// <summary>
        /// 进行中
        /// </summary>
        public int Doing { get; set; }
        /// <summary>
        /// 已完成
        /// </summary>
        public int Done { get; set; }
        /// <summary>
        /// 重做
        /// </summary>
        public int Redo { get; set; }
        /// <summary>
        /// 撤销
        /// </summary>
        public int Revoke { get; set; }
    }
}
