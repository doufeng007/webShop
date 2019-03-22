using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class GetProjectAuditStopForEidtInput
    {
        public Guid? Id { get; set; }
        public Guid ProjectBaseId { get; set; }
    }
}
