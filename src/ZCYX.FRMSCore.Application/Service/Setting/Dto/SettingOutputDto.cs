using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public class SettingOutputDto 
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime CreationTime { get; set; }
    }

}
