using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    public enum SupplyType
    {
        固定资产 = 1,
        低值易耗品 = 2,
        无形资产 = 3,
    }

    public enum ApplyStatus
    {
        处理中 = 0,
        已发放 = 1,
        已领取 = 2,
        采购中 = 3

    }

    public enum SupplyRepairStatus {
        已处理=-1,
        已驳回=-2,
        申请维修=0,
        行政处理=1,
        等待领取=2,
        领导审核=3,
    }
}
