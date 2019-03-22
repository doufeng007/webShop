using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 业务实例Id
        /// </summary>
        public string InstanceId { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public Guid StepId { get; set; }


        /// <summary>
        /// 当前实例的状态
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 当前实例状态描述
        /// </summary>
        public string StatusTitle { get; set; }


        public string CurrentStepNames { get; set; }


        /// <summary>
        ///  1 处理 2 查看
        /// </summary>
        public int OpenModel { get; set; }


        public int OldOpenModel { get; set; }

        /// <summary>
        /// 处理时参数 任务id
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// 处理时参数
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 模型id
        /// </summary>
        public Guid? WorkFlowModelId { get; set; }

        //public string SubFlowID { get; set; }


        public List<TodoParameter> DoParameters { get; set; } = new List<TodoParameter>();

        public bool IsMultipleStep { get; set; } = false;


    }


    public class TodoParameter
    {
        public Guid WorkFlowModelId { get; set; }

        public Guid GroupId { get; set; }


        public Guid TaskId { get; set; }

        public Guid StepId { get; set; }


    }


    public class FirstTaskModelScrap
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public Guid StepId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Guid GroupId { get; set; }
    }


}
