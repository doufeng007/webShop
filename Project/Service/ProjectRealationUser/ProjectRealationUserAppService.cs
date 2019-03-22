using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectRealationUserAppService : ApplicationService, IProjectRealationUserAppService
    {
        private readonly IRepository<WorkFlowTask, Guid> _task;
        private readonly IRepository<ProjectRealationUser, Guid> _projectRelationUser;
        private readonly IRepository<ProjectRegistration, Guid> _projectRegistration;
        private readonly IRepository<ProjectBase, Guid> _projectBase;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectRepository;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTask;

        public ProjectRealationUserAppService(IRepository<WorkFlowTask, Guid> task, IRepository<ProjectRealationUser, Guid> projectRelationUser,
            IRepository<ProjectRegistration, Guid> projectRegistration, IRepository<ProjectBase, Guid> projectBase, IRepository<WorkFlowTask, Guid> workFlowTask
            , IRepository<SingleProjectInfo, Guid> singleProjectRepository)
        {
            _task = task;
            _projectRelationUser = projectRelationUser;
            _projectRegistration = projectRegistration;
            _projectBase = projectBase;
            _workFlowTask = workFlowTask;
            _singleProjectRepository = singleProjectRepository;
        }
        public void Create(ProjectRelationUserCreate input)
        {
            var task = _task.Get(input.TaskID);
            if (task == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到对应的待办");
            var instanceId = Guid.Empty;
            var projectId = Guid.Empty;
            var singleProjectModel = _singleProjectRepository.FirstOrDefault(task.InstanceID.ToGuid());
            if (singleProjectModel != null)
            {
                instanceId = singleProjectModel.Id;
                projectId = singleProjectModel.ProjectId;
            }
            else
            {
                var projectModel = _projectBase.FirstOrDefault(task.InstanceID.ToGuid());
                if (projectModel == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数错误");
                instanceId = projectModel.Id;
                projectId = projectModel.Id;
            }
            foreach (var nexttask in input.NextTasks)
            {
                var model = new ProjectRealationUser()
                {
                    FlowID = input.FlowID,
                    InstanceID = Guid.Parse(input.InstanceID),
                    StepID = input.StepID,
                    UserID = nexttask.ReceiveID,
                    ProjectId = projectId,
                };
                _projectRelationUser.Insert(model);
            }

        }


        public string GetProjectRealationMember(ProjectRelationUserCreate input)
        {
            var id = input.InstanceID.ToGuid();

            var prModel = _projectRegistration.GetAll().FirstOrDefault(x => x.Id == id);
            var sort = (from a in _workFlowTask.GetAll()
                        where a.InstanceID == prModel.ProjectId.ToString()
                        select a).Max(x => x.Sort);
            var list = (from a in _workFlowTask.GetAll()
                        where a.InstanceID == prModel.ProjectId.ToString() && a.Sort == sort
                        select a.ReceiveID).ToList();

            var task = _workFlowTask.GetAll().FirstOrDefault(x => x.Id == input.TaskID);

            StringBuilder sb = new StringBuilder();
            if (list != null)
            {
                foreach (var u in list)
                {
                    if (u == task.ReceiveID)
                        continue;
                    sb.Append("u_");
                    sb.Append(u);
                    sb.Append(",");
                }
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
