using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{

    public class EmployeeWorkOvertimeDto
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }


        public DateTime ApplyDate { get; set; }

        public string Reason { get; set; }

        public int PreHours { get; set; }

        public int? Hours { get; set; }


        public string Remark { get; set; }


        public int Status { get; set; }


        public string StatusTitle { get; set; }



        public List<EmployeeWorkOvertimeMemberpDto> EmployeeWorkOvertimeMember { get; set; }


        public EmployeeWorkOvertimeDto()
        {
            this.EmployeeWorkOvertimeMember = new List<EmployeeWorkOvertimeMemberpDto>();
        }

    }

    public class EmployeeWorkOvertimeMemberpDto
    {
        public Guid Id { get; set; }


        public long UserId { get; set; }

        public string UserName { get; set; }
    }

    public class GetEmployeeWorkOvertimeDtoInput : GetWorkFlowTaskCommentInput
    {
        public Guid Id { get; set; }
    }

    public class GetEmployeeWorkOvertimeOutput : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }


        public DateTime ApplyDate { get; set; }

        public string Reason { get; set; }

        public int PreHours { get; set; }

        public int? Hours { get; set; }



        public int Status { get; set; }


        public string StatusTitle { get; set; }

        public string Remark { get; set; }


        public List<EmployeeWorkOvertimeMemberpDto> EmployeeWorkOvertimeMember { get; set; }


        public GetEmployeeWorkOvertimeOutput()
        {
            this.EmployeeWorkOvertimeMember = new List<EmployeeWorkOvertimeMemberpDto>();
        }
    }


    
}
