using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapFrom(typeof(RealationSystem))]
    public class RealationSystemDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string ServiceUrl { get; set; }


        public int SystemType { get; set; }

        public string Remark { get; set; }

        public long? UserId { get; set; }

        public string UserName { get; set; }
    }
}
