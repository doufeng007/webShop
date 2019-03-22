using System;
using System.Collections.Generic;
using System.Text;
using Abp.Organizations;
using Abp.Domain.Entities;

namespace ZCYX.FRMSCore.Application
{
    public class WorkFlowOrganizationUnits : OrganizationUnit, IMayHaveTenant
    {
        public int Type { get; set; }

        public int Status { get; set; }

        public int Sort { get; set; }

        public int Depth { get; set; }

        public int ChildsLength { get; set; }

        public string ChargeLeader { get; set; }

        public string Leader { get; set; }

        public string Note { get; set; }

    }
}
