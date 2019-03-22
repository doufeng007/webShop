using Abp.File;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Abp.WorkFlow
{


    public class WorkFlowRunUrlInput
    {
        public Guid FlowId { get; set; }


        public Guid? InstanceId { get; set; }


        public Guid? TaskId { get; set; }


        public Guid? StepId { get; set; }


        public Guid? Groupid { get; set; }

        public int AppTypeId { get; set; }

        


    }



    public class WorkFlowRunUrlOutput
    {
        public bool IsSuccesefull { get; set; }

        public string RetUrl { get; set; }


        public string ErroMsg { get; set; }

    }

    public class InitWorkFlowInput : Service.Dto.CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }



        public string InStanceId { get; set; }


        public long? ReciveUserId { get; set; }



    }


    public class TaskFileInput
    {
        public Guid TaskId { get; set; }
        public string Comment { get; set; }
        public List<GetAbpFilesOutput> FlowFileList { get; set; } = new List<GetAbpFilesOutput>();

    }

    public class InitWorkFlowOutput
    {
        public Guid FlowId { get; set; }

        public Guid GroupId { get; set; }


        public Guid StepId { get; set; }

        public string StepName { get; set; }

        public Guid TaskId { get; set; }

        public string InStanceId { get; set; }

    }

    public class ExecuteWorkFlowInput
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// freesubmit  submit  save back  completed  redirect  addwrite
        /// </summary>
        public string ActionType { get; set; }


        public List<ExecuteWorkChooseStep> Steps { get; set; }
        public List<RelationUser> Users { get; set; } = new List<RelationUser>();


        /// <summary>
        /// 是否签名
        /// </summary>
        public bool IsSign { get; set; }

        /// <summary>
        /// 备注 （意见）
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 流程id
        /// </summary>
        public Guid FlowId { get; set; }

        /// <summary>
        /// 实例id
        /// </summary>
        public string InstanceId { get; set; }

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

        /// <summary>
        /// 财评类别
        /// </summary>
        public int AppTypeId { get; set; }

        /// <summary>
        /// 是否隐藏新创建的任务 （由hangfire改状态显示）
        /// </summary>
        public bool IsHideNextTask { get; set; } = false;

        //public string opationType { get; set; }

        


        public string Title { get; set; }



        public ExecuteWorkFlowInput()
        {
            Steps = new List<ExecuteWorkChooseStep>();
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }

        public List<GetAbpFilesOutput> FileList { get; set; }



    }









    public class ExecuteWorkChooseStep
    {
        public string id { get; set; }

        public string member { get; set; }
    }



    public class ExecuteWorkFlowOutput
    {
        public bool IsSuccesefull { get; set; }

        public string NextStepUrl { get; set; }

        public string ErrorMsg { get; set; }

        public string ReturnMsg { get; set; }

        public ExecuteWorkFlowErrorType ErrorType { get; set; }

        public List<GetNextStepOutput> Steps { get; set; } = new List<GetNextStepOutput>();
    }


    public class FlowCopyForInput
    {
        /// <summary>
        /// 流程id
        /// </summary>
        public Guid FlowId { get; set; }

        /// <summary>
        /// 实例id
        /// </summary>
        public string InstanceId { get; set; }

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

        public string UserIds { get; set; }

    }




    public enum ExecuteWorkFlowErrorType
    {
        未找到流程 = 1,
        未找到流程运行时实体 = 2,
        未找到当前步骤 = 3,
        参数为空 = 4,
    }

}
