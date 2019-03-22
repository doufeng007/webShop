using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMapTo(typeof(EmployeeBusinessTrip))]
    public class CreateEmployeeBusinessTripInput : CreateWorkFlowInstance
    {

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


        public string Remark { get; set; }

        public List<EmployeeBusinessTripMemberDto> EmployeeBusinessTripMember { get; set; }


        public List<CreateEmployeeBusinessTripTaskInput> EmployeeBusinessTripTask { get; set; }

        public CreateEmployeeBusinessTripInput()
        {
            this.EmployeeBusinessTripMember = new List<EmployeeBusinessTripMemberDto>();
            this.EmployeeBusinessTripTask = new List<CreateEmployeeBusinessTripTaskInput>();
        }


    }
    public class EmployeeBusinessTripMemberDto{
        public long UserId { get; set; }

        public string Remark { get; set; }
    }
    public class CreateEmployeeBusinessTripTaskInput
    {
        public string TaskName { get; set; }

        public string Remark { get; set; }
    }



}
