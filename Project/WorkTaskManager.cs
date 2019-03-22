using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;

namespace Project
{
    public class WorkTaskManager : ITransientDependency
    {
        private readonly IRepository<ProjectWorkTask, Guid> _projectWorkTaskRepository;
        private readonly IRepository<ProjectWorkLog, Guid> _projectWorkLogRepository;
        private readonly IRepository<ProjectAudit, Guid> _projectAuditRepository;
        private readonly IRepository<ProjectInformationEnter, Guid> _projectInformationEnterRepository;
        private readonly IRepository<ProjectRegistration, Guid> _projectRegistrationRepository;
        private readonly IRepository<DispatchMessage, Guid> _dispatchMessageRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        public WorkTaskManager(IRepository<ProjectWorkTask, Guid> projectWorkTaskRepository, IRepository<ProjectWorkLog, Guid> projectWorkLogRepository,
            IRepository<ProjectAudit, Guid> projectAuditRepository, IRepository<ProjectInformationEnter, Guid> projectInformationEnterRepository,
            IRepository<ProjectRegistration, Guid> projectRegistrationRepository,
            IRepository<DispatchMessage, Guid> dispatchMessageRepository,
            IProjectBaseRepository projectBaseRepository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository)
        {
            _projectWorkTaskRepository = projectWorkTaskRepository;
            _projectWorkLogRepository = projectWorkLogRepository;
            _projectAuditRepository = projectAuditRepository;
            _projectInformationEnterRepository = projectInformationEnterRepository;
            _projectRegistrationRepository = projectRegistrationRepository;
            _dispatchMessageRepository = dispatchMessageRepository;
            _projectBaseRepository = projectBaseRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _singleProjectInfoRepository = singleProjectInfoRepository;
        }

        public async Task<Guid> InsertWorkLog(ProjectWorkTask task, ProjectWorkLog log)
        {
            var taskId = Guid.NewGuid();
            var instanceId = Guid.NewGuid();
            task.Id = taskId;
            task.TaskType = 1;
            task.CreationTime = DateTime.Now;
            task.InstanceId = instanceId;
            await _projectWorkTaskRepository.InsertAsync(task);
            log.Id = instanceId;
            log.TaskId = taskId;
            log.CreationTime = DateTime.Now;
            await _projectWorkLogRepository.InsertAsync(log);
            return instanceId;

        }

        public async Task<Guid> InsertRegistration(ProjectWorkTask task, ProjectRegistration registration, List<GetAbpFilesOutput> files)
        {
            var taskId = Guid.NewGuid();
            var instanceId = Guid.NewGuid();
            task.Id = taskId;
            task.CreationTime = DateTime.Now;
            task.InstanceId = instanceId;
            await _projectWorkTaskRepository.InsertAsync(task);

            registration.Id = instanceId;
            registration.TaskId = taskId;
            registration.RecieveUserId = 0;
            registration.IsRead = false;
            registration.IsSendEmail = false;
            registration.IsSendSms = false;
            registration.IsSendWx = false;
            registration.IsSendDispatch = false;
            registration.CreationTime = DateTime.Now;
            await _projectRegistrationRepository.InsertAsync(registration);
            var fileList = new List<AbpFileListInput>();
            foreach (var filemodel in files)
            {
                var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                fileList.Add(fileone);
            }

            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = instanceId.ToString(),
                BusinessType = (int)AbpFileBusinessType.工作联系附件,
                Files = fileList
            });

            return instanceId;

        }

        public async Task InsertAudit(ProjectWorkTask task, List<LogColumnModel> list)
        {
            var taskId = Guid.NewGuid();
            task.Id = taskId;
            //task.TaskType = 2;
            task.CreationTime = DateTime.Now;
            await _projectWorkTaskRepository.InsertAsync(task);

            foreach (var m in list)
            {
                await _projectAuditRepository.InsertAsync(new ProjectAudit
                {
                    Id = Guid.NewGuid(),
                    TaskId = task.Id,
                    UserId = task.UserId,
                    InstanceId = task.ProjectId.ToString(),
                    CreationTime = DateTime.Now,
                    FieldName = m.FieldName,
                    OldValue = m.OldValue,
                    NewValue = m.NewValue,
                    ChangeType = m.ChangeType,
                    TableName = "ProjectBase"

                });
            }



        }

        public async Task InsertInformationEnter(ProjectWorkTask task, ProjectInformationEnter enter)
        {
            var taskId = Guid.NewGuid();
            var instanceId = Guid.NewGuid();
            task.Id = taskId;
            task.TaskType = 3;
            task.CreationTime = DateTime.Now;
            task.InstanceId = instanceId;
            await _projectWorkTaskRepository.InsertAsync(task);
            enter.Id = instanceId;
            enter.TaskId = taskId;
            enter.ProjectId = task.ProjectId.Value;
            enter.CreationTime = DateTime.Now;
            await _projectInformationEnterRepository.InsertAsync(enter);

        }

        public async Task InsertDispatchAsync(ProjectWorkTask task, DispatchMessage dispatch)
        {
            var taskId = Guid.NewGuid();
            var instanceId = Guid.NewGuid();
            task.Id = taskId;
            task.TaskType = 5;
            task.CreationTime = DateTime.Now;
            task.InstanceId = instanceId;
            await _projectWorkTaskRepository.InsertAsync(task);
            dispatch.Id = instanceId;
            dispatch.TaskId = taskId;
            dispatch.StartDate =
            dispatch.CreationTime = DateTime.Now;
            await _dispatchMessageRepository.InsertAsync(dispatch);
        }

       

        public async Task InsertReturnAuditAsync(ProjectWorkTask task, string returnauditSummary)
        {
            var taskId = Guid.NewGuid();
            //var instanceId = Guid.NewGuid();
            task.Id = taskId;
            task.TaskType = 6;
            task.CreationTime = DateTime.Now;
            task.InstanceId = task.ProjectId;
            await _projectWorkTaskRepository.InsertAsync(task);
            var model = await _singleProjectInfoRepository.GetAsync(task.ProjectId.Value);
            model.IsReturnAudit = true;
            model.ReturnAuditSmmary = returnauditSummary;
            await _singleProjectInfoRepository.UpdateAsync(model);
        }


    }
}