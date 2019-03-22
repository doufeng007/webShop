using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    [AutoMap(typeof(AbpPermissionBase))]
    public class AbpPermissionBaseDto : FullAuditedEntity<long>, IEntityDto<long>
    {

        public int? TenantId { get; set; }

        public long? ParentId { get; set; }

        public string DisplayName { get; set; }

        public string Code { get; set; }


        public string MoudleName { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
}
