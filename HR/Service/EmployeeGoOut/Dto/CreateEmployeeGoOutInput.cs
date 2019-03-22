using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMapTo(typeof(EmployeeGoOut))]
    public class CreateEmployeeGoOutInput: CreateWorkFlowInstance
    {
        public long? UserId { get; set; }


        public DateTime GoOutTime { get; set; }

        public int GoOutHour { get; set; }

        public DateTime BackTime { get; set; }

        public string Reason { get; set; }


        public string OutTele { get; set; }

        public string Remark { get; set; }

    }
}
