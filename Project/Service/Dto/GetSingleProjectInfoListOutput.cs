using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetSingleProjectInfoListOutput
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string SingleProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string SingleProjectCode { get; set; }
        public string AppraisalTypeName { get; set; }
        public string SendUnitName { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
