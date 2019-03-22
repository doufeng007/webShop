using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    public enum SupplyStatus
    {
        在库 = 1,
        被领用 = 2,
        报废中 = 3,
        已报废 = 4,
        退还中 = 5,
        维修中 = 6,
        已修好 = 7,
        待领取 = 8
    }

    public enum SupplyBackSubStatus
    {
        申请中 = 0,
        已退还 = 1,
        驳回 = 2
    }
    public enum SupplyScrapSubStatus
    {
        申请中 = 0,
        已报废 = 1,
        驳回 = 2
    }
    public enum SupplyBackMainStatus
    {
        草稿 = 0,
        行政处理 = 1,
        处理完成 = -1
    }


    public enum RepairResultEnum
    {
        维修成功 = 1,
        维修失败 = 2,
    }
}
