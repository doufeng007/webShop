using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public class ContractWithSystem : Entity<Guid>
    {

        public Guid SystemId { get; set; }

        public long UserId { get; set; }

    }
}
