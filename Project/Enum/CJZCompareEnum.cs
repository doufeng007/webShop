using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public enum CJZCompareEnum
    {
        送审资料与数据库对比 = 1,

        初审结果与数据库对比 = 2,

        复核结果与初审结果对比 = 4,

        二级复核结果与复核结果对比 = 8,

        三级复核结果与二级复核结果对比 = 16,

    }
}
