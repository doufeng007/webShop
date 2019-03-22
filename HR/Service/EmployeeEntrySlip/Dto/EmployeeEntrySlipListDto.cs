using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    public class EmployeeEntrySlipListDto
    {
        public Guid Id { get; set; }

        public Guid EmployeeInterviewId { get; set; }

        public string EmployeeInterviewName { get; set; }

        public string EmployeeNumber { get; set; }

        public DateTime EntryDate { get; set; }


        public long OrgId { get; set; }


        public string OrgName { get; set; }


        public Guid PostId { get; set; }

        public string PostName { get; set; }

        public int Status { get; set; }


        public string StatusTitle { get; set; }



    }
}
