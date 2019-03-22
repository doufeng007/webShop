using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(OAFixedAssetsPurchase))]
    public class OAFixedAssetsPurchaseListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string ApplyUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplyType { get; set; }

        public string FeeSource { get; set; }



    }
}
