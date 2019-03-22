using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public class RealationSystem : Entity<Guid>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string ServiceUrl { get; set; }


        public SystemType SystemType { get; set; }


        public string Remark { get; set; }

        public long? UserId { get; set; }
    }

    public enum SystemType {
        企业版=1,
        政务版=2
    }
}
