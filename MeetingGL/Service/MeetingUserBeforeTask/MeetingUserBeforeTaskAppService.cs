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
using Abp.Authorization;
using Abp;
using Abp.Application.Services;

namespace MeetingGL
{
    public class MeetingUserBeforeTaskAppService : FRMSCoreAppServiceBase, IMeetingUserBeforeTaskAppService
    {
        private readonly IRepository<MeetingUserBeforeTask, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        private readonly IRepository<MeetingFile, Guid> _meetingFileRepository;
        private readonly IRepository<MeetingLogisticsRelation, Guid> _meetingLogisticsRelationRepository;

        public MeetingUserBeforeTaskAppService(IRepository<MeetingUserBeforeTask, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager,
            WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository
            , IRepository<MeetingFile, Guid> meetingFileRepository, IRepository<MeetingLogisticsRelation, Guid> meetingLogisticsRelationRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _meetingFileRepository = meetingFileRepository;
            _meetingLogisticsRelationRepository = meetingLogisticsRelationRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<MeetingUserBeforeTaskListOutputDto>> GetList(GetMeetingUserBeforeTaskListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        let openModel = (from b in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value) select b)
                        select new MeetingUserBeforeTaskListOutputDto()
                        {
                            Id = a.Id,
                            MeetingId = a.MeetingId,
                            TaskType = a.TaskType,
                            UserId = a.UserId,
                            Remark = a.Remark,
                            Status = a.Status,
                            CreationTime = a.CreationTime
                            ,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<MeetingUserBeforeTaskListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<MeetingUserBeforeTaskOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var ret = new MeetingUserBeforeTaskOutputDto();
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLMeetingAppService>();
            var meetingInfo = service.GetForView(new EntityDto<Guid>() { Id = model.MeetingId });
            ret.MeetingInfo = meetingInfo.DeepClone();
            ret.MeetingInfo.IssueList = new List<MeetingIssueOutputDto>();
            ret.MeetingInfo.FileList = new List<XZGLMeetingFileOutput>();
            ret.MeetingInfo.LogisticsList = new List<XZGLMeetingLogisticsROutput>();
            if (model.TaskType == 1 || model.TaskType == 3 || model.TaskType == 5 || model.TaskType == 7)
                ret.MeetingInfo.IssueList = meetingInfo.IssueList.Where(r => r.UserId.Split(",").Contains("u_"+AbpSession.UserId.Value.ToString())).ToList();
            if (model.TaskType == 2 || model.TaskType == 3 || model.TaskType == 6 || model.TaskType == 7)
                ret.MeetingInfo.FileList = meetingInfo.FileList.Where(r => r.UserId == AbpSession.UserId.Value).ToList();
            if (model.TaskType == 4 || model.TaskType == 6 || model.TaskType == 7)
                ret.MeetingInfo.LogisticsList = meetingInfo.LogisticsList.Where(r => r.UserId == AbpSession.UserId.Value).ToList();
            return ret;
        }
        /// <summary>
        /// 添加一个MeetingUserBeforeTask
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateMeetingUserBeforeTaskInput input)
        {
            var newmodel = new MeetingUserBeforeTask()
            {
                MeetingId = input.MeetingId,
                TaskType = input.TaskType,
                UserId = input.UserId,
                Remark = input.Remark,
                Status = input.Status
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }


        public InitWorkFlowOutput CreateSelf(CreateMeetingUserBeforeTaskInput input)
        {
            var newmodel = new MeetingUserBeforeTask()
            {
                MeetingId = input.MeetingId,
                TaskType = input.TaskType,
                UserId = input.UserId,
            };
            _repository.Insert(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个MeetingUserBeforeTask
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateMeetingUserBeforeTaskInput input)
        {
            var model =await _repository.GetAsync(input.InStanceId);
            if (model.TaskType == 1 || model.TaskType == 3 || model.TaskType == 5 || model.TaskType == 7)
            { }
            if (model.TaskType == 2 || model.TaskType == 3 || model.TaskType == 6 || model.TaskType == 7)
            {
                var updateFiles = input.FileList.Where(r => r.Id.HasValue);
                foreach (var item in updateFiles)
                {
                    var updateFileModel = await _meetingFileRepository.GetAsync(item.Id.Value);
                    updateFileModel.FileName = item.Name;
                    updateFileModel.UserId = item.UserId;
                    var fileList = new List<AbpFileListInput>();
                    foreach (var entity in item.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
                    }
                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                    {
                        BusinessId = item.Id.Value.ToString(),
                        BusinessType = (int)AbpFileBusinessType.会议资料,
                        Files = fileList
                    });
                }
            }
            if (model.TaskType == 4 || model.TaskType == 6 || model.TaskType == 7)
            {
                var updateLogs = input.LogList.Where(r => r.Id.HasValue);
                foreach (var item in updateLogs)
                {
                    var updateLogModel = await _meetingLogisticsRelationRepository.GetAsync(item.Id.Value);
                    updateLogModel.Remark = item.Remark;
                    updateLogModel.UserId = item.UserId;
                }
            }
                

        }

        private MeetingUserBeforeTaskLogDto GetChangeModel(MeetingUserBeforeTask model)
        {
            var ret = model.MapTo<MeetingUserBeforeTaskLogDto>();
            return ret;
        }
        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }


        /// <summary>
        /// 会议终止 管理员作废会前任务待办
        /// </summary>
        [RemoteService(IsEnabled = false)]
        public void AutoInvalidBeforeTaskTodos(Guid meetingId)
        {
            var tasks = from a in _workFlowTaskRepository.GetAll()
                        join b in _repository.GetAll() on a.InstanceID equals b.Id.ToString()
                        where b.MeetingId == meetingId && (a.Status == 0 || a.Status == 1)
                        select a;
            foreach (var item in tasks)
            {
                item.Status = 10;
                _workFlowTaskRepository.Update(item);
            }
        }
    }
}