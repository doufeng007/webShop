using Abp.AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ZCYX.FRMSCore.Application
{
    public class UpdateSettingInput
    {
        #region 表字段
        public List<UpdateSetting> updateSettings { get; set; } = new List<UpdateSetting>();		
        #endregion
    }
    public class UpdateSetting {
        public string Name { get; set; }
        public string  Value { get; set; }
    }
}