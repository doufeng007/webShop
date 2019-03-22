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
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Authorization.Users;

namespace TaskGL
{
    public class TaskManagementChangeAppService : FRMSCoreAppServiceBase, ITaskManagementChangeAppService
    { 
        private readonly IRepository<TaskManagementChange, Guid> _repository;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IRepository<TaskManagement, Guid> _taskManagementRepository;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        public TaskManagementChangeAppService(IRepository<TaskManagementChange, Guid> repository, IRepository<User, long> usersRepository, IRepository<TaskManagement, Guid> taskManagementRepository, IRepository<WorkFlowTask, Guid> workFlowTaskRepository)
        {
            this._repository = repository;
            _usersRepository = usersRepository;
            _taskManagementRepository = taskManagementRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }
		
	    /// <summary>
        /// 获取变更列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<TaskManagementChangeListOutputDto>> GetList(GetTaskManagementChangeListInput input)
        {
			var query = from a in _repository.GetAll()
                        join b in _usersRepository.GetAll() on a.CreatorUserId equals b.Id
                        where a.TaskManagementId==input.TaskManagementId
                        select new TaskManagementChangeListOutputDto()
                        {
                            Id = a.Id,
                            Type = a.Type,
                            TypeName = a.Type.GetLocalizedDescription(),
                            ReasonName = a.Reason.GetLocalizedDescription(),
                            TaskManagementId = a.TaskManagementId,
                            Reason = a.Reason,
                            Assessment = a.Assessment,
                            Requirement = a.Requirement,
                            PerformanceScore = a.PerformanceScore,
                            SpiritScore = a.SpiritScore,
                            CreatorUserId = a.CreatorUserId,
                            CreationTime = a.CreationTime,
                            CreatorUserName = b.Name
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.PageBy(input).ToListAsync();
			
            return new PagedResultDto<TaskManagementChangeListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<TaskManagementChangeOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<TaskManagementChangeOutputDto>();
		}
        /// <summary>
        /// 创建变更
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task<Guid> Create(CreateTaskManagementChangeInput input)
        {
            var model =  _taskManagementRepository.Get(input.TaskManagementId);
            switch (input.Type)
            {
                case Enum.TaskManagementChangeTypeEnum.TaskRedo:
                    model.TaskStatus = TaskManagementStateEnum.Redo;
                    break;
                case Enum.TaskManagementChangeTypeEnum.RevokeTask:
                    model.TaskStatus = TaskManagementStateEnum.Revoke;
                    break;
            }
            if (input.Type != Enum.TaskManagementChangeTypeEnum.ChangeMission) { 
                await _taskManagementRepository.UpdateAsync(model);
                var tasks = _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == model.Id.ToString() && x.Status.In(0, 1)).ToList();
                foreach (var task in tasks)
                {
                    task.Status = 2;
                    task.CompletedTime1 = DateTime.Now;
                    await _workFlowTaskRepository.UpdateAsync(task);
                }
            }

            var users = model.UserId + "," + model.TaskCreateUserId + "," + model.Superintendent;
            var taskUsers =string.Join(',', _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == model.Id.ToString()).Select(x => x.ReceiveID).ToList());
            users += "," + taskUsers;
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
            var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
            noticeInput.Content = $"{input.Type.GetLocalizedDescription()} 通知 <a class='ivu-table' href='/#/task/detail?instanceID={model.Id}'>查看详细</a>";
            noticeInput.Title = $"{model.TaskName} {input.Type.GetLocalizedDescription()} ";
            noticeInput.NoticeUserIds = users;
            noticeInput.NoticeType = 1;
            noticeInput.SendUserId = model.CreatorUserId.Value;
            noticeService.CreateOrUpdateNotice(noticeInput);

            var id = Guid.NewGuid();
            var newmodel = new TaskManagementChange()
            {
                Id = id,
                Type = input.Type,
                TaskManagementId = input.TaskManagementId,
                Reason = input.Reason,
                Assessment = input.Assessment,
                Requirement = input.Requirement,
                PerformanceScore = input.PerformanceScore,
                SpiritScore = input.SpiritScore,
                CreatorUserId = AbpSession.UserId.Value,
                CreationTime = DateTime.Now
            };
            await _repository.InsertAsync(newmodel);
            return id;
        }

		/// <summary>
        ///修改变更
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateTaskManagementChangeInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.Type = input.Type;
			   dbmodel.TaskManagementId = input.TaskManagementId;
			   dbmodel.Reason = input.Reason;
			   dbmodel.Assessment = input.Assessment;
			   dbmodel.Requirement = input.Requirement;
			   dbmodel.PerformanceScore = input.PerformanceScore;
			   dbmodel.SpiritScore = input.SpiritScore;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		
		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}