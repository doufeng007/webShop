using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    [AutoMapTo(typeof(AbpPermissionBase))]
    public class CreateAbpPermissionBaseInput
    {

        public long? ParentId { get; set; }

        public string DisplayName { get; set; }

        public string Code { get; set; }

        public string MoudleName { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
}
