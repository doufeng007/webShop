using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{


    public class CreateSupplyPurchaseInput : CreateWorkFlowInstance
    {
        public List<CreateSupplyPurchasePlanInput> Plans { get; set; }
    }

    /// <summary>
    /// 发起申购
    /// </summary>
    public class CreateSupplyPurchasePlanInput
    {

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 0:固定资产 1：低值易耗品 2：无形资产
        /// </summary>
        public int Type { get; set; }
    }

}
