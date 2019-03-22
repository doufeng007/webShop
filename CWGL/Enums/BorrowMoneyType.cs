using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CWGL.Enums
{
    public enum BorrowMoney
    {
        [Description("备用金")] 备用金 = 0,
        [Description("普通借款")] 普通借款 = 1,
    }
    public enum SettleState
    {
        [Description("未结")] 未结 = 0,
        [Description("未结清")] 未结清 = 1,
        [Description("已结清")] 已结清 = 2,
    }
    public enum CredentialType
    {
        [Description("借款")] 借款 = 0,
        [Description("普通报销")] 普通报销 = 1,
        [Description("差旅费报销")] 差旅费报销 = 2,
        [Description("付款")] 付款 = 3,
        [Description("收款")] 收款 = 4,
    }
    public enum MoneyMode
    {
        [Description("现金")] 现金 = 0,
        [Description("银行转账")] 银行转账 = 1,
        [Description("微信")] 微信 = 2,
        [Description("支付宝")] 支付宝 = 3,
    }

    /// <summary>
    /// 公司处理结果
    /// </summary>
    public enum RefundResultType
    {
        财务应付款 = 1,
        财务应收款 = 2,
        无退无报 = 3,
        收付款 = 4,
    }


    public enum RefundActionType
    {
        收款 = 1,
        付款 = 2,

    }
}
