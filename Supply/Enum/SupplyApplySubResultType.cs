using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    public enum SupplyApplySubResultType
    {
        处理中 = 0,
        已发放 = 1,
        已驳回 = 2,
        申购 = 3,
        已领取 = 4,

    }

    public enum SupplyApplyMainResult
    {
        处理中 = 0,
        已处理 = 1
    }


    public enum SupplyApplySubStatusType
    {
        处理中 = 0,
        已发放 = 1,
        已驳回 = 2,
        申购中 = 3,
        已加入采购计划 = 4,
        已提交采购计划 = 5,
        采购申请驳回 = 6,
        采购申请同意 = 7,
        采购入库 = 8,
        已领取 = 9,

    }
}
