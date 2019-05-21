using System;
using System.Collections.Generic;
using System.ComponentModel;
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


    public enum SystemAmoutType
    {
        订单 = 1,
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


    public enum BonusRuleStatusEnum
    {
        有效 = 1,
        失效 = 2

    }


    public enum B_AgencySalesBusinessTypeEnum
    {
        销售额 = 1,
        利润 = 2,
        提货奖金 = 3,

        /// <summary>
        /// 团队奖金
        /// </summary>
        销售折扣 = 4,


    }

    public enum B_SyatemAmoutStatisType
    {
        销售额 = 1,
        充值金额 = 2,
        提现金额 = 3,
        保证金 = 4,
        //奖励金额 = 5,
        推荐奖 = 6,
        提货奖 = 7,
        销售返点奖 = 8



    }


    /// <summary>
    /// 发文对接操作类型
    /// </summary>
    public enum FineexMehodEnum
    {
        /// <summary>
        /// 出入库单确认
        /// </summary>
        [Description("出入库单确认")]
        FineexWmsPurchaseOutinorderConfirm = 1,


        /// <summary>
        /// 订单确认
        /// </summary>
        [Description("订单确认")]
        FineexWmsTradeConfirm = 2,


        /// <summary>
        /// 退货单确认
        /// </summary>
        [Description("退货单确认")]
        FineexWmsTradeReturnorderConfirm = 3,


        /// <summary>
        /// 快递物流详情查询
        /// </summary>
        [Description("快递物流详情查询")]
        FineexWmsTradeWaybillprocessGet = 3,


    }
}
