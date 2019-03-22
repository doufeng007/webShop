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
using Abp.WorkFlowDictionary;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Application.Services;
using ZCYX.FRMSCore.Model;
using HR;

namespace Train
{
    public class TrainLeaveAppService : FRMSCoreAppServiceBase, ITrainLeaveAppService
    {
        private readonly IRepository<TrainLeave, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<Train, Guid> _trainRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<Lecturer, Guid> _lecturerRepository;
        private readonly IRepository<AbpDictionary, Guid> _dictionaryRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public TrainLeaveAppService(IRepository<TrainLeave, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, IRepository<Lecturer, Guid> lecturerRepository, IRepository<Train, Guid> trainRepository, IRepository<AbpDictionary, Guid> dictionaryRepository, IRepository<User, long> userRepository, IWorkFlowTaskRepository workFlowTaskRepository)
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _trainRepository = trainRepository;
            _lecturerRepository = lecturerRepository;
            _dictionaryRepository = dictionaryRepository;
            _userRepository = userRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TrainLeaveListOutputDto>> GetList(GetTrainLeaveListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.TrainId==input.TrainId)
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        join c in _dictionaryRepository.GetAll().Where(x => !x.IsDeleted) on a.LevelType equals c.Id                                        
                        select new TrainLeaveListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserName = b.Name,
                            TrainId = a.TrainId,
                            LevelType = a.LevelType,
                            LevelTypeName = c.Title,
                            Reason = a.Reason,
                            CreationTime = a.CreationTime,
                            StartTime = a.StartTime,
                            Status = a.Status,
                            EndTime = a.EndTime,
                            Day = a.Day
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<TrainLeaveListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<TrainLeaveOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            var ret = model.MapTo<TrainLeaveOutputDto>();

            var dModel = _dictionaryRepository.GetAll().FirstOrDefault(x => !x.IsDeleted && x.Id == ret.LevelType);
            ret.LevelTypeName = dModel?.Title;
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == ret.UserId);
            if (user == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            ret.UserName = user.Name;
            var train = await _trainRepository.FirstOrDefaultAsync(x => x.Id == ret.TrainId);
            if (train == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            var traintmp = train.MapTo<TrainOutputDto>();
            dModel = _dictionaryRepository.GetAll().FirstOrDefault(x => !x.IsDeleted && x.Id == train.Type);
            traintmp.TypeName = dModel?.Title;


            user = _userRepository.GetAll().FirstOrDefault(x => x.Id == traintmp.InitiatorId);
            traintmp.InitiatorName = user?.Name;

            var arr = traintmp.LecturerUser.Split(',').ToArray();
            traintmp.LecturerUserName = string.Join<string>(',', _lecturerRepository.GetAll().Where(x => arr.Contains(x.Id.ToString())).Select(x => x.Name));
            traintmp.JoinUserName = _workFlowOrganizationUnitsManager.GetNames(traintmp.JoinUser);
            traintmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = train.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.培训
            });
            ret.Train = traintmp;
            return ret;
        }
        /// <summary>
        /// 添加一个TrainLeave
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateTrainLeaveInput input)
        {
            var newmodel = new TrainLeave()
            {
                UserId = AbpSession.UserId.Value,
                TrainId = input.TrainId,
                LevelType = input.LevelType,
                Reason = input.Reason,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                Day = input.Day
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个TrainLeave
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateTrainLeaveInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
                }
                var logModel = new TrainLeave();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<TrainLeave>();
                }
                dbmodel.LevelType = input.LevelType;
                dbmodel.Reason = input.Reason;
                dbmodel.StartTime = input.StartTime;
                dbmodel.EndTime = input.EndTime;
                dbmodel.Day = input.Day;

                await _repository.UpdateAsync(dbmodel);
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
        }
        private TrainLeaveLogDto GetChangeModel(TrainLeave model)
        {
            var ret = model.MapTo<TrainLeaveLogDto>();
            return ret;
        }


        /// <summary>
        /// 培训，激活子流程事件
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(false)]
        public string TrainLeaveFlowActive(Guid instanceId)
        {
            var subInstaceId = "";
            var train = _trainRepository.GetAll().FirstOrDefault(x=>x.Id==instanceId);
            if (train!=null)
            {
                var model = new TrainLeave();
                model.Id = Guid.NewGuid();
                model.UserId = AbpSession.UserId.Value;
                model.TrainId = instanceId;
                _repository.Insert(model);
                subInstaceId = model.Id.ToString();
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取培训数据异常");
            }
            return subInstaceId;
        }
        public bool TrainLeaveVerification(Guid instanceId)
        {
            var subInstaceId = "";
            var train = _repository.GetAll().FirstOrDefault(x=>x.Id==instanceId);
            if (train!=null)
            {
                if(!train.StartTime.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请假开始时间必填。");
                if(!train.EndTime.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请假结束时间必填。");
                if(string.IsNullOrEmpty( train.Reason))
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请假事由必填。");
                if(!train.LevelType.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请假类型必填。");
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取培训请假数据异常");
            }
            return true;
        }
        public string GetTrainLeaveAuditUser(Guid instanceId)
        {
            var trainLeave = _repository.GetAll().FirstOrDefault(x=>x.Id==instanceId);
            if (trainLeave != null) {
                var train = _trainRepository.GetAll().FirstOrDefault(x => x.Id == trainLeave.TrainId);
                if (train != null)
                    return "u_" + train.InitiatorId;
            }
            return "";
        }
    }
}