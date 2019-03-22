using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMapTo(typeof(EmployeeRegular))]
    public class CreateEmployeeRegularInput: CreateWorkFlowInstance
    {
        public Guid EmployeeId { get; set; }

        public DateTime ApplyDate { get; set; }

        public bool IsAdvanced { get; set; }


        /// <summary>
        /// 提前申请的工作业绩补充
        /// </summary>
        public string WorkSummary { get; set; }


        public DateTime StrialBeginTime { get; set; }

        public DateTime StrialEndTime { get; set; }


        public string Remark { get; set; }


    }
}
