using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply
{


    public class UpdateSupplyPurchaseInput
    {
        public Guid Id { get; set; }
        public List<UpdateSupplyPurchasePlanInput> Plans { get; set; }
    }

    /// <summary>
    /// 发起申购
    /// </summary>
    public class UpdateSupplyPurchasePlanInput : CreateSupplyPurchasePlanInput
    {
        public Guid? Id { get; set; }

        public Guid? SupplyApplyMainId { get; set; }

        public Guid? SupplyApplySubId { get; set; }
    }

    public class CreateOrUpdateSupplyPurchasePlanInput : UpdateSupplyPurchasePlanInput
    {
        public Guid MainId { get; set; }
    }


    public class RegisterSupplyPurchaseInput
    {
        public Guid SupplyPurchasePlanId { get; set; }

        public Guid? ResultId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }


        public string Unit { get; set; }

        public decimal Money { get; set; }

        public string Des { get; set; }

        public string UserId { get; set; }

        public int Type { get; set; }

    }


    public class RegisterSupplyPurchasePlanInput
    {
        [Required(ErrorMessage = "采购计划参数异常")]
        public Guid PlanId { get; set; }


        public List<RegisterSupplyPurchaseSupplyInput> Supplys { get; set; }


        public RegisterSupplyPurchasePlanInput()
        {
            this.Supplys = new List<RegisterSupplyPurchaseSupplyInput>();
        }
    }


    public class RegisterSupplyPurchaseSupplyInput
    {
        public Guid? ResultId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }


        public string Unit { get; set; }

        public decimal Money { get; set; }

        public string Des { get; set; }

        public string UserId { get; set; }

        public int Type { get; set; }


        /// <summary>
        /// 检定到期时间
        /// </summary>
        public DateTime? ExpiryDate { get; set; }


        public bool HasDo { get; set; } = false;


    }


    public class RegisterSupplyPurchaseOutput
    {
        public Guid ResultId { get; set; }
        public string Name { get; set; }

        public string Version { get; set; }


        public string Unit { get; set; }

        public decimal Money { get; set; }

        //public string Des { get; set; }

        public string UserId { get; set; }

        public string UserId_Name { get; set; }

        public string Code { get; set; }


        /// <summary>
        /// 是否用于申领，为true 则该行不能删除 只能编辑
        /// </summary>
        public bool IsUseApply { get; set; } = false;

    }


    public class GetSupplyPurchaseResultInput : PagedAndSortedInputDto
    {
        [Required]
        public Guid PlanId { get; set; }
    }

    public class UpdaterPurchasePlanStatusInput
    {
        public Guid PlanId { get; set; }

        /// <summary>
        ///  1 同意 2 驳回
        /// </summary>
        public int ActionType { get; set; }
    }

}
