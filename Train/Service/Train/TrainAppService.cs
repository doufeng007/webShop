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
using ZCYX.FRMSCore.Model;
using Abp.Configuration;
using Train.Jobs;
using Train.Enum;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.SignalR.Core;
using Abp.RealTime;
using HR;
using Abp.Events.Bus;

namespace Train
{
    public class TrainAppService : FRMSCoreAppServiceBase, ITrainAppService
    {
        private readonly IRepository<TrainUserExperience, Guid> _trainUserExperienceRepository;
        private readonly IRepository<Train, Guid> _repository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<TrainLeave, Guid> _trainLeaveRepository;
        private readonly IRepository<Lecturer, Guid> _lecturerRepository;
        private readonly IRepository<AbpDictionary, Guid> _dictionaryRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectNoticeManager _noticeManager;
        private readonly ITrainHangFire _trainHangFire;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IUserTrainScoreRecordAppService _trainScoreRecordAppService;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IOnlineClientManager _onlineClientManager;
        public IEventBus _eventBus { get; set; }
        public TrainAppService(IRepository<Train, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<AbpDictionary, Guid> dictionaryRepository, IRepository<Lecturer, Guid> lecturerRepository, ProjectNoticeManager noticeManager, IRepository<TrainLeave, Guid> trainLeaveRepository, ITrainHangFire trainHangFire, IRepository<TrainUserExperience, Guid> trainUserExperienceRepository, IUserTrainScoreRecordAppService trainScoreRecordAppService, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, IRepository<User, long> userRepository, IWorkFlowTaskRepository workFlowTaskRepository, IOnlineClientManager onlineClientManager)
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _dictionaryRepository = dictionaryRepository;
            _lecturerRepository = lecturerRepository;
            _trainLeaveRepository = trainLeaveRepository;
            _noticeManager = noticeManager;
            _trainHangFire = trainHangFire;
            _onlineClientManager = onlineClientManager;
            _trainUserExperienceRepository = trainUserExperienceRepository;
            _trainScoreRecordAppService = trainScoreRecordAppService;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _userRepository = userRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _eventBus = NullEventBus.Instance;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TrainListOutputDto>> GetList(GetTrainListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _dictionaryRepository.GetAll().Where(x => !x.IsDeleted) on a.Type equals b.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new TrainListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Type= a.Type,
                            TypeName = b.Title,
                            LecturerUser = a.LecturerUser,
                            Address = a.Address,
                            StartTime = a.StartTime,
                            Status = a.Status,
                            EndTime = a.EndTime,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.Title.Contains(input.SearchKey) || x.TypeName.Contains(input.SearchKey));
            if (input.Type.HasValue)
                query = query.Where(x => x.Type==input.Type.Value);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.StartTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                var arr = item.LecturerUser.Split(',').ToArray();
                item.LecturerUser = string.Join<string>(',', _lecturerRepository.GetAll().Where(x => arr.Contains(x.Id.ToString())).Select(x => x.Name));
                item.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.培训 });

            }
            return new PagedResultDto<TrainListOutputDto>(toalCount, ret);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TrainExperienceListOutputDto>> GetExperienceList(GetTrainExperienceListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where _trainUserExperienceRepository.GetAll().Count(x => x.TrainId == a.Id && x.Type == TrainExperienceType.Train)>0
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new TrainExperienceListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            JoinUser = a.JoinUser,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            Status = a.Status,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.StartTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                var arr = item.JoinUser.Split(',').ToArray();
                var leaveCount = _trainLeaveRepository.GetAll().Count(x => x.EndTime == item.EndTime && x.Status == -1 && x.StartTime == item.StartTime && x.TrainId == item.Id);
                var eCount = _trainUserExperienceRepository.GetAll().Count(x => x.TrainId == item.Id && x.Type == TrainExperienceType.Train);
                item.SendCount = eCount;
                item.NoSendCount = arr.Count() - leaveCount - eCount;
            }
            return new PagedResultDto<TrainExperienceListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TrainListOutputDto>> GetMyList(GetTrainMyListInput input)
        {
            var u = "u_" + AbpSession.UserId.Value;
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && (x.JoinUser.GetStrContainsArray(u)))
                        join b in _dictionaryRepository.GetAll().Where(x => !x.IsDeleted) on a.Type equals b.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new TrainListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Type = a.Type,
                            TypeName = b.Title,
                            LecturerUser = a.LecturerUser,
                            Address = a.Address,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            IsOver = a.Status < 0,
            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };

            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.Title.Contains(input.SearchKey) || x.TypeName.Contains(input.SearchKey));
            if (input.Type.HasValue)
                query = query.Where(x => x.Type == input.Type.Value);
            if (input.State.HasValue) { 
                if(input.State==0)
                    query = query.Where(x => !x.IsOver);
                if(input.State==1)
                    query = query.Where(x => x.IsOver);
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.StartTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);        
                var arr = item.LecturerUser.Split(',').ToArray();
                item.LecturerUser = string.Join<string>(',', _lecturerRepository.GetAll().Where(x => arr.Contains(x.Id.ToString())).Select(x => x.Name));
                item.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.培训 });

            }
            return new PagedResultDto<TrainListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<TrainOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
            var tmp = model.MapTo<TrainOutputDto>();
            var dModel = _dictionaryRepository.GetAll().FirstOrDefault(x => !x.IsDeleted && x.Id == model.Type);
            if (dModel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
            tmp.TypeName = dModel.Title;
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == model.InitiatorId);
            if (user == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            tmp.InitiatorName = user.Name;
            var arr = tmp.LecturerUser.Split(',').ToArray();
            tmp.LecturerUserName = string.Join<string>(',', _lecturerRepository.GetAll().Where(x => arr.Contains(x.Id.ToString())).Select(x => x.Name));

            tmp.JoinUserName = _workFlowOrganizationUnitsManager.GetNames(tmp.JoinUser);
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.培训
            });
            return tmp;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<List<CommentListOutput>> GetComment(GetWorkFlowTaskCommentUserInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var list = new List<CommentListOutput>();
            var commentList = from a in _workFlowTaskRepository.GetAll()
                              where a.Status == 2 && !string.IsNullOrEmpty(a.Comment) && a.InstanceID == id.ToString() &&
                              a.ReceiveID == input.UserId && a.FlowID == input.FlowId
                              select new CommentListOutput()
                              {
                                  UserId = a.ReceiveID,
                                  UserName = a.ReceiveName,
                                  Comment = a.Comment,
                                  CommentDate = a.CompletedTime1.Value
                              };
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            foreach (var item in commentList)
            {
                var organ = organizeManager.GetDeptByUserID(item.UserId);
                if (organ != null)
                {
                    item.DepartmentId = organ.Id;
                    item.DepartmentName = organ.DisplayName;
                }
            }
            return commentList.ToList();
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<List<CommentOutput>> GetCommentList(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var list = new List<CommentListOutput>();
            var commentList = from a in _workFlowTaskRepository.GetAll()
                              where a.Status == 2 && !string.IsNullOrEmpty(a.Comment) && a.InstanceID == id.ToString() && a.FlowID == input.FlowId
                              orderby a.CompletedTime1 descending
                              select new CommentOutput()
                              {
                                  UserId = a.ReceiveID,
                                  UserName = a.ReceiveName,
                                  Comment = a.Comment,
                                  CommentDate = a.CompletedTime1.Value
                              };

            return commentList.ToList();
        }
        /// <summary>
        /// 添加一个Train
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateTrainInput input)
        {
            if (input.StartTime < DateTime.Now)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不得小于当前时间。");
            if (input.StartTime >= input.EndTime)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不得大于等于结束时间。");
            if(input.IsTips && input.TipsTime<=0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提醒时间必填。");
            var id = Guid.NewGuid();
            var newmodel = new Train()
            {
                Id = id,
                Title = input.Title,
                Type = input.Type,
                Address = input.Address,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                InitiatorId = input.InitiatorId,
                Introduction = input.Introduction,
                JoinUser = input.JoinUser,
                CommentScore = input.CommentScore,
                ExperienceScore = input.ExperienceScore,
                LecturerUser = input.LecturerUser,
                IsExperience = input.IsExperience,
                JoinScore = input.JoinScore,
                MeetingRoomId = input.MeetingRoomId,
                Traffic = input.Traffic,
                Eat = input.Eat,
                Accommodation = input.Accommodation,
                ProjectionSystem = input.ProjectionSystem,
                Whiteboard = input.Whiteboard,
                SoundSystem = input.SoundSystem,
                MeetingRoom = input.MeetingRoom,
                IsTips = input.IsTips,
                TipsTime = input.TipsTime,
                TipsUnit = input.TipsUnit
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            if (newmodel.MeetingRoomId.HasValue) { 
            _eventBus.Trigger(new MeetingRoomUseInfoByEvent() {
                BusinessId = newmodel.Id,
                BusinessType = 1,
                StartTime = newmodel.StartTime,
                EndTime = newmodel.EndTime,
                BusinessName = newmodel.Title,
                MeetingRoomId = newmodel.MeetingRoomId.Value,
                IsDelete = false
            });
            }
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.培训,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }
        /// <summary>
        /// 修改抄送人事
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task UpdatePersonnel(UpdateTrainCopyForInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
                }
                dbmodel.Personnel = "u_" + AbpSession.UserId.Value;
                await _repository.UpdateAsync(dbmodel);
            }
        }
        /// <summary>
        /// 公文编号
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task UpdateDocumentId(UpdateTrainDocumentInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
                }
                dbmodel.DocumentId = input.DocumentId;
                await _repository.UpdateAsync(dbmodel);
            }
        }

        /// <summary>
        /// 修改一个Train
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateTrainInput input)
        {
            if (input.Id != Guid.Empty)
            {
                if (input.StartTime < DateTime.Now)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不得小于当前时间。");
                if (input.StartTime >= input.EndTime)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不得大于等于结束时间。");
                if (input.IsTips && input.TipsTime <= 0)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提醒时间必填。");
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
                }
                var logModel = new Train();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<Train>();
                }
                if (input.MeetingRoomId.HasValue)
                {
                    _eventBus.Trigger(new MeetingRoomUseInfoByEvent()
                    {
                        BusinessId = dbmodel.Id,
                        BusinessType = 1,
                        StartTime = input.StartTime,
                        EndTime = input.EndTime,
                        Status=0,
                        BusinessName=input.Title,
                        MeetingRoomId = input.MeetingRoomId.Value,
                        IsDelete = false
                    });
                }
                if (dbmodel.MeetingRoomId.HasValue &&  dbmodel.MeetingRoomId != input.MeetingRoomId)
                {
                    _eventBus.Trigger(new MeetingRoomUseInfoByEvent()
                    {
                        BusinessId = dbmodel.Id,
                        BusinessType = 1,
                        StartTime = dbmodel.StartTime,
                        EndTime = dbmodel.EndTime,
                        Status = 0,
                        BusinessName = dbmodel.Title,
                        MeetingRoomId = dbmodel.MeetingRoomId.Value,
                        IsDelete = true
                    });
                }
                dbmodel.Title = input.Title;
                dbmodel.Type = input.Type;
                dbmodel.Address = input.Address;
                dbmodel.StartTime = input.StartTime;
                dbmodel.EndTime = input.EndTime;
                dbmodel.InitiatorId = input.InitiatorId;
                dbmodel.Introduction = input.Introduction;
                dbmodel.JoinUser = input.JoinUser;
                dbmodel.CommentScore = input.CommentScore;
                dbmodel.ExperienceScore = input.ExperienceScore;
                dbmodel.LecturerUser = input.LecturerUser;
                dbmodel.IsExperience = input.IsExperience;
                dbmodel.JoinScore = input.JoinScore;
                dbmodel.MeetingRoomId = input.MeetingRoomId;
                dbmodel.Traffic = input.Traffic;
                dbmodel.Eat = input.Eat;
                dbmodel.Accommodation = input.Accommodation;
                dbmodel.ProjectionSystem = input.ProjectionSystem;
                dbmodel.Whiteboard = input.Whiteboard;
                dbmodel.SoundSystem = input.SoundSystem;
                dbmodel.IsTips = input.IsTips;
                dbmodel.TipsTime = input.TipsTime;
                dbmodel.TipsUnit = input.TipsUnit;
                await _repository.UpdateAsync(dbmodel);

    
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
                    BusinessType = (int)AbpFileBusinessType.培训,
                    Files = fileList
                });
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
        private TrainLogDto GetChangeModel(Train model)
        {
            var ret = model.MapTo<TrainLogDto>();
            ret.TipsUnit = model.TipsUnit.ToString();
            var dModel = _dictionaryRepository.GetAll().FirstOrDefault(x => !x.IsDeleted && x.Id == model.Type);
            ret.Type = dModel?.Title;
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == model.InitiatorId);
            ret.InitiatorName = user?.Name;
            var arr = model.LecturerUser.Split(',').ToArray();
            ret.LecturerUser = string.Join<string>(',', _lecturerRepository.GetAll().Where(x => arr.Contains(x.Id.ToString())).Select(x => x.Name));
            ret.JoinUser = _workFlowOrganizationUnitsManager.GetNames(model.JoinUser);
            return ret;
        }

        /// <summary>
        /// 培训前通知参训人员
        /// </summary>
        /// <param name="eventParams"></param>
        public void SendMessageForJoinUser(Guid guid)
        {
            var train = _repository.Get(guid);
            if (train != null && (train.CreatorUserId ?? 0) != 0)
            {
                var signalrNoticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISignalrNoticeAppService>();
                var users = train.JoinUser.Split(',');
                var longUsers = new List<long?>();
                foreach (var item in users)
                {
                    var uid = Convert.ToInt64(item.Replace("u_", ""));
                    if (_trainLeaveRepository.GetAll().Any(x => x.TrainId == train.Id && x.UserId == uid && x.Status == -1 && x.StartTime == train.StartTime && x.EndTime == train.EndTime))
                        continue;
                    longUsers.Add(uid);
                }
                var link = "/pxgl/pxxq?id="+train.Id;
                var onlineclients =
    _onlineClientManager.GetAllClients().Where(r => longUsers.Contains(r.UserId)).ToList();
                signalrNoticeService.SendNoticeToClient(onlineclients, train.Id.ToString(), train.Title + "将在：" + train.StartTime.ToString("yyyy/MM/dd HH:mm") + "开始，请准时参加。", link);
            }
        }

        /// <summary>
        /// 培训前通知参训人员
        /// </summary>
        /// <param name="eventParams"></param>
        public void SendMessageByUser(Guid guid)
        {
            var train = _repository.Get(guid);
            if (train != null)
            {
                var dt = DateTime.Now.AddSeconds(10);
                if (train.IsTips)
                {
                    var startTime = train.StartTime;
                    switch (train.TipsUnit.Value)
                    {
                        case TrainTipsType.Day:
                            startTime = startTime.AddDays(-train.TipsTime.Value);
                            break;
                        case TrainTipsType.Minute:
                            startTime = startTime.AddMinutes(-train.TipsTime.Value);
                            break;
                        case TrainTipsType.Hour:
                            startTime = startTime.AddHours(-train.TipsTime.Value);
                            break;
                    }
                    if (startTime > dt)
                        dt = startTime;
                    _trainHangFire.CreateJob(train.Id, dt);
                }
            }
        }
        public string GetNeedExperienceUsers(string guid)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(guid, out fileId))
                return "";
            var train = _repository.GetAll().FirstOrDefault(x => x.Id == fileId);
            if (train == null)
                return "";
            var arr = train.JoinUser.Split(',');
            var users = "";
            foreach (var item in arr)
            {
                var userId = Convert.ToInt32(item.Replace("u_", ""));
                if (!_trainLeaveRepository.GetAll().Any(x => x.UserId == userId && x.StartTime == train.StartTime && x.EndTime == train.EndTime && x.Status == -1 && x.TrainId == train.Id))
                    users += item + ",";
            }
            return users.TrimEnd(',');
        }
        /// <summary>
        /// 获取带有人事的抄送人员
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string GetTrainNeedCopyForUsers(string guid)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(guid, out fileId))
                return "";
            var train = _repository.GetAll().FirstOrDefault(x => x.Id == fileId);
            if (train == null)
                return "";
            var arr = train.JoinUser.Split(',');
            var users = "";
            foreach (var item in arr)
            {
                var userId = Convert.ToInt32(item.Replace("u_", ""));
                if (!_trainLeaveRepository.GetAll().Any(x => x.UserId == userId && x.StartTime == train.StartTime && x.EndTime == train.EndTime && x.Status == -1 && x.TrainId == train.Id))
                    users += item + ",";
            }

            users += train.Personnel;
            return users.TrimEnd(',');
        }
        public string GetExperienceLeader(string guid)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(guid, out fileId))
                return "";
            var train = _repository.GetAll().FirstOrDefault(x => x.Id == fileId);
            if (train == null)
                return "";
            var arr = train.JoinUser.Split(',');
            var users = "";
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            foreach (var item in arr)
            {
                var userId = Convert.ToInt32(item.Replace("u_", ""));
                var leader = organizeManager.GetChargeLeader(userId);
                if (!string.IsNullOrEmpty(leader))
                    users += leader + ",";
            }
            return users.TrimEnd(',');
        }
        public async void AddJoinScore(string guid)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(guid, out fileId))
                return;
            var train = _repository.Get(fileId);
            if (train != null && train.JoinScore > 0)
            {
                var arr = train.JoinUser.Split(',');
                foreach (var item in arr)
                {
                    var id = Convert.ToInt64(item.Replace("u_", ""));
                    if (_trainLeaveRepository.GetAll().Any(x => x.TrainId == train.Id && x.StartTime == train.StartTime && x.EndTime == train.EndTime && x.Status != -1 && x.UserId == id))
                        continue;
                    await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                    {
                        FromId = train.Id,
                        FromType = TrainScoreFromType.TrainLearn,
                        Score = train.JoinScore.Value,
                        UserId = id
                    });
                }
            }
        }

    }
}