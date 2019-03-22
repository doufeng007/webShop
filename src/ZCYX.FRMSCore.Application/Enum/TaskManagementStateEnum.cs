using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public enum TaskManagementStateEnum
    {
        /// <summary>
        /// 审批中
        /// </summary>
        [Description("审批中")] Approvaling = 0,
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")] Reject = 1,
        /// <summary>
        /// 待开始
        /// </summary>
        [Description("待开始")] BefreStart = 2,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")] Doing = 3,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")] Done = 4,
        /// <summary>
        /// 重做
        /// </summary>
        [Description("重做")] Redo = 5,
        /// <summary>
        /// 撤销
        /// </summary>
        [Description("撤销")] Revoke = 6,
    }
}
