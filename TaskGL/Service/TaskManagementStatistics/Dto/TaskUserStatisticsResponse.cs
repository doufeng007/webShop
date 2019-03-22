using System;
using System.Collections.Generic;
using System.Text;

namespace TaskGL.Service.TaskManagementStatistics.Dto
{
    public class TaskUserStatisticsResponse
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 全部任务
        /// </summary>
        public int All { get; set; }
        /// <summary>
        /// 进行中
        /// </summary>
        public int Doing { get; set; }
        /// <summary>
        /// 已完成
        /// </summary>
        public int Done { get; set; }
        /// <summary>
        /// 逾期
        /// </summary>
        public int BeOverdue { get; set; }
        /// <summary>
        /// 紧急
        /// </summary>
        public int Urgent { get; set; }
    }
}
