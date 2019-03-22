using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MeetingGL
{
    public enum WraningDataTypeEnum
    {
        [Description("分钟")] Minute = 0,
        [Description("小时")] Hour = 1,
        [Description("天")] Day = 2,
    }

    public enum WraingTypeEnum
    {
        [Description("短信")] SMS = 1,
        [Description("事务通知")] Information = 0
    }


    public enum MeetingRoomType
    {
        [Description("会议")] Meeting = 0,
        [Description("培训")] Train = 1
    }


    public enum MeetingRoomSizeType
    {
        [Description("小型")] S = 0,
        [Description("中型")] M = 1,
        [Description("大型")] L = 2,
    }

    public enum MeetingIsssueType
    {
        自定义议程 = 1,
        议题 = 2,
    }

    public enum MeetingUserRole
    {
        记录者 = 1,
        参会人员 = 2,
        主持人 = 3,
    }

    public enum ReturnReceiptStatus
    {
        无回执 = 0,
        确定参会 = 1,
        申请请假 = 2
    }
    public enum BeforeMeetingUserTaskType
    {
        议题 = 1,
        会议资料 = 2,
        会务后勤 = 4
    }


    public enum MeetingIssueStatus
    {
        草稿 = 0,
        待议 = 1,
        准备中 = 2,
        延迟 = 3,
        已议 = 4
    }

    public enum MeetingIssueResultStatus
    {
        [Description("未议")]
        NoPass = 0,
        [Description("已议")]
        HasPass = 1,

    }


    public enum PeriodType
    {
        按天 = 1,
        按周 = 2,
        按月 = 3,
        按年 = 4

    }

    public enum PreTimeType
    {
        分钟 = 1,
        小时 = 2,
        天 = 3

    }


    public enum MeetingRoomUseBusinessType
    {
        培训 = 1,
        会议 = 2
    }

    /// <summary>
    /// 议题类型
    /// </summary>
    public enum IssueType
    {
        部门议题 = 0,
        项目议题 = 1
    }

    public enum PeriodRuleStatus
    {
        正常 = 0,
        失效 = 1,
        取消 = 2
    }
}
