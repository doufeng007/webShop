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

        订单 = 1,
        款项 = 2
    }


    public enum GoodsType
    {
        试用 = 1,
        云仓下单 = 2,
        非云仓下单 = 3,
        直购 = 4,

    }

    public enum GoodStatusEnum
    {
        上架 = 0,
        下架 = 1,
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
        上级缺货 = 1,
        已完成 = 2
    }


    public enum B_AgencyAcountStatusEnum
    {
        正常 = 0,
        封号 = 1
    }


    public enum B_AgencyApplyStatusEnum
    {
        待审核 = 0,
        已通过 = 1,
        未通过 = 2
    }


    public enum B_PrePayStatusEnum
    {
        待审核 = 0,
        已通过 = 1,
        未通过 = 2
    }

    public enum B_WithdrawalStatusEnum
    {
        待审核 = 0,
        待打款 = 1,
        已打款 = 2,
        未通过 = 3,
        打款异常 = 4
    }


    public enum B_InventoryAddConfigEnum
    {
        否 = 0,
        是 = 1
    }


    public enum PayAccountType
    {
        支付宝 = 1,
        银行卡 = 2,

    }


    public enum PayAccountStatus
    {
        上线 = 0,
        下线 = 1
    }

    public enum NoticeStatusEnum
    {
        草稿 = 0,
        正常 = 1,
        撤销 = 2
    }

    public enum OrderAmoutEnum
    {
        出账 = 0,
        入账 = 1
    }

    public enum OrderAmoutBusinessTypeEnum
    {
        进货 = 1,
        提货 = 2,
        提现 = 3,
        充值 = 4,
        团队管理奖金 = 5,
        保证金 = 6,
        推荐奖金 = 7
    }

    public enum OrderOutStauts
    {
        待发货 = 0,
        已发货 = 1,
        已完成 = 2
    }


    public enum FirestLevelCategroyProperty
    {
        进提货 = 1,
        直购 = 2,
        试装 = 3
    }


    public enum CWDetailTypeEnum
    {
        入仓 = 1,
        出仓 = 2
    }

    public enum CWDetailBusinessTypeEnum
    {
        自身进货入仓 = 1,
        下级进货自身出仓 = 2,
        自身提货出仓 = 3,

    }

    public enum AgencyApplyEnum
    {
        代理邀请 = 0,
        代理升级 = 1
    }
}
