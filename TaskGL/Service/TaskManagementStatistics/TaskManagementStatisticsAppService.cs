using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.Reflection.Extensions;
using Abp.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project;
using TaskGL.Enum;
using TaskGL.Service.TaskManagementStatistics.Dto;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Extensions;

namespace TaskGL.Service.TaskManagementStatistics
{
    public class TaskManagementStatisticsAppService : FRMSCoreAppServiceBase, ITaskManagementStatisticsAppService
    {
        private readonly IRepository<TaskManagement, Guid> _taskRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<ProjectAuditGroup, Guid> _projectAuditGroupRepository;
        private readonly IRepository<ProjectAuditGroupUser, Guid> _projectAuditGroupUserRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<Follow, Guid> _followRepository;
        private readonly IRepository<User, long> _usersRepository;
        public Guid Flowid;

        public TaskManagementStatisticsAppService(
            IRepository<TaskManagement, Guid> taskRepository,
            IWorkFlowTaskRepository workFlowTaskRepository
            , IRepository<ProjectAuditGroup, Guid> projectAuditGroupRepository
            , IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository
            , IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository
            , IRepository<OrganizationUnit, long> organizationUnitRepository
            , IRepository<Follow, Guid> followRepository
            , IRepository<ProjectAuditGroupUser, Guid> projectAuditGroupUserRepository,
            IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository,
            IRepository<User, long> usersRepository
        )
        {
            _taskRepository = taskRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _appConfiguration = AppConfigurations.Get(Directory.GetCurrentDirectory());
            _projectAuditGroupRepository = projectAuditGroupRepository;
            _singleProjectInfoRepository = singleProjectInfoRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _followRepository = followRepository;
            _projectAuditGroupUserRepository = projectAuditGroupUserRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _usersRepository = usersRepository;
            Flowid = Guid.Parse(_appConfiguration["App:TaskManagementFlowId"] ??
                                "ed931ed4-242d-4358-8572-2b3bdb76f9bd");
        }

        /// <summary>
        /// 获取日常任务的统计
        /// </summary>
        /// <returns></returns>
        public async Task<TaskStatisticResponse> GetTaskStatistic()
        {
            var doneStatusList = new List<TaskManagementStateEnum>
            {
                TaskManagementStateEnum.Redo,
                TaskManagementStateEnum.Reject,
                TaskManagementStateEnum.Revoke,
                TaskManagementStateEnum.Done
            };
            var unStartStatusList = new List<TaskManagementStateEnum>
            {
                TaskManagementStateEnum.BefreStart,
                TaskManagementStateEnum.Approvaling,
            };
            var dbresult = (await (from a in _taskRepository.GetAll()
                    .Where(x => !x.IsDeleted && x.Type == TaskManagementTypeEnum.Offline)
                                   group a by a.TaskStatus
                into g
                select g).ToListAsync());
            var result = new TaskStatisticResponse
            {
                DoingCount = dbresult.FirstOrDefault(x => x.Key == TaskManagementStateEnum.Doing)?.Count() ?? 0
            }; 
            dbresult.Where(x => doneStatusList.Contains(x.Key)).ToList().ForEach(x => { result.DoneCount += x.Count(); });
            dbresult.Where(x => unStartStatusList.Contains(x.Key)).ToList().ForEach(x => { result.TodoCount += x.Count(); });
            result.HandleRate = (result.TodoCount + result.DoingCount + result.DoneCount) == 0 ? 0 :
                Math.Round(
                    Convert.ToDecimal(result.DoneCount) /
                    Convert.ToDecimal(result.TodoCount + result.DoingCount + result.DoneCount), 2);
            return result;
        }

        /// <summary>
        /// 获取与我相关的统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<MyTaskStatisticDetailResponse>> GetMyTaskStatistic()
        {
            //获取我的ID
            var myid = AbpSession.UserId;
            //获取我所在部门下面所有人的ID
            var myOrganizationUserid = _userOrganizationUnitRepository.GetAll().Where(x =>
                _userOrganizationUnitRepository.GetAll().Where(y => y.UserId == myid)
                    .Select(y => y.OrganizationUnitId).Contains(x.OrganizationUnitId)).Select(x => x.UserId);
            //查询关注ID
            var flowid = _followRepository.GetAll()
                .Where(x => x.BusinessType == FollowType.任务管理 && x.CreatorUserId == myid)
                .Select(x => x.BusinessId);
            //根据以上筛选条件查询任务管理记录
            var taskResult = await (from a in _taskRepository.GetAll().Where(x =>
                    x.UserId == myid || x.Superintendent == myid || myOrganizationUserid.Contains(x.UserId.Value) ||
                    flowid.Contains(x.Id))
                select new
                {
                    a.Id,
                    a.UserId,
                    a.Superintendent,
                    a.TaskStatus
                }).ToListAsync();
            //返回任务管理统计信息
            var result = new List<MyTaskStatisticDetailResponse>
            {
                MyTaskFactory(TaskStatisticDetailTypeEnum.MyTask,
                    taskResult.Where(x => x.UserId == myid).Select(x => x.TaskStatus).ToList()),
                MyTaskFactory(TaskStatisticDetailTypeEnum.MySuperintendentTask,
                    taskResult.Where(x => x.Superintendent == myid).Select(x => x.TaskStatus).ToList()),
                MyTaskFactory(TaskStatisticDetailTypeEnum.MyOrganizationTask,
                    taskResult.Where(x => myOrganizationUserid.Contains(x.UserId.Value)).Select(x => x.TaskStatus)
                        .ToList()),
                //TODO:项目管理和任务管理关联后再补齐统计信息
                MyTaskFactory(TaskStatisticDetailTypeEnum.MyProjectTask, new List<TaskManagementStateEnum>()),
                MyTaskFactory(TaskStatisticDetailTypeEnum.MyFollowTask,
                    taskResult.Where(x => flowid.Contains(x.Id)).Select(x => x.TaskStatus).ToList())
            };
            return result;
        }

        /// <summary>
        /// 获取统计页面的下拉菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<dynamic>> GetStatisticDroplist()
        {
            var result = new List<dynamic>();
            var myid = AbpSession.UserId;
            var org= (await (from a in _userOrganizationUnitRepository.GetAll().Where(y => y.UserId == myid)
                             join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                    select new
                    {
                        b.Id,
                        b.DisplayName
                    }).Distinct().ToListAsync());
            org.ForEach(x =>
            {
                dynamic item = new ExpandoObject();
                item.SearchCode = TaskStatisticsDropTypeEnum.Organization;
                item.SearchId = x.Id.ToString();
                item.SearchName = x.DisplayName;
                result.Add(item);
            });
            return result;
        }

        /// <summary>
        /// 人员统计接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TaskUserStatisticsResponse>> GetTaskUserStatistics(TaskUserStatisticsRequest input)
        {
            input = await InitSearch(input);
            var now = DateTime.Now;
            var query = from a in _usersRepository.GetAll()
                    .Where(x => input.OrgUser == null || input.OrgUser.Contains(x.Id))
                let b = (from b in _taskRepository.GetAll().Where(x =>
                        (input.StartTime == null || x.CreationTime >= input.StartTime)
                        && (input.EndTime == null || x.CreationTime < input.EndTime)
                        && x.UserId == a.Id) select new{b.TaskStatus ,b.EndTime,b.IsUrgent })
                    select new TaskUserStatisticsResponse
                    {
                        UserName = a.Name,
                        All = b.Count(),
                        Doing = b.Count(x => x.TaskStatus == TaskManagementStateEnum.Doing),
                        Done = b.Count(x => x.TaskStatus == TaskManagementStateEnum.Done),
                        BeOverdue = b.Count(x =>
                            x.EndTime < now && (x.TaskStatus == TaskManagementStateEnum.Approvaling ||
                                                x.TaskStatus == TaskManagementStateEnum.BefreStart ||
                                                x.TaskStatus == TaskManagementStateEnum.Doing)),
                        Urgent = b.Count(x => x.IsUrgent == true)
                    };
                  var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.UserName).PageBy(input).ToListAsync();
            return new PagedResultDto<TaskUserStatisticsResponse>(toalCount, ret);
        }

        /// <summary>
        /// 任务类型统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetTaskTypeStatistics(TaskUserStatisticsRequest input)
        {
            var result = new List<dynamic>();
            input = await InitSearch(input);
            var query = await (from a in _taskRepository.GetAll().Where(x =>
                    (input.StartTime == null || x.CreationTime >= input.StartTime)
                    && (input.EndTime == null || x.CreationTime < input.EndTime)
                    && (input.OrgUser == null || input.OrgUser.Contains(x.UserId.Value))
                    && x.Type != null)
                group a by a.Type
                into g
                select new
                {
                    Type = g.Key,
                    Count = g.Count()
                }).ToListAsync();
            query.ForEach(x =>
            {
                dynamic item = new ExpandoObject();
                item.Name = x.Type.GetLocalizedDescription();
                item.Vaule = x.Count;
                result.Add(item);
            });
            return result;
        }

        #region 私有方法
        /// <summary>
        /// 构造与我相关dto工厂
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stateList"></param>
        /// <returns></returns>
        private MyTaskStatisticDetailResponse MyTaskFactory(TaskStatisticDetailTypeEnum type,
            List<TaskManagementStateEnum> stateList)
        {
            return new MyTaskStatisticDetailResponse()
            {
                TaskType = type,
                All = stateList.Count,
                Approvaling = stateList.Count(x => x == TaskManagementStateEnum.Approvaling),
                Reject = stateList.Count(x => x == TaskManagementStateEnum.Reject),
                BefreStart = stateList.Count(x => x == TaskManagementStateEnum.BefreStart),
                Doing = stateList.Count(x => x == TaskManagementStateEnum.Doing),
                Done = stateList.Count(x => x == TaskManagementStateEnum.Done),
                Redo = stateList.Count(x => x == TaskManagementStateEnum.Redo),
                Revoke = stateList.Count(x => x == TaskManagementStateEnum.Revoke),
            };
        }

        /// <summary>
        /// 初始化人员统计/类型统计查询dto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<TaskUserStatisticsRequest> InitSearch(TaskUserStatisticsRequest input)
        {
            var myid = AbpSession.UserId;
            if (input.EndTime != null)
            {
                input.EndTime = input.EndTime.Value.AddDays(1);
            }
            input.OrgUser = null;
            if (input.SearchCode == TaskStatisticsDropTypeEnum.Organization)
            {
                long orgid = 0;
                if (!string.IsNullOrEmpty(input.SearchId))
                {
                    long.TryParse(input.SearchId, out orgid);
                }
                input.OrgUser = await _userOrganizationUnitRepository.GetAll().Where(x =>
                        _userOrganizationUnitRepository.GetAll().Where(y => y.UserId == myid)
                            .Select(y => y.OrganizationUnitId).Contains(x.OrganizationUnitId) &&
                        (string.IsNullOrEmpty(input.SearchId) || x.OrganizationUnitId == orgid)
                    ).Select(x => x.UserId)
                    .Distinct().ToListAsync();
            }
            return input;
        }
        #endregion
    }
}
