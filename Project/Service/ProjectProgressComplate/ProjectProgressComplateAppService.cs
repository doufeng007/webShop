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
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.WorkFlow;
using Abp.Domain.Uow;
using Abp.SignalR.Core;
using Abp.RealTime;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectProgressComplateAppService : FRMSCoreAppServiceBase, IProjectProgressComplateAppService
    {
        private readonly IRepository<ProjectProgressComplate, Guid> _repository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectRepository;
        private readonly IRepository<ProjectBase, Guid> _projectRepository;
        private readonly IRepository<ProjectProgressConfig, Guid> _projectProgressConfigRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IBackgroudWorkJobWithHangFire _backgroudWorkJobWithHangFire;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IRepository<ProjectProgressFault, Guid> _projectProgressFaultRepository;
        public ProjectProgressComplateAppService(IRepository<ProjectProgressComplate, Guid> repository, IRepository<SingleProjectInfo, Guid> singleProjectRepository,
            IBackgroudWorkJobWithHangFire backgroudWorkJobWithHangFire, IWorkFlowTaskRepository workFlowTaskRepository,
            IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, IOnlineClientManager onlineClientManager, IRepository<ProjectProgressFault, Guid> projectProgressFaultRepository,
        IRepository<ProjectBase, Guid> projectRepository, IRepository<ProjectProgressConfig, Guid> projectProgressConfigRepository)
        {
            _repository = repository;
            _singleProjectRepository = singleProjectRepository;
            _projectRepository = projectRepository;
            _projectProgressConfigRepository = projectProgressConfigRepository;
            _backgroudWorkJobWithHangFire = backgroudWorkJobWithHangFire;
            _workFlowTaskRepository = workFlowTaskRepository;
            _onlineClientManager = onlineClientManager;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _projectProgressFaultRepository = projectProgressFaultRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProjectProgressComplateListOutputDto>> GetList(GetProjectProgressComplateListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        select new ProjectProgressComplateListOutputDto()
                        {
                            Id = a.Id,
                            ProjectBaseId = a.ProjectBaseId,
                            FirstAuditComplateTime = a.FirstAuditComplateTime,
                            FirstAduitDelayHour = a.FirstAduitDelayHour,
                            JiliangComplateTime = a.JiliangComplateTime,
                            JiliangDelayHour = a.JiliangDelayHour,
                            JijiaComplateTime = a.JijiaComplateTime,
                            JijiaDelayHour = a.JijiaDelayHour,
                            SelfAuditComplateTime = a.SelfAuditComplateTime,
                            SelfAuditDelayHour = a.SelfAuditDelayHour,
                            SecondAuditComplateTime = a.SecondAuditComplateTime,
                            SecondAuditDelayHour = a.SecondAuditDelayHour,
                            LastAuditComplateTime = a.LastAuditComplateTime,
                            LastAuditDelayHour = a.LastAuditDelayHour,
                            CreationTime = a.CreationTime,
                            Status = a.Status
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<ProjectProgressComplateListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<ProjectProgressComplateOutputDto> Get(NullableIdDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var fault = await _projectProgressFaultRepository.GetAll().Where(ite => ite.ProgressComplate == input.Id.Value).ToListAsync();
            var ret = model.MapTo<ProjectProgressComplateOutputDto>();
            if (fault != null)
            {
                ret.ProjectProgressFault = fault.MapTo<List<ProjectProgressFaultDto>>();
            }

            return ret;
        }
        /// <summary>
        /// 添加一个ProjectProgressComplate
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateProjectProgressComplateInput input)
        {
            var project = _projectRepository.Get(input.ProjectBaseId);

            var newmodel = new ProjectProgressComplate()
            {
                ProjectBaseId = project.Id,
            };
            newmodel.Id = Guid.NewGuid();
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }
        /// <summary>
        /// 创建待办并发送到下一步
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public async Task Send(Guid projectId)
        {
            var project = _singleProjectRepository.Get(projectId);
            var newmodel = new ProjectProgressComplate()
            {
                ProjectBaseId = project.Id,
            };
            newmodel.Id = Guid.NewGuid();
            var task = new InitWorkFlowOutput();
            await _repository.InsertAsync(newmodel);
            task = _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
            {
                FlowId = new Guid("812eed79-c3dc-4381-9eab-f0c424c3049d"),
                FlowTitle = $"{project.SingleProjectName}完成进度",
                InStanceId = newmodel.Id.ToString()
            });
            CurrentUnitOfWork.SaveChanges();
            var flower = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowAppService>();
            var next = await flower.GetNextStepForRun(new GetNextStepForRunInput()
            {
                TaskId = task.TaskId
            });
            var run = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            //var defaultUserId = "";//根据项目评审组获取第一联系人为默认接受者
            var steps = next.Steps.Select(ite => new ExecuteWorkChooseStep() { id = ite.NextStepId.ToString(), member = ite.DefaultUserId }).ToList();
            var ret = await run.ExecuteTask(new ExecuteWorkFlowInput()
            {
                ActionType = "submit",
                FlowId = task.FlowId,
                GroupId = task.GroupId,
                InstanceId = task.InStanceId,
                IsHideNextTask = true,
                StepId = task.StepId,
                Steps = steps,
                TaskId = task.TaskId,
                Title = $"{project.SingleProjectName}【{project.ProjectStatus.ToString()}阶段】完成进度"
            });
            ChangeProjectStatus(task.TaskId, Guid.Parse(task.InStanceId));
        }
        /// <summary>
        /// 当前项目状态完成后修改项目状态和项目进度记录
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="processId"></param>
        private void ChangeProjectStatus(Guid taskId, Guid processId)
        {

            var process = _repository.Get(processId);
            var project = _projectRepository.Get(process.ProjectBaseId);
            var hours = 0;
            //定时
            var x = _projectProgressConfigRepository.GetAll().ToList();
            var config = _projectProgressConfigRepository.GetAll().FirstOrDefault(ite => ite.ProjectBaseId == process.ProjectBaseId);
            if (config == null)
            {
                config = _projectProgressConfigRepository.GetAll().FirstOrDefault(ite => ite.ProjectBaseId.HasValue == false);
            }
            if (config == null)
            {
                //throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到进度配置。");
                config = new ProjectProgressConfig();
                config.FirstAuditKey = 10;
                config.JiliangKey = 20;
                config.JijiaKey = 20;
                config.SelfAuditKey = 20;
                config.SecondAuditKey = 20;
                config.LastAuditKey = 10;
            }
            switch (project.ProjectStatus)
            {
                case ProjectStatus.在审://
                    hours = project.Days * 24 * config.FirstAuditKey / 100;
                    project.ProjectStatus = ProjectStatus.初审;
                    break;
                case ProjectStatus.初审:
                    hours = project.Days * 24 * config.JiliangKey / 100;
                    project.ProjectStatus = ProjectStatus.计量;
                    process.FirstAuditComplateTime = DateTime.Now;
                    var delayHour = process.FirstAuditComplateTime - process.CreationTime;
                    delayHour = delayHour.Value.Add(new TimeSpan(0 - (project.Days * 24 * config.FirstAuditKey / 100), 0, 0));
                    process.FirstAduitDelayHour = (int)delayHour.Value.TotalHours;
                    break;
                case ProjectStatus.计量:
                    hours = project.Days * 24 * config.JijiaKey / 100;
                    project.ProjectStatus = ProjectStatus.记价;
                    process.JiliangComplateTime = DateTime.Now;
                    delayHour = process.JiliangComplateTime - process.FirstAuditComplateTime;
                    delayHour = delayHour.Value.Add(new TimeSpan(0 - (project.Days * 24 * config.JiliangKey / 100), 0, 0));
                    process.JiliangDelayHour = (int)delayHour.Value.TotalHours;
                    break;
                case ProjectStatus.记价:
                    hours = project.Days * 24 * config.SelfAuditKey / 100;
                    project.ProjectStatus = ProjectStatus.内核;
                    process.JijiaComplateTime = DateTime.Now;
                    delayHour = process.JijiaComplateTime - process.JiliangComplateTime;
                    delayHour = delayHour.Value.Add(new TimeSpan(0 - (project.Days * 24 * config.JijiaKey / 100), 0, 0));
                    process.JijiaDelayHour = (int)delayHour.Value.TotalHours;
                    break;
                case ProjectStatus.内核:
                    hours = project.Days * 24 * config.SecondAuditKey / 100;
                    project.ProjectStatus = ProjectStatus.复核;
                    process.SelfAuditComplateTime = DateTime.Now;
                    delayHour = process.SelfAuditComplateTime - process.JijiaComplateTime;
                    delayHour = delayHour.Value.Add(new TimeSpan(0 - (project.Days * 24 * config.SelfAuditKey / 100), 0, 0));
                    process.SelfAuditDelayHour = (int)delayHour.Value.TotalHours;
                    break;
                case ProjectStatus.复核:
                    hours = project.Days * 24 * config.LastAuditKey / 100;
                    project.ProjectStatus = ProjectStatus.总核;
                    process.SecondAuditComplateTime = DateTime.Now;
                    delayHour = process.SecondAuditComplateTime - process.SelfAuditComplateTime;
                    delayHour = delayHour.Value.Add(new TimeSpan(0 - (project.Days * 24 * config.SecondAuditKey / 100), 0, 0));
                    process.SecondAuditDelayHour = (int)delayHour.Value.TotalHours;
                    break;
                case ProjectStatus.总核:
                    process.LastAuditComplateTime = DateTime.Now;
                    delayHour = process.LastAuditComplateTime - process.SecondAuditComplateTime;
                    delayHour = delayHour.Value.Add(new TimeSpan(0 - (project.Days * 24 * config.LastAuditKey / 100), 0, 0));
                    process.LastAuditDelayHour = (int)delayHour.Value.TotalHours;
                    break;
            }
            _projectRepository.Update(project);
            _repository.Update(process);
            _backgroudWorkJobWithHangFire.CreatProjectProgressTask(process.Id, taskId, hours);
            CurrentUnitOfWork.SaveChanges();
        }
        /// <summary>
        /// 修改一个ProjectProgressComplate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(ProjectProgressComplateOutputDto input)
        {
            if (input.ProjectProgressFault != null)
            {
                var ids = input.ProjectProgressFault.Select(ite => ite.Id);
                await _projectProgressFaultRepository.DeleteAsync(ite => ite.ProgressComplate == input.Id && !ids.Contains(ite.Id));
                foreach (var a in input.ProjectProgressFault)
                {
                    var model = a.MapTo<ProjectProgressFault>();
                    model.ProgressComplate = input.Id;
                    if (a.Id.HasValue)
                    {
                        await _projectProgressFaultRepository.UpdateAsync(model);
                    }
                    else
                    {
                        await _projectProgressFaultRepository.InsertAsync(model);
                    }

                }
            }

        }

        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }

        /// <summary>
        /// higefire定时器工作项时间到后调用
        /// </summary>
        /// <param name="taskId">这里的taskid为上一步的id  需要找到下一步的id</param>
        public void TimeOut(Guid instancId, Guid taskId)
        {
            var task = _workFlowTaskRepository.GetAll().FirstOrDefault(ite => ite.PrevID == taskId);
            if (task == null)
            {
                //throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前待办。");
                return;
            }
            task.Status = 0;
            _workFlowTaskRepository.Update(task);
            var process = _repository.Get(instancId);
            var project = _projectRepository.Get(process.ProjectBaseId);
            // s ignalr 提醒
            var link = $"/dynamicpage?fid={task.FlowID}&instanceID={task.InstanceID}&stepID={task.StepID}&groupID={task.GroupID}&taskID={task.Id}";// $"/Mpa/Project/DataChange?instanceId={projectId}&appTypeId={projectmodel.AppraisalTypeId}&notInAudit=1";
            var onlineclients =
                _onlineClientManager.GetAllClients().Where(r => r.UserId == task.ReceiveID).ToList();
            var signalrNoticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISignalrNoticeAppService>();

            signalrNoticeService.SendNoticeToClient(onlineclients, task.InstanceID.ToString(), "项目进度提醒", $"项目{project.ProjectName}评审阶段【{project.ProjectStatus.ToString()}】计划时间已到，请确认进度。", link);
            _backgroudWorkJobWithHangFire.RemoveProjectProgressTask(instancId);
        }
        /// <summary>
        /// 项目进度询问，当前状态完成时调用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task FinishAndSend(FinishAndSendInput input)
        {

            var id = Guid.Parse(input.InstanceId);

            var progress = _repository.Get(id);
            var project = _projectRepository.Get(progress.ProjectBaseId);
            var flower = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowAppService>();
            var next = await flower.GetNextStepForRun(new GetNextStepForRunInput() { TaskId = input.TaskId });
            var run = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            //var defaultUserId = "";//根据项目评审组获取第一联系人为默认接受者
            var steps = next.Steps.Select(ite => new ExecuteWorkChooseStep() { id = ite.NextStepId.ToString(), member = ite.DefaultUserId }).ToList();
            var ActionType = "submit";
            if (project.ProjectStatus == ProjectStatus.总核)
            {
                ActionType = "completed";
            }
            var ret = await run.ExecuteTask(new ExecuteWorkFlowInput()
            {
                ActionType = ActionType,
                FlowId = input.FlowId,
                GroupId = input.GroupId,
                InstanceId = input.InstanceId,
                IsHideNextTask = true,
                StepId = input.StepId,
                Steps = steps,
                TaskId = input.TaskId,
                Title = $"{project.ProjectName}【{ ((ProjectStatus)(((int)project.ProjectStatus) * 2)).ToString()}阶段】完成进度"
            });
            ChangeProjectStatus(input.TaskId, id);
        }
    }
}