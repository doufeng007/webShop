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
using Train.Enum;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Train
{
    public class UserCourseExperienceAppService : FRMSCoreAppServiceBase, IUserCourseExperienceAppService
    { 
        private readonly IRepository<UserCourseExperience, Guid> _repository;
		private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly WorkFlowOrganizationUnitsManager _unitsManager;
        private readonly IRepository<TrainUserExperience, Guid> _experienceRepository;
        private readonly IRepository<Course, Guid> _courseRepository;
        private readonly IRepository<TrainUserExperience, Guid> _trainUserExperienceRepository;
        private readonly IUserTrainScoreRecordAppService _trainScoreRecordAppService;
        private readonly ICourseSettingAppService _courseSettingAppService;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly ITrainUserExperienceAppService _experienceAppService;
        public UserCourseExperienceAppService(IRepository<UserCourseExperience, Guid> repository
		,WorkFlowBusinessTaskManager workFlowBusinessTaskManager,IAbpFileRelationAppService abpFileRelationAppService
            , ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager
            , WorkFlowOrganizationUnitsManager unitsManager, IRepository<TrainUserExperience, Guid> experienceRepository
            , IRepository<Course, Guid> courseRepository, IRepository<TrainUserExperience, Guid> trainUserExperienceRepository
            , IUserTrainScoreRecordAppService trainScoreRecordAppService, ICourseSettingAppService courseSettingAppService,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository
            , ITrainUserExperienceAppService experienceAppService
        )
        {
            this._repository = repository;
			_workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
             _projectAuditManager = projectAuditManager;
             _workFlowCacheManager = workFlowCacheManager;
            _unitsManager = unitsManager;
            _experienceRepository = experienceRepository;
            _courseRepository = courseRepository;
            _trainUserExperienceRepository = trainUserExperienceRepository;
            _trainScoreRecordAppService = trainScoreRecordAppService;
            _courseSettingAppService = courseSettingAppService;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _experienceAppService = experienceAppService;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<UserCourseExperienceOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var experienceIds = string.IsNullOrEmpty(model.ExperienceId) ? new List<Guid>() : model.ExperienceId.Split(",").Select(Guid.Parse).ToList();
            var result = new UserCourseExperienceOutputDto();
            //若当前处理者部门领导，则仅看自己部门的心得体会
            var depuser = new List<long>();
            var user = await base.GetCurrentUserAsync();
            var leader = _unitsManager.GetChargeLeader(user.Id);
            if (leader.Split(",").Select(x => long.Parse(x.Replace("u_", ""))).ToList().Contains(user.Id))
            {
                var dep = await _userOrganizationUnitRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.IsMain && x.UserId == user.Id);
                depuser = await (from a in _userOrganizationUnitRepository.GetAll().Where(x =>
                        !x.IsDeleted && x.IsMain && x.OrganizationUnitId == dep.OrganizationUnitId)
                    join b in UserManager.Users on a.UserId equals b.Id
                    select b.Id
                ).ToListAsync();
            }
            result.ExperienceList = await
            (from a in _trainUserExperienceRepository.GetAll().Where(x =>
                    !x.IsDeleted && x.Type == TrainExperienceType.Course && x.TrainId == model.CourseId &&
                    (!experienceIds.Any() || experienceIds.Contains(x.Id)) && (!depuser.Any() || depuser.Contains(x.CreatorUserId ?? 0)))
                join b in UserManager.Users.Where(x => !x.IsDeleted) on a.UserId equals b.Id
                select new CourseExperienceDetailOutputDto
                {
                    ExperienceId = a.Id,
                    UserName = b.Name,
                    IsComplate = true,
                    SubmitTime = a.CreationTime
                }).ToListAsync();
            result.ExperienceCommentList = await _experienceAppService.GetOrganList(
                new GetTrainUserExperienceInput()
                {
                    ExperienceType = TrainExperienceType.Course,
                    TrainId = model.Id
                });
            var userid =(await base.GetCurrentUserAsync()).Id;
            var myExperience = await _experienceRepository.FirstOrDefaultAsync(x =>
                x.TrainId == model.CourseId && x.Type == TrainExperienceType.Course && x.UserId== userid);
            result.MyExperienceApproval = myExperience?.Approval;
            return result;
        }

        /// <summary>
        /// 添加一个UserCourseExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateUserCourseExperienceInput input)
        {
            var submituser = (await (from c in _trainUserExperienceRepository.GetAll()
                    .Where(x => !x.IsDeleted && x.TrainId == input.CourseId && x.Type == TrainExperienceType.Course)
                select "u_" + c.UserId).ToListAsync());
            var createuser = await base.GetCurrentUserAsync();
            submituser.Add("u_" + createuser.Id);
            var newmodel = new UserCourseExperience()
            {
                UserId = input.UserId,
                CourseId = input.CourseId,
                ExperienceId = "",
                SubmitUsers = string.Join(",", submituser.Distinct()),
                DealWithUsers = input.DealWithUsers,
                Status = input.Status,
                CopyForUsers = input.CopyForUsers
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() {InStanceId = newmodel.Id.ToString()};
        }

        /// <summary>
        /// 修改一个UserCourseExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateUserCourseExperienceInput input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int) ErrorCode.CodeValErr, "该数据不存在！");
                }
                var logModel = new UserCourseExperience();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<UserCourseExperience>();
                }
                var dbexp = string.IsNullOrEmpty(dbmodel.ExperienceId)
                    ? new List<string>()
                    : dbmodel.ExperienceId.Split(',').ToList();
                if (string.IsNullOrEmpty(input.ExperienceId))
                {
                    throw new UserFriendlyException((int) ErrorCode.CodeValErr, "请至少选择一条心得体会汇总！");
                }
                var inputexp = input.ExperienceId.Split(',').ToList();
                var newexp = inputexp.Except(dbexp).ToList();
                dbexp.AddRange(newexp);
                dbmodel.ExperienceId = string.Join(",", dbexp);
                await _repository.UpdateAsync(dbmodel);
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int) ErrorCode.CodeValErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.InStanceId.ToString(),
                        flowModel.TitleField.Table);
                }
                //采纳心得体会加积分
                var course = _courseRepository.Get(dbmodel.CourseId);
                var set = await _courseSettingAppService.GetSetVal(course.LearnType, course.IsSpecial);

                newexp.ForEach(async x =>
                {
                    var exp = _experienceRepository.Get(Guid.Parse(x));
                    exp.IsUse = true;
                    _experienceRepository.Update(exp);
                    if (set.ExperienceScore > 0)
                    {
                        await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                        {
                            FromId = dbmodel.CourseId,
                            FromType = TrainScoreFromType.CourseExperience,
                            Score = set.ExperienceScore,
                            UserId = exp.CreatorUserId ?? 0
                        });
                    }
                });
            }
            else
            {
                throw new UserFriendlyException((int) ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        private UserCourseExperienceLogDto GetChangeModel(UserCourseExperience model)
        {
            var ret = model.MapTo<UserCourseExperienceLogDto>();
            return ret;
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

        /// <summary>
        /// 获取心得体会分管领导
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetCouserExperienceUserLeader(string InstanceID)
        {
            Guid.TryParse(InstanceID, out Guid id);
            var courseExperience = _repository.GetAll().FirstOrDefault(x => x.Id == id);
            if (courseExperience == null)
                return "";
            var arr = courseExperience.SubmitUsers.Split(',');
            var users = "";
            foreach (var item in arr)
            {
                var userId = Convert.ToInt32(item.Replace("u_", ""));
                var leader = _unitsManager.GetChargeLeader(userId);
                if (!string.IsNullOrEmpty(leader))
                    users += leader + ",";
            }
            return users.TrimEnd(',');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CourseExperienceSummaryOutputDto>> GetAllCourseExperienceSummary(
            PagedAndSortedInputDto input)
        {
            var query = (from a in _courseRepository.GetAll().Where(x => !x.IsDeleted && x.IsExperience)
                let sendFlow = (from b in _repository.GetAll().Where(x => !x.IsDeleted && x.CourseId == a.Id)
                    select b.Id).Any()
                let submit = (from c in _trainUserExperienceRepository.GetAll()
                        .Where(x => !x.IsDeleted && x.TrainId == a.Id && x.Type == TrainExperienceType.Course)
                    select c.Id).Count()
                select new CourseExperienceSummaryOutputDto
                {
                    CourseId = a.Id,
                    CourseName = a.CourseName,
                    CreateTime = a.CreationTime,
                    SubmitUser = submit,
                    LearnUser = a.LearnUser,
                    LearnType = a.LearnType,
                    IsSendFlow = sendFlow
                }).Where(x => !x.IsSendFlow);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreateTime).PageBy(input).ToListAsync();
            ret.ForEach(x =>
            {
                var unSubmitUser = -1;
                switch (x.LearnType)
                {
                    case CourseLearnType.MustAll:
                        unSubmitUser = UserManager.Users.Count() - x.SubmitUser;
                        unSubmitUser = unSubmitUser < 0 ? 0 : unSubmitUser;
                        break;
                    case CourseLearnType.Must:
                        unSubmitUser = x.LearnUser.Split(',').Length - x.SubmitUser;
                        unSubmitUser = unSubmitUser < 0 ? 0 : unSubmitUser;
                        break;
                }
                x.UnSubmitUser = unSubmitUser;
            });
            return new PagedResultDto<CourseExperienceSummaryOutputDto>(toalCount, ret);
        }

        public async Task<PagedResultDto<CourseExperienceDetailOutputDto>> GetCourseExperienceDetail(
            CourseExperienceDetailInput input)
        {
            var query = (from a in _trainUserExperienceRepository.GetAll().Where(x =>
                    !x.IsDeleted && x.Type == TrainExperienceType.Course && x.TrainId == input.CourseId)
                join b in UserManager.Users.Where(x => !x.IsDeleted) on a.UserId equals b.Id
                select new CourseExperienceDetailOutputDto
                {
                    ExperienceId = a.Id,
                    UserName = b.Name,
                    IsComplate = true,
                    SubmitTime = a.CreationTime
                });
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.SubmitTime).PageBy(input).ToListAsync();
            return new PagedResultDto<CourseExperienceDetailOutputDto>(toalCount, ret);
        }

        public async Task<WorkFlowTrainUserExperienceOutputDto> GetMyExperience(GetTrainUserExperienceInput input)
        {
            var result = new WorkFlowTrainUserExperienceOutputDto();
            var user =await base.GetCurrentUserAsync();
            var userid = "u_" + user.Id;
            result.UserExperience = await _experienceAppService.GetIsExistence(input);
            result.CanChange = result.UserExperience == null || !(await _repository.GetAll().AnyAsync(x =>
                                   !x.IsDeleted && x.CourseId == input.TrainId && x.SubmitUsers.GetStrContainsArray(userid)));
            return result;
        }
    }
}