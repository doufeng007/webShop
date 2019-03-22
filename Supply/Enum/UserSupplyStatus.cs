using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    public enum UserSupplyStatus
    {
        已退还 = 2,
        使用中 = 0,
        退还中=1,
        报废中 = 3,
        已报废 = 4,
        维修中=5,
        已修好=6
    }
}
