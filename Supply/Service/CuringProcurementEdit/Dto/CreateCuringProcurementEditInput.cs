using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace Supply
{
    [AutoMapTo(typeof(CuringProcurementEdit))]
    public class CreateCuringProcurementEditInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// MainId
        /// </summary>
        public Guid MainId { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// NeedMember
        /// </summary>
        public string NeedMember { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// ExecuteSummary
        /// </summary>
        public string ExecuteSummary { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        public List<CreateCuringProcurementPlanInput> Plans { get; set; }

        public CreateCuringProcurementEditInput()
        {
            this.Plans = new List<CreateCuringProcurementPlanInput>();
        }


        #endregion
    }


    public class UpdateCuringProcurementEditInput

    {
        public Guid Id { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// NeedMember
        /// </summary>
        public string NeedMember { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// ExecuteSummary
        /// </summary>
        public string ExecuteSummary { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }


        public List<CreateOrUpdateCuringProcurementPlanInput> Plans { get; set; }

        public UpdateCuringProcurementEditInput()
        {
            this.Plans = new List<CreateOrUpdateCuringProcurementPlanInput>();
        }

    }
}