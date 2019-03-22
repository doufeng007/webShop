using System;
using System.Collections.Generic;
using System.Text;

namespace TaskGL.Service.TaskManagementStatistics.Dto
{
    public class TaskStatisticResponse
    {
        /// <summary>
        ///待办项目数量
        /// </summary>
        public int TodoCount { get; set; }
        /// <summary>
        /// 在办项目数量
        /// </summary>
        public int DoingCount { get; set; }
        /// <summary>
        /// 已办项目数量
        /// </summary>
        public int DoneCount { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal HandleRate { get; set; }
    }
}
