using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using Docment;
using GWGL;
using HR;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    public class EmployeeEvent
    {
   
        /// <summary>
        /// 创建待归档记录
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static void EmployeeReceiptRun(WorkFlowCustomEventParams eventParams)
        {
            var employeeReceiptRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<EmployeeReceipt, Guid>>();
            var employeeReceiptModel = employeeReceiptRepository.Get(eventParams.InstanceID.ToGuid());
            switch (employeeReceiptModel.TaskType)
            {
                case TaskTypeProperty.CreateTask:
                    break;
                case TaskTypeProperty.Distribute:
                    var workFlowWorkTaskAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
                    if (!string.IsNullOrEmpty(employeeReceiptModel.CopyForUsers))
                    {
                        if (employeeReceiptModel.CopyForType == CopyForTypeProperty.Sync)
                        {
                            var flowCopyForInput = new FlowCopyForInput()
                            {
                                FlowId=eventParams.FlowID,
                                InstanceId=eventParams.InstanceID,
                                TaskId=eventParams.TaskID,
                                StepId=eventParams.StepID,
                                GroupId=eventParams.GroupID,
                                UserIds= employeeReceiptModel.CopyForUsers,
                            };
                            workFlowWorkTaskAppService.FlowCopyFor(flowCopyForInput);
                        }
                    }
                    break;
                case TaskTypeProperty.Filing:
                    break;
                    
            }
        }
        public string GetEmployeeReceiptUser(WorkFlowCustomEventParams eventParams)
        {
            var employeeReceiptRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<EmployeeReceipt, Guid>>();
            var employeeReceiptModel = employeeReceiptRepository.Get(eventParams.InstanceID.ToGuid());
            if (string.IsNullOrEmpty(employeeReceiptModel.CopyForUsers))
                return "";
            var user = employeeReceiptModel.CopyForUsers.Split(',').LastOrDefault();
            return user;
        }
        public static void CreateDocment(WorkFlowCustomEventParams eventParams)
        {
            var employeeReceiptRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<EmployeeReceipt, Guid>>();
            var employeeReceiptModel = employeeReceiptRepository.Get(eventParams.InstanceID.ToGuid());
            var docmentService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDocmentAppService>();
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var flow = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlow, Guid>>();
            var taskModel = repository.Get(eventParams.TaskID);
            var flowModel = flow.Get(eventParams.FlowID);
            docmentService.Create(new CreateDocmentInput()
            {
                Attr = (DocmentAttr)employeeReceiptModel.DocProperty,
                Des = taskModel.Title,
                FlowId = new Guid("1734da80-9ddf-4bc3-8f1a-d85fa80868d5"),
                FlowTitle = flowModel.Name + "-归档请求",
                QrCodeId=employeeReceiptModel.QrCodeId,
                Name = taskModel.Title + "-档案待收",
                Type = new Guid("71300f49-4542-43c5-1b44-08d5bbbe5313"),
                UserId = taskModel.SenderID
            });
        }
            /// <summary>
            /// 调岗处理
            /// </summary>
            /// <param name="eventParams"></param>
            /// <returns></returns>
            public bool AdjustPostRun(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IEmployeeAdjustPostAppService>();
            return _service.AdjustPostRun(eventParams.InstanceID.ToGuid());
        }
        /// <summary>
        /// 离职成功后禁用账号
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeeResignSuccess(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IEmployeeResignAppService>();
            return _service.EmployeeResignSuccess(eventParams.InstanceID.ToGuid());
        }
        /// <summary>
        /// 离职是否分管领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeeResignIsLeader(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IEmployeeResignAppService>();
            return _service.EmployeeResignIsLeader(eventParams.InstanceID.ToGuid());
        }
        /// <summary>
        /// 离职是否分管领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeeResignIsNotLeader(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IEmployeeResignAppService>();
            return !_service.EmployeeResignIsLeader(eventParams.InstanceID.ToGuid());
        }
        /// <summary>
        /// 调岗是否分管领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeeAdjustPostIsLeader(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IEmployeeAdjustPostAppService>();
            return _service.EmployeeAdjustPostIsLeader(eventParams.InstanceID.ToGuid());
        }
        /// <summary>
        /// 调岗是否分管领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeeAdjustPostIsNotLeader(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IEmployeeAdjustPostAppService>();
            return !_service.EmployeeAdjustPostIsLeader(eventParams.InstanceID.ToGuid());
        }
        /// <summary>
        /// 人员面试后记录面试结果和增加面试轮数
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeePlanFinish(WorkFlowCustomEventParams eventParams) {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<EmployeePlan, Guid>>();
            var logrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<EmployeeResult, Guid>>();
            var plan = repository.Get(Guid.Parse(eventParams.InstanceID));

            var log = new EmployeeResult()
            {
                AdminUserId = plan.AdminUserId,
                AdminVerifyDiscuss = plan.AdminVerifyDiscuss,
                ApplyCount = plan.ApplyCount,
                ApplyJob = plan.ApplyJob,
                ApplyNo = plan.ApplyNo,
                ApplyOrgId = plan.ApplyOrgId,
                ApplyPostId = plan.ApplyPostId,
                ApplyTime = plan.ApplyTime,
                ApplyUser = plan.ApplyUser,
                Comment = plan.Comment,
                Discuss = plan.Discuss,
                EmployeePlanId = plan.Id,
                MergeUserId = plan.MergeUserId,
                NeedAdmin = plan.NeedAdmin,
                RecordUserId = plan.RecordUserId,
                Result = plan.Result,
                Phone=plan.Phone,
                VerifyDiscuss = plan.VerifyDiscuss,
                VerifyUserId = plan.VerifyUserId,
                EmployeeUserIds=plan.EmployeeUserIds,
            };
            logrepository.Insert(log);
            if (plan.NeedAdmin.HasValue && plan.NeedAdmin.Value)
            {
                //需要领导审核时
               
            }
            else {
                //不需要领导审核时
                plan.ApplyCount = (ApplyCount)((int)plan.ApplyCount) + 1;
                plan.EmployeeUserIds = "";
                plan.AdminVerifyDiscuss = "";
                plan.Comment = "";
                plan.Discuss = "";
                plan.JoinDes = "";
                plan.VerifyDiscuss = "";
                plan.AdminUserId = "";
                plan.MergeUserId = "";
                plan.NeedAdmin = null;
                plan.RecordUserId = "";
                plan.Result = null;
                plan.VerifyUserId = "";
                repository.Update(plan);
            }
           
            
            return true;
        }

        /// <summary>
        /// 安排面试人员后，人才库资源改为面试中
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeePlaning(WorkFlowCustomEventParams eventParams) {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
               .Resolve<IEmployeePlanAppService>();
            _service.ChangeResumeStatus(new HR.Service.ChangeStatusInput() {
                 Id= eventParams.InstanceID.ToGuid(),
                  Status= ResumeStatus.面试中
            });
            return true;
        }
        /// <summary>
        /// 安排面试后，淘汰的
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool EmployeePlaned(WorkFlowCustomEventParams eventParams) {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
              .Resolve<IEmployeePlanAppService>();
            _service.ChangeResumeStatus(new HR.Service.ChangeStatusInput() {
                Id= eventParams.InstanceID.ToGuid(),
                 Status= ResumeStatus.淘汰
            });
            return true;
        }
        
        public bool InsertIssue(WorkFlowCustomEventParams eventParams) {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
              .Resolve<IEmployeeProposalAppService>();
            _service.InsertIssue(eventParams.InstanceID.ToGuid());
            return true;
        }



    }
}
