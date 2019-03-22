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
using Train.Enum;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;
using System.Text.RegularExpressions;

namespace Train
{
    public class TrainUserExperienceAppService : FRMSCoreAppServiceBase, ITrainUserExperienceAppService
    {
        private readonly IRepository<TrainUserExperience, Guid> _repository;
        private readonly IRepository<TrainLeave, Guid> _trainLeaveRepository;
        private readonly IUserTrainScoreRecordAppService _trainScoreRecordAppService;
        private readonly IRepository<Train, Guid> _trainRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<UserCourseExperience, Guid> _courseExperienceRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public TrainUserExperienceAppService(IRepository<TrainUserExperience, Guid> repository, IUserTrainScoreRecordAppService trainScoreRecordAppService, IRepository<Train, Guid> trainRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<User, long> userRepository, IRepository<UserCourseExperience, Guid> courseExperienceRepository, IWorkFlowTaskRepository workFlowTaskRepository
        )
        {
            this._repository = repository;
            _trainRepository = trainRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _userRepository = userRepository;
            _courseExperienceRepository = courseExperienceRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _trainScoreRecordAppService = trainScoreRecordAppService;
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TrainUserExperienceSumOutputDto>> GetList(GetTrainUserExperienceListInput input)
        {
            var train = _trainRepository.GetAll().FirstOrDefault(x => x.Id == input.TrainId);
            if (train == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            var users = train.JoinUser.Split(',').ToArray().Select(x => Convert.ToInt64(x.Replace("u_", "")));
            var query = from a in users
                        join b in _userRepository.GetAll() on a equals b.Id
                        let ex = _repository.GetAll().Where(x => !x.IsDeleted && x.TrainId == input.TrainId && x.UserId == a && x.Type == TrainExperienceType.Train).FirstOrDefault()
                        select new TrainUserExperienceSumOutputDto()
                        {
                            Id = ex?.Id,
                            UserId = a,
                            UserName = b.Name,
                            TrainId = input.TrainId,
                            Experience = ex?.Experience,
                            Approval = ex?.Approval,
                            CreationTime = ex?.CreationTime,
                            IsOver = ex != null
                        };
            var toalCount = query.Count();
            var ret = query.OrderByDescending(x => x.IsOver).OrderByDescending(r => r.CreationTime).ToList();

            return new PagedResultDto<TrainUserExperienceSumOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TrainUserExperienceListOutputDto>> GetListByUser(GetTrainUserExperienceListInput input)
        {
            var users = new List<long>();
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var train = _trainRepository.GetAll().FirstOrDefault(x => x.Id == input.TrainId);
            var arr = train.JoinUser.Split(',');
            foreach (var item in arr)
            {
                var userId = Convert.ToInt64(item.Replace("u_", ""));
                var organUser = organizeManager.GetChargeLeader(userId);
                var lId = !string.IsNullOrEmpty(organUser) ? MemberPerfix.RemovePrefix(organUser).ToLong() : 0;
                if (lId == AbpSession.UserId.Value)
                    users.Add(userId);
            }


            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.TrainId == input.TrainId && x.Type == TrainExperienceType.Train && users.Contains(x.UserId))
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        select new TrainUserExperienceListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            UserName = b.Name,
                            TrainId = a.TrainId,
                            Experience = a.Experience,
                            Approval = a.Approval,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<TrainUserExperienceListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<List<TrainUserExperienceOrganListOutputDto>> GetOrganList(GetTrainUserExperienceInput input)
        {
            var list = new List<TrainUserExperienceOrganListOutputDto>();

            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var arr = new List<long>();
            var courseid = Guid.Empty;
            switch (input.ExperienceType)
            {
                case TrainExperienceType.Train:
                    var train = _trainRepository.GetAll().FirstOrDefault(x => x.Id == input.TrainId);
                    arr = train.JoinUser.Split(',').Select(x => long.Parse(x.Replace("u_", ""))).ToList();
                    break;
                case TrainExperienceType.Course:
                    var course = await _courseExperienceRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == input.TrainId);
                    var exps = string.IsNullOrEmpty(course?.ExperienceId)
                        ? new List<Guid>()
                        : course?.ExperienceId.Split(",").Select(Guid.Parse);
                    arr = await _repository.GetAll().Where(x => x.IsUse && exps.Contains(x.Id)).Select(x => x.UserId).ToListAsync();
                    courseid = course.CourseId;
                    break;
            }
            var leaders = new List<long>();
            var myLeader = organizeManager.GetChargeLeader(AbpSession.UserId.Value);
            foreach (var userId in arr)
            {
                var leaderstr = organizeManager.GetChargeLeader(userId);
                if (!string.IsNullOrEmpty(leaderstr))
                {
                    var leaderarr = leaderstr.Split(',').Select(x => long.Parse(x.Replace("u_", ""))).ToList();
                    foreach (var leaderInt in leaderarr)
                    {
                        var organ = organizeManager.GetDeptByUserID(leaderInt);
                        var user = _userRepository.GetAll().FirstOrDefault(x => x.Id == leaderInt);
                        if (list.Count(x => x.LeaderId == leaderInt) == 0)
                        {
                            var users = new List<long>();
                            users.Add(userId);
                            if (!input.IsMy)
                            {
                                list.Add(new TrainUserExperienceOrganListOutputDto
                                {
                                    LeaderId = leaderInt,
                                    Users = users,
                                    LeaderName = user?.Name,
                                    OrganName = organ?.DisplayName,
                                    Id = organ != null ? organ.Id : 0
                                });
                            }
                            else
                            {
                                if (myLeader.Split(",").Select(x => long.Parse(x.Replace("u_", ""))).ToList().Contains(leaderInt))
                                    list.Add(new TrainUserExperienceOrganListOutputDto
                                    {
                                        LeaderId = leaderInt,
                                        Users = users,
                                        LeaderName = user?.Name,
                                        OrganName = organ?.DisplayName,
                                        Id = organ != null ? organ.Id : 0
                                    });
                            }

                            leaders.Add(leaderInt);
                        }
                        else
                        {
                            list.First(x => x.LeaderId == leaderInt).Users.Add(userId);
                        }

                    }
                }
            }
            if (!input.IsMy)
            {
                foreach (var item in list)
                {
                    var trainId = (input.ExperienceType == TrainExperienceType.Train ? input.TrainId : courseid);
                    item.List = (from a in _repository.GetAll().Where(x => !x.IsDeleted && x.IsUse && x.TrainId == trainId && x.Type == input.ExperienceType && item.Users.Contains(x.UserId))
                                 join b in _userRepository.GetAll() on a.UserId equals b.Id
                                 select new TrainUserExperienceListOutputDto()
                                 {
                                     Id = a.Id,
                                     UserId = a.UserId,
                                     UserName = b.Name,
                                     TrainId = a.TrainId,
                                     Experience = a.Experience,
                                     Approval = a.Approval,
                                     CreationTime = a.CreationTime
                                 }).ToList();
                    var tasks = _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == input.TrainId.ToString() && x.Status == 2 && !string.IsNullOrEmpty(x.Comment) && x.ReceiveID == item.LeaderId).OrderByDescending(x => x.CompletedTime1);
                    var approval = "";
                    foreach (var info in tasks.Select(x => x.Comment).ToList())
                        approval += info + "<br>";
                    item.Approval = approval;
                    var task = tasks.FirstOrDefault();
                    if (task != null)
                        item.ApprovalTime = task.CompletedTime1;
                }
            }

            var tasklist = _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == input.TrainId.ToString() && x.Status == 2 && !string.IsNullOrEmpty(x.Comment) && !leaders.Contains(x.ReceiveID)).ToList();
            foreach (var item in tasklist)
            {
                var organ = organizeManager.GetDeptByUserID(item.ReceiveID);
                if (organ != null)
                    list.Add(new TrainUserExperienceOrganListOutputDto
                    {
                        LeaderId = item.ReceiveID,
                        Approval = item.Comment,
                        ApprovalTime=item.CompletedTime1,
                        LeaderName = item.ReceiveName,
                        Id = organ.Id
                    });
            }

            return list;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<TrainUserExperienceOutputDto> Get(NullableIdDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
            var tmp = model.MapTo<TrainUserExperienceOutputDto>();
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.培训心得
            });
            return tmp;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<TrainUserExperienceOutputDto> GetIsExistence(GetTrainUserExperienceInput input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.TrainId == input.TrainId && x.Type == input.ExperienceType && x.UserId == AbpSession.UserId.Value);
            if (dbmodel == null)
                return null;
            var tmp = dbmodel.MapTo<TrainUserExperienceOutputDto>();
            tmp.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = dbmodel.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.培训心得
            });
            return tmp;
        }
        /// <summary>
        /// 添加一个TrainUserExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateTrainUserExperienceInput input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.TrainId == input.TrainId && x.Type == input.ExperienceType && x.UserId == AbpSession.UserId.Value);
            if (dbmodel != null)
            {
                dbmodel.Experience = input.Experience;
                dbmodel.CreationTime = DateTime.Now;
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
                    BusinessId = dbmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.培训心得,
                    Files = fileList
                });
                return;
            }
            var id = Guid.NewGuid();
            var newmodel = new TrainUserExperience()
            {
                Id = id,
                UserId = AbpSession.UserId.Value,
                TrainId = input.TrainId,
                IsUse = false,
                Type = input.ExperienceType,
                Experience = input.Experience
            };
            await _repository.InsertAsync(newmodel);
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
                    BusinessType = (int)AbpFileBusinessType.培训心得,
                    Files = fileList
                });
            }
        }


        /// <summary>
        /// 添加一个TrainUserExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task UpdateUse(TrainUserExperienceUseInput input)
        {
            if (input.Guids.Count > 0)
            {
                foreach (var item in input.Guids)
                {
                    var model = _repository.GetAll().FirstOrDefault(x => x.Id == item);
                    if (model != null)
                    {
                        model.IsUse = true;
                        await _repository.UpdateAsync(model);
                        var train = _trainRepository.GetAll().FirstOrDefault(x => x.Id == model.TrainId);
                        if (train != null && train.ExperienceScore > 0)
                        {
                            await _trainScoreRecordAppService.Create(new CreateUserTrainScoreRecordInput()
                            {
                                FromId = train.Id,
                                FromType = TrainScoreFromType.TrainExperience,
                                Score = train.ExperienceScore.Value,
                                UserId = model.UserId
                            });
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 修改一个TrainUserExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateTrainUserExperienceInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
                }
                if (string.IsNullOrEmpty(input.Approval))
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请填写批示。");
                dbmodel.Approval = input.Approval;
                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
    }
}