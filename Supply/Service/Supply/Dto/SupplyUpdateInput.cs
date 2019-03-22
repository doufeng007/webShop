using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    [AutoMap(typeof(SupplyBase))]
    public class SupplyUpdateInput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        public string Version { get; set; }


        public decimal Money { get; set; }


        public int Type { get; set; }


        public int Status { get; set; }

        public string Code { get; set; }


        public string UserId { get; set; }

        public DateTime? ProductDate { get; set; }


        public DateTime? ExpiryDate { get; set; }


        public string Unit { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? PutInDate { get; set; }


        public SupplyUpdateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }
}
