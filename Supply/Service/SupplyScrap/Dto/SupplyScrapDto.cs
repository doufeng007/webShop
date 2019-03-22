using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using CWGL.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply.Service
{
    /// <summary>
    /// 报废申请
    /// </summary>
    public class CreateSupplyScrapMainInput : CreateWorkFlowInstance
    {
        public List<CreateSupplyScrapSubInput> SupplyScrapSub { get; set; }


    }


    public class CreateSupplyScrapSubInput
    {
        public Guid SupplyId { get; set; }


        public Guid UserSupplyId { get; set; }

        public string Reason { get; set; }
    }

    public class SupplyScrapInput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 0报废1驳回
        /// </summary>
        public int Type { get; set; }
    }
    /// <summary>
    /// 报废申请明细
    /// </summary>
    [AutoMap(typeof(SupplyScrapMain))]
    public class SupplyScrapMainDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public List<SupplyScrapSubDto> SupplyScrapSub { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string UserId_Name { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }



    public class GetSupplyScrapMainDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public List<SupplyScrapSubDto> SupplyScrapSub { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string UserId_Name { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }





    /// <summary>
    /// 报废申请详情
    /// </summary>
    [AutoMap(typeof(SupplyScrapSub))]
    public class SupplyScrapSubDto : BusinessWorkFlowListOutput
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 报废申请状态 0:申请中  1：已报废
        /// </summary>


        /// <summary>
        /// 物品id
        /// </summary>
        public Guid SupplyId { get; set; }
        /// <summary>
        /// 主表id
        /// </summary>
        public Guid? MainId { get; set; }
        public string Name { get; set; }

        public string Version { get; set; }

        public decimal Money { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string UserId_Name { get; set; }
        public DateTime? ProductDate { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public Guid FirstTaskId { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public Guid FirstStepId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Guid FirstGroupId { get; set; }


        public string Reason { get; set; }


        public decimal? PreResidueValue { get; set; }


        public long UserSupply_UserId { get; set; }

        public string UserSupply_UserName { get; set; }
    }


    public class GetSupplyScrapSubDto : WorkFlowTaskCommentResult
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 报废申请状态 0:申请中  1：已报废
        /// </summary>


        /// <summary>
        /// 物品id
        /// </summary>
        public Guid SupplyId { get; set; }
        /// <summary>
        /// 主表id
        /// </summary>
        public Guid? MainId { get; set; }
        public string Name { get; set; }

        public string Version { get; set; }

        public decimal Money { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string UserId_Name { get; set; }
        public DateTime? ProductDate { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public Guid FirstTaskId { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public Guid FirstStepId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Guid FirstGroupId { get; set; }


        public string Reason { get; set; }


        public decimal? PreResidueValue { get; set; }


        public long UserSupply_UserId { get; set; }

        public string UserSupply_UserName { get; set; }
    }



    public class SubmitSupplyScrapInput
    {
        public Guid SubId { get; set; }

        public decimal PreValue { get; set; }
    }


    public class SupplyScrapListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public string Status { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }



    public class SupplyScrapUpdateInput
    {
        public Guid Id { get; set; }


        public Guid FlowId { get; set; }


        public List<SupplyScrapSubTodoUpdateInput> SubList { get; set; } = new List<SupplyScrapSubTodoUpdateInput>();

    }

    public class SupplyScrapSubTodoUpdateInput
    {
        public Guid SubId { get; set; }

        public RefundResultType CWType { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amout_Pay { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Amout_Gather { get; set; }

    }



}
