using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaskGL.Enum
{
    public enum TaskStatisticsDropTypeEnum
    {
        /// <summary>
        /// 部门
        /// </summary>
        [Description("部门")] Organization = 0,

        /// <summary>
        /// 项目组
        /// </summary>
        [Description("项目组")] ProjectGroup = 1
    }
}
