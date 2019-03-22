using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    public class SupplyApplySubBakDto
    {
        public Guid Id { get; set; }

        public Guid MainId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }


        public DateTime? ReceiveTime { get; set; }

        public Guid? SupplyId { get; set; }

        public string UserId { get; set; }


        public string UserId_Name { get; set; }
        /// <summary>
        /// 0:固定资产 1：低值易耗品 2：无形资产
        /// </summary>
        public int Type { get; set; }


        public string TypeName { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }


        public DateTime CreationTime { get; set; }

        public int DoPurchaseStatus { get; set; }



        public DateTime ApplyDateTime { get; set; }

    }
}
