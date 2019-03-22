using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    public class UpdateEmployeeWorkOvertimeInput
    {
        public Guid Id { get; set; }


        public DateTime ApplyDate { get; set; }

        public string Reason { get; set; }

        public int PreHours { get; set; }

        public int? Hours { get; set; }

        public string Remark { get; set; }


        public List<UpdateEmployeeWorkOvertimeMemberpInput> EmployeeWorkOvertimeMember { get; set; }


        public UpdateEmployeeWorkOvertimeInput()
        {
            this.EmployeeWorkOvertimeMember = new List<UpdateEmployeeWorkOvertimeMemberpInput>();
        }


    }

    public class UpdateEmployeeWorkOvertimeMemberpInput
    {
        public Guid? Id { get; set; }

        public long UserId { get; set; }
    }

    
}
