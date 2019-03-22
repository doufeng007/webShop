using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using Newtonsoft.Json.Linq;
using Abp.UI;
using Abp.Runtime.Session;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Application.Services;
using Abp.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using Abp.Authorization;
using Abp.Extensions;
using ZCYX.FRMSCore.Model;
using System.Threading;

namespace Project
{
    public class ProjectAuditStopAppService : ApplicationService, IProjectAuditStopAppService
    {
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<AappraisalFileType, int> _aappraisalFileTypeRepository;
        private readonly IRepository<Code_AppraisalType, int> _code_AppraisalTypeRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        private readonly IRepository<ProjectAuditStop, Guid> _projectAuditStopRepository;
        private readonly IWorkFlowTaskRepository _roadFlowWorkFlowTaskRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<ProjectAuditRole> _projectAuditRoleRepository;
        private string AuditFlowId { get; set; }
        private string ProjectStopFlow { get; set; }



        public ProjectAuditStopAppService(IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IRepository<User, long> userRepository,
            IProjectBaseRepository projectBaseRepository, IRepository<AappraisalFileType, int> aappraisalFileTypeRepository,
            IRepository<Code_AppraisalType, int> code_AppraisalTypeRepository, IRepository<ProjectAuditStop, Guid> projectAuditStopRepository,
            IWorkFlowTaskRepository roadFlowWorkFlowTaskRepository, IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository
            , IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<ProjectAuditRole> projectAuditRoleRepository)
        {
            _organizeRepository = organizeRepository;
            _userRepository = userRepository;
            _projectBaseRepository = projectBaseRepository;
            _aappraisalFileTypeRepository = aappraisalFileTypeRepository;
            _code_AppraisalTypeRepository = code_AppraisalTypeRepository;
            _projectAuditStopRepository = projectAuditStopRepository;
            _roadFlowWorkFlowTaskRepository = roadFlowWorkFlowTaskRepository;
            var coreAssemblyDirectoryPath = typeof(ProjectAuditStopAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath); ;
            AuditFlowId = _appConfiguration["App:AuditFlow"];
            ProjectStopFlow = _appConfiguration["App:ProjectStopFlow"];
            _singleProjectInfoRepository = singleProjectInfoRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _projectAuditRoleRepository = projectAuditRoleRepository;
        }


        [AbpAuthorize]
        public async Task<GetProjectAuditStopForEidtOutput> GetProjectForAuditStop(GetProjectForAuditStopInput input)
        {
            if (!input.ProjectId.HasValue && !input.Id.HasValue)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数错误");
            var retmodel = new GetProjectAuditStopForEidtOutput();
            if (input.ProjectId.HasValue && input.Id.HasValue)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数错误");
            if (input.Id.HasValue)
            {
                var projectStopModel = await _projectAuditStopRepository.GetAsync(input.Id.Value);
                var projectModel = await _singleProjectInfoRepository.GetAsync(projectStopModel.ProjectId);
                retmodel.ProjectBaseId = projectModel.Id;
                retmodel.ProjectName = projectModel.SingleProjectName;
                retmodel.SingleProjectName = projectModel.SingleProjectName;
                retmodel.ProjectCode = projectModel.ProjectCode;
                retmodel.SingleProjectCode = projectModel.SingleProjectCode;
                retmodel.SendTotalBudget = projectModel.SingleProjectbudget;
                retmodel.IsNew = false;
                retmodel.Remark = projectStopModel.Remark;
                retmodel.Id = projectStopModel.Id;
                retmodel.DelayDay = projectStopModel.DelayDay;
                retmodel.RelieveRemark = projectStopModel.RelieveRemark;
            }
            else
            {
                var projectmodel = await _singleProjectInfoRepository.GetAsync(input.ProjectId.Value);
                retmodel.ProjectBaseId = projectmodel.Id;
                retmodel.ProjectName = projectmodel.SingleProjectName;
                retmodel.SingleProjectName = projectmodel.SingleProjectName;
                retmodel.ProjectCode = projectmodel.ProjectCode;
                retmodel.SingleProjectCode = projectmodel.SingleProjectCode;
                retmodel.SendTotalBudget = projectmodel.SingleProjectbudget;
                retmodel.IsNew = true;

            }
            return retmodel;
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> CreateStopAsync(CreateProjectAuditStopInput input)
        {
            var projectModel = await _singleProjectInfoRepository.GetAsync(input.ProjectBaseId);
            if (projectModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "项目已被删除");

            var exitprojectStopModelCount = await _projectAuditStopRepository.GetAll().Where(r => r.Status >= 0 && r.ProjectId == input.ProjectBaseId).CountAsync();
            if (exitprojectStopModelCount > 0)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "存在未解除的停滞申请。");
            var model = new ProjectAuditStop()
            {
                Id = Guid.NewGuid(),
                Status = 0,
                ProjectId = input.ProjectBaseId,
                Remark = input.Remark,
                DelayDay = input.DelayDay,
            };
            await _projectAuditStopRepository.InsertAsync(model);
            projectModel.IsStop = ProjectStopTypeEnum.AapplyStop;
            await _singleProjectInfoRepository.UpdateAsync(projectModel);
            await StopProjectInWorkFlowAsync(input.ProjectBaseId, AuditFlowId.ToGuid());

            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = input.ProjectBaseId;
            noticeInput.Content = "提交停滞申请";
            noticeInput.UserType = 1;
            noticeInput.Title = $"项目：{projectModel.SingleProjectName} 停滞申请";
            await noticeService.CreateProjectWorkSpaceNotice(noticeInput);

            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };

        }


        public async Task UpdateStopAsync(UpdateProjectAuditStopInput input)
        {
            var model = await _projectAuditStopRepository.GetAsync(input.Id);
            if (model != null)
            {
                model.Remark = input.Remark;
                model.RelieveRemark = input.RelieveRemark;
            }
            await _projectAuditStopRepository.UpdateAsync(model);
        }

        [AbpAuthorize]
        public async Task<SubmitProjectRelieveOutput> SubmitProjectRelieve(EntityDto<Guid> input)
        {
            var ret = new SubmitProjectRelieveOutput();
            var singleProjectModel = await _singleProjectInfoRepository.GetAsync(input.Id);
            if (singleProjectModel.IsStop != ProjectStopTypeEnum.WaitRelieve)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "项目状态无法发起解除停滞");
            singleProjectModel.IsStop = ProjectStopTypeEnum.Relieve;
            var stopModel = await _projectAuditStopRepository.GetAll().FirstOrDefaultAsync(r => r.ProjectId == input.Id && r.Status > 0);
            if (stopModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前项目不存在停滞申请");
            var projectStopFlow = ProjectStopFlow.ToGuid();
            var maxSort = await _roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == projectStopFlow && r.InstanceID == stopModel.Id.ToString()).MaxAsync(r => r.Sort);
            var tasks = _roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == projectStopFlow && r.InstanceID == stopModel.Id.ToString() && r.Sort == maxSort);

            foreach (var task in tasks)
            {
                task.Status = 0;
                if (task.ReceiveID == AbpSession.UserId.Value)
                {
                    ret.IsCurrentUserTodo = true;
                    ret.TaskId = task.Id;
                    ret.FlowId = task.FlowID;
                    ret.InstanceId = task.InstanceID;
                    ret.GroupId = task.GroupID;
                    ret.StepId = task.StepID;
                }
                _roadFlowWorkFlowTaskRepository.Update(task);
            }
            if (!ret.IsCurrentUserTodo)
            {
                ret.ReturnMsg = $"已发送：{tasks.FirstOrDefault().StepName},处理者：{string.Join(",", tasks.Select(r => r.ReceiveName))}";
            }
            return ret;
        }



        protected async Task StopProjectInWorkFlowAsync(Guid projectId, Guid flowId, bool isSubmit = true)
        {
            if (_roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == flowId && r.InstanceID == projectId.ToString()).Count() > 0)
            {
                var maxSort = await _roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == flowId && r.InstanceID == projectId.ToString()).MaxAsync(r => r.Sort);
                var tasks = _roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == flowId && r.InstanceID == projectId.ToString() && r.Sort == maxSort);
                foreach (var task in tasks)
                {
                    if (isSubmit)
                    {
                        task.Status = 9;
                        task.CompletedTime1 = DateTime.Now;
                    }
                    else
                    {
                        task.Status = 0;
                        task.CompletedTime1 = null;
                    }

                    _roadFlowWorkFlowTaskRepository.Update(task);
                }
            }
        }


        protected void StopProjectInWorkFlow(Guid projectId, Guid flowId, bool isSubmit = true)
        {
            if (_roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == flowId && r.InstanceID == projectId.ToString()).Count() > 0)
            {
                var maxSort = _roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == flowId && r.InstanceID == projectId.ToString()).Max(r => r.Sort);
                var tasks = _roadFlowWorkFlowTaskRepository.GetAll().Where(r => r.FlowID == flowId && r.InstanceID == projectId.ToString() && r.Sort == maxSort);
                foreach (var task in tasks)
                {
                    if (isSubmit)
                    {
                        task.Status = 9;
                        task.CompletedTime1 = DateTime.Now;
                    }
                    else
                    {
                        task.Status = 0;
                        task.CompletedTime1 = null;
                    }

                    _roadFlowWorkFlowTaskRepository.Update(task);
                }
            }
        }


        public bool GetStopIsCreateByProjecetLeader(Guid taskId)
        {
            var task = _roadFlowWorkFlowTaskRepository.Get(taskId);
            var firstTask = _roadFlowWorkFlowTaskRepository.FirstOrDefault(r => r.FlowID == task.FlowID && r.InstanceID == task.InstanceID && r.Sort == 1);
            var stopId = task.InstanceID.ToGuid();
            var stopModel = _projectAuditStopRepository.Get(stopId);
            var query = from member in _projectAuditMemberRepository.GetAll()
                        join auditrole in _projectAuditRoleRepository.GetAll() on member.UserAuditRole equals auditrole.Id
                        where member.ProjectBaseId == stopModel.ProjectId && member.UserAuditRole == 1
                        select member;
            var members = query.FirstOrDefault();
            /// 项目负责人不存在的时候，按项目负责人发起的逻辑处理；
            if (members == null) return true;
            if (members.UserId == firstTask.ReceiveID)
                return true;
            else
                return false;
        }

        [RemoteService(IsEnabled = false)]
        public string GetProjectLeaderForStop(Guid instanceId)
        {
            var stopModel = _projectAuditStopRepository.Get(instanceId);
            var query = from member in _projectAuditMemberRepository.GetAll()
                        join auditrole in _projectAuditRoleRepository.GetAll() on member.UserAuditRole equals auditrole.Id
                        where member.ProjectBaseId == stopModel.ProjectId && member.UserAuditRole == 1
                        select member;
            var members = query.FirstOrDefault();
            if (members == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常，查询项目负责人失败");
            return MemberPerfix.UserPREFIX + members.UserId;
        }


        /// <summary>
        /// 停滞申请驳回
        /// </summary>
        [RemoteService(IsEnabled = false)]
        public void RejectProjectStop(Guid instanceId)
        {
            var stopModel = _projectAuditStopRepository.Get(instanceId);
            var singleProjectModel = _singleProjectInfoRepository.Get(stopModel.ProjectId);
            singleProjectModel.IsStop = ProjectStopTypeEnum.NoStop;
            _singleProjectInfoRepository.Update(singleProjectModel);
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = singleProjectModel.Id;
            noticeInput.Content = "停滞申请驳回";
            noticeInput.UserType = 1;
            noticeInput.Title = $"项目：{singleProjectModel.SingleProjectName} 停滞申请被驳回";
            noticeService.CreateProjectWorkSpaceNoticeSync(noticeInput);

            StopProjectInWorkFlow(singleProjectModel.Id, AuditFlowId.ToGuid(), false);

        }

        /// <summary>
        /// 解除停滞申请驳回
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(IsEnabled = false)]
        public void ReturnProjectRelieve(Guid instanceId)
        {
            var stopModel = _projectAuditStopRepository.Get(instanceId);
            var singleProjectModel = _singleProjectInfoRepository.Get(stopModel.ProjectId);
            singleProjectModel.IsStop = ProjectStopTypeEnum.WaitRelieve;
            _singleProjectInfoRepository.Update(singleProjectModel);
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = singleProjectModel.Id;
            noticeInput.Content = "解除停滞被退回";
            noticeInput.UserType = 1;
            noticeInput.Title = $"项目：{singleProjectModel.SingleProjectName} 解除停滞被退回";
            noticeService.CreateProjectWorkSpaceNoticeSync(noticeInput);
        }

        /// <summary>
        /// 完成解除停滞
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(IsEnabled = false)]
        public void PassProjectStop(Guid instanceId)
        {
            var stopModel = _projectAuditStopRepository.Get(instanceId);
            var singleProjectModel = _singleProjectInfoRepository.Get(stopModel.ProjectId);
            singleProjectModel.IsStop = ProjectStopTypeEnum.WaitRelieve;
            _singleProjectInfoRepository.Update(singleProjectModel);
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = singleProjectModel.Id;
            noticeInput.Content = "开始停滞";
            noticeInput.UserType = 1;
            noticeInput.Title = $"项目：{singleProjectModel.SingleProjectName} 开始停滞";
            noticeService.CreateProjectWorkSpaceNoticeSync(noticeInput);

        }


        /// <summary>
        /// 完成解除停滞
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(IsEnabled = false)]
        public void CompleteRejectProject(Guid instanceId)
        {
            var stopModel = _projectAuditStopRepository.Get(instanceId);
            var singleProjectModel = _singleProjectInfoRepository.Get(stopModel.ProjectId);
            singleProjectModel.IsStop = ProjectStopTypeEnum.NoStop;
            _singleProjectInfoRepository.Update(singleProjectModel);
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = singleProjectModel.Id;
            noticeInput.Content = "停滞结束";
            noticeInput.UserType = 1;
            noticeInput.Title = $"项目：{singleProjectModel.SingleProjectName} 停滞结束";
            noticeService.CreateProjectWorkSpaceNoticeSync(noticeInput);

            StopProjectInWorkFlow(singleProjectModel.Id, AuditFlowId.ToGuid(), false);
        }











    }
}
