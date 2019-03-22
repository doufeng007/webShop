using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
   public class CommendResumeListOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Guid? Job { get; set; }

        public string JobName { get; set; }
        public string Phone { get; set; }

        public DateTime CreationTime { get; set; }
        public int Status { get; set; }

        public string StatusTitle { get; set; }
    }
}
