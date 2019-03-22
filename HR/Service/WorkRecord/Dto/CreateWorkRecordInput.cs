using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using CWGL;
using System;

namespace HR
{
    [AutoMapTo(typeof(WorkRecord))]
    public class CreateWorkRecordInput: CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// BusinessId
        /// </summary>
        public Guid BusinessId { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Remuneration
        /// </summary>
        public decimal Remuneration { get; set; }


        /// <summary>
        /// 数字化绩效
        /// </summary>
        public decimal DataPerformance { get; set; }
        /// <summary>
        /// 非数字化绩效
        /// </summary>
        public decimal NoDataPerformance { get; set; }


        #endregion
    }



    public class UpdateWorkRecordInput :ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        #region 表字段


        public Guid Id { get; set; }

        public bool IsUpdateForChange { get; set; }


        public Guid FlowId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// BusinessId
        /// </summary>
        public Guid BusinessId { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Remuneration
        /// </summary>
        public decimal Remuneration { get; set; }

        /// <summary>
        /// 数字化绩效
        /// </summary>
        public decimal DataPerformance { get; set; }
        /// <summary>
        /// 非数字化绩效
        /// </summary>
        public decimal NoDataPerformance { get; set; }

        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
        public bool IsSaveFAC { get; set; }



        #endregion
    }
}