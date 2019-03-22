using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.WorkFlow;
using Castle.Windsor;
using HR;
using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    public class CustEvent
    {
        //private static IProjectAppService _projectAppService;

        //public CustEvent(IProjectAppService projectAppService)
        //{
        //    _projectAppService = projectAppService;
        //}

        /// <summary>
        /// 出差关联用户权限转移
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool WorkoutRelationUserId(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Project.IOAWorkoutAppService>();
            _projectAppService.WorkoutRelationUserId(eventParams.InstanceID.ToGuid());
            return true;
        }

        /// <summary>
        /// 请假关联用户权限转移
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool AskForLeaveRelationUserId(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IEmployeeAskForLeaveAppService>();
            _projectAppService.AskForLeaveRelationUserId(eventParams.InstanceID.ToGuid());
            return true;
        }
        /// <summary>
        /// 获取是否是项目负责人
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool GetIsNanBuProjectLeader(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Project.IProjectAppService>();
            var isleader = _projectAppService.GetIsCreateByProjecetLeader(eventParams.TaskID);
            return isleader;
        }


        public bool GetIsNotNanBuProjectLeader(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Project.IProjectAppService>();
            var isleader = _projectAppService.GetIsCreateByProjecetLeader(eventParams.TaskID);
            return !isleader;
        }

        public bool GetStopIsCreateByProjecetLeader(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditStopAppService>();
            var isleader = _projectAppService.GetStopIsCreateByProjecetLeader(eventParams.TaskID);
            return isleader;
        }

        public bool GetStopIsNotCreateByProjecetLeader(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditStopAppService>();
            var isleader = _projectAppService.GetStopIsCreateByProjecetLeader(eventParams.TaskID);
            return !isleader;
        }


        public string GetProjectLeaderForStop(WorkFlowCustomEventParams eventParams)
        {
            var _projectAppService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditStopAppService>();
            return _projectAppService.GetProjectLeaderForStop(eventParams.InstanceID.ToGuid());
        }


        public string GetProjectCWFMember(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Project.IProjectMemberAppService>();
            var data = service.GetProjectAuditMember(new GetProjectAuditMembersInput() { ProjectId = eventParams.InstanceID.ToGuid(), AuditRoleIds = ((int)AuditRoleEnum.财务初审).ToString() });
            var item = data.FirstOrDefault();
            return $"u_{item.UserId}";
        }

        /// <summary>
        /// 获取财务终审人员
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetProjectCWSMember(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Project.IProjectMemberAppService>();
            var data = service.GetProjectAuditMember(new GetProjectAuditMembersInput() { ProjectId = eventParams.InstanceID.ToGuid(), AuditRoleIds = ((int)AuditRoleEnum.财务评审).ToString() });
            var item = data.FirstOrDefault();
            return $"u_{item.UserId}";
        }

        /// <summary>
        /// 获取抄送人员
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public string GetProjectRegistrationMember(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Project.IProjectRealationUserAppService>();
            var data = service.GetProjectRealationMember(new ProjectRelationUserCreate()
            {
                InstanceID = eventParams.InstanceID,
                FlowID = eventParams.FlowID,
                StepID = eventParams.StepID,
                GroupID = eventParams.GroupID,
                TaskID = eventParams.TaskID
            });

            return data;
        }


        /// <summary>
        /// 项目组的任务都以抄送的形式 通知其他人
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string CopyForProjectAuditGroupOther(WorkFlowCustomEventParams eventParams)
        {
            var copyService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditGroupAppService>();
            copyService.CopyForProjectAuditGroup(new CopyForProjectAuditGroupInput()
            {
                FlowID = eventParams.FlowID,
                GroupID = eventParams.GroupID,
                InstanceID = eventParams.InstanceID,
                TaskID = eventParams.TaskID,
                HasFinancialReviewMember = true
            });
            return "成功";
        }

        /// <summary>
        /// 项目组的任务都以抄送的形式 通知联系人
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string CopyForProjectAuditGroupContact(WorkFlowCustomEventParamsForAfterSubmit eventParams)
        {
            var copyService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditGroupAppService>();
            //Task.Run(() =>
            //{
            //    copyService.CopyForProjectAuditGroup(new ProjectApp.Dto.CopyForProjectAuditGroupInput()
            //    {
            //        FlowID = eventParams.FlowID,
            //        GroupID = eventParams.GroupID,
            //        InstanceID = eventParams.InstanceID,
            //        TaskID = eventParams.TaskID
            //    });


            //});

            copyService.CopyForProjectAuditGroup(new CopyForProjectAuditGroupInput()
            {
                FlowID = eventParams.FlowID,
                GroupID = eventParams.GroupID,
                InstanceID = eventParams.InstanceID,
                TaskID = eventParams.TaskID,
                HasFinancialReviewMember = false
            });
            var projectRegistrationUser = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectRealationUserAppService>();
            projectRegistrationUser.Create(new ProjectRelationUserCreate()
            {
                FlowID = eventParams.FlowID,
                GroupID = eventParams.GroupID,
                InstanceID = eventParams.InstanceID,
                StepID = eventParams.StepID,
                TaskID = eventParams.TaskID,
                NextTasks = eventParams.NextTasks,
            });
            return "成功";
        }
        /// <summary>
        /// 项目干系人(发送给分管领导后)
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string CreatProjectRegistration(WorkFlowCustomEventParamsForAfterSubmit eventParams)
        {

            var projectRegistrationUser = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectRealationUserAppService>();
            projectRegistrationUser.Create(new ProjectRelationUserCreate()
            {
                FlowID = eventParams.FlowID,
                GroupID = eventParams.GroupID,
                InstanceID = eventParams.InstanceID,
                StepID = eventParams.StepID,
                TaskID = eventParams.TaskID,
                NextTasks = eventParams.NextTasks,
            });


            return "成功";
        }


        /// <summary>
        /// 当前任务处理者是否是部门领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CurrentRecevieUserIsChargeLeader(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var taskModel = repository.Get(eventParams.TaskID);
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            return manager.IsChargerLeader(taskModel.ReceiveID);
        }

        /// <summary>
        /// 当前任务处理者是否是部门领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CurrentRecevieUserIsNotChargeLeader(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var taskModel = repository.Get(eventParams.TaskID);
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            return !manager.IsChargerLeader(taskModel.ReceiveID);
        }

        /// <summary>
        /// 工作记录-公文发布子流程激活自定义事件
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public WorkFlowExecute ProjectWriteRegisFlowActive(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkTaskAppService>();
            var data = _service.ProjectWriteRegisFlowActive(eventParams.InstanceID.ToGuid());
            var ret = new WorkFlowExecute();
            ret.Title = $"工作记录-公文发布";
            ret.InstanceID = data;
            return ret;
        }
        /// <summary>
        /// 部门领导审核后 设置项目准备开始
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool SetProjectReadyStartTime(WorkFlowCustomEventParamsForAfterSubmit eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectBaseRepository>();
            var project = repository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (project != null)
            {
                if (project.ReadyStartTime.HasValue == false)
                {
                    project.ReadyStartTime = DateTime.Now;
                    repository.Update(project);
                }
            }
            var projectRegistrationUser = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectRealationUserAppService>();
            projectRegistrationUser.Create(new ProjectRelationUserCreate()
            {
                FlowID = eventParams.FlowID,
                GroupID = eventParams.GroupID,
                InstanceID = eventParams.InstanceID,
                StepID = eventParams.StepID,
                TaskID = eventParams.TaskID,
                NextTasks = eventParams.NextTasks,
            });
            return true;
        }


        #region OA流程状态改变

        /// <summary>
        /// 固定资产改变状态-采购完成
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string UpdateFAStatusByPurchase(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAFixedAssetsAppService>();
            service.UpdateOAFixedAssetsStatusAsync(new UpdateOAFixedAssetsStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), Status = OAFixedAssetsStatus.采购, ToStatus = OAFixedAssetsStatus.在库 });
            return "成功";
        }

        /// <summary>
        /// 固定资产改变状态-领用申请完成
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string UpdateFAStatusByUseApply(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAFixedAssetsAppService>();
            service.UpdateOAFixedAssetsStatusAsync(new UpdateOAFixedAssetsStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), Status = OAFixedAssetsStatus.领用申请, ToStatus = OAFixedAssetsStatus.领用 });
            return "成功";
        }

        /// <summary>
        /// 固定资产改变状态-归还完成
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string UpdateFAStatusByReturn(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAFixedAssetsAppService>();
            service.UpdateOAFixedAssetsStatusAsync(new UpdateOAFixedAssetsStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), Status = OAFixedAssetsStatus.归还申请, ToStatus = OAFixedAssetsStatus.在库 });
            return "成功";
        }

        /// <summary>
        /// 固定资产改变状态-报销完成
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string UpdateFAStatusByScrap(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAFixedAssetsAppService>();
            service.UpdateOAFixedAssetsStatusAsync(new UpdateOAFixedAssetsStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), Status = OAFixedAssetsStatus.报废申请, ToStatus = OAFixedAssetsStatus.报废 });
            return "成功";
        }

        /// <summary>
        /// 固定资产改变状态-维修完成
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string UpdateFAStatusByRepair(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAFixedAssetsAppService>();
            service.UpdateOAFixedAssetsStatusAsync(new UpdateOAFixedAssetsStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), Status = OAFixedAssetsStatus.维修申请, ToStatus = OAFixedAssetsStatus.在库 });
            return "成功";
        }

        /// <summary>
        /// 固定资产改变状态-开始维修
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string UpdateFAStatusByRepairBegin(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAFixedAssetsRepairAppService>();
            service.BeginRepairOAFA(eventParams.InstanceID.ToGuid());
            return "成功";
        }


        /// <summary>
        /// 固定资产改变状态-开始维修
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string UpdateOABidProjectStatus1(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.新增, ToStatus = OABidProjectStatus.正在购买招标文件 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus2(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.正在购买招标文件, ToStatus = OABidProjectStatus.购买招标文件完成 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus3(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.购买招标文件完成, ToStatus = OABidProjectStatus.正在资格自审 });
            return "成功";
        }

        public static string UpdateOABidProjectStatus4(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.正在资格自审, ToStatus = OABidProjectStatus.资格自审完成 });
            return "成功";
        }

        public static string UpdateOABidProjectStatus5(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.资格自审完成, ToStatus = OABidProjectStatus.正在项目勘察 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus6(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.正在项目勘察, ToStatus = OABidProjectStatus.项目勘察完成 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus8(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.项目勘察完成, ToStatus = OABidProjectStatus.投标文件审查 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus9(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.投标文件审查, ToStatus = OABidProjectStatus.投标文件审查完成 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus10(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.投标文件审查完成, ToStatus = OABidProjectStatus.投标保证金申请 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus11(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.投标保证金申请, ToStatus = OABidProjectStatus.投标保证金申请完成 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus12(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.投标保证金申请完成, ToStatus = OABidProjectStatus.分析竞争对手 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus13(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.分析竞争对手, ToStatus = OABidProjectStatus.分析竞争对手完成 });
            return "成功";
        }
        public static string UpdateOABidProjectStatus14(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOABidProjectAppService>();
            service.UpdateOABidProjectStatus(new UpdateOABidProjectStatusInput() { BusinessId = eventParams.InstanceID.ToGuid(), FromStatus = OABidProjectStatus.申请项目业务费用, ToStatus = OABidProjectStatus.项目业务费用 });
            return "成功";
        }
        #endregion

        #region 停滞申请和解除停滞申请
        /// <summary>
        /// 停滞申请驳回
        /// </summary>
        /// <returns></returns>
        public bool RejectProjectStop(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditStopAppService>();
            _service.RejectProjectStop(Guid.Parse(eventParams.InstanceID));
            return true;
        }
        /// <summary>
        /// 解除停滞申请驳回
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool RejectProjectRelieve(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditStopAppService>();
            _service.ReturnProjectRelieve(Guid.Parse(eventParams.InstanceID));
            return true;
        }
        /// <summary>
        /// 完成解除停滞
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool CompleteRejectProject(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditStopAppService>();
            _service.CompleteRejectProject(Guid.Parse(eventParams.InstanceID));
            return true;
        }

        /// <summary>
        /// 停滞申请通过
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool PassProjectStop(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAuditStopAppService>();
            _service.PassProjectStop(Guid.Parse(eventParams.InstanceID));
            return true;
        }


        


        #endregion

        #region 开始执行逻辑处理
        /// <summary>
        /// 判断是否能发送到汇总步骤（先点开始执行）
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public ExecuteWorkFlowOutput CanSendToHuizong(WorkFlowCustomEventParams eventParams)
        {
            var ret = new ExecuteWorkFlowOutput()
            {
                IsSuccesefull = true
            };
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectBaseRepository>();
            var project = repository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (project != null)
            {
                if (project.ProjectStatus.HasValue == false || project.ProjectStatus == ProjectStatus.待审)
                {
                    ret.IsSuccesefull = false;
                    ret.ErrorMsg = "请先点击【开始执行】后再发送到下一步。";
                }
            }
            return ret;
        }

        #endregion




        public List<WorkFlowExecute> SingleProjectFlowActive(WorkFlowCustomEventParams eventParams)
        {
            var ret = new List<WorkFlowExecute>();
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectAppService>();
            var data = _service.SingleProjectFlowActive(eventParams.InstanceID.ToGuid(), eventParams.NextRecevieUserId);
            foreach (var item in data)
            {
                var entity = new WorkFlowExecute()
                {
                    Title = item.Value,
                    InstanceID = item.Key.ToString(),
                };
                ret.Add(entity);
            }

            return ret;
        }



        public string GetPorjectDepartmentLeaders(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectMemberV2AppService>();
            var ret = _service.GetPorjectDepartmentLeaders(eventParams.InstanceID.ToGuid());
            return ret;
        }
    }
}
