using Abp.Runtime.Validation;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply
{
    public class SupplyApplyListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public long CreatorUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreationTime { get; set; }
        public bool IsImportant { get; set; }


        public List<SupplyApplySubBaseDto> Subs { get; set; }

        public SupplyApplyListDto()
        {
            this.Subs = new List<SupplyApplySubBaseDto>();
        }

    }


    public class SupplyPurchaseListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public long CreatorUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreationTime { get; set; }
        public string Code { get; set; }

    }


}
