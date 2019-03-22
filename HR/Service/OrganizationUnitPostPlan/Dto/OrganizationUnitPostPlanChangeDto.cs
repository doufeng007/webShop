using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace HR
{
    public class OrganizationUnitPostPlanChangeDto
    {
        [LogColumn("编制人数", IsLog = true)]
        public int PrepareNumber { get; set; }
    }
}
