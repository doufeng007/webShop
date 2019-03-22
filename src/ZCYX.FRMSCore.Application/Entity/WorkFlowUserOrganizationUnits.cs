using System;
using System.Collections.Generic;
using System.Text;
using Abp.Organizations;
using Abp.Domain.Entities;
using Abp.Authorization.Users;

namespace ZCYX.FRMSCore.Application
{
    public class WorkFlowUserOrganizationUnits : UserOrganizationUnit, IMayHaveTenant
    {
        public bool IsMain { get; set; }

    }
}
