using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    /// <summary>
    /// 用品申领-申购处理状态
    /// </summary>
    public enum SupplyPurchaseQDStatus
    {
        未处理 = 0,
        未完成 = 1,
        已完成 = 2,
    }

    /// <summary>
    /// 申购清单处理状态
    /// </summary>
    public enum SupplyPurchaseQOneDStatus
    {
        未处理 = 0,
        已加入采购计划 = 1,
        待审批 = 2,
        同意 = 3,
        驳回 = 4,
        已入库 = 5,

    }
}
