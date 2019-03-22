using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    public class UpdateWorkSummaryInput
    {
        public Guid Id { get; set; }

        public List<Guid> Files { get; set; }

        public UpdateWorkSummaryInput()
        {
            this.Files = new List<Guid>();
        }

    }
}
