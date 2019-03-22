using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Extensions;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.File;

namespace Project
{
    public class OATaskAppService : FRMSCoreAppServiceBase, IOATaskAppService
    {
        private readonly IRepository<OATask, Guid> _oATaskRepository;
        private readonly IRepository<OATaskUser, Guid> _oATaskUserRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        public OATaskAppService(IRepository<OATask, Guid> oATaskRepository, IRepository<OATaskUser, Guid> oATaskUserRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService,
            IWorkFlowTaskRepository workFlowTaskRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager)
        {
            _oATaskRepository = oATaskRepository;
            _oATaskUserRepository = oATaskUserRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _organizeRepository = organizeRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OATaskDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oATask = await _oATaskRepository.GetAsync(id);
            var oAusers = await _oATaskUserRepository.GetAll().Where(r => r.OATaskId == oATask.Id).ToListAsync();
            var output = oATask.MapTo<OATaskDto>();
            var valUser = await UserManager.GetUserByIdAsync(output.ValUser);
            output.ValUser_Name = valUser.Name;
            oAusers.ForEach(r =>
            {
                var entity = new CreateOrUpdateOATaskUserInput();
                entity.OATaskId = r.OATaskId;
                entity.UserId = r.UserId;
                output.Users.Add(entity);
            });
            output.ExecutorUser_Name = _workFlowOrganizationUnitsManager.GetNames(output.ExecutorUser);

            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA任务附件 });

            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OATaskListDto>> GetAll(GetOATaskListInput input)
        {

            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oATaskRepository.GetAll()
                        join musers in _oATaskUserRepository.GetAll() on m.Id equals musers.OATaskId into muser
                        where (input.ValByCurrentUser && m.ValUser == currentUserId)
                        || (input.HasParticipate && muser.Select(r => r.UserId).Contains(currentUserId)) || (input.CreateByCurrentUser && m.CreatorUserId == currentUserId)
                        select m;

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Title.Contains(input.SearchKey));

            }
            var count = await query.CountAsync();
            var oATasks = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oATaskDtos = new List<OATaskListDto>();
            foreach (var oATask in oATasks)
            {
                var entity = new OATaskListDto() { Id = oATask.Id, Title = oATask.Title };
                //entity.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, entity.Status);
                entity.InstanceId = oATask.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, entity as BusinessWorkFlowListOutput);
                oATaskDtos.Add(entity);
            }
            return new PagedResultDto<OATaskListDto>(count, oATaskDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OATaskCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oATask = new OATask();
            input.MapTo(oATask);
            oATask.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oATask.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA任务附件,
                    Files = fileList
                });
            }
            oATask.NotifyUsers = oATask.ExecutorUser;
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = manager.GetAllUsers(input.ExecutorUser);
            foreach (var user in users)
            {
                var entity = new OATaskUser();
                entity.Id = Guid.NewGuid();
                entity.OATaskId = oATask.Id;
                entity.UserId = user.Id;
                await _oATaskUserRepository.InsertAsync(entity);
            }

            oATask.ValUser1 = "u_" + input.ValUser.ToString();
            oATask.NotifyUsers = oATask.ExecutorUser + ",u_" + oATask.ValUser;
            await _oATaskRepository.InsertAsync(oATask);
            ret.InStanceId = oATask.Id.ToString();
            return ret;
        }


        public async Task Update(OATaskUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oATask = await _oATaskRepository.GetAsync(input.Id);
            input.MapTo(oATask);
            var fileList = new List<AbpFileListInput>();
            if (input.FileList != null)
            {
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
            }
            await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.OA任务附件,
                Files = fileList
            });
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = manager.GetAllUsers(input.ExecutorUser);
            await _oATaskUserRepository.DeleteAsync(r => r.OATaskId == input.Id);

            foreach (var item in users)
            {
                var entity = new OATaskUser();
                entity.Id = Guid.NewGuid();
                entity.OATaskId = input.Id;
                entity.UserId = item.Id;
                await _oATaskUserRepository.InsertAsync(entity);
            }
            oATask.ValUser1 = "u_" + oATask.ValUser;
            oATask.NotifyUsers = oATask.ExecutorUser + ",u_" + oATask.ValUser;
            await _oATaskRepository.UpdateAsync(oATask);

        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="oATaskId"></param>
        /// <returns></returns>
        public async Task BeginOATask(Guid oATaskId, Guid taskId)
        {
            var model = await _oATaskRepository.GetAsync(oATaskId);
            model.Status = 2;
            var task = await _workFlowTaskRepository.GetAsync(taskId);
            task.Status = 1;
            await _workFlowTaskRepository.UpdateAsync(task);
            await _oATaskRepository.UpdateAsync(model);
        }
    }
}

