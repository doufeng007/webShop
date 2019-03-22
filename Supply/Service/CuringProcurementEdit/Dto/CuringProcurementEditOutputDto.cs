using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    [AutoMapFrom(typeof(CuringProcurementEdit))]
    public class CuringProcurementEditOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

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

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        public List<CuringProcurementPlanOutputDto> Plans { get; set; }


        public CuringProcurementEditOutputDto()
        {
            this.Plans = new List<CuringProcurementPlanOutputDto>();
        }


    }
}
