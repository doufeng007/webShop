using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply.Service
{
    /// <summary>
    /// 个人用品申请主表
    /// </summary>
    [AutoMap(typeof(SupplyApplyMain))]
    public class CreateApplyDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public bool IsImportant { get; set; }
        public List<CreateApplySubDto> SupplyApplySub { get; set; }


        public bool IsUpdateForChange { get; set; }
    }



    /// <summary>
    /// 个人用品申请明细表
    /// </summary>
    [AutoMap(typeof(SupplyApplySub), typeof(SupplyApplySubBak))]
    public class CreateApplySubDto
    {
        public Guid? Id { get; set; }
        public Guid MainId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public Guid? SupplyId { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 1:固定资产 2：低值易耗品 3：无形资产
        /// </summary>
        public int Type { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }
    }

    [AutoMap(typeof(SupplyApplySub))]
    public class UpdateApplySubDto
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

        public Guid? SupplyId { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 0:固定资产 1：低值易耗品 2：无形资产
        /// </summary>
        public int Type { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }
    }

    /// <summary>
    /// 个人用品申请明细表
    /// </summary>
    [AutoMap(typeof(SupplyApplySub))]
    public class ApplySubDto
    {
        public Guid Id { get; set; }
        public Guid MainId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public decimal? Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public Guid? SupplyId { get; set; }

        public string UserId { get; set; }

        public int Status { get; set; }
        public string StatusTitle { get; set; }

        public string UserId_Name { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }

    }




    public class ApplyResultDto
    {
        public Guid ApplySubId { get; set; }
        public Guid ApplyResultId { get; set; }

        public string Name { get; set; }


        public string Code { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public decimal? Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public Guid? SupplyId { get; set; }

        public string UserId { get; set; }

        public int Status { get; set; }
        public string StatusTitle { get; set; }

        public string UserId_Name { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }

        public DateTime CreationTime { get; set; }


        public DateTime ApplyTime { get; set; }

    }

    public class SearchSupply
    {
        public string SearchKey { get; set; }
    }

    [AutoMapFrom(typeof(SupplyBase))]
    public class SupplySelectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public decimal Money { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }



        public int Status { get; set; }


        public string Code { get; set; }
    }

    /// <summary>
    /// 行政处理申领
    /// </summary>
    public class GrantDto
    {
        /// <summary>
        /// 申领明细ID
        /// </summary>
        public Guid ApplySubId { get; set; }

        /// <summary>
        /// 发放物品ID
        /// </summary>
        public List<Guid> SupplyIds { get; set; }

        /// <summary>
        ///  已发放 = 1,已驳回 = 2,申购中 = 3,
        /// </summary>
        public SupplyApplySubResultType ActionType { get; set; }
    }



    /// <summary>
    /// 行政新增用品申领明显
    /// </summary>
    public class CreateOneSupplyApplySubInput
    {
        public Guid MainId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public Guid? SupplyId { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 0:固定资产 1：低值易耗品 2：无形资产
        /// </summary>
        public int Type { get; set; }

        public int Result { get; set; }

        public string ResultRemark { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }
    }

    
}
