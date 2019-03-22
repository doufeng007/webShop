using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    /// <summary>
    /// 行政发放用品
    /// </summary>
    public class SupplySendDto
    {
        public long UserId { get; set; }

        public List<Guid> SupplyId { get; set; }
    }
}
