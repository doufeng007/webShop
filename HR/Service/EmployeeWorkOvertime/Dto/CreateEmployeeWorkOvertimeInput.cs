using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMapTo(typeof(EmployeeWorkOvertime))]
    public class CreateEmployeeWorkOvertimeInput : CreateWorkFlowInstance
    {

        public DateTime ApplyDate { get; set; }


        public string Reason { get; set; }

        public int PreHours { get; set; }

        public int? Hours { get; set; }


        public string Remark { get; set; }

        public List<EmployeeWorkOvertimeMemberDto> EmployeeWorkOvertimeMember { get; set; }


        public CreateEmployeeWorkOvertimeInput()
        {
            this.EmployeeWorkOvertimeMember = new List<EmployeeWorkOvertimeMemberDto>();
        }


    }
    public class EmployeeWorkOvertimeMemberDto {
        public long UserId { get; set; }
    }




}
