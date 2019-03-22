using System;
using System.Collections.Generic;
using System.Text;
using Train.Enum;

namespace Train
{
    public class DepartmentImageInput
    {
        /// <summary>
        /// 是否必修
        /// </summary>
        public bool? IsMustLearn { get; set; }

        /// <summary>
        /// 查询类别
        /// </summary>
        public DepartmentStatisticType Type { get; set; }

        /// <summary>
        /// 统计时间-年
        /// </summary>
        public int StatisticYear { get; set; }

        /// <summary>
        /// 统计时间-月
        /// </summary>
        public int StatisticMonth { get; set; }
    }
}
