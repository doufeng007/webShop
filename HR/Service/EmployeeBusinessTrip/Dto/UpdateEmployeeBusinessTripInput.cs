using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    public class UpdateEmployeeBusinessTripInput
    {
        public Guid Id { get; set; }


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


        public List<UpdateEmployeeBusinessTripMemberpInput> EmployeeBusinessTripMember { get; set; }

        public List<UpdateEmployeeBusinessTripTaskInput> EmployeeBusinessTripTask { get; set; }

        public UpdateEmployeeBusinessTripInput()
        {
            this.EmployeeBusinessTripMember = new List<UpdateEmployeeBusinessTripMemberpInput>();
            this.EmployeeBusinessTripTask = new List<UpdateEmployeeBusinessTripTaskInput>();
        }


    }

    public class UpdateEmployeeBusinessTripMemberpInput
    {
        public Guid? Id { get; set; }

        public long UserId { get; set; }
    }

    public class UpdateEmployeeBusinessTripTaskInput : CreateEmployeeBusinessTripTaskInput
    {
        public Guid? Id { get; set; }

        public EmployeeBusinessTripTaskCompleteStatus CompleteStatus { get; set; }
    }
}
