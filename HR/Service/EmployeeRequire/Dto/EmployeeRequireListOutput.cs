using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
   public class EmployeeRequireListOutput
    {
        public Guid Id { get; set; }
        public long Department { get; set; }
        public string  DepartmentName { get; set; }
        public Guid? Job { get; set; }
        public string JobName { get; set; }
        public DateTime ApplyTime { get; set; }
        public long CreatorUserId { get; set; }

        public string CreatorUserName { get; set; }

        public int Status { get; set; }

        public string StatusTitle { get; set; }
    }
}
