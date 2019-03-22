using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HR
{
    public enum ProposalType
    {
        [Description("提案")] proposal = 0,
        [Description("申诉")] Appeal = 1,
        [Description("招聘")] Recruit = 2
    }
}
