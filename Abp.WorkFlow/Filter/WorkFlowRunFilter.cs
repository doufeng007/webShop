using Abp.Application.Services.Dto;
using Abp.Dependency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Abp.WorkFlow
{
    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WorkFlowRunFilter : IActionFilter, ITransientDependency
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {


        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }


    public class WorkFlowRunParameterAttribute : Attribute
    {
        public Guid WorkFlowId { get; set; }


    }
    public class WorkFlowRunFilterAttribute : ActionFilterAttribute
    {
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        public WorkFlowRunFilterAttribute(IWorkFlowWorkTaskAppService workFlowWorkTaskAppService)
        {
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
        }
        public Guid WorkFlowId { get; set; }
        public Guid? RelationTaskId { get; set; }

        public string WorkFlowTitle { get; set; }


        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {

            base.OnActionExecuted(actionExecutedContext);
            var retobj = actionExecutedContext.Result as ObjectResult;
            if (retobj != null)
            {
                if (retobj.Value is InitWorkFlowOutput)
                {
                    var retModel = retobj.Value as InitWorkFlowOutput;
                    var initRetdata = _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput() { FlowId = this.WorkFlowId, FlowTitle = this.WorkFlowTitle, InStanceId = retModel.InStanceId , RelationTaskId = this.RelationTaskId });
                    retModel.FlowId = this.WorkFlowId;
                    retModel.GroupId = initRetdata.GroupId;
                    retModel.TaskId = initRetdata.TaskId;
                    //retModel.InStanceId = retModel.InStanceId;
                    retModel.StepId = initRetdata.StepId;
                    retModel.StepName = initRetdata.StepName;
                    retobj.Value = retModel;
                    Abp.Logging.LogHelper.Logger.Info($"初始化流程：{this.WorkFlowTitle},InStanceId:{retModel.InStanceId},TaskId:{initRetdata.TaskId}");
                }
            }
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var argument = context.ActionArguments["input"];
            if (argument != null && argument is Service.Dto.CreateWorkFlowInstance)
            {
                var argumentmodel = (Service.Dto.CreateWorkFlowInstance)argument;
                this.WorkFlowId = argumentmodel.FlowId;
                this.RelationTaskId = argumentmodel.RelationTaskId;
                this.WorkFlowTitle = argumentmodel.FlowTitle;
            }

        }



    }


    //public class WorkFlowBusinessListAttribute : ActionFilterAttribute
    //{
    //    private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
    //    private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
    //    public WorkFlowBusinessListAttribute(IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
    //    {
    //        _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
    //        _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
    //    }
    //    public Guid WorkFlowId { get; set; }


    //    public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
    //    {
    //        try
    //        {
    //            base.OnActionExecuted(actionExecutedContext);
    //            var retobj = actionExecutedContext.Result as ObjectResult;
    //            if (retobj == null)
    //            {
    //                throw new ArgumentException($"{nameof(actionExecutedContext)} should be ObjectResult!");
    //            }
    //            if (retobj.Value is PagedResultDto<BusinessWorkFlowListOutput>)
    //            {
    //                var datas = retobj.Value as PagedResultDto<BusinessWorkFlowListOutput>;
    //                var retModel =  _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(WorkFlowId,datas.Items.ToList());
    //                retobj.Value = retModel;
    //            }
    //            else
    //            {
    //                return;
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //            throw;
    //        }



    //    }


    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        base.OnActionExecuting(context);
    //        var argument = context.ActionArguments["input"];
    //        if (argument != null && argument is Service.Dto.CreateWorkFlowInstance)
    //        {
    //            var argumentmodel = (Service.Dto.CreateWorkFlowInstance)argument;
    //            this.WorkFlowId = argumentmodel.FlowId;
    //        }

    //    }



    //}
}
