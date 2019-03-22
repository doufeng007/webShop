using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{


    public class WorkFlowCommitFilterAttribute : ActionFilterAttribute
    {
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        public WorkFlowCommitFilterAttribute(IWorkFlowWorkTaskAppService workFlowWorkTaskAppService)
        {
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
        }
        public Guid WorkFlowId { get; set; }

        public string InstanceId { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var argument = context.ActionArguments["input"];
            if (argument != null && argument is GetWorkFlowTaskCommentInput)
            {
                var argumentmodel = (GetWorkFlowTaskCommentInput)argument;
                this.WorkFlowId = argumentmodel.FlowId;
                this.InstanceId = argumentmodel.InstanceId;
            }

        }


        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            var retobj = actionExecutedContext.Result as ObjectResult;
            if (retobj != null)
            {
                if (retobj.Value is WorkFlowTaskCommentResult)
                {
                    var retModel = retobj.Value as WorkFlowTaskCommentResult;
                    var commentRetdata = _workFlowWorkTaskAppService.GetInstanceComment(new GetWorkFlowTaskCommentInput() { FlowId = this.WorkFlowId, InstanceId = this.InstanceId });
                    var filedata = _workFlowWorkTaskAppService.GetInstanceFiles(new GetWorkFlowTaskCommentInput() { FlowId = this.WorkFlowId, InstanceId = this.InstanceId });
                    var coments = _workFlowWorkTaskAppService.GetCurrentUserComents(new GetWorkFlowTaskCommentInput() { FlowId = this.WorkFlowId, InstanceId = this.InstanceId });
                    retModel.CommentList = commentRetdata;
                    retModel.StepFiles = filedata;
                    retModel.Coments = coments;
                }
            }
        }






    }
}
