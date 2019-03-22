using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Service
{
    public class ChangeStatusInput
    {
        public Guid Id { get; set; }

        public ResumeStatus Status {get;set;}
    }
}
