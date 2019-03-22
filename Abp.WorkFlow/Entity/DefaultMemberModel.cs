using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class DefaultMemberModel : Entity<Guid>
    {
        public string Key { get; set; }
    }

    public class SqlConditionResultModel : Entity<Guid>
    {
        public int Count { get; set; }
    }
}
