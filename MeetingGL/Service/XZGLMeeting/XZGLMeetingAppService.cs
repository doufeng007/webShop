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
using System.Dynamic;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Domain.Uow;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Users.Dto;
using ZCYX.FRMSCore.Model;
using Abp.Configuration;
using Abp;
using Abp.Application.Services;
using Abp.Authorization;
using Hangfire;
using ZCYX.FRMSCore.Users;
using Cronos;
using ZCYX.FRMSCore.Authorization.Users;

namespace MeetingGL
{
    public class XZGLMeetingAppService : FRMSCoreAppServiceBase, IXZGLMeetingAppService
    {
        private readonly IRepository<XZGLMeeting, Guid> _repository;
        private readonly IRepository<MeetingTypeBase, Guid> _meetingTypeBaseRepository;
        private readonly IRepository<MeetingIssue, Guid> _meetingIssueRepository;
        private readonly IRepository<MeetingIssueRelation, Guid> _meetingIssueRelationRepository;
        private readonly IRepository<MeetingFile, Guid> _meetingFileRepository;
        private readonly IRepository<MeetingLogisticsRelation, Guid> _meetingLogisticsRelationRepository;
        private readonly IRepository<MeetingUser, Guid> _meetingUserRepository;
        private readonly IRepository<XZGLMeetingRoom, Guid> _xZGLMeetingRoomRepository;
        private readonly IRepository<MeetingLlogistics, Guid> _meetingLlogisticsRepository;
        private readonly IRepository<MeetingPeriodRule, Guid> _meetingPeriodRuleRepository;
        private readonly IRepository<MeetingRoomUseInfo, Guid> _meetingRoomUseInfoRepository;
        private readonly IRepository<MeetingUserBeforeTask, Guid> _meetingUserBeforeTaskRepository;
        private readonly IRepository<User, long> _useRepository;


        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<XZGLMeetingRoom, Guid> _roomRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        private readonly IRepository<Setting, long> _settingRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workflowOrganizationUnitsRepository;
        private readonly ICreatePeriodMeetingWithHangFire _createPeriodMeetingWithHangFire;


        public XZGLMeetingAppService(IRepository<XZGLMeeting, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService,
            IRepository<XZGLMeetingRoom, Guid> roomRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager,
            IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, IUnitOfWorkManager unitOfWorkManager, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager
            , IRepository<WorkFlowTask, Guid> workFlowTaskRepository, IRepository<MeetingTypeBase, Guid> meetingTypeBaseRepository
            , IRepository<MeetingIssueRelation, Guid> meetingIssueRelationRepository, IRepository<MeetingFile, Guid> meetingFileRepository
            , IRepository<MeetingLogisticsRelation, Guid> meetingLogisticsRelationRepository, IRepository<MeetingUser, Guid> meetingUserRepository
            , IRepository<XZGLMeetingRoom, Guid> xZGLMeetingRoomRepository, IRepository<MeetingIssue, Guid> meetingIssueRepository
            , IRepository<MeetingLlogistics, Guid> meetingLlogisticsRepository, IRepository<Setting, long> settingRepository
            , IRepository<MeetingPeriodRule, Guid> meetingPeriodRuleRepository, IRepository<MeetingRoomUseInfo, Guid> meetingRoomUseInfoRepository
            , IRepository<MeetingUserBeforeTask, Guid> meetingUserBeforeTaskRepository, IRepository<WorkFlowOrganizationUnits, long> workflowOrganizationUnitsRepository
            , ICreatePeriodMeetingWithHangFire createPeriodMeetingWithHangFire, IRepository<User, long> useRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _roomRepository = roomRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _meetingTypeBaseRepository = meetingTypeBaseRepository;
            _meetingIssueRelationRepository = meetingIssueRelationRepository;
            _meetingFileRepository = meetingFileRepository;
            _meetingLogisticsRelationRepository = meetingLogisticsRelationRepository;
            _meetingUserRepository = meetingUserRepository;
            _xZGLMeetingRoomRepository = xZGLMeetingRoomRepository;
            _meetingIssueRepository = meetingIssueRepository;
            _meetingLlogisticsRepository = meetingLlogisticsRepository;
            _settingRepository = settingRepository;
            _meetingPeriodRuleRepository = meetingPeriodRuleRepository;
            _meetingRoomUseInfoRepository = meetingRoomUseInfoRepository;
            _meetingUserBeforeTaskRepository = meetingUserBeforeTaskRepository;
            _workflowOrganizationUnitsRepository = workflowOrganizationUnitsRepository;
            _createPeriodMeetingWithHangFire = createPeriodMeetingWithHangFire;
            _useRepository = useRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize("XZGL.Hygl")]
        public async Task<PagedResultDto<XZGLMeetingListOutputDto>> GetList(GetXZGLMeetingListInput input)
        {
            var retData = new List<XZGLMeetingListOutputDto>();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _meetingTypeBaseRepository.GetAll() on a.MeetingTypeId equals b.Id
                        join c in UserManager.Users on a.ModeratorId equals c.Id
                        where !a.IsPeriod
                        let openModel = (from b in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value) select b)
                        select new
                        {
                            a.Id,
                            a.Name,
                            MeetingTypeName = b.Name,
                            a.MeetingRoomName,
                            a.StartTime,
                            a.EndTime,
                            ModeratorName = c.Name,
                            a.Status,
                            a.RoomId,
                            a.MeetingTypeId,
                            a.CreationTime,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                            a.MeetingCreateUser,
                            a.ModeratorId,
                            a.RecorderId,
                            a.JoinPersonnel,
                            a.AttendingLeaders,


                        };

            var userRoles = await UserManager.GetRolesAsync(await UserManager.GetUserByIdAsync(AbpSession.UserId.Value));

            if (input.RoomId.HasValue)
            {
                query = query.Where(x => x.RoomId == input.RoomId);
            }
            if (input.MeetingTypeId.HasValue)
            {
                query = query.Where(x => x.MeetingTypeId == input.MeetingTypeId);
            }
            if (input.StartTime.HasValue)
            {
                query = query.Where(x => x.StartTime.Date >= input.StartTime.Value.Date);
            }
            if (input.EndTime.HasValue)
                query = query.Where(x => x.EndTime.Date <= input.EndTime.Value.Date);
            if (!string.IsNullOrEmpty(input.SearchKey))
            {
                query = query.Where(x => x.Name.Contains(input.SearchKey) || x.MeetingRoomName.Contains(input.SearchKey));
            }

            if (userRoles.Any(r => string.Compare(r, "XZRY", true) == 0))
            {
            }
            else if (userRoles.Any(r => string.Compare(r, "XMFZR", true) == 0 || string.Compare(r, "DLEADER", true) == 0))
            {
                query = query.Where(r => r.MeetingCreateUser == AbpSession.UserId.Value);
            }
            else
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "没有权限获取会议数据");
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.StartTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                var entity = new XZGLMeetingListOutputDto()
                {
                    CreationTime = item.CreationTime,
                    EndTime = item.EndTime,
                    Id = item.Id,
                    InstanceId = item.Id.ToString(),
                    MeetingRoomName = item.MeetingRoomName,
                    MeetingTypeName = item.MeetingTypeName,
                    ModeratorName = item.ModeratorName,
                    Name = item.Name,
                    StartTime = item.StartTime,
                    Status = item.Status,
                };
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, entity);
                retData.Add(entity);
            }
            return new PagedResultDto<XZGLMeetingListOutputDto>(toalCount, retData);
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<XZGLMeetingListOutputDto>> GetMyList(GetXZGLMeetingListInput input)
        {
            var retData = new List<XZGLMeetingListOutputDto>();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _meetingTypeBaseRepository.GetAll() on a.MeetingTypeId equals b.Id
                        join c in UserManager.Users on a.ModeratorId equals c.Id
                        where !a.IsPeriod
                        let openModel = (from b in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value) select b)
                        select new
                        {
                            a.Id,
                            a.Name,
                            MeetingTypeName = b.Name,
                            a.MeetingRoomName,
                            a.StartTime,
                            a.EndTime,
                            ModeratorName = c.Name,
                            a.Status,
                            a.RoomId,
                            a.MeetingTypeId,
                            a.CreationTime,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                            a.MeetingCreateUser,
                            a.ModeratorId,
                            a.RecorderId,
                            a.JoinPersonnel,
                            a.AttendingLeaders,
                        };

            var userRoles = await UserManager.GetRolesAsync(await UserManager.GetUserByIdAsync(AbpSession.UserId.Value));

            if (input.RoomId.HasValue)
            {
                query = query.Where(x => x.RoomId == input.RoomId);
            }
            if (input.MeetingTypeId.HasValue)
            {
                query = query.Where(x => x.MeetingTypeId == input.MeetingTypeId);
            }
            if (input.StartTime.HasValue)
            {
                query = query.Where(x => x.StartTime.Date >= input.StartTime.Value.Date);
            }
            if (input.EndTime.HasValue)
                query = query.Where(x => x.EndTime.Date <= input.EndTime.Value.Date);
            if (!string.IsNullOrEmpty(input.SearchKey))
            {
                query = query.Where(x => x.Name.Contains(input.SearchKey) || x.MeetingRoomName.Contains(input.SearchKey));
            }
            var currentUser = $"u_{AbpSession.UserId.Value}";
            query = query.Where(r => r.MeetingCreateUser == AbpSession.UserId.Value || r.ModeratorId == AbpSession.UserId.Value ||
            r.RecorderId == AbpSession.UserId.Value || r.JoinPersonnel.Contains(currentUser) || r.AttendingLeaders.Contains(currentUser));

            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.StartTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                var entity = new XZGLMeetingListOutputDto()
                {
                    CreationTime = item.CreationTime,
                    EndTime = item.EndTime,
                    Id = item.Id,
                    InstanceId = item.Id.ToString(),
                    MeetingRoomName = item.MeetingRoomName,
                    MeetingTypeName = item.MeetingTypeName,
                    ModeratorName = item.ModeratorName,
                    Name = item.Name,
                    StartTime = item.StartTime,
                    Status = item.Status,
                };
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, entity);
                retData.Add(entity);
            }
            return new PagedResultDto<XZGLMeetingListOutputDto>(toalCount, retData);
        }

        /// <summary>
        /// 周期会议取消
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdatePeriodClear(PeriodClearInput input)
        {
            var model = _meetingPeriodRuleRepository.Get(input.Id);
            var meeting = _repository.Get(model.MeetingId);
            if (model.ActiveEndTime < DateTime.Now)
                model.Status = PeriodRuleStatus.失效;
            if (model.Status == PeriodRuleStatus.失效)
                return;
            else if (model.Status == PeriodRuleStatus.正常)
                model.Status = PeriodRuleStatus.取消;
            else
                model.Status = PeriodRuleStatus.正常;
            await _meetingPeriodRuleRepository.UpdateAsync(model);
        }
        /// <summary>
        /// 周期会议-列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<XZGLMeetingPeriodListOutputDto>> GetPeriodList(GetXZGLMeetingListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join r in _meetingPeriodRuleRepository.GetAll() on a.Id equals r.MeetingId
                        join b in _meetingTypeBaseRepository.GetAll() on a.MeetingTypeId equals b.Id
                        join c in UserManager.Users on a.ModeratorId equals c.Id
                        where a.IsPeriod == true
                        select new XZGLMeetingPeriodListOutputDto
                        {
                            ActiveEndTime = r.ActiveEndTime,
                            ActiveStartTime = r.ActiveStartTime,
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            EndTime = a.EndTime,
                            JoinPersonnel = a.JoinPersonnel,
                            MeetingRoomName = a.MeetingRoomName,
                            Name = a.Name,
                            StartTime = a.StartTime,
                            Status = a.Status,
                            PeriodType = r.PeriodType,
                            PeriodNumber1 = r.PeriodNumber1,
                            PeriodNumber2 = r.PeriodNumber2,
                            PeriodStatus = r.Status,
                            PeriodId = r.Id,
                            PeriodNumber3 = r.PeriodNumber3,
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
            {
                query = query.Where(x => x.Name.Contains(input.SearchKey) || x.MeetingRoomName.Contains(input.SearchKey));
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.JoinPersonnelName = _workFlowOrganizationUnitsManager.GetNames(item.JoinPersonnel);
                item.PeriodStatusTitle = item.PeriodStatus.ToString();
                if (item.PeriodType == PeriodType.按周)
                {
                    switch (item.PeriodNumber1)
                    {
                        case 1:
                            item.PeriodName = "每周 星期一";
                            break;
                        case 2:
                            item.PeriodName = "每周 星期二";
                            break;
                        case 3:
                            item.PeriodName = "每周 星期三";
                            break;
                        case 4:
                            item.PeriodName = "每周 星期四";
                            break;
                        case 5:
                            item.PeriodName = "每周 星期五";
                            break;
                        case 6:
                            item.PeriodName = "每周 星期六";
                            break;
                        case 7:
                            item.PeriodName = "每周 星期天";
                            break;
                        default:
                            item.PeriodName = "每周 星期一";
                            break;
                    }
                }
                else if (item.PeriodType == PeriodType.按天)
                {
                    if (item.PeriodNumber1 == 0)
                        item.PeriodName = "每天";
                    else
                        item.PeriodName = $"每{item.PeriodNumber1}天";
                }
                else if (item.PeriodType == PeriodType.按年)
                {
                    if (item.PeriodNumber1 == 1)
                    {
                        item.PeriodName = $"每年 第{item.PeriodNumber2}个月 第{item.PeriodNumber3}天";
                    }
                    else if (item.PeriodNumber1 == 2)
                    {
                        item.PeriodName = $"每年 第{item.PeriodNumber2}个月 最后第{item.PeriodNumber3}天";
                    }
                    else
                        item.PeriodName = $"每年";
                }
                else if (item.PeriodType == PeriodType.按月)
                {
                    if (item.PeriodNumber1 == 1)
                    {
                        item.PeriodName = $"每月 第{item.PeriodNumber2}天";
                    }
                    else if (item.PeriodNumber1 == 2)
                    {
                        item.PeriodName = $"每月 最后第{item.PeriodNumber2}天";
                    }
                    else
                        item.PeriodName = $"每月";
                }
                else if (item.PeriodType == PeriodType.按年)
                {
                    if (item.PeriodNumber1 == 1)
                    {
                        item.PeriodName = $"每年{item.PeriodNumber2}月 第{item.PeriodNumber3}天";
                    }
                    else if (item.PeriodNumber1 == 2)
                    {
                        item.PeriodName = $"每月{item.PeriodNumber2}月 最后第{item.PeriodNumber3}天";
                    }
                    else
                        item.PeriodName = $"每年";
                }

            }
            return new PagedResultDto<XZGLMeetingPeriodListOutputDto>(toalCount, ret);
        }




        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<XZGLMeetingOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var ret = new XZGLMeetingOutputDto();
            var id = input.InstanceId.ToGuid();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll()
                            join b in _meetingTypeBaseRepository.GetAll() on a.MeetingTypeId equals b.Id
                            join c in UserManager.Users on a.MeetingCreateUser equals c.Id
                            join d in UserManager.Users on a.ModeratorId equals d.Id
                            join e in UserManager.Users on a.RecorderId equals e.Id
                            where a.Id == id
                            select new XZGLMeetingOutputDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                MeetingTypeId = a.MeetingTypeId,
                                MeetingTypeName = b.Name,
                                ReturnReceiptEnable = b.ReturnReceiptEnable,
                                RoomId = a.RoomId,
                                MeetingRoomName = a.MeetingRoomName,
                                StartTime = a.StartTime,
                                EndTime = a.EndTime,
                                OrgId = a.OrgId,
                                OrganizeName = a.OrganizeName,
                                MeetingCreateUser = c.Id,
                                MeetingCreateUser_Name = c.Name,
                                ModeratorId = d.Id,
                                ModeratorId_Name = d.Name,
                                RecorderId = e.Id,
                                RecorderName = e.Name,
                                AttendingLeaders = a.AttendingLeaders,
                                MeetingGuest = a.MeetingGuest,
                                JoinPersonnel = a.JoinPersonnel,
                                MeetingTheme = a.MeetingTheme,
                                MeetingIsssueType = a.MeetingIsssueType,
                                MeetingIssue = a.MeetingIssue,
                                IsNeedFile = a.IsNeedFile,
                                IsNeedLogistics = a.IsNeedLogistics,
                                CopyForUsers = a.CopyForUsers,
                                Record = a.Record,
                                RealAttendeeUsers = a.RealAttendeeUsers,
                                RealMeetingGuest = a.RealMeetingGuest,
                                AbsentUser = a.AbsentUser,
                                HasSubmitRecord = a.HasSubmitRecord,
                            };
                ret = await query.SingleOrDefaultAsync();
                if (ret == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "会议不存在。");
                var period = _meetingPeriodRuleRepository.FirstOrDefault(item => item.MeetingId == ret.Id);
                if (period != null)
                {
                    ret.MeetingPeriodRule = period.MapTo<MeetingPeriodRuleOutputDto>();
                }
                ret.AttendingLeadersName = _workFlowOrganizationUnitsManager.GetNames(ret.AttendingLeaders);
                ret.JoinPersonnelName = _workFlowOrganizationUnitsManager.GetNames(ret.JoinPersonnel);


                if (ret.MeetingIsssueType == MeetingIsssueType.议题)
                {
                    var query_Issue = from a in _meetingIssueRelationRepository.GetAll()
                                      join b in _meetingIssueRepository.GetAll() on a.IssueId equals b.Id
                                      where a.MeetingId == ret.Id
                                      select new MeetingIssueOutputDto()
                                      {
                                          Id = a.Id,
                                          Content = b.Content,
                                          Name = b.Name,
                                          UserId = a.UserIds,
                                          Stauts = b.Status,
                                          IssueId = b.Id,
                                          ResultStatus = a.Status,
                                      };
                    var issueList = await query_Issue.ToListAsync();
                    foreach (var item in issueList)
                    {
                        item.UserName = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                        item.StautsTitle = item.Stauts.ToString();
                        item.HasPass = item.ResultStatus == MeetingIssueResultStatus.HasPass ? true : false;
                        var beforeTasks = _meetingUserBeforeTaskRepository.GetAll().Where(r => r.MeetingId == id && item.UserId.GetStrContainsArray(r.UserId.ToString()));
                        if (beforeTasks.Any(r => r.Status != -1))
                            item.ReadStatus = "未完成";
                        else
                            item.ReadStatus = "已完成";

                    }
                    ret.IssueList = issueList;
                }
                if (ret.IsNeedFile)
                {
                    var query_File = from a in _meetingFileRepository.GetAll()
                                     join b in UserManager.Users on a.UserId equals b.Id
                                     where !a.IsDeleted && a.MeetingId == ret.Id
                                     select new XZGLMeetingFileOutput
                                     {
                                         Id = a.Id,
                                         Name = a.FileName,
                                         UserId = a.UserId,
                                         UserName = b.Name,
                                     };
                    var file_List = await query_File.ToListAsync();
                    foreach (var item in file_List)
                    {
                        item.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.会议资料 });
                        var beforeTasks = _meetingUserBeforeTaskRepository.GetAll().Where(r => r.MeetingId == id && item.UserId == r.UserId);
                        if (beforeTasks.Any(r => r.Status != -1))
                            item.ReadStatus = "未完成";
                        else
                            item.ReadStatus = "已完成";
                    }
                    ret.FileList = file_List;
                }
                if (ret.IsNeedLogistics)
                {
                    var query_Log = from a in _meetingLogisticsRelationRepository.GetAll()
                                    join b in _meetingLlogisticsRepository.GetAll() on a.LogisticsId equals b.Id
                                    join c in UserManager.Users on a.UserId equals c.Id
                                    where !a.IsDeleted && a.MeetingId == ret.Id
                                    select new XZGLMeetingLogisticsROutput
                                    {
                                        Id = a.Id,
                                        LogisticsId = a.LogisticsId,
                                        Remark = a.Remark,
                                        UserId = a.UserId,
                                        LogisticsName = b.Name,
                                        UserName = c.Name
                                    };
                    var logList = await query_Log.ToListAsync();
                    ret.LogisticsList = logList;
                }

                var query_User = from a in _meetingUserRepository.GetAll()
                                 join b in UserManager.Users on a.UserId equals b.Id
                                 where a.MeetingId == ret.Id
                                 select new MeetingUserOutputDtoForView
                                 {
                                     Id = a.Id,
                                     MeetingId = a.MeetingId,
                                     MeetingUserRole = a.MeetingUserRole,
                                     ReturnReceiptStatus = a.ReturnReceiptStatus,
                                     UserId = a.UserId,
                                     UserName = b.Name,
                                     Status = a.Status,
                                     WorkNumber = b.WorkNumber,
                                     ConfirmData = a.ConfirmData,
                                 };
                var userList = await query_User.ToListAsync();
                foreach (var item in userList)
                {
                    if (item.UserId == AbpSession.UserId.Value)
                    {
                        ret.CurrntUserReturnReceiptStatus = item.ReturnReceiptStatus;
                        ret.CurrntUserReturnReceiptStatusName = item.ReturnReceiptStatus.ToString();
                    }
                    item.MeetingUserRoleName = item.MeetingUserRole.ToString();
                    item.ReturnReceiptStatusName = item.ReturnReceiptStatus.ToString();
                    if (item.ReturnReceiptStatus == ReturnReceiptStatus.无回执)
                        item.StatusRemark = "无回执";
                    else if (item.ReturnReceiptStatus == ReturnReceiptStatus.确定参会)
                        item.StatusRemark = item.ConfirmData.ToString();
                    else
                    {
                        item.StatusRemark = ReturnReceiptStatus.申请请假.ToString();
                    }
                }
                ret.MeetingUsers = userList;
                ret.MeetingUserReturnReceipt = userList;

                if (!ret.CopyForUsers.IsNullOrEmpty())
                    ret.CopyForUsersName = _workFlowOrganizationUnitsManager.GetNames(ret.CopyForUsers);

                var meetingUserIdList = new List<string>();
                meetingUserIdList.AddRange(ret.JoinPersonnel.Split(",").ToList());
                meetingUserIdList.Add("u_" + ret.ModeratorId);
                meetingUserIdList.Add("u_" + ret.RecorderId);
                meetingUserIdList.Distinct();
                ret.MeetingUserShouldCount = meetingUserIdList.Count();
                ret.MeetingUserAbsentCount = 0;
                if (ret.ReturnReceiptEnable)
                    ret.MeetingUserAbsentCount = ret.MeetingUserReturnReceipt.Count(r => r.ReturnReceiptStatus == ReturnReceiptStatus.申请请假 && r.Status == -1);
            }
            return ret;
        }


        [AbpAuthorize]
        public async Task<XZGLMeetingOutputDto> GetForViewAsync(EntityDto<Guid> input)
        {
            var ret = new XZGLMeetingOutputDto();
            var id = input.Id;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll()
                            join b in _meetingTypeBaseRepository.GetAll() on a.MeetingTypeId equals b.Id
                            join c in UserManager.Users on a.MeetingCreateUser equals c.Id
                            join d in UserManager.Users on a.ModeratorId equals d.Id
                            join e in UserManager.Users on a.RecorderId equals e.Id
                            where a.Id == id
                            select new XZGLMeetingOutputDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                MeetingTypeId = a.MeetingTypeId,
                                MeetingTypeName = b.Name,
                                RoomId = a.RoomId,
                                MeetingRoomName = a.MeetingRoomName,
                                StartTime = a.StartTime,
                                EndTime = a.EndTime,
                                OrgId = a.OrgId,
                                OrganizeName = a.OrganizeName,
                                MeetingCreateUser = c.Id,
                                MeetingCreateUser_Name = c.Name,
                                ModeratorId = d.Id,
                                ModeratorId_Name = d.Name,
                                RecorderId = e.Id,
                                RecorderName = e.Name,
                                AttendingLeaders = a.AttendingLeaders,
                                MeetingGuest = a.MeetingGuest,
                                JoinPersonnel = a.JoinPersonnel,
                                MeetingTheme = a.MeetingTheme,
                                MeetingIsssueType = a.MeetingIsssueType,
                                MeetingIssue = a.MeetingIssue,
                                IsNeedFile = a.IsNeedFile,
                                IsNeedLogistics = a.IsNeedLogistics,
                                CopyForUsers = a.CopyForUsers,
                                Record = a.Record,
                                RealAttendeeUsers = a.RealAttendeeUsers,
                                RealMeetingGuest = a.RealMeetingGuest,
                                AbsentUser = a.AbsentUser,
                                HasSubmitRecord = a.HasSubmitRecord,

                            };
                ret = await query.SingleOrDefaultAsync();
                ret.AttendingLeadersName = _workFlowOrganizationUnitsManager.GetNames(ret.AttendingLeaders);
                ret.JoinPersonnelName = _workFlowOrganizationUnitsManager.GetNames(ret.JoinPersonnel);

                if (ret.MeetingIsssueType == MeetingIsssueType.议题)
                {
                    var query_Issue = from a in _meetingIssueRelationRepository.GetAll()
                                      join b in _meetingIssueRepository.GetAll() on a.IssueId equals b.Id
                                      where a.MeetingId == ret.Id
                                      select new MeetingIssueOutputDto()
                                      {
                                          Id = a.Id,
                                          Content = b.Content,
                                          Name = b.Name,
                                          UserId = a.UserIds,
                                          Stauts = b.Status,
                                          IssueId = b.Id,
                                          ResultStatus = a.Status,

                                      };
                    var issueList = await query_Issue.ToListAsync();
                    foreach (var item in issueList)
                    {
                        item.UserName = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                        item.StautsTitle = item.Stauts.ToString();
                        item.HasPass = item.ResultStatus == MeetingIssueResultStatus.HasPass ? true : false;
                        var beforeTasks = _meetingUserBeforeTaskRepository.GetAll().Where(r => r.MeetingId == id && item.UserId.GetStrContainsArray(r.UserId.ToString()));
                        if (beforeTasks.Any(r => r.Status != -1))
                            item.ReadStatus = "未完成";
                        else
                            item.ReadStatus = "已完成";
                    }
                    ret.IssueList = issueList;

                }
                if (ret.IsNeedFile)
                {
                    var query_File = from a in _meetingFileRepository.GetAll()
                                     join b in UserManager.Users on a.UserId equals b.Id
                                     where !a.IsDeleted && a.MeetingId == ret.Id
                                     select new XZGLMeetingFileOutput
                                     {
                                         Id = a.Id,
                                         Name = a.FileName,
                                         UserId = a.UserId,
                                         UserName = b.Name,
                                     };
                    var file_List = await query_File.ToListAsync();
                    foreach (var item in file_List)
                    {
                        item.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.会议资料 });
                        var beforeTasks = _meetingUserBeforeTaskRepository.GetAll().Where(r => r.MeetingId == id && item.UserId == r.UserId);
                        if (beforeTasks.Any(r => r.Status != -1))
                            item.ReadStatus = "未完成";
                        else
                            item.ReadStatus = "已完成";
                    }
                    ret.FileList = file_List;
                }
                if (ret.IsNeedLogistics)
                {
                    var query_Log = from a in _meetingLogisticsRelationRepository.GetAll()
                                    join b in _meetingLlogisticsRepository.GetAll() on a.LogisticsId equals b.Id
                                    join c in UserManager.Users on a.UserId equals c.Id
                                    where !a.IsDeleted && a.MeetingId == ret.Id
                                    select new XZGLMeetingLogisticsROutput
                                    {
                                        Id = a.Id,
                                        LogisticsId = a.LogisticsId,
                                        Remark = a.Remark,
                                        UserId = a.UserId,
                                        LogisticsName = b.Name,
                                        UserName = c.Name
                                    };
                    var logList = await query_Log.ToListAsync();
                    ret.LogisticsList = logList;
                }

                var query_User = from a in _meetingUserRepository.GetAll()
                                 join b in UserManager.Users on a.UserId equals b.Id
                                 where a.MeetingId == ret.Id
                                 select new MeetingUserOutputDtoForView
                                 {
                                     Id = a.Id,
                                     MeetingId = a.MeetingId,
                                     MeetingUserRole = a.MeetingUserRole,
                                     ReturnReceiptStatus = a.ReturnReceiptStatus,
                                     UserId = a.UserId,
                                     UserName = b.Name,
                                     Status = a.Status,
                                     WorkNumber = b.WorkNumber,
                                     ConfirmData = a.ConfirmData,
                                 };
                var userList = await query_User.ToListAsync();
                foreach (var item in userList)
                {
                    if (item.UserId == AbpSession.UserId.Value)
                    {
                        ret.CurrntUserReturnReceiptStatus = item.ReturnReceiptStatus;
                        ret.CurrntUserReturnReceiptStatusName = item.ReturnReceiptStatus.ToString();
                    }
                    item.MeetingUserRoleName = item.MeetingUserRole.ToString();
                    item.ReturnReceiptStatusName = item.ReturnReceiptStatus.ToString();
                    if (item.ReturnReceiptStatus == ReturnReceiptStatus.无回执)
                        item.StatusRemark = "无回执";
                    else if (item.ReturnReceiptStatus == ReturnReceiptStatus.确定参会)
                        item.StatusRemark = item.ConfirmData.ToString();
                    else
                    {
                        //if (item.Status == -1)
                        //    item.StatusRemark = "已请假";
                        //else if (item.Status == -2)
                        //    item.StatusRemark = "请假被驳回";
                        //else
                        //    item.StatusRemark = "请假中";
                        item.StatusRemark = ReturnReceiptStatus.申请请假.ToString();
                    }
                }
                ret.MeetingUsers = userList;
                ret.MeetingUserReturnReceipt = userList;


                if (!ret.CopyForUsers.IsNullOrEmpty())
                    ret.CopyForUsersName = _workFlowOrganizationUnitsManager.GetNames(ret.CopyForUsers);
            }
            return ret;
        }

        [AbpAuthorize]
        public XZGLMeetingOutputDto GetForView(EntityDto<Guid> input)
        {
            var ret = new XZGLMeetingOutputDto();
            var id = input.Id;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll()
                            join b in _meetingTypeBaseRepository.GetAll() on a.MeetingTypeId equals b.Id
                            join c in UserManager.Users on a.MeetingCreateUser equals c.Id
                            join d in UserManager.Users on a.ModeratorId equals d.Id
                            join e in UserManager.Users on a.RecorderId equals e.Id
                            where a.Id == id
                            select new XZGLMeetingOutputDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                MeetingTypeId = a.MeetingTypeId,
                                MeetingTypeName = b.Name,
                                RoomId = a.RoomId,
                                MeetingRoomName = a.MeetingRoomName,
                                StartTime = a.StartTime,
                                EndTime = a.EndTime,
                                OrgId = a.OrgId,
                                OrganizeName = a.OrganizeName,
                                MeetingCreateUser = c.Id,
                                MeetingCreateUser_Name = c.Name,
                                ModeratorId = d.Id,
                                ModeratorId_Name = d.Name,
                                RecorderId = e.Id,
                                RecorderName = e.Name,
                                AttendingLeaders = a.AttendingLeaders,
                                MeetingGuest = a.MeetingGuest,
                                JoinPersonnel = a.JoinPersonnel,
                                MeetingTheme = a.MeetingTheme,
                                MeetingIsssueType = a.MeetingIsssueType,
                                MeetingIssue = a.MeetingIssue,
                                IsNeedFile = a.IsNeedFile,
                                IsNeedLogistics = a.IsNeedLogistics,
                                CopyForUsers = a.CopyForUsers,
                                Record = a.Record,
                                RealAttendeeUsers = a.RealAttendeeUsers,
                                RealMeetingGuest = a.RealMeetingGuest,
                                AbsentUser = a.AbsentUser,
                                HasSubmitRecord = a.HasSubmitRecord,

                            };
                ret = query.SingleOrDefault();
                ret.AttendingLeadersName = _workFlowOrganizationUnitsManager.GetNames(ret.AttendingLeaders);
                ret.JoinPersonnelName = _workFlowOrganizationUnitsManager.GetNames(ret.JoinPersonnel);

                if (ret.MeetingIsssueType == MeetingIsssueType.议题)
                {
                    var query_Issue = from a in _meetingIssueRelationRepository.GetAll()
                                      join b in _meetingIssueRepository.GetAll() on a.IssueId equals b.Id
                                      where a.MeetingId == ret.Id
                                      select new MeetingIssueOutputDto()
                                      {
                                          Id = a.Id,
                                          Content = b.Content,
                                          Name = b.Name,
                                          UserId = a.UserIds,
                                          Stauts = b.Status,
                                          IssueId = b.Id,
                                          ResultStatus = a.Status,
                                      };
                    var issueList = query_Issue.ToList();
                    foreach (var item in issueList)
                    {
                        item.UserName = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                        item.StautsTitle = item.Stauts.ToString();
                        item.HasPass = item.ResultStatus == MeetingIssueResultStatus.HasPass ? true : false;
                        var beforeTasks = _meetingUserBeforeTaskRepository.GetAll().Where(r => r.MeetingId == id && item.UserId.GetStrContainsArray(r.UserId.ToString()));
                        if (beforeTasks.Any(r => r.Status != -1))
                            item.ReadStatus = "未完成";
                        else
                            item.ReadStatus = "已完成";
                    }
                    ret.IssueList = issueList;
                }
                if (ret.IsNeedFile)
                {
                    var query_File = from a in _meetingFileRepository.GetAll()
                                     join b in UserManager.Users on a.UserId equals b.Id
                                     where !a.IsDeleted && a.MeetingId == ret.Id
                                     select new XZGLMeetingFileOutput
                                     {
                                         Id = a.Id,
                                         Name = a.FileName,
                                         UserId = a.UserId,
                                         UserName = b.Name,
                                     };
                    var file_List = query_File.ToList();
                    foreach (var item in file_List)
                    {
                        item.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.会议资料 });
                        var beforeTasks = _meetingUserBeforeTaskRepository.GetAll().Where(r => r.MeetingId == id && item.UserId == r.UserId);
                        if (beforeTasks.Any(r => r.Status != -1))
                            item.ReadStatus = "未完成";
                        else
                            item.ReadStatus = "已完成";
                    }
                    ret.FileList = file_List;
                }
                if (ret.IsNeedLogistics)
                {
                    var query_Log = from a in _meetingLogisticsRelationRepository.GetAll()
                                    join b in _meetingLlogisticsRepository.GetAll() on a.LogisticsId equals b.Id
                                    join c in UserManager.Users on a.UserId equals c.Id
                                    where !a.IsDeleted && a.MeetingId == ret.Id
                                    select new XZGLMeetingLogisticsROutput
                                    {
                                        Id = a.Id,
                                        LogisticsId = a.LogisticsId,
                                        Remark = a.Remark,
                                        UserId = a.UserId,
                                        LogisticsName = b.Name,
                                        UserName = c.Name
                                    };
                    var logList = query_Log.ToList();
                    ret.LogisticsList = logList;
                }


                var query_User = from a in _meetingUserRepository.GetAll()
                                 join b in UserManager.Users on a.UserId equals b.Id
                                 where a.MeetingId == ret.Id
                                 select new MeetingUserOutputDtoForView
                                 {
                                     Id = a.Id,
                                     MeetingId = a.MeetingId,
                                     MeetingUserRole = a.MeetingUserRole,
                                     ReturnReceiptStatus = a.ReturnReceiptStatus,
                                     UserId = a.UserId,
                                     UserName = b.Name,
                                     Status = a.Status,
                                     WorkNumber = b.WorkNumber,
                                     ConfirmData = a.ConfirmData,
                                 };
                var userList = query_User.ToList();
                foreach (var item in userList)
                {
                    if (item.UserId == AbpSession.UserId.Value)
                    {
                        ret.CurrntUserReturnReceiptStatus = item.ReturnReceiptStatus;
                        ret.CurrntUserReturnReceiptStatusName = item.ReturnReceiptStatus.ToString();
                    }
                    item.MeetingUserRoleName = item.MeetingUserRole.ToString();
                    item.ReturnReceiptStatusName = item.ReturnReceiptStatus.ToString();
                    if (item.ReturnReceiptStatus == ReturnReceiptStatus.无回执)
                        item.StatusRemark = "无回执";
                    else if (item.ReturnReceiptStatus == ReturnReceiptStatus.确定参会)
                        item.StatusRemark = item.ConfirmData.ToString();
                    else
                        item.StatusRemark = ReturnReceiptStatus.申请请假.ToString();
                }
                ret.MeetingUsers = userList;
                ret.MeetingUserReturnReceipt = userList;


                if (!ret.CopyForUsers.IsNullOrEmpty())
                    ret.CopyForUsersName = _workFlowOrganizationUnitsManager.GetNames(ret.CopyForUsers);
            }
            return ret;
        }

        /// <summary>
        /// 添加一个XZGLMeeting
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateXZGLMeetingInput input)
        {
            if (input.RoomId.HasValue)
                if (!_roomRepository.GetAll().Any(x => x.Id == input.RoomId))
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该会议室不存在。");
            if (input.EndTime <= input.StartTime)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "会议结束时间不能小于等于会议开始时间。");
            }
            if (input.ModeratorId == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请选择/录入一个主持人。");
            if (input.RecorderId == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请选择/录入一个记录人。");
            var newmodel = new XZGLMeeting()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                MeetingTypeId = input.MeetingTypeId,
                RoomId = input.RoomId,
                MeetingRoomName = input.MeetingRoomName,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                OrgId = input.OrgId,
                OrganizeName = input.OrganizeName,
                MeetingCreateUser = input.MeetingCreateUser,
                ModeratorId = input.ModeratorId,
                RecorderId = input.RecorderId,
                AttendingLeaders = input.AttendingLeaders,
                MeetingGuest = input.MeetingGuest,
                JoinPersonnel = input.JoinPersonnel,
                MeetingTheme = input.MeetingTheme,
                MeetingIsssueType = input.MeetingIsssueType,
                MeetingIssue = input.MeetingIssue,
                IsNeedFile = input.IsNeedFile,
                IsNeedLogistics = input.IsNeedLogistics,
            };
            newmodel.Status = 0;
            newmodel.IsPeriod = false;
            await _repository.InsertAsync(newmodel);
            if (input.MeetingIsssueType == MeetingIsssueType.议题)
            {
                foreach (var item in input.Issues)
                {
                    if (!MemberPerfix.IsUserListString(item.UserIds))
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "议题的汇报人添加异常。");
                    var entity = new MeetingIssueRelation()
                    {
                        Id = Guid.NewGuid(),
                        MeetingId = newmodel.Id,
                        IssueId = item.IssueId,
                        UserIds = item.UserIds,
                    };
                    await _meetingIssueRelationRepository.InsertAsync(entity);
                }
            }
            if (input.IsNeedFile)
                foreach (var item in input.FileList)
                {
                    var fileModel = new MeetingFile()
                    {
                        Id = Guid.NewGuid(),
                        FileName = item.Name,
                        UserId = item.UserId,
                        MeetingId = newmodel.Id
                    };
                    await _meetingFileRepository.InsertAsync(fileModel);

                    var fileList = new List<AbpFileListInput>();
                    foreach (var entity in item.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
                    }
                    await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                    {
                        BusinessId = fileModel.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.会议资料,
                        Files = fileList
                    });
                }
            if (input.IsNeedLogistics)
            {
                foreach (var item in input.LogisticsList)
                {
                    var entity = new MeetingLogisticsRelation()
                    {
                        Id = Guid.NewGuid(),
                        MeetingId = newmodel.Id,
                        LogisticsId = item.LogisticsId,
                        Remark = item.Remark,
                        UserId = item.UserId,
                    };
                    await _meetingLogisticsRelationRepository.InsertAsync(entity);
                }
            }

            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingRoomUseInfoAppService>();
            if (newmodel.RoomId.HasValue)
                await service.Create(new CreateMeetingRoomUseInfoInput()
                {
                    BusinessId = newmodel.Id,
                    BusinessName = newmodel.Name,
                    BusinessType = MeetingRoomUseBusinessType.会议,
                    EndTime = newmodel.EndTime,
                    MeetingRoomId = newmodel.RoomId.Value,
                    StartTime = newmodel.StartTime,
                });

            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 提前通知参会人员（参会人，主持人，记录人）
        /// </summary>
        /// <param name="eventParams"></param>
        [RemoteService(IsEnabled = false)]
        public void SendMessageForJoinUser(Guid guid)
        {
            var model = _repository.Get(guid);
            if (model != null)
            {
                var joinUser = model.JoinPersonnel + "," + model.ModeratorId + "," + model.RecorderId;
                var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
                var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
                noticeInput.Content = $"【{model.Name}】会议将于：{model.StartTime.ToString("yyyy/MM/dd HH:mm:ss")}开始，请您准时参加。";
                noticeInput.Title = $"会议提醒";
                noticeInput.NoticeUserIds = joinUser.Replace("u_", ""); ;
                noticeInput.NoticeType = 1;
                noticeInput.SendUserId = model.CreatorUserId.Value;
                noticeService.CreateOrUpdateNotice(noticeInput);
            }
        }
        public async Task<InitWorkFlowOutput> CreatePeriod(CreateXZGLMeetingInput input)
        {
            if (input.RoomId.HasValue)
                if (!_roomRepository.GetAll().Any(x => x.Id == input.RoomId))
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该会议室不存在。");
            if (input.MeetingPeriodRule.EndTime <= input.MeetingPeriodRule.StartTime)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "会议结束时间不能小于等于会议开始时间。");
            }
            if (input.ModeratorId == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请选择/录入一个主持人。");
            if (input.RecorderId == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请选择/录入一个记录人。");
            var serverFire = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICreatePeriodMeetingWithHangFire>();





            var newmodel = new XZGLMeeting()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                MeetingTypeId = input.MeetingTypeId,
                RoomId = input.RoomId,
                MeetingRoomName = input.MeetingRoomName,
                StartTime = input.MeetingPeriodRule.StartTime,
                EndTime = input.MeetingPeriodRule.EndTime,
                OrgId = input.OrgId,
                OrganizeName = input.OrganizeName,
                MeetingCreateUser = input.MeetingCreateUser,
                ModeratorId = input.ModeratorId,
                RecorderId = input.RecorderId,
                AttendingLeaders = input.AttendingLeaders,
                MeetingGuest = input.MeetingGuest,
                JoinPersonnel = input.JoinPersonnel,
                MeetingTheme = input.MeetingTheme,
                MeetingIsssueType = input.MeetingIsssueType,
                MeetingIssue = input.MeetingIssue,
                IsNeedFile = input.IsNeedFile,
                IsNeedLogistics = input.IsNeedLogistics,
            };
            newmodel.Status = 0;
            newmodel.IsPeriod = true;
            await _repository.InsertAsync(newmodel);
            var newPeriodModel = input.MeetingPeriodRule.MapTo<MeetingPeriodRule>();
            newPeriodModel.MeetingId = newmodel.Id;
            var ruleModelWithCron = serverFire.MakePeriodMeetingCronAndNextTime(newPeriodModel, input.FlowId);
            newPeriodModel.CronExpression = ruleModelWithCron.CronExpression;
            newPeriodModel.NextCreateTime = ruleModelWithCron.NextCreateTime;
            newPeriodModel.HasAdvanceLessOneDay = ruleModelWithCron.HasAdvanceLessOneDay;

            await _meetingPeriodRuleRepository.InsertAsync(newPeriodModel);
            if (input.MeetingIsssueType == MeetingIsssueType.议题)
            {
                foreach (var item in input.Issues)
                {
                    var entity = new MeetingIssueRelation()
                    {
                        Id = Guid.NewGuid(),
                        MeetingId = newmodel.Id,
                        IssueId = item.IssueId,
                        UserIds = item.UserIds,
                    };
                    await _meetingIssueRelationRepository.InsertAsync(entity);
                }
            }
            if (input.IsNeedFile)
                foreach (var item in input.FileList)
                {
                    var fileModel = new MeetingFile()
                    {
                        Id = Guid.NewGuid(),
                        FileName = item.Name,
                        UserId = item.UserId,
                        MeetingId = newmodel.Id
                    };
                    await _meetingFileRepository.InsertAsync(fileModel);

                    var fileList = new List<AbpFileListInput>();
                    foreach (var entity in item.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
                    }
                    await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                    {
                        BusinessId = fileModel.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.会议资料,
                        Files = fileList
                    });
                }
            if (input.IsNeedLogistics)
            {
                foreach (var item in input.LogisticsList)
                {
                    var entity = new MeetingLogisticsRelation()
                    {
                        Id = Guid.NewGuid(),
                        LogisticsId = item.LogisticsId,
                        Remark = item.Remark,
                        UserId = item.UserId,
                        MeetingId = newmodel.Id
                    };
                    await _meetingLogisticsRelationRepository.InsertAsync(entity);
                }
            }
            serverFire.JobForCycleForDay(input.FlowId);
            CurrentUnitOfWork.SaveChanges();
            CreatePeriodJobForCycleForDay(input.FlowId, DateTime.Now);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }


        //public async Task UpdatePeriod(UpdateXZGLMeetingInput input)
        //{
        //    if (input.Id != Guid.Empty)
        //    {
        //        var dbmodel = await _repository.GetAsync(input.Id);
        //        if (dbmodel.RoomId.HasValue && !input.RoomId.HasValue)
        //            await _meetingRoomUseInfoRepository.DeleteAsync(r => r.BusinessId == dbmodel.Id && r.BusinessType == MeetingRoomUseBusinessType.会议);
        //        if (input.RoomId.HasValue)
        //        {
        //            var meetingUseInfo = await _meetingRoomUseInfoRepository.GetAll().FirstOrDefaultAsync(r => r.BusinessId == dbmodel.Id && r.BusinessType == MeetingRoomUseBusinessType.会议);
        //            if (meetingUseInfo == null)
        //            {
        //                var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingRoomUseInfoAppService>();
        //                await service.Create(new CreateMeetingRoomUseInfoInput()
        //                {
        //                    BusinessId = dbmodel.Id,
        //                    BusinessName = input.Name,
        //                    BusinessType = MeetingRoomUseBusinessType.会议,
        //                    EndTime = input.EndTime,
        //                    MeetingRoomId = input.RoomId.Value,
        //                    StartTime = input.StartTime,
        //                });
        //            }
        //            else
        //            {
        //                meetingUseInfo.MeetingRoomId = input.RoomId.Value;
        //                meetingUseInfo.StartTime = input.StartTime;
        //                meetingUseInfo.EndTime = input.EndTime;
        //                await _meetingRoomUseInfoRepository.UpdateAsync(meetingUseInfo);
        //            }
        //        }


        //        var ruleModel = await _meetingPeriodRuleRepository.GetAsync(input.MeetingPeriodRule.Id);
        //        input.MeetingPeriodRule.MapTo(ruleModel);
        //        await _meetingPeriodRuleRepository.UpdateAsync(ruleModel);



        //        var logModel = new XZGLMeeting();
        //        if (input.IsUpdateForChange)
        //        {
        //            logModel = dbmodel.DeepClone<XZGLMeeting>();
        //            dbmodel.Name = input.Name;
        //            dbmodel.MeetingTypeId = input.MeetingTypeId;
        //            dbmodel.RoomId = input.RoomId;
        //            dbmodel.MeetingRoomName = input.MeetingRoomName;
        //            dbmodel.StartTime = input.StartTime;
        //            dbmodel.EndTime = input.EndTime;
        //            dbmodel.OrganizeName = input.OrganizeName;
        //            dbmodel.OrgId = input.OrgId;
        //            dbmodel.RecorderId = input.RecorderId;
        //            dbmodel.ModeratorId = input.ModeratorId;
        //            dbmodel.AttendingLeaders = input.AttendingLeaders;
        //            dbmodel.MeetingGuest = input.MeetingGuest;
        //            dbmodel.MeetingTheme = input.MeetingTheme;

        //            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
        //            if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
        //            var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
        //            _projectAuditManager.Insert(logs, input.Id.ToString(), flowModel.TitleField.Table);
        //        }
        //        else
        //        {
        //            var newModel = input.MapTo(dbmodel);
        //            await _repository.UpdateAsync(newModel);
        //            if (input.MeetingIsssueType == MeetingIsssueType.自定义议程)
        //                await _meetingIssueRelationRepository.DeleteAsync(r => r.MeetingId == input.Id);
        //            else
        //            {
        //                await _meetingIssueRelationRepository.DeleteAsync(r => r.MeetingId == input.Id);
        //                foreach (var item in input.Issues)
        //                {
        //                    await _meetingIssueRelationRepository.InsertAsync(new MeetingIssueRelation()
        //                    { Id = Guid.NewGuid(), IssueId = item.IssueId, MeetingId = input.Id, UserIds = item.UserIds });
        //                }

        //            }
        //            if (input.IsNeedFile)
        //            {
        //                var updateFiles = input.FileList.Where(r => r.Id.HasValue);
        //                foreach (var item in updateFiles)
        //                {
        //                    var updateFileModel = await _meetingFileRepository.GetAsync(item.Id.Value);
        //                    updateFileModel.FileName = item.Name;
        //                    updateFileModel.UserId = item.UserId;
        //                    var fileList = new List<AbpFileListInput>();
        //                    foreach (var entity in item.FileList)
        //                    {
        //                        fileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
        //                    }
        //                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
        //                    {
        //                        BusinessId = item.Id.Value.ToString(),
        //                        BusinessType = (int)AbpFileBusinessType.会议资料,
        //                        Files = fileList
        //                    });
        //                }
        //                var old_FileModels = _meetingFileRepository.GetAll().Where(r => r.MeetingId == input.Id).ToList();
        //                var deleteFiles = old_FileModels.Where(o => old_FileModels.Select(r => r.Id).Except(input.FileList.Where(r => r.Id.HasValue).Select(r => r.Id.Value)).ToList().Contains(o.Id));
        //                foreach (var item in deleteFiles)
        //                    await _meetingFileRepository.DeleteAsync(item);

        //                var new_FileModels = input.FileList.Where(r => !r.Id.HasValue);
        //                foreach (var item in new_FileModels)
        //                {
        //                    var fileModel = new MeetingFile()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        FileName = item.Name,
        //                        UserId = item.UserId,
        //                        MeetingId = input.Id
        //                    };
        //                    await _meetingFileRepository.InsertAsync(fileModel);

        //                    var fileList = new List<AbpFileListInput>();
        //                    foreach (var entity in item.FileList)
        //                    {
        //                        fileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
        //                    }
        //                    await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
        //                    {
        //                        BusinessId = fileModel.Id.ToString(),
        //                        BusinessType = (int)AbpFileBusinessType.会议资料,
        //                        Files = fileList
        //                    });
        //                }

        //            }
        //            else
        //            {
        //                await _meetingFileRepository.DeleteAsync(r => r.MeetingId == input.Id);
        //            }

        //            if (input.IsNeedLogistics)
        //            {
        //                var updateLogs = input.LogisticsList.Where(r => r.Id.HasValue);
        //                foreach (var item in updateLogs)
        //                {
        //                    var updateLogModel = await _meetingLogisticsRelationRepository.GetAsync(item.Id.Value);
        //                    updateLogModel.Remark = item.Remark;
        //                    updateLogModel.UserId = item.UserId;
        //                }
        //                var old_LogModels = _meetingLogisticsRelationRepository.GetAll().Where(r => r.MeetingId == input.Id).ToList();
        //                var deleteLogs = old_LogModels.Where(o => old_LogModels.Select(r => r.Id).Except(input.LogisticsList.Where(r => r.Id.HasValue).Select(r => r.Id.Value)).ToList().Contains(o.Id));
        //                foreach (var item in deleteLogs)
        //                    await _meetingLogisticsRelationRepository.DeleteAsync(item);

        //                var new_LogModels = input.LogisticsList.Where(r => !r.Id.HasValue);
        //                foreach (var item in new_LogModels)
        //                {
        //                    var entity = new MeetingLogisticsRelation()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        MeetingId = input.Id,
        //                        LogisticsId = item.LogisticsId,
        //                        Remark = item.Remark,
        //                        UserId = item.UserId,
        //                    };
        //                    await _meetingLogisticsRelationRepository.InsertAsync(entity);
        //                }
        //            }
        //            else
        //            {
        //                await _meetingLogisticsRelationRepository.DeleteAsync(r => r.MeetingId == input.Id);
        //            }
        //        }
        //        var serverFire = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICreatePeriodMeetingWithHangFire>();
        //        serverFire.CreateOrUpdateJobForeCreatePeriodMeeting(ruleModel, input.FlowId);
        //    }
        //    else
        //        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
        //}

        //[RemoteService(IsEnabled = false)]

        public void CreatePeriodSelf(Guid id, Guid flowId)
        {
            var model = _repository.Get(id);
            var ruleModel = _meetingPeriodRuleRepository.GetAll().SingleOrDefault(r => r.MeetingId == id);
            if (ruleModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "周期会议规则数据不存在。");
            if (ruleModel.Status == PeriodRuleStatus.取消)
                return;
            if (ruleModel.ActiveStartTime > DateTime.Now)
                return;
            if (ruleModel.ActiveEndTime < DateTime.Now)
            {
                ruleModel.Status = PeriodRuleStatus.失效;
                _meetingPeriodRuleRepository.Update(ruleModel);
            }
            var newModel = model.DeepClone();
            newModel.IsPeriod = false;
            newModel.Id = Guid.NewGuid();
            newModel.CreationTime = DateTime.Now;
            var acturlMeetingDate = DateTime.Now;
            if (ruleModel.HasAdvanceLessOneDay)
            {
                if (ruleModel.PreTimeType == PreTimeType.天)
                    acturlMeetingDate = ruleModel.NextCreateTime.Value.AddDays(ruleModel.PreTimeNum);
                else
                    acturlMeetingDate = ruleModel.NextCreateTime.Value.AddDays(1);
            }
            else
                acturlMeetingDate = ruleModel.NextCreateTime.Value;
            newModel.StartTime = new DateTime(acturlMeetingDate.Year, acturlMeetingDate.Month, acturlMeetingDate.Day, newModel.StartTime.Hour, newModel.StartTime.Minute, 0);
            newModel.EndTime = new DateTime(acturlMeetingDate.Year, acturlMeetingDate.Month, acturlMeetingDate.Day, newModel.EndTime.Hour, newModel.EndTime.Minute, 0);
            _repository.Insert(newModel);
            var meetingIssueModels = _meetingIssueRelationRepository.GetAll().Where(r => r.MeetingId == id).ToList();
            foreach (var item in meetingIssueModels)
            {
                var entity = item.DeepClone();
                entity.Id = Guid.NewGuid();
                entity.MeetingId = newModel.Id;
                _meetingIssueRelationRepository.Insert(entity);
            }
            var meetingFileModels = _meetingFileRepository.GetAll().Where(r => r.MeetingId == id);
            foreach (var entityModel in meetingFileModels)
            {
                var item = entityModel.DeepClone();
                var fileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.会议资料 });
                item.MeetingId = newModel.Id;
                item.Id = Guid.NewGuid();
                var newfileList = new List<AbpFileListInput>();
                foreach (var entity in fileList)
                {
                    newfileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
                }
                _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                {
                    BusinessId = item.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.会议资料,
                    Files = newfileList
                });

                _meetingFileRepository.Insert(item);

            }
            var meetingLogModels = _meetingLogisticsRelationRepository.GetAll().Where(r => r.MeetingId == id);
            foreach (var entiyModel in meetingLogModels)
            {
                var item = entiyModel.DeepClone();
                item.Id = Guid.NewGuid();
                item.MeetingId = newModel.Id;
                _meetingLogisticsRelationRepository.Insert(item);
            }

            var taskId = _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flowId, newModel.Id.ToString(), "周期性会议", 1);
            CurrentUnitOfWork.SaveChanges();

            var taskModel = _workFlowTaskRepository.Get(taskId);
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowAppService>();
            var flowData = service.GetNextStepForRunSync(new GetNextStepForRunInput() { TaskId = taskId });

            var steps = new List<ExecuteWorkChooseStep>();
            foreach (var item in flowData.Steps)
            {
                steps.Add(new ExecuteWorkChooseStep()
                {
                    id = item.NextStepId.ToString(),
                    member = item.DefaultUserId,
                });
            }

            var ret = _workFlowWorkTaskAppService.ExecuteTaskWithUser(new ExecuteWorkFlowInput()
            {
                TaskId = taskId,
                ActionType = "Submit",
                FlowId = flowId,
                GroupId = taskModel.GroupID,
                InstanceId = taskModel.InstanceID,
                StepId = taskModel.StepID,
                Steps = steps,
                Title = "周期性会议",
            }, _useRepository.Get(1));
            if (!ret.IsSuccesefull)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "创建周期会议后发送给会议确认流程失败");




        }

        [RemoteService(IsEnabled = false)]
        public void CreatePeriodSelfForDay(Guid id, Guid flowId)
        {
            CreatePeriodSelf(id, flowId);
            var ruleModel = _meetingPeriodRuleRepository.FirstOrDefault(r => r.MeetingId == id);
            if (ruleModel.PeriodType == PeriodType.按天)
                ruleModel.NextCreateTime = ruleModel.NextCreateTime.Value.AddDays(ruleModel.PeriodNumber1);
            else
                ruleModel.NextCreateTime = _createPeriodMeetingWithHangFire.GetNextOccurrence(ruleModel.NextCreateTime.Value, ruleModel.CronExpression);
            _meetingPeriodRuleRepository.Update(ruleModel);
        }



        public void CreatePeriodJobForCycleForDay(Guid flowId, DateTime doTime)
        {
            doTime = DateTime.Now;
            var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var query = from a in _repository.GetAll()
                        join b in _meetingPeriodRuleRepository.GetAll() on a.Id equals b.MeetingId
                        where a.IsPeriod && b.NextCreateTime.HasValue && b.NextCreateTime >= startTime && b.NextCreateTime <= endTime
                        && b.Status == PeriodRuleStatus.正常
                        select new { a, b };
            var ret1 = query.ToList();
            foreach (var item in ret1)
            {
                var nextTime = item.b.NextCreateTime.Value;

                var acturlMeetingDate = DateTime.Now;
                if (item.b.HasAdvanceLessOneDay)
                {
                    if (item.b.PreTimeType == PreTimeType.天)
                        acturlMeetingDate = item.b.NextCreateTime.Value.AddDays(item.b.PreTimeNum);
                    else
                        acturlMeetingDate = item.b.NextCreateTime.Value.AddDays(1);
                }
                else
                    acturlMeetingDate = item.b.NextCreateTime.Value;
                var meetingStartTime = new DateTime(acturlMeetingDate.Year, acturlMeetingDate.Month, acturlMeetingDate.Day, item.b.StartTime.Hour, item.b.StartTime.Minute, 0);
                var ruleActiveEndTime = new DateTime(item.b.ActiveEndTime.Year, item.b.ActiveEndTime.Month, item.b.ActiveEndTime.Day, item.b.StartTime.Hour, item.b.StartTime.Minute, 0);
                if (meetingStartTime > ruleActiveEndTime)
                {
                    item.b.Status = PeriodRuleStatus.失效;
                    _meetingPeriodRuleRepository.Update(item.b);
                    continue;
                }

                if (nextTime.Year == doTime.Year && nextTime.Month == doTime.Month && nextTime.Day == doTime.Day && nextTime.Hour == doTime.Hour)
                    CreatePeriodSelfForDay(item.a.Id, flowId);
            }
            var time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0, 0);
            var query_NeedUpdateNextTime = from a in _repository.GetAll()
                                           join b in _meetingPeriodRuleRepository.GetAll() on a.Id equals b.MeetingId
                                           where a.IsPeriod && b.NextCreateTime.HasValue && b.NextCreateTime < time
                                           && b.Status == PeriodRuleStatus.正常
                                           select new { a, b };
            var ret2 = query_NeedUpdateNextTime.ToList();
            foreach (var item in ret2)
            {
                var nextTime = item.b.NextCreateTime.Value;
                var newNextTime = nextTime;
                if (item.b.PeriodType == PeriodType.按天)
                    newNextTime = newNextTime.AddDays(item.b.PeriodNumber1);
                else
                    newNextTime = _createPeriodMeetingWithHangFire.GetNextOccurrence(nextTime, item.b.CronExpression);
                var doCount = 1000;
                while (doCount > 0)
                {
                    if (newNextTime < time)
                    {
                        if (item.b.PeriodType == PeriodType.按天)
                            newNextTime = newNextTime.AddDays(item.b.PeriodNumber1);
                        else
                            newNextTime = _createPeriodMeetingWithHangFire.GetNextOccurrence(newNextTime, item.b.CronExpression);
                        doCount--;
                    }
                    else
                        break;
                }
                item.b.NextCreateTime = newNextTime;
                _meetingPeriodRuleRepository.Update(item.b);
            }
        }







        /// <summary>
        /// 修改一个XZGLMeeting
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateXZGLMeetingInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.GetAsync(input.Id);
                if (dbmodel.RoomId.HasValue && !input.RoomId.HasValue)
                    await _meetingRoomUseInfoRepository.DeleteAsync(r => r.BusinessId == dbmodel.Id && r.BusinessType == MeetingRoomUseBusinessType.会议);
                if (input.RoomId.HasValue)
                {
                    var meetingUseInfo = await _meetingRoomUseInfoRepository.GetAll().FirstOrDefaultAsync(r => r.BusinessId == dbmodel.Id && r.BusinessType == MeetingRoomUseBusinessType.会议);
                    if (meetingUseInfo == null)
                    {
                        var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingRoomUseInfoAppService>();
                        await service.Create(new CreateMeetingRoomUseInfoInput()
                        {
                            BusinessId = dbmodel.Id,
                            BusinessName = input.Name,
                            BusinessType = MeetingRoomUseBusinessType.会议,
                            EndTime = input.EndTime,
                            MeetingRoomId = input.RoomId.Value,
                            StartTime = input.StartTime,
                        });
                    }
                    else
                    {
                        meetingUseInfo.MeetingRoomId = input.RoomId.Value;
                        meetingUseInfo.StartTime = input.StartTime;
                        meetingUseInfo.EndTime = input.EndTime;
                        await _meetingRoomUseInfoRepository.UpdateAsync(meetingUseInfo);
                    }
                }


                var logModel = new XZGLMeeting();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<XZGLMeeting>();
                    dbmodel.Name = input.Name;
                    dbmodel.MeetingTypeId = input.MeetingTypeId;
                    dbmodel.RoomId = input.RoomId;
                    dbmodel.MeetingRoomName = input.MeetingRoomName;
                    dbmodel.StartTime = input.StartTime;
                    dbmodel.EndTime = input.EndTime;
                    dbmodel.OrganizeName = input.OrganizeName;
                    dbmodel.OrgId = input.OrgId;
                    dbmodel.RecorderId = input.RecorderId;
                    dbmodel.ModeratorId = input.ModeratorId;
                    dbmodel.AttendingLeaders = input.AttendingLeaders;
                    dbmodel.MeetingGuest = input.MeetingGuest;
                    dbmodel.MeetingTheme = input.MeetingTheme;

                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    _projectAuditManager.Insert(logs, input.Id.ToString(), flowModel.TitleField.Table);
                }
                else
                {
                    var newModel = input.MapTo(dbmodel);
                    await _repository.UpdateAsync(newModel);
                    if (input.MeetingIsssueType == MeetingIsssueType.自定义议程)
                        await _meetingIssueRelationRepository.DeleteAsync(r => r.MeetingId == input.Id);
                    else
                    {
                        await _meetingIssueRelationRepository.DeleteAsync(r => r.MeetingId == input.Id);
                        foreach (var item in input.Issues)
                        {
                            await _meetingIssueRelationRepository.InsertAsync(new MeetingIssueRelation()
                            { Id = Guid.NewGuid(), IssueId = item.IssueId, MeetingId = input.Id, UserIds = item.UserIds });
                        }

                    }
                    if (input.IsNeedFile)
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
                        var old_FileModels = _meetingFileRepository.GetAll().Where(r => r.MeetingId == input.Id).ToList();
                        var deleteFiles = old_FileModels.Where(o => old_FileModels.Select(r => r.Id).Except(input.FileList.Where(r => r.Id.HasValue).Select(r => r.Id.Value)).ToList().Contains(o.Id));
                        foreach (var item in deleteFiles)
                            await _meetingFileRepository.DeleteAsync(item);

                        var new_FileModels = input.FileList.Where(r => !r.Id.HasValue);
                        foreach (var item in new_FileModels)
                        {
                            var fileModel = new MeetingFile()
                            {
                                Id = Guid.NewGuid(),
                                FileName = item.Name,
                                UserId = item.UserId,
                                MeetingId = input.Id
                            };
                            await _meetingFileRepository.InsertAsync(fileModel);

                            var fileList = new List<AbpFileListInput>();
                            foreach (var entity in item.FileList)
                            {
                                fileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
                            }
                            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                            {
                                BusinessId = fileModel.Id.ToString(),
                                BusinessType = (int)AbpFileBusinessType.会议资料,
                                Files = fileList
                            });
                        }

                    }
                    else
                    {
                        await _meetingFileRepository.DeleteAsync(r => r.MeetingId == input.Id);
                    }

                    if (input.IsNeedLogistics)
                    {
                        var updateLogs = input.LogisticsList.Where(r => r.Id.HasValue);
                        foreach (var item in updateLogs)
                        {
                            var updateLogModel = await _meetingLogisticsRelationRepository.GetAsync(item.Id.Value);
                            updateLogModel.Remark = item.Remark;
                            updateLogModel.UserId = item.UserId;
                        }
                        var old_LogModels = _meetingLogisticsRelationRepository.GetAll().Where(r => r.MeetingId == input.Id).ToList();
                        var deleteLogs = old_LogModels.Where(o => old_LogModels.Select(r => r.Id).Except(input.LogisticsList.Where(r => r.Id.HasValue).Select(r => r.Id.Value)).ToList().Contains(o.Id));
                        foreach (var item in deleteLogs)
                            await _meetingLogisticsRelationRepository.DeleteAsync(item);

                        var new_LogModels = input.LogisticsList.Where(r => !r.Id.HasValue);
                        foreach (var item in new_LogModels)
                        {
                            var entity = new MeetingLogisticsRelation()
                            {
                                Id = Guid.NewGuid(),
                                MeetingId = input.Id,
                                LogisticsId = item.LogisticsId,
                                Remark = item.Remark,
                                UserId = item.UserId,
                            };
                            await _meetingLogisticsRelationRepository.InsertAsync(entity);
                        }
                    }
                    else
                    {
                        await _meetingLogisticsRelationRepository.DeleteAsync(r => r.MeetingId == input.Id);
                    }
                }



            }
            else
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
        }


        private async Task<XZGLMeetingChangeDto> GetChangeModel(XZGLMeeting model)
        {
            var ret = model.MapTo<XZGLMeetingChangeDto>();
            ret.ModeratorName = (await UserManager.GetUserByIdAsync(model.ModeratorId)).Name;
            ret.RecorderName = (await UserManager.GetUserByIdAsync(model.RecorderId)).Name;
            ret.AttendingLeadersName = _workFlowOrganizationUnitsManager.GetNames(model.AttendingLeaders);
            return ret;
        }

        /// <summary>
        /// 检查用户权限
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckPostFromUser(Guid TaskId, int type)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var taskModel = repository.Get(TaskId);
            switch (type)
            {
                case 0:
                case 2:
                    var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
                    var userRoles = userManager.GetRoles(taskModel.SenderID);
                    return type == 0 ? userRoles.Any(r => r == "XZRY") : userRoles.Any(r => r == "ZJL");
                case 1:
                    var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                        .Resolve<WorkFlowOrganizationUnitsManager>();
                    return manager.IsChargerLeader(taskModel.SenderID);
            }
            return false;
        }



        /// <summary>
        /// 完成抄送会议
        /// </summary>
        /// <param name="eventParams"></param>
        public void NotifyMeeting(WorkFlowCustomEventParams eventParams)
        {
            var users = new FlowCopyForInput()
            {
                FlowId = eventParams.FlowID,
                GroupId = eventParams.GroupID,
                InstanceId = eventParams.InstanceID,
                StepId = eventParams.StepID,
                TaskId = eventParams.TaskID
            };
            var meetid = Guid.Parse(eventParams.InstanceID);
            var meetingUser = _repository.Get(meetid);
            if (meetingUser != null && !string.IsNullOrEmpty(meetingUser.CopyForUsers))
            {
                users.UserIds = string.Join(",", meetingUser.CopyForUsers.Split(',').Select(x => $"u_{x}"));
                _workFlowWorkTaskAppService.FlowCopyFor(users);
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            if (!_repository.GetAll().Any(x => x.Id == input.Id && x.Status == -2))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "会议不存在。");
            }
            await _repository.DeleteAsync(x => x.Id == input.Id);

            StopMeeting(input.Id);

        }



        public async Task DeletePeriodMeeting(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
            var ruleModel = _meetingPeriodRuleRepository.GetAll().SingleOrDefault(r => r.MeetingId == input.Id);
            await _meetingPeriodRuleRepository.DeleteAsync(ruleModel);
            var serverFire = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICreatePeriodMeetingWithHangFire>();
            if (ruleModel.PeriodType != PeriodType.按天)
                serverFire.Cancle(input.Id);
        }


        [RemoteService(IsEnabled = false)]
        public void StopMeeting(Guid instanceId)
        {
            var model = _repository.Get(instanceId);
            //会议室占用删除
            _meetingRoomUseInfoRepository.Delete(r => r.BusinessId == model.Id);


            var meetingIssues = from a in _meetingIssueRepository.GetAll()
                                join b in _meetingIssueRelationRepository.GetAll() on a.Id equals b.IssueId
                                where b.MeetingId == instanceId
                                select a;
            foreach (var item in meetingIssues)
            {
                if (item.Status == MeetingIssueStatus.准备中)
                    item.Status = MeetingIssueStatus.待议;
            }

            var flow1Str = _settingRepository.FirstOrDefault(r => r.Name == "MeetingUserReturnReceiptFlowId");
            var flow2Str = _settingRepository.FirstOrDefault(r => r.Name == "MeetingUserBeforeFlowId");
            if (flow1Str == null || flow2Str == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "任务回执流程或会前任务流程配置失败");
            var flow1 = flow1Str.Value.ToGuid();
            var flow2 = flow2Str.Value.ToGuid();
            var statusArry = new int[] { 0, 1 };
            var userlist = _meetingUserRepository.GetAll().Where(r => r.MeetingId == instanceId);
            var tasks = _workFlowTaskRepository.GetAll().Where(r => userlist.Select(o => o.Id.ToString()).Contains(r.InstanceID) && r.FlowID == flow1 && statusArry.Contains(r.Status));
            foreach (var item in tasks)
                _workFlowWorkTaskAppService.EndTask(new EndTaskInput() { TaskId = item.Id });

            var beforeTask = _meetingUserBeforeTaskRepository.GetAll().Where(r => r.MeetingId == instanceId);
            var tasks2 = _workFlowTaskRepository.GetAll().Where(r => beforeTask.Select(o => o.Id.ToString()).Contains(r.InstanceID) && r.FlowID == flow2 && statusArry.Contains(r.Status));
            foreach (var item in tasks2)
                _workFlowWorkTaskAppService.EndTask(new EndTaskInput() { TaskId = item.Id });

        }


        [RemoteService(false)]
        public void CreateMeetingBeforeTask(Guid instanceId)
        {
            var model = GetForView(new EntityDto<Guid>() { Id = instanceId });

            var flow1 = _settingRepository.FirstOrDefault(r => r.Name == "MeetingUserReturnReceiptFlowId");
            var flow2 = _settingRepository.FirstOrDefault(r => r.Name == "MeetingUserBeforeFlowId");
            if (flow1 == null || flow2 == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "任务回执流程或会前任务流程配置失败");

            var meetingIssues = from a in _meetingIssueRepository.GetAll()
                                join b in _meetingIssueRelationRepository.GetAll() on a.Id equals b.IssueId
                                where b.MeetingId == instanceId
                                select a;
            foreach (var item in meetingIssues)
            {
                if (item.Status == MeetingIssueStatus.待议 || item.Status == MeetingIssueStatus.延迟)
                    item.Status = MeetingIssueStatus.准备中;
            }
            #region  会议通知

            var meetingTypeModel = _meetingTypeBaseRepository.FirstOrDefault(r => r.Id == model.MeetingTypeId);
            if (meetingTypeModel.ReturnReceiptEnable)
            {
                var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingUserAppService>();
                var meetingJoinUsers = _workFlowOrganizationUnitsManager.GetAllUsers(model.JoinPersonnel);
                foreach (var item in meetingJoinUsers)
                {
                    var ret1 = service.CreateSelf(new CreateMeetingUserInput()
                    {

                        FlowId = flow1.Value.ToGuid(),
                        FlowTitle = $"会议通知",
                        MeetingId = model.Id,
                        MeetingUserRole = MeetingUserRole.参会人员,
                        UserId = item.Id
                    });
                    if (AbpSession.UserId.HasValue)
                        _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                        {
                            FlowId = flow1.Value.ToGuid(),
                            InStanceId = ret1.InStanceId,
                            ReciveUserId = item.Id,
                            FlowTitle = $"会议通知",
                        });
                    else
                        _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow1.Value.ToGuid(), ret1.InStanceId, "会议通知", item.Id);
                }
                var ret2 = service.CreateSelf(new CreateMeetingUserInput()
                {
                    FlowId = flow1.Value.ToGuid(),
                    FlowTitle = $"会议通知",
                    MeetingId = model.Id,
                    MeetingUserRole = MeetingUserRole.记录者,
                    UserId = model.RecorderId
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow1.Value.ToGuid(),
                        InStanceId = ret2.InStanceId,
                        ReciveUserId = model.RecorderId,
                        FlowTitle = $"会议通知",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow1.Value.ToGuid(), ret2.InStanceId, "会议通知", model.RecorderId);
                var ret3 = service.CreateSelf(new CreateMeetingUserInput()
                {
                    FlowId = flow1.Value.ToGuid(),
                    FlowTitle = $"会议通知",
                    MeetingId = model.Id,
                    MeetingUserRole = MeetingUserRole.主持人,
                    UserId = model.ModeratorId
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow1.Value.ToGuid(),
                        InStanceId = ret3.InStanceId,
                        ReciveUserId = model.ModeratorId,
                        FlowTitle = $"会议通知",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow1.Value.ToGuid(), ret3.InStanceId, "会议通知", model.ModeratorId);
            }

            #endregion

            #region 会前任务
            var issueUsers = new List<long>();
            var fileUsers = new List<long>();
            var logUsers = new List<long>();
            if (model.MeetingIsssueType == MeetingIsssueType.议题)
            {
                var issueUserStr = string.Join(",", model.IssueList.Select(r => r.UserId));
                issueUserStr.Split(",").ToList().Distinct().ToList().ForEach(r =>
                {
                    if (!string.IsNullOrEmpty(r))
                        issueUsers.Add(MemberPerfix.RemovePrefix(r).ToLong());
                });
            }
            if (model.IsNeedFile)
                fileUsers = model.FileList.Select(r => r.UserId).Distinct().ToList();
            if (model.IsNeedLogistics)
                logUsers = model.LogisticsList.Select(r => r.UserId).Distinct().ToList();


            var serviceBeforeTask = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingUserBeforeTaskAppService>();
            var user7 = issueUsers.Intersect(fileUsers).Intersect(logUsers).Distinct();
            foreach (var item in user7)
            {
                var retInstance = serviceBeforeTask.CreateSelf(new CreateMeetingUserBeforeTaskInput()
                {
                    FlowId = flow2.Value.ToGuid(),
                    FlowTitle = "会前任务",
                    MeetingId = model.Id,
                    TaskType = 7,
                    UserId = item,
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow2.Value.ToGuid(),
                        InStanceId = retInstance.InStanceId,
                        ReciveUserId = item,
                        FlowTitle = "会前任务",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow2.Value.ToGuid(), retInstance.InStanceId, "会前任务", item);

            }

            var user6 = fileUsers.Intersect(logUsers).Except(issueUsers).Distinct();

            foreach (var item in user6)
            {
                var retInstance = serviceBeforeTask.CreateSelf(new CreateMeetingUserBeforeTaskInput()
                {
                    FlowId = flow2.Value.ToGuid(),
                    FlowTitle = "会前任务",
                    MeetingId = model.Id,
                    TaskType = 6,
                    UserId = item,
                });

                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow2.Value.ToGuid(),
                        InStanceId = retInstance.InStanceId,
                        ReciveUserId = item,
                        FlowTitle = "会前任务",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow2.Value.ToGuid(), retInstance.InStanceId, "会前任务", item);



            }


            var user5 = issueUsers.Intersect(logUsers).Except(fileUsers).Distinct();

            foreach (var item in user5)
            {
                var retInstance = serviceBeforeTask.CreateSelf(new CreateMeetingUserBeforeTaskInput()
                {
                    FlowId = flow2.Value.ToGuid(),
                    FlowTitle = "会前任务",
                    MeetingId = model.Id,
                    TaskType = 5,
                    UserId = item,
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow2.Value.ToGuid(),
                        InStanceId = retInstance.InStanceId,
                        ReciveUserId = item,
                        FlowTitle = "会前任务",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow2.Value.ToGuid(), retInstance.InStanceId, "会前任务", item);

            }

            var user4 = logUsers.Except(issueUsers).Except(fileUsers).Distinct();

            foreach (var item in user4)
            {
                var retInstance = serviceBeforeTask.CreateSelf(new CreateMeetingUserBeforeTaskInput()
                {
                    FlowId = flow2.Value.ToGuid(),
                    FlowTitle = "会前任务",
                    MeetingId = model.Id,
                    TaskType = 4,
                    UserId = item,
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow2.Value.ToGuid(),
                        InStanceId = retInstance.InStanceId,
                        ReciveUserId = item,
                        FlowTitle = "会前任务",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow2.Value.ToGuid(), retInstance.InStanceId, "会前任务", item);


            }

            var user3 = issueUsers.Intersect(fileUsers).Except(logUsers).Distinct();
            foreach (var item in user3)
            {
                var retInstance = serviceBeforeTask.CreateSelf(new CreateMeetingUserBeforeTaskInput()
                {
                    FlowId = flow2.Value.ToGuid(),
                    FlowTitle = "会前任务",
                    MeetingId = model.Id,
                    TaskType = 3,
                    UserId = item,
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow2.Value.ToGuid(),
                        InStanceId = retInstance.InStanceId,
                        ReciveUserId = item,
                        FlowTitle = "会前任务",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow2.Value.ToGuid(), retInstance.InStanceId, "会前任务", item);

            }


            var user2 = fileUsers.Except(issueUsers).Except(logUsers).Distinct();
            foreach (var item in user2)
            {
                var retInstance = serviceBeforeTask.CreateSelf(new CreateMeetingUserBeforeTaskInput()
                {
                    FlowId = flow2.Value.ToGuid(),
                    FlowTitle = "会前任务",
                    MeetingId = model.Id,
                    TaskType = 2,
                    UserId = item,
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow2.Value.ToGuid(),
                        InStanceId = retInstance.InStanceId,
                        ReciveUserId = item,
                        FlowTitle = "会前任务",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow2.Value.ToGuid(), retInstance.InStanceId, "会前任务", item);

            }


            var user1 = issueUsers.Except(fileUsers).Except(logUsers).Distinct();
            foreach (var item in user1)
            {
                var retInstance = serviceBeforeTask.CreateSelf(new CreateMeetingUserBeforeTaskInput()
                {
                    FlowId = flow2.Value.ToGuid(),
                    FlowTitle = "会前任务",
                    MeetingId = model.Id,
                    TaskType = 1,
                    UserId = item,
                });
                if (AbpSession.UserId.HasValue)
                    _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput()
                    {
                        FlowId = flow2.Value.ToGuid(),
                        InStanceId = retInstance.InStanceId,
                        ReciveUserId = item,
                        FlowTitle = "会前任务",
                    });
                else
                    _workFlowWorkTaskAppService.InsertWorkFlowTaskForUserId(flow2.Value.ToGuid(), retInstance.InStanceId, "会前任务", item);

            }
            #endregion

            #region 创建事务通知
            var dbmodel = _repository.Get(instanceId);
            var meetingType = _meetingTypeBaseRepository.Get(dbmodel.MeetingTypeId);
            if (meetingType.WarningEnable)
            {
                var time = dbmodel.StartTime;
                var dateNumber = -Convert.ToDouble(meetingType.WarningDateNumber);
                switch (meetingType.WraningDataType)
                {
                    case WraningDataTypeEnum.Day:
                        time = time.AddDays(dateNumber);
                        break;
                    case WraningDataTypeEnum.Minute:
                        time = time.AddMinutes(dateNumber);
                        break;
                    case WraningDataTypeEnum.Hour:
                        time = time.AddHours(dateNumber);
                        break;
                }
                if (meetingType.WraingType == WraingTypeEnum.Information && time > DateTime.Now)
                {
                    DateTimeOffset dt = new DateTimeOffset(time, TimeZoneInfo.Local.GetUtcOffset(time));
                    var jobId = BackgroundJob.Schedule<IXZGLMeetingAppService>(x => x.SendMessageForJoinUser(dbmodel.Id), dt);
                    dbmodel.HangfireJobId = jobId;
                    _repository.Update(dbmodel);
                }
            }
            #endregion

        }

        /// <summary>
        /// 提交会议记录-获取
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<GetViewForRecodeOutput> GetViewForRecord(Guid instanceId)
        {
            var ret = new GetViewForRecodeOutput();
            ret.MeetingInfo = await GetForViewAsync(new EntityDto<Guid>() { Id = instanceId });
            if (ret.MeetingInfo.HasSubmitRecord.HasValue && ret.MeetingInfo.HasSubmitRecord.Value)
            {
                ret.MeetingInfo.RealAttendeeUsers_Name = _workFlowOrganizationUnitsManager.GetNames(ret.MeetingInfo.RealAttendeeUsers);
                ret.MeetingInfo.AbsentUserName = _workFlowOrganizationUnitsManager.GetNames(ret.MeetingInfo.AbsentUser);

            }
            else
            {

                if (ret.MeetingInfo.MeetingUsers.Count() > 0 && ret.MeetingInfo.MeetingUsers.Where(r => r.ReturnReceiptStatus == ReturnReceiptStatus.申请请假 && r.Status == -1).Count() > 0)
                {
                    ret.MeetingInfo.AbsentUser = string.Join(",", ret.MeetingInfo.MeetingUsers.Where(r => r.ReturnReceiptStatus == ReturnReceiptStatus.申请请假 && r.Status == -1)
                        .Select(r => "u_" + r.UserId).ToList());
                    ret.MeetingInfo.AbsentUserName = _workFlowOrganizationUnitsManager.GetNames(ret.MeetingInfo.AbsentUser);
                }

                var realAttendeeUsersList = new List<string>();
                if (ret.MeetingInfo.AttendingLeaders.IsNullOrEmpty())
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "会议不存在参会领导");
                realAttendeeUsersList.AddRange(ret.MeetingInfo.AttendingLeaders.Split(',').ToList());
                if (ret.MeetingInfo.JoinPersonnel.IsNullOrEmpty())
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "会议不存在参会人");
                realAttendeeUsersList.AddRange(ret.MeetingInfo.JoinPersonnel.Split(",").ToList());
                realAttendeeUsersList.Add("u_" + ret.MeetingInfo.ModeratorId);
                realAttendeeUsersList.Add("u_" + ret.MeetingInfo.RecorderId);
                realAttendeeUsersList.Distinct();
                if (ret.MeetingInfo.MeetingUsers.Count() > 0 && ret.MeetingInfo.MeetingUsers.Where(r => r.ReturnReceiptStatus == ReturnReceiptStatus.申请请假 && r.Status == -1).Count() > 0)
                {
                    realAttendeeUsersList = realAttendeeUsersList.Except(ret.MeetingInfo.MeetingUsers.Where(r => r.ReturnReceiptStatus == ReturnReceiptStatus.申请请假 && r.Status == -1)
                        .Select(r => "u_" + r.UserId).ToList()).ToList();
                }



                ret.MeetingInfo.RealAttendeeUsers = string.Join(",", realAttendeeUsersList);
                ret.MeetingInfo.RealAttendeeUsers_Name = _workFlowOrganizationUnitsManager.GetNames(ret.MeetingInfo.RealAttendeeUsers);
                ret.MeetingInfo.RealMeetingGuest = ret.MeetingInfo.MeetingGuest;

            }



            var query = from a in _meetingIssueRepository.GetAll()
                        where a.RelationMeetingId == instanceId
                        select new MeetingIssueOutputDto
                        {
                            Content = a.Content,
                            Id = a.Id,
                            Name = a.Name,
                            OrgId = a.OrgId,
                            Stauts = a.Status,
                            UserId = a.UserId,
                            IssueType= a.IssueType,
                            SingleProjectId=a.SingleProjectId
                        };
            ret.NewIsseuList = await query.ToListAsync();
            foreach (var item in ret.NewIsseuList)
            {
                if (item.OrgId.HasValue)
                    item.OrgName = (await _workflowOrganizationUnitsRepository.GetAsync(item.OrgId.Value)).DisplayName;
                if (!item.UserId.IsNullOrEmpty())
                    item.UserName = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                item.StautsTitle = item.Stauts.ToString();
            }
            return ret;
        }


        /// <summary>
        /// 提交会议记录-保存
        /// </summary>
        /// <param name="input"></param>
        public void SubmitRecord(SubmitRecordInput input)
        {
            var model = _repository.Get(input.Id);
            model.Record = input.Record;
            model.StartTime = input.StartTime;
            model.EndTime = input.EndTime;
            model.CopyForUsers = input.CopyForUsers;
            model.RealAttendeeUsers = input.RealAttendeeUsers;
            model.RealMeetingGuest = input.RealMeetingGuest;
            model.AbsentUser = input.AbsentUser;
            model.HasSubmitRecord = true;

            if (model.RoomId.HasValue)
            {
                var meetingUse = _meetingRoomUseInfoRepository.GetAll().SingleOrDefault(r => r.BusinessId == model.Id && r.BusinessType == MeetingRoomUseBusinessType.会议);
                meetingUse.StartTime = input.StartTime;
                meetingUse.EndTime = input.EndTime;
                _meetingRoomUseInfoRepository.Update(meetingUse);
            }

            if (model.MeetingIsssueType == MeetingIsssueType.议题)
            {

                var meetingIssues = from a in _meetingIssueRepository.GetAll()
                                    join b in _meetingIssueRelationRepository.GetAll() on a.Id equals b.IssueId
                                    where b.MeetingId == input.Id
                                    select new { a, b };
                foreach (var item in meetingIssues)
                {
                    var entity = input.IssueResults.FirstOrDefault(r => r.IssueId == item.a.Id);
                    if (entity == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数错误");
                    if (item.a.Status != MeetingIssueStatus.准备中)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "议题状态错误");
                    item.b.Status = entity.HasPass ? MeetingIssueResultStatus.HasPass : MeetingIssueResultStatus.NoPass;
                    _meetingIssueRelationRepository.Update(item.b);
                }
            }
            if (input.NewIsseuList.Count() > 0)
            {
                var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IMeetingIssueAppService>();

                var exitIssus = _meetingIssueRepository.GetAll().Where(r => r.RelationMeetingId == model.Id && r.Status == MeetingIssueStatus.草稿);
                var updateIdList = exitIssus.Select(r => r.Id).Intersect(input.NewIsseuList.Where(r => r.Id.HasValue).Select(r => r.Id.Value));
                foreach (var item in updateIdList)
                {
                    var entity = input.NewIsseuList.FirstOrDefault(r => r.Id == item);
                    service.UpdateSelf(new UpdateMeetingIssueInput()
                    {
                        UserId = entity.UserId,
                        SingleProjectId = entity.SingleProjectId,
                        RelationProposalId = entity.RelationProposalId,
                        RelationMeetingId = entity.RelationMeetingId,
                        OrgId = entity.OrgId,
                        Content = entity.Content,
                        Id = entity.Id.Value,
                        IssueType = entity.IssueType,
                        Name = entity.Name,
                    });
                }


                var removeIdList = exitIssus.Select(r => r.Id).Except(input.NewIsseuList.Where(r => r.Id.HasValue).Select(r => r.Id.Value));
                foreach (var item in removeIdList)
                {
                    service.Delete(new EntityDto<Guid>() { Id = item });
                }
                foreach (var item in input.NewIsseuList.Where(r => !r.Id.HasValue))
                {
                    item.RelationMeetingId = input.Id;
                    service.CreateSelf(new CreateMeetingIssueInput()
                    {
                        Content = item.Content,
                        IssueType = item.IssueType,
                        Name = item.Name,
                        OrgId = item.OrgId,
                        RelationMeetingId = item.RelationMeetingId,
                        RelationProposalId = item.RelationProposalId,
                        SingleProjectId = item.SingleProjectId,
                        UserId = item.UserId
                    });
                }

            }

        }

        /// <summary>
        /// 查找会议确认人
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public string GetMeetingConfirmUser(Guid instanceId)
        {
            var model = _repository.Get(instanceId);
            return $"u_{model.MeetingCreateUser}";
        }


        /// <summary>
        /// 查找会议记录人
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public string GetMeetingRecodeUser(Guid instanceId)
        {
            var model = _repository.Get(instanceId);
            return $"u_{model.RecorderId}";
        }

        /// <summary>
        /// 终止会议时，清空会议室的占用
        /// </summary>
        /// <param name="meetingId"></param>
        [RemoteService(IsEnabled = false)]
        public void EndMeetingRoomUse(Guid meetingId)
        {
            var model = _repository.Get(meetingId);
            if (model.RoomId.HasValue)
                _meetingRoomUseInfoRepository.Delete(r => r.BusinessId == model.Id && r.BusinessType == MeetingRoomUseBusinessType.会议);
        }




    }
}