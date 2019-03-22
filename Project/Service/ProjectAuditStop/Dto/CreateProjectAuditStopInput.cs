using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class CreateProjectAuditStopInput : CreateWorkFlowInstance
    {

        public Guid? Id { get; set; }

        public Guid ProjectBaseId { get; set; }


        public string Remark { get; set; }

        public string RelieveRemark { get; set; }

        public int DelayDay { get; set; }

        public CreateProjectAuditStopInput()
        {

        }
    }



    public class GetProjectForAuditStopInput
    {
        public Guid? ProjectId { get; set; }


        public Guid? Id { get; set; }
    }


    public class UpdateProjectAuditStopInput
    {

        public Guid Id { get; set; }

        public Guid ProjectBaseId { get; set; }


        public string Remark { get; set; }

        public string RelieveRemark { get; set; }

        public int DelayDay { get; set; }

        public UpdateProjectAuditStopInput()
        {

        }
    }

    public class CreateProjectRelieveStopInput : CreateWorkFlowInstance
    {

        public Guid Id { get; set; }

        public Guid ProjectBaseId { get; set; }


        public string Remark { get; set; }

        public string RelieveRemark { get; set; }

        public int DelayDay { get; set; }

        public CreateProjectRelieveStopInput()
        {

        }
    }

    public class UpdateProjectRelieveStopInput
    {
        public Guid ProjectBaseId { get; set; }
        public int Status { get; set; }

    }


    public class SubmitProjectRelieveOutput
    {
        /// <summary>
        /// true 表示当前用户处理解除待办 直接跳转进入流程处理页面 false 表示不是当前用户处理， 则弹 ReturnMsg 提示；
        /// </summary>
        public bool IsCurrentUserTodo { get; set; } = false;


        public string ReturnMsg { get; set; }

        public Guid? TaskId { get; set; }

        public string InstanceId { get; set; }

        public Guid? FlowId { get; set; }


        public Guid? GroupId { get; set; }


        public Guid? StepId { get; set; }
    }

}
