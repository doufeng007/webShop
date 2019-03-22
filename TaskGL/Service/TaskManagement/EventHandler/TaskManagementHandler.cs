using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Abp.File;
using Abp.Runtime.Caching;
using Abp.WorkFlow;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;

namespace TaskGL
{
    public class TaskManagementHandler : IEventHandler<TaskManagementData>, ISingletonDependency
    {
        private readonly IRepository<TaskManagement, Guid> _repository;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        public TaskManagementHandler(IRepository<TaskManagement, Guid> repository, IRepository<WorkFlowTask, Guid> workFlowTaskRepository)
        {
            _repository = repository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        public void HandleEvent(TaskManagementData eventData)
        {
            var model = _repository.Get(eventData.Id);
            model.TaskStatus = eventData.TaskStatus;
            _repository.Update(model);
            if (model.TaskStatus == TaskManagementStateEnum.Done)
            {
                var users = model.UserId + ","  + model.Superintendent;
                var taskUsers = string.Join(',', _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == model.Id.ToString()).Select(x => x.ReceiveID).ToList());
                users += "," + taskUsers;
                var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
                var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
                noticeInput.Content = $"任务完成 通知 <a class='ivu-table' href='/#/task/detail?instanceID={model.Id}'>查看详细</a>";
                noticeInput.Title = $"{model.TaskName}  已完成 ";
                noticeInput.NoticeUserIds = users;
                noticeInput.NoticeType = 1;
                noticeInput.SendUserId = model.CreatorUserId.Value;
                noticeService.CreateOrUpdateNotice(noticeInput);
            }
        }
    }
}
