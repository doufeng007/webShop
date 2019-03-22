using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HR.Enum
{
    public enum ReceiptDocType
    {
        [Description("决定")] Decision = 0,
        [Description("公报")] Bulletin = 1,
        [Description("通告")] Circular = 2,
        [Description("意见")] View = 3,
        [Description("通知")] Notice = 4,
        [Description("通报")] Notified = 5,
        [Description("报告")] Report = 6,
        [Description("请示")] Request = 7,
        [Description("批复")] Reply = 8,
        [Description("函")] Letter = 9,
        [Description("纪要")] Summary = 10
    }
}
