using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{

    public class EmployeeBusinessTripDto
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }


        public string Destination { get; set; }

        public DateTime PreBeginDate { get; set; }

        public DateTime PreEndDate { get; set; }

        public string PreSchedule { get; set; }


        public DateTime? BeginDate { get; set; }


        public DateTime? EndDate { get; set; }


        public string FeePlan { get; set; }


        public decimal PreFeeTotal { get; set; }

        public decimal FeeTotal { get; set; }

        public int Status { get; set; }


        public string StatusTitle { get; set; }

        public string Remark { get; set; }


        public List<EmployeeBusinessTripMemberpDto> Members { get; set; }

        public List<EmployeeBusinessTripTaskDto> Tasks { get; set; }

        public EmployeeBusinessTripDto()
        {
            this.Members = new List<EmployeeBusinessTripMemberpDto>();
            this.Tasks = new List<EmployeeBusinessTripTaskDto>();
        }

    }

    public class EmployeeBusinessTripMemberpDto
    {
        public Guid Id { get; set; }


        public long UserId { get; set; }

        public string UserName { get; set; }
        public string Remark { get; set; }
    }

    public class EmployeeBusinessTripTaskDto
    {
        public Guid Id { get; set; }

        public bool NotInPlan { get; set; }

        public string TaskName { get; set; }


        public EmployeeBusinessTripTaskCompleteStatus CompleteStatus { get; set; }


        public string Remark { get; set; }
    }


    public class GetEmployeeBusinessTripDtoInput : GetWorkFlowTaskCommentInput
    {
        public Guid Id { get; set; }
    }

    public class GetEmployeeBusinessTripOutput : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }


        public string Destination { get; set; }

        public DateTime PreBeginDate { get; set; }

        public DateTime PreEndDate { get; set; }

        public string PreSchedule { get; set; }


        public DateTime? BeginDate { get; set; }


        public DateTime? EndDate { get; set; }


        public string Schedule { get; set; }

        public string FeePlan { get; set; }


        public decimal PreFeeTotal { get; set; }

        public decimal FeeTotal { get; set; }

        public decimal FeeAccommodation { get; set; }

        public decimal FeeOther { get; set; }


        public int Status { get; set; }


        public string StatusTitle { get; set; }

        public string Remark { get; set; }


        public List<EmployeeBusinessTripMemberpDto> EmployeeBusinessTripMember { get; set; }

        public List<EmployeeBusinessTripTaskDto> EmployeeBusinessTripTask { get; set; }

        public GetEmployeeBusinessTripOutput()
        {
            this.EmployeeBusinessTripMember = new List<EmployeeBusinessTripMemberpDto>();
            this.EmployeeBusinessTripTask = new List<EmployeeBusinessTripTaskDto>();
        }
    }


    
}
