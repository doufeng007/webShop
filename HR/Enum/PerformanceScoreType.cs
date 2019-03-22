using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HR
{
    public enum PerformanceUnit
    {
        [Description("百分比")] 百分比 = 0,
        [Description("个")] 个 = 1,
        [Description("次")] 次 = 2
    }

    public enum PerformanceScoreTypeEnum
    {
        [Description("选择")] 选择 = 0,
        [Description("减分")] 减分 = 1,
        [Description("加分")] 加分 = 2
    }

    public enum PerformanceType
    {
        [Description("数字化绩效考核")] 数字化绩效考核 = 0,
        [Description("非数字化绩效考核")] 非数字化绩效考核 = 1
    }
}