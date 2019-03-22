using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public enum GW_SealTypeEnmu
    {
        [Description("电子印章")] 电子印章 = 1,
        [Description("实体印章")] 实体印章 = 2,
    }

    public enum GW_SealStatusEnmu
    {
        [Description("启用")] 启用 = 1,
        [Description("禁用")] 禁用 = 2,
        [Description("作废")] 作废 = 3,
    }


    public enum GW_EmployeesSignTypelEnmu
    {
        [Description("手写")] 手写 = 1,
        [Description("图片")] 图片 = 2,
    }
    public enum GW_EmployeesSignStatusEnmu
    {
        [Description("启用")] 启用 = 1,
        [Description("禁用")] 禁用 = 2,
    }


}
