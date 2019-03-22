using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Project
{
    /// <summary>
    /// 发文类型
    /// </summary>
    public enum NoticeDocumentType
    {
        决定 = 0,
        公报 = 1,
        通告 = 2,
        意见 = 3,
        通知 = 4,
        通报 = 5,
        报告 = 6,
        请示 = 7,
        批复 = 8,
        函 = 9,
        纪要 = 10
    }


    public enum NoticeDocumentBusinessType
    {
        普通公文 = 0,
        工作联系公文 = 1,
        工作联系汇总公文 = 2,
        项目评审发文 = 3,
    }

    public enum EmergencyDegreeProperty
    {
        [Description("特急")] 特急 = 0,
        [Description("急件")] 急件 = 1,
        [Description("平件")] 平件 = 2
    }

    public enum RankProperty
    {
        [Description("绝密")] 绝密 = 0,
        [Description("机密")] 机密 = 1,
        [Description("秘密")] 秘密 = 2
    }
}
