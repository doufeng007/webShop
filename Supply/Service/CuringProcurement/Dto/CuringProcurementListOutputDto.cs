using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    [AutoMapFrom(typeof(CuringProcurement))]
    public class CuringProcurementListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// NeedMember
        /// </summary>
        public string NeedMember { get; set; }


        public string NeedMember_Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }


        public string TypeName { get; set; }

        /// <summary>
        /// ExecuteSummary
        /// </summary>
        public string ExecuteSummary { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        


    }
}
