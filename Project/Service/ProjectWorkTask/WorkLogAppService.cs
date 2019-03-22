using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.File;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Authorization.Users;

namespace Project
{
    public class WorkLogAppService : ApplicationService, IWorkLogAppService
    {
        private readonly IRepository<ProjectWorkLog, Guid> _projectWorkLogRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProjectWorkTask, Guid> _projectWorkTaskRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectBaseRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public WorkLogAppService(IRepository<ProjectWorkLog, Guid> projectWorkLogRepository, IRepository<User, long> userRepository, IRepository<ProjectWorkTask, Guid> projectWorkTaskRepository,
            IProjectBaseRepository projectBaseRepository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<SingleProjectInfo, Guid> singleProjectBaseRepository)
        {
            _singleProjectBaseRepository = singleProjectBaseRepository;
            _projectWorkLogRepository = projectWorkLogRepository;
            _userRepository = userRepository;
            _projectWorkTaskRepository = projectWorkTaskRepository;
            _projectBaseRepository = projectBaseRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        public async Task<WriteLogViewModelOut> GetForEdit(NullableIdDto<Guid> input)
        {
            if (!input.Id.HasValue)
                return new WriteLogViewModelOut();
            var model = await _projectWorkLogRepository.GetAsync(input.Id.Value);
            var model_task = await _projectWorkTaskRepository.GetAsync(model.TaskId);
            var singleProject = await _singleProjectBaseRepository.GetAsync(model.ProjectId.Value);
            var projectmodel = await _projectBaseRepository.GetAsync(singleProject.ProjectId);
            var output = new WriteLogViewModelOut();
            output.ProjectCode = projectmodel.ProjectCode;
            output.ProjectId = projectmodel.Id;
            output.ProjectName = projectmodel.ProjectName;
            output.StepId = model_task.StepId.Value;
            output.StepName = model_task.StepName;
            output.Content = model.Content;
            output.Title = model.Title;
            output.LogType = model.LogType;
            output.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工作底稿附件 });
            return output;
        }

        public async Task<PagedResultDto<WorkLogList>> GetWorkLogPage(GetWorkLogListInput input)
        {
            var userId = base.AbpSession.UserId;

            var query = (from log in _projectWorkLogRepository.GetAll()
                         join user in _userRepository.GetAll() on log.UserId equals user.Id into worklog
                         from user in worklog.DefaultIfEmpty()
                         where log.ProjectId == input.ProjectId
                         //&& log.UserId == userId
                         orderby log.CreationTime descending
                         select new WorkLogList
                         {
                             Id = log.Id,
                             CreationTime = log.CreationTime,
                             //Content = log.Content,
                             ProjectId = log.ProjectId,
                             Title = log.Title,
                             UserId = log.UserId,
                             UserName = user.Name,
                             TaskId = log.TaskId,
                             FilesJson = log.Files,
                             LogType = log.LogType,
                             StepId = log.StepId,
                             StepName = log.StepName
                         });
            if (query == null)
                return new PagedResultDto<WorkLogList>(0, new List<WorkLogList>());

            var total = await query.CountAsync();
            var list = await query.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(ite => ite.LogType).OrderBy(ite => ite.StepId).ToListAsync();
            foreach (var l in list)
            {
                l.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = l.Id.ToString(), BusinessType = (int)AbpFileBusinessType.工作底稿附件 });
            }
            return new PagedResultDto<WorkLogList>(total, list);
        }
    }
}