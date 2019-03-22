using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    public enum EmployeeResignType
    {
        辞职=0,
        辞退=1,
        自动离职=2,
        合同到期=3,
        其他=4
    }

    public enum EmployeeResignStatus {
        驳回=-2,
        已通过=-1,
        草稿=0,
        沟通=1,
        部门审核=2,
        领导审核=3
    }
}
