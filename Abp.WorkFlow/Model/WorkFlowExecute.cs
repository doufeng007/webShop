using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization.Users;

namespace Abp.WorkFlow
{
    /// <summary>
    /// 任务相关的枚举类型
    /// </summary>
    public class WorkFlowEnumType
    {
        /// <summary>
        /// 处理类型
        /// </summary>
        public enum WorkFlowExecuteType
        {
            /// <summary>
            /// 提交
            /// </summary>
            Submit,
            /// <summary>
            /// 保存
            /// </summary>
            Save,
            /// <summary>
            /// 退回
            /// </summary>
            Back,
            /// <summary>
            /// 完成
            /// </summary>
            Completed,
            /// <summary>
            /// 转交
            /// </summary>
            Redirect,
            /// <summary>
            /// 加签
            /// </summary>
            AddWrite,
            /// <summary>
            /// 抄送后完成
            /// </summary>
            CopyforCompleted,

            /// <summary>
            /// 意见征询完成
            /// </summary>
            InquiryCompleted,
        }
    }

    /// <summary>
    /// 任务处理模型
    /// </summary>
    [Serializable]
    public class WorkFlowExecute : Entity<Guid>
    {
        public WorkFlowExecute()
        {
            Steps = new Dictionary<Guid, List<User>>();
            OtherType = 0;
        }
        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid FlowID { get; set; }


        /// <summary>
        /// 流程版本号
        /// </summary>
        public int VersionNum { get; set; }


        /// <summary>
        /// 步骤ID
        /// </summary>
        public Guid StepID { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid TaskID { get; set; }
        /// <summary>
        /// 实例ID
        /// </summary>
        public string InstanceID { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public WorkFlowEnumType.WorkFlowExecuteType ExecuteType { get; set; }
        /// <summary>
        /// 发送人员
        /// </summary>
        public User Sender { get; set; }
        /// <summary>
        /// 接收的步骤和人员
        /// </summary>
        public Dictionary<Guid, List<User>> Steps { get; set; }
        public List<RelationUser> Users { get; set; }
        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 是否签章
        /// </summary>
        public bool IsSign { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 其他类型
        /// </summary>
        public int OtherType { get; set; }
        /// <summary>
        /// 相关附件
        /// </summary>
        public string Files { get; set; }


    }

    /// <summary>
    /// 任务处理结果
    /// </summary>
    [Serializable]
    public class WorkFlowResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Messages { get; set; }
        /// <summary>
        /// 调试信息
        /// </summary>
        public string DebugMessages { get; set; }
        /// <summary>
        /// 其它信息
        /// </summary>
        public object[] Other { get; set; }
        /// <summary>
        /// 后续任务
        /// </summary>
        public IEnumerable<WorkFlowTask> NextTasks { get; set; }


        private List<WorkFlowNoticeMessage> _send = new List<WorkFlowNoticeMessage>();

        public List<WorkFlowNoticeMessage> Send
        {
            get { return _send; }
            set { _send = value; }
        }


    }

    [Serializable]
    public class WorkFlowNoticeMessage
    {
        public long RecieveId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string LinkUrl { get; set; }
    }


    public class SubWorkFlowExecuteResult : Entity<Guid>
    {

        public string SubTitle { get; set; }

        public string SubInstanceID { get; set; }

    }
}
