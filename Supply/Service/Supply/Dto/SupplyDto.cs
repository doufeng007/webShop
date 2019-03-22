using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    [AutoMapFrom(typeof(SupplyBase))]
    public class SupplyDto : SupplyCreateInput
    {
        public Guid Id { get; set; }

        public int Status { get; set; }

        public string StatusTitle { get; set; }


        public string UserId_Name { get; set; }

        public string Unit { get; set; }


        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? PutInDate { get; set; }
    }
}
