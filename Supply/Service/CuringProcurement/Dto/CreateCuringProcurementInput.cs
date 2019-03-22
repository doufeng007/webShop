using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    [AutoMapTo(typeof(CuringProcurement))]
    public class CreateCuringProcurementInput : CreateWorkFlowInstance
    {
        #region 表字段
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


        public List<CreateCuringProcurementPlanInput> Plans { get; set; }


        #endregion
    }

    public class UpdateCuringProcurementInput

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
    }
}