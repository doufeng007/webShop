using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    public class UpdateEmployeeRegularInput
    {
        public Guid Id { get; set; }

        public DateTime ApplyDate { get; set; }

        public DateTime StrialBeginTime { get; set; }

        public DateTime StrialEndTime { get; set; }

        public string Remark { get; set; }

        public string WorkSummary { get; set; }
    }
}
