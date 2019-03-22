using Abp.File;
using Abp.Runtime.Validation;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply
{
    public class SupplyPurchaseQDFromApplyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }

        public string Status { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " Status, CreationTime desc";
            }
        }
    }


    public class SupplyPurchaseQDOneFromApplyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }

        public Guid SupplyApplyMainId { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " Status, CreationTime desc";
            }
        }
    }




    public class AddSupplyPurchasePlanInput
    {

        public Guid? PlanId { get; set; }

        public Guid? SupplyApplyMainId { get; set; }

        public Guid? SupplyApplySubId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public string UserId { get; set; }

        public int Type { get; set; }

    }


    public class SubmitSupplyPurchasePlansInput : CreateWorkFlowInstance
    {
        public List<Guid> PlanId { get; set; }
    }


    public class CreateSupplyPruchasePlanInstanceInput : CreateWorkFlowInstance
    {
        public string Code { get; set; }

        public Guid Id { get; set; }
    }



    public class SupplyPurchasePlansListOutput
    {
        public Guid Id { get; set; }

        public Guid? SupplyApplyMainId { get; set; }

        public Guid? SupplyApplySubId { get; set; }


        public Guid? SupplyPurchaseId { get; set; }

        public string SupplyPurchaseCode { get; set; }


        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public string UserId { get; set; }

        public string User_Name { get; set; }

        public int Type { get; set; }


        public string TypeName { get; set; }


        public int DoPurchaseStatus { get; set; }


        public string DoPurchaseStatusTitle { get; set; }


        public string ApplyUserName { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public string PutInData { get; set; }


        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 发票
        /// </summary>
        public List<GetAbpFilesOutput> PutInFileList { get; set; }


        public Entity.SupplyApplyMain SupplyApply { get; set; }


        /// <summary>
        /// 申领时间
        /// </summary>
        public string SupplyApplyTime { get; set; }


        //public string SupplyApplyApplyName { get; set; }


        public SupplyPurchasePlansListOutput()
        {
            this.PutInFileList = new List<GetAbpFilesOutput>();
        }
    }


    public class GetSupplyPurchasePlansInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 1 采购计划 2已办采购 3已审采购
        /// </summary>
        public int ActionType { get; set; }



    }
}
