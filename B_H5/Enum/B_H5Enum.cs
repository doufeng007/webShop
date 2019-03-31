using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5
{
    /// <summary>
    /// 消息类别
    /// </summary>
    public enum B_H5MesagessType
    {

        提货订单 = 1,
        订单 = 2,
        提现 = 3,
    }


    public enum GoodsType
    {
        试用 = 1,
        云仓下单 = 2,
        非云仓下单 = 3,
        直购 = 4,

    }

    /// <summary>
    /// 代理类别
    /// </summary>
    public enum B_AgencyTypeEnum
    {
        直属代理 = 1,
        分销商 = 2,
    }

    public enum B_CWInOrOutEnum
    {
        出仓 = 1,
        入仓 = 2
    }


    /// <summary>
    /// 进货订单状态
    /// </summary>
    public enum InOrderStatusEnum
    {
        待充值 = 1,
        待审核 = 2,
        上级缺货 = 3,
        已完成 = 4
    }

}
