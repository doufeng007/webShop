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
    [AutoMap(typeof(SupplyApplyMain))]
    public class SupplyApplyDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public long CreatorUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreationTime { get; set; }
        public bool IsImportant { get; set; }

        public int Status { get; set; }

        public List<SupplyApplySubDto> SupplyApplySub { get; set; }
        public List<SupplyApplySubBakDto> SupplyApplySubBak { get; set; }


        public SupplyApplyDto()
        {
            this.SupplyApplySub = new List<SupplyApplySubDto>();
            this.SupplyApplySubBak = new List<SupplyApplySubBakDto>();
        }
    }

}
