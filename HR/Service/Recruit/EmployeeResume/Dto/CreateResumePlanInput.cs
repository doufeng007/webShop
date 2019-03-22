using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Service
{
    public class CreateResumePlanInput
    {
        public Guid ResumeId { get; set; }

        public Guid PostId { get; set; }
    }
}
