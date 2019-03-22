using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow.Entity
{
    /// <summary>
    /// 模型字段权限
    /// </summary>
    public class WorkFlowModelColumnAuth : Entity<Guid>
    {
        /// <summary>
        /// 模型
        /// </summary>
        public Guid ModelId { get; set; }
        public string ModelCode { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public Guid ColumneId { get; set; }

        public string ColumneCode { get; set; }
        /// <summary>
        /// 工作流
        /// </summary>
        public Guid FlowId { get; set; }
        /// <summary>
        /// 步骤
        /// </summary>
        public Guid StepId { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public AuthType AuthType { get; set; }
       
    }
    public enum AuthType
    {
        读写 = 0,
        只读 = 1,
        无权限 = -1
    }
}
