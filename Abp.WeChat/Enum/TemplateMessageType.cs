using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WeChat.Enum
{
    public enum TemplateMessageBusinessTypeEnum
    {
        当前用户进货订单完成 = 1,
        当前用户进货订单上级缺货 = 2,
        下级代理进货订单完成 = 3,
        下级代理进货订单上级缺货 = 4,


        当前用户生成提货订单 = 5,


        下级代理升级成功 = 6,
        当前用户升级成功 = 7,


        当前用户申请代理审核通过 = 8,
        邀请下级代理审核通过 = 9,


        下级代理升级失败 = 10,
        当前用户升级失败 = 11,


        当前用户申请代理审核不通过 = 12,
        邀请下级代理审核不通过 = 13,


        充值成功 = 14,

        提现申请审核通过 = 15,
        提现申请审核不通过 = 16,






    }
}
