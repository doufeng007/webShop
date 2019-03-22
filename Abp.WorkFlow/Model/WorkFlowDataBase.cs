using Abp.WorkFlow.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    /// <summary>
    /// 数据库连接结构体
    /// </summary>
    [Serializable]
    public class WorkFlowDataBases
    {
        /// <summary>
        /// 连接ID
        /// </summary>
        public Guid LinkID { get; set; }
        /// <summary>
        /// 连接名称
        /// </summary>
        public string LinkName { get; set; }
        /// <summary>
        /// 连接表
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 表主键
        /// </summary>
        public string PrimaryKey { get; set; }
    }

    /// <summary>
    /// 标题字段结构体
    /// </summary>
    [Serializable]
    public class WorkFlowTitleField
    {
        /// <summary>
        /// 连接ID
        /// </summary>
        public Guid LinkID { get; set; }
        /// <summary>
        /// 连接名称
        /// </summary>
        public string LinkName { get; set; }
        /// <summary>
        /// 连接表
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Field { get; set; }
    }

    /// <summary>
    /// 步骤实体类
    /// </summary>
    [Serializable]
    public class WorkFlowStep
    {
        /// <summary>
        /// 步骤ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 步骤类型 normal 一般步骤 subflow 子流程步骤
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 步骤名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 意见显示 0不显示 1显示
        /// </summary>
        public int OpinionDisplay { get; set; }
        /// <summary>
        /// 超期提示 0不提示 1要提示
        /// </summary>
        public int ExpiredPrompt { get; set; }
        /// <summary>
        /// 审签类型 0无签批意见栏 1有签批意见(无须签章) 2有签批意见(须签章)
        /// </summary>
        public int SignatureType { get; set; }

        /// <summary>
        /// 当前步骤意见显示的标题
        /// </summary>
        public string SugguestionTitle { get; set; }
        /// <summary>
        /// 工时(小时)
        /// </summary>
        public decimal? WorkTime { get; set; }
        /// <summary>
        /// 限额时间(小时)
        /// </summary>
        public decimal? LimitTime { get; set; }
        /// <summary>
        /// 额外时间(小时)
        /// </summary>
        public decimal? OtherTime { get; set; }
        /// <summary>
        /// 是否归档 0不归档 1要归档
        /// </summary>
        public int Archives { get; set; } = 0;

        /// <summary>
        /// 待办类型 0为流程待办 1位事项待办
        /// </summary>
        public int TodoType { get; set; } = 0;

        /// <summary>
        /// 归档参数
        /// </summary>
        public string ArchivesParams { get; set; }
        /// <summary>
        /// 数据保存方式 0共同编辑(多人编辑一条数据) 1独立编辑(每个人新增一条数据)
        /// </summary>
        public int DataSaveType { get; set; } = 0;
        /// <summary>
        /// 当为独立编辑里的查询条件
        /// </summary>
        public string DataSaveTypeWhere { get; set; }
        /// <summary>
        /// 步骤备注说明
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 步骤发送后提示语
        /// </summary>
        public string SendShowMsg { get; set; }
        /// <summary>
        /// 步骤退回后提示语
        /// </summary>
        public string BackShowMsg { get; set; }

        /// <summary>
        /// 步骤行为相关参数
        /// </summary>
        public WorkFlowBehavior Behavior { get; set; }

        /// <summary>
        /// 流程表单
        /// </summary>
        public IEnumerable<WFForm> Forms { get; set; }

        /// <summary>
        /// 模型ID
        /// </summary>
        public Guid? WorkFlowModelId { get; set; }
        /// <summary>
        /// 模版类型
        /// </summary>
        public TemplateType TemplateType { get; set; }
        /// <summary>
        /// 流程按钮
        /// </summary>
        public IEnumerable<WorkFlowButton> Buttons { get; set; }

        /// <summary>
        /// 字段状态
        /// </summary>
        public IEnumerable<FieldStatus> FieldStatus { get; set; }

        /// <summary>
        /// 流程事件
        /// </summary>
        public WorkFlowEvent Event { get; set; }

        /// <summary>
        /// 设计时x坐标(用于排序)
        /// </summary>
        public decimal? Position_x { get; set; }

        /// <summary>
        /// 设计时y坐标(用于排序)
        /// </summary>
        public decimal? Position_y { get; set; }
        /// <summary>
        /// 图形坐标
        /// </summary>
        public Position Position { get; set; }
        /// <summary>
        /// 子流程ID
        /// </summary>
        public string SubFlowID { get; set; }
        /// <summary>
        /// 子流程产生实例类型 0:所有人同一实例 1:每个人单独实例  
        /// </summary>
        public int SubFlowTaskType { get; set; }


        /// <summary>
        /// 子流程实例是否合并 若是需要合并 则子流程的instanceId 有一定的相同规则；  instanceId， receiveUserId相同 则合并
        /// </summary>
        public bool IsSubFlowTaskMerge { get; set; } = false;
        /// <summary>
        /// 步骤迁移状态
        /// </summary>
        public int StepToStatus { get; set; }

        /// <summary>
        /// 步骤状态名
        /// </summary>
        public string StepToStatusTitle { get; set; }

        /// <summary>
        /// 是否迁移状态
        /// </summary>
        public bool ChangeStatus { get; set; }
        /// <summary>
        /// 是否允许变更
        /// </summary>
        public bool IsChange { get; set; }


        public bool IsAutoCompleteStep { get; set; } = false;


        /// <summary>
        /// 是否隐藏新创建的任务 （由hangfire改状态显示）
        /// </summary>
        public bool IsHideTask { get; set; } = false;

        public WorkFlowStep()
        {
            this.Behavior = new WorkFlowBehavior();
            this.Forms = new List<WFForm>();
            this.Buttons = new List<WorkFlowButton>();
            this.FieldStatus = new List<FieldStatus>();
            this.Event = new WorkFlowEvent();
        }
    }

    public class Position
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
    /// <summary>
    /// 流程连线实体
    /// </summary>
    public class WorkFlowLine
    {
        /// <summary>
        /// 连线ID
        /// </summary>
        public Guid ID { get; set; }

        public string Text { get; set; }
        /// <summary>
        /// 连线源步骤ID
        /// </summary>
        public Guid FromID { get; set; }
        /// <summary>
        /// 连线目标ID
        /// </summary>
        public Guid ToID { get; set; }
        /// <summary>
        /// 连线流转条件判断方法
        /// </summary>
        public string CustomMethod { get; set; }
        /// <summary>
        /// 连线提交条件sql条件
        /// </summary>
        public string SqlWhere { get; set; }
        /// <summary>
        /// 条件不满足时的提示信息
        /// </summary>
        public string NoAccordMsg { get; set; }
        /// <summary>
        /// 组织机构相关条件
        /// </summary>
        public string Organize { get; set; }
    }

    /// <summary>
    /// 步骤行为实体
    /// </summary>
    [Serializable]
    public class WorkFlowBehavior
    {
        /// <summary>
        /// 流转类型 0系统控制单分支 1单选一个分支流转 2多选几个分支流转 3系统控制多分支
        /// </summary>
        public int FlowType { get; set; }
        /// <summary>
        /// 运行时选择 0不允许 1允许
        /// </summary>
        public int RunSelect { get; set; }
        /// <summary>
        /// 处理者类型 0所有成员 1部门 2岗位 3工作组 4人员 5发起者 6前一步骤处理者 7某一步骤处理者 8字段值 9发起者主管 10发起者分管领导 11当前处理者主管 12当前处理者分管领导 17自定义方法 18角色
        /// </summary>
        public int HandlerType { get; set; }
        /// <summary>
        /// 选择范围
        /// </summary>
        public string SelectRange { get; set; }

        public string SelectRangeName { get; set; }



        /// <summary>
        /// 当处理者类型为 7某一步骤处理者 时的处理者步骤
        /// </summary>
        public Guid? HandlerStepID { get; set; }
        /// <summary>
        /// 当处理者类型为 8字段值 时的字段
        /// </summary>
        public string ValueField { get; set; }
        /// <summary>
        /// 默认处理者
        /// </summary>
        public string DefaultHandler { get; set; }

        public string DefaultHandlerName { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public int? RoleId { get; set; }
        /// <summary>
        /// 退回策略 0不能退回 1根据处理策略退回 2一人退回全部退回 3所人有退回才退回 4独立退回（一般用于上一步处理者为多个的单独退回，最匹配的上一步为独立处理）
        /// </summary>
        public int BackModel { get; set; }
        /// <summary>
        /// 处理策略 0所有人必须处理 1一人同意即可 2依据人数比例 3独立处理
        /// </summary>
        public int HanlderModel { get; set; }
        /// <summary>
        /// 退回类型 0退回前一步 1退回第一步 2退回某一步
        /// </summary>
        public int BackType { get; set; }
        /// <summary>
        /// 策略百分比
        /// </summary>
        public decimal? Percentage { get; set; }
        /// <summary>
        /// 退回步骤ID 当退回类型为 2退回某一步 时
        /// </summary>
        public Guid? BackStepID { get; set; }
        /// <summary>
        /// 会签策略 0 不会签 1 所有步骤同意 2 一个步骤同意即可 3 依据比例
        /// </summary>
        public int Countersignature { get; set; }
        /// <summary>
        /// 会签策略是依据比例时设置的百分比
        /// </summary>
        public decimal? CountersignaturePercentage { get; set; }
        /// <summary>
        /// 子流程处理策略 0 子流程完成后才能提交 1 子流程发起即可提交
        /// </summary>
        public int SubFlowStrategy { get; set; }
        /// <summary>
        /// 抄送人员
        /// </summary>
        public string CopyFor { get; set; }
        public string CopyForName { get; set; }
        public string copyForSend { get; set; }
        public string copyForSendName { get; set; }

   
        /// <summary>
        /// 接受抄送
        /// </summary>
        public bool IsCopyFor { get; set; }

        /// <summary>
        /// 处理抄送
        /// </summary>
        public bool IsCopyForSend { get; set; }

        /// <summary>
        /// 处理者类型 0所有成员 1部门 2岗位 3工作组 4人员 5发起者 6前一步骤处理者 7某一步骤处理者 8字段值 9发起者主管 10发起者分管领导 11当前处理者主管 12当前处理者分管领导 17自定义方法 18角色
        /// </summary>
        public int? CopyForHandlerType { get; set; }
        /// <summary>
        /// 选择范围
        /// </summary>
        public string CopyForSelectRange { get; set; }

        public string CopyForSelectRangeName { get; set; }
        /// <summary>
        /// 当处理者类型为 7某一步骤处理者 时的处理者步骤
        /// </summary>
        public Guid? CopyForHandlerStepID { get; set; }
        /// <summary>
        /// 当处理者类型为 8字段值 时的字段
        /// </summary>
        public string CopyForValueField { get; set; }
        public int? CopyForRoleId { get; set; }
        public string CopyForCustomEvent { get; set; }
        /// <summary>
        /// 处理者类型 0所有成员 1部门 2岗位 3工作组 4人员 5发起者 6前一步骤处理者 7某一步骤处理者 8字段值 9发起者主管 10发起者分管领导 11当前处理者主管 12当前处理者分管领导 17自定义方法 18角色
        /// </summary>
        public int? CopyForSendHandlerType { get; set; }
        /// <summary>
        /// 选择范围
        /// </summary>
        public string CopyForSendSelectRange { get; set; }
        public string CopyForSendSelectRangeName { get; set; }
        /// <summary>
        /// 当处理者类型为 7某一步骤处理者 时的处理者步骤
        /// </summary>
        public Guid? CopyForSendHandlerStepID { get; set; }
        /// <summary>
        /// 当处理者类型为 8字段值 时的字段
        /// </summary>
        public string CopyForSendValueField { get; set; }

        public int? CopyForSendRoleId { get; set; }
        public string CopyForSendCustomEvent { get; set; }


        /// <summary>
        /// 当选择 部门领导 部门直属成员 部门所有成员 类型时使用
        /// </summary>
        public string CopyForSendSelelctOrgIds { get; set; }

        public string CopyForSendSelelctOrgIdNames { get; set; }

        /// <summary>
        /// 选择岗位
        /// </summary>
        public string CopyForSendSelectPostIds { get; set; }


        public string CopyForSendSelectPostIdNamse { get; set; }
        
        /// <summary>
        /// 当选择 部门领导 部门直属成员 部门所有成员 类型时使用
        /// </summary>
        public string CopyForSelelctOrgIds { get; set; }

        public string CopyForSelelctOrgIdNames { get; set; }

        /// <summary>
        /// 选择岗位
        /// </summary>
        public string CopyForSelectPostIds { get; set; }


        public string CopyForSelectPostIdNamse { get; set; }

        /// <summary>
        /// 并发控制 0不控制 1控制
        /// </summary>
        public int ConcurrentModel { get; set; }

        /// <summary>
        /// 获取处理人员的自定义方法
        /// </summary>
        public string CustomEvent { get; set; }


        /// <summary>
        /// 当选择 部门领导 部门直属成员 部门所有成员 类型时使用
        /// </summary>
        public string SelelctOrgIds { get; set; }

        public string SelelctOrgIdNames { get; set; }

        /// <summary>
        /// 选择岗位
        /// </summary>
        public string SelectPostIds { get; set; }


        public string SelectPostIdNamse { get; set; }


    }

    /// <summary>
    /// 表单实体
    /// </summary>
    [Serializable]
    public class WFForm
    {
        /// <summary>
        /// 表单ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }

    /// <summary>
    /// 流程按钮
    /// </summary>
    [Serializable]
    public class WorkFlowButton
    {
        /// <summary>
        /// 按钮ID(为guid则是按钮库中的按钮，否则为其它特定功能按钮)
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 按钮说明
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 显示文字
        /// </summary>
        public string Title { get; set; }

        public string Script { get; set; }
        //public string ApiUrl { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Ico { get; set; }

    }

    /// <summary>
    /// 字段状态
    /// </summary>
    [Serializable]
    public class FieldStatus
    {
        /// <summary>
        /// 字段 
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 状态 0编辑 1只读 2隐藏
        /// </summary>
        public int Status1 { get; set; }
        /// <summary>
        /// 数据检查 0不检查 1允许为空,非空时检查 2不允许为空,并检查
        /// </summary>
        public int Check { get; set; }
    }

    /// <summary>
    /// 相关事件
    /// </summary>
    [Serializable]
    public class WorkFlowEvent
    {
        /// <summary>
        /// 步骤提交前事件
        /// </summary>
        public string SubmitBefore { get; set; }

        /// <summary>
        /// 步骤提交后事件
        /// </summary>
        public string SubmitAfter { get; set; }

        /// <summary>
        /// 步骤退回前事件
        /// </summary>
        public string BackBefore { get; set; }

        /// <summary>
        /// 步骤退回后事件
        /// </summary>
        public string BackAfter { get; set; }

        /// <summary>
        /// 步骤收回后事件
        /// </summary>
        public string Withdraw { get; set; }

        /// <summary>
        /// 子流程激活前事件
        /// </summary>
        public string SubFlowActivationBefore { get; set; }

        /// <summary>
        /// 子流程完成后事件
        /// </summary>
        public string SubFlowCompletedBefore { get; set; }
        /// <summary>
        /// 驳回后事件
        /// </summary>
        public string EndTaskAfter { get; set; }
    }

}
