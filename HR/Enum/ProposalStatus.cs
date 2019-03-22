using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HR
{
    public enum ProposalStatus
    {
        [Description("未读")] UnRead = 0,
        [Description("已读")] Readed = 1,
    }
}
