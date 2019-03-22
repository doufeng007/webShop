using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaskGL.Enum
{
    public enum TaskManagementTypeEnum
    {

        [Description("线下任务")] Offline = 0,

        /// <summary>
        /// 拟稿
        /// </summary>
        [Description("拟稿")] Draft = 1,
        /// <summary>
        /// 归档
        /// </summary>
        [Description("归档")] File = 2,
        /// <summary>
        /// 用车
        /// </summary>
        [Description("用车")] Car = 3,
        /// <summary>
        /// 用品申领
        /// </summary>
        [Description("用品申领")] Apply = 4
    }
    public enum TaskManagementChangeTypeEnum
    {
        /// <summary>
        /// 任务重做
        /// </summary>
        [Description("任务重做")] TaskRedo = 0,
        /// <summary>
        /// 变更任务
        /// </summary>
        [Description("变更任务")] ChangeMission = 1,
        /// <summary>
        /// 撤销任务
        /// </summary>
        [Description("撤销任务")] RevokeTask = 2,
    }


    public enum TaskManagementReasonEnum
    {
        /// <summary>
        /// 需求变更
        /// </summary>
        [Description("需求变更")] Update = 0,
        /// <summary>
        /// 资料有误
        /// </summary>
        [Description("资料有误")] Error = 1,
        /// <summary>
        /// 任务取消
        /// </summary>
        [Description("任务取消")] Cancel = 2
    }

    public enum CreateUserRoleTypeEnum
    {
        [Description("办公室主任")]
        BGSZR = 0,
        [Description("部门领导")]
        DepartLeader = 1,
        [Description("部门分管领导")]
        OrgFGLD = 2,
        [Description("总经理")]
        ZJL = 3,
    }
}
