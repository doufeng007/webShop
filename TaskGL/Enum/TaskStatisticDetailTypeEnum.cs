using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaskGL.Enum
{
    public enum TaskStatisticDetailTypeEnum
    {
        /// <summary>
        /// 我的任务
        /// </summary>
        [Description("我的任务")] MyTask = 0,

        /// <summary>
        /// 我督办的任务
        /// </summary>
        [Description("我督办的任务")] MySuperintendentTask = 1,

        /// <summary>
        /// 我部门的任务
        /// </summary>
        [Description("我部门的任务")] MyOrganizationTask = 2,

        /// <summary>
        /// 我项目组的任务
        /// </summary>
        [Description("我项目组的任务")] MyProjectTask = 3,

        /// <summary>
        /// 我关注的任务
        /// </summary>
        [Description("我关注的任务")] MyFollowTask = 4
    }
}
