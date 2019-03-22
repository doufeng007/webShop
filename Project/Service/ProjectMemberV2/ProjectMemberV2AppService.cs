using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Abp.UI;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.Extensions;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;
using Abp.Events.Bus;

namespace Project
{
    public class ProjectMemberV2AppService : FRMSCoreAppServiceBase, IProjectMemberV2AppService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<ProjectPersentFinish, Guid> _projectPersentFinishRepository;
        private readonly IRepository<ProjectPersentFinishAllot, Guid> _projectPersentFinishAllotRepository;
        private readonly IRepository<ProjectAuditRole, int> _projectAuditRoleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProjectBudgetControl, Guid> _projectBudgetControlRepository;
        private readonly IRepository<ProjectBudgetControlAuditResult, Guid> _projectBudgetControlAuditResultRepository;
        private readonly IRepository<ProjectAuditGroup, Guid> _projectAuditGroupRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workFlowOrganizationUnitsRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        private readonly IEventBus _eventBus;


        public ProjectMemberV2AppService(IProjectBaseRepository projectBaseRepository, IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<ProjectPersentFinish, Guid> projectPersentFinishRepository,
            IRepository<ProjectPersentFinishAllot, Guid> projectPersentFinishAllotRepository, IRepository<ProjectAuditRole, int> projectAuditRoleRepository, IRepository<User, long> userRepository
           , IRepository<ProjectBudgetControl, Guid> projectBudgetRepository, IRepository<ProjectBudgetControlAuditResult, Guid> projectBudgetControlAuditResultRepository
            , IRepository<ProjectAuditGroup, Guid> projectAuditGroupRepository
            , IWorkFlowTaskRepository workFlowTaskRepository, IRepository<WorkFlowOrganizationUnits, long> workFlowOrganizationUnitsRepository
            , IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository, IEventBus eventBus)
        {
            _projectBaseRepository = projectBaseRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _projectPersentFinishRepository = projectPersentFinishRepository;
            _projectPersentFinishAllotRepository = projectPersentFinishAllotRepository;
            _projectAuditRoleRepository = projectAuditRoleRepository;
            _userRepository = userRepository;
            _projectBudgetControlAuditResultRepository = projectBudgetControlAuditResultRepository;
            _projectBudgetControlRepository = projectBudgetRepository;
            _projectAuditGroupRepository = projectAuditGroupRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowOrganizationUnitsRepository = workFlowOrganizationUnitsRepository;
            _singleProjectInfoRepository = singleProjectInfoRepository;
            _eventBus = eventBus;
        }




        /// <summary>
        /// 新增或编辑工程评审人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateProjectAuditMembersV2(CreateProjectAuditMemberV2Input input)
        {
            var baseinfo = _projectBaseRepository.Get(input.ProjectId);
            baseinfo.HasFinancialReview = input.HasFinancialReview;

            //原来的
            var oldaudit = _projectAuditMemberRepository.GetAll().Where(ite => ite.ProjectBaseId == input.ProjectId && (ite.UserAuditRole == (int)AuditRoleEnum.工程评审 || ite.UserAuditRole == (int)AuditRoleEnum.汇总人员)).ToList();
            var shouldadd = input.Members.Where(r => !r.Id.HasValue && (r.AuditRoleId == (int)AuditRoleEnum.工程评审 || r.AuditRoleId == (int)AuditRoleEnum.汇总人员)).ToList();
            var shouldupdate = input.Members.Where(r => r.Id.HasValue && (r.AuditRoleId == (int)AuditRoleEnum.工程评审 || r.AuditRoleId == (int)AuditRoleEnum.汇总人员)).ToList();
            var shoulddel_Ids = oldaudit.Select(r => r.Id).ToList().Except(shouldupdate.Select(r => r.Id.Value).ToList()).ToList();

            //删除
            shoulddel_Ids.ForEach(ite =>
            {
                var deleModel = oldaudit.FirstOrDefault(r => r.Id == ite);
                _projectAuditMemberRepository.Delete(deleModel);
                if (deleModel.UserAuditRole == (int)AuditRoleEnum.工程评审)
                {
                    var finishsAllots = _projectPersentFinishAllotRepository.GetAll().Where(r => r.AuditMembeId == deleModel.Id);
                    if (finishsAllots.Count() > 0)
                    {
                        finishsAllots.ForEachAsync(r =>
                        {
                            _projectPersentFinishAllotRepository.Delete(r);
                            _projectPersentFinishRepository.Delete(r.FinishId);
                        });
                    }

                }

            });

            //更新
            shouldupdate.ForEach(ite =>
            {
                var a = oldaudit.First(it => it.Id == ite.Id);
                //a.FinishItems = ite.FinishItems;
                a.WorkDays = ite.WorkDays;
                a.WorkDes = ite.WorkDes;
                a.Percentes = ite.Percentes;
                a.Des = ite.Des;
                _projectAuditMemberRepository.Update(a);
            });
            //新增
            shouldadd.ForEach(ite =>
            {
                var newMember = new ProjectAuditMember()
                {
                    Id = Guid.NewGuid(),
                    UserAuditRole = ite.AuditRoleId,
                    ProjectBaseId = input.ProjectId,
                    UserId = ite.UserId,
                    WorkDays = ite.WorkDays,
                    WorkDes = ite.WorkDes,
                    Percentes = ite.Percentes,
                    Des = ite.Des
                };
                _projectAuditMemberRepository.Insert(newMember);
                if (ite.AuditRoleId == (int)AuditRoleEnum.工程评审)
                {
                    var finishId = Guid.NewGuid();
                    var finishModel = new ProjectPersentFinish() { Id = finishId, ProjectId = input.ProjectId };
                    var finishAllotModel = new ProjectPersentFinishAllot() { Id = Guid.NewGuid(), FinishId = finishId, AuditMembeId = newMember.Id, ProjectId = input.ProjectId, IsMain = true };
                    _projectPersentFinishRepository.Insert(finishModel);
                    _projectPersentFinishAllotRepository.Insert(finishAllotModel);
                }
            });



            #region  对财务评审人员的更新
            var inputgroupMemberList = new List<CreateOrUpdateProjectAuditMembersInput>();
            if (input.HasFinancialReview)
            {
                if (input.OrgFinancial1 == 0 || input.OrgFinancial2 == 0)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "财务评审人员未指定");
                var orgmodel = _workFlowOrganizationUnitsRepository.Get(input.OrgFinancial1);
                if (orgmodel.ChargeLeader.IsNullOrWhiteSpace())
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "财务初审部门未指定分管领导");
                var chargeleader = MemberPerfix.RemovePrefix(orgmodel.ChargeLeader).ToLong();
                var exit_Financial1Member = await _projectAuditMemberRepository.FirstOrDefaultAsync(r => r.ProjectBaseId == input.ProjectId && r.UserAuditRole == (int)AuditRoleEnum.财务初审);
                if (exit_Financial1Member != null)
                {
                    exit_Financial1Member.FlowId = input.OrgFinancial1.ToString();
                    exit_Financial1Member.UserId = chargeleader;
                }
                else
                {
                    var entity = new CreateOrUpdateProjectAuditMembersInput()
                    {
                        UserAuditRole = (int)AuditRoleEnum.财务初审,
                        UserId = chargeleader,
                        FlowId = input.OrgFinancial1.ToString(),
                        ProjectBaseId = input.ProjectId
                    };
                    inputgroupMemberList.Add(entity);
                }



                var orgmodel2 = _workFlowOrganizationUnitsRepository.Get(input.OrgFinancial2);
                if (orgmodel2.ChargeLeader.IsNullOrWhiteSpace())
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "财务终审部门未指定分管领导");
                var chargeleader2 = MemberPerfix.RemovePrefix(orgmodel2.ChargeLeader).ToLong();
                var exit_Financial2Member = await _projectAuditMemberRepository.FirstOrDefaultAsync(r => r.ProjectBaseId == input.ProjectId && r.UserAuditRole == (int)AuditRoleEnum.财务评审);
                if (exit_Financial2Member != null)
                {
                    exit_Financial2Member.FlowId = input.OrgFinancial2.ToString();
                    exit_Financial2Member.UserId = chargeleader2;
                }
                else
                {
                    var entity = new CreateOrUpdateProjectAuditMembersInput()
                    {
                        UserAuditRole = (int)AuditRoleEnum.财务评审,
                        UserId = chargeleader2,
                        FlowId = input.OrgFinancial2.ToString(),
                        ProjectBaseId = input.ProjectId
                    };
                    inputgroupMemberList.Add(entity);
                }

            }
            else
            {
                if (await _projectAuditMemberRepository.GetAll().AnyAsync(r => r.ProjectBaseId == input.ProjectId && r.UserAuditRole == (int)AuditRoleEnum.财务初审))
                    await _projectAuditMemberRepository.DeleteAsync(r => r.ProjectBaseId == input.ProjectId && r.UserAuditRole == (int)AuditRoleEnum.财务初审);
                if (await _projectAuditMemberRepository.GetAll().AnyAsync(r => r.ProjectBaseId == input.ProjectId && r.UserAuditRole == (int)AuditRoleEnum.财务评审))
                    await _projectAuditMemberRepository.DeleteAsync(r => r.ProjectBaseId == input.ProjectId && r.UserAuditRole == (int)AuditRoleEnum.财务评审);
            }
            foreach (var item in inputgroupMemberList)
            {
                _projectAuditMemberRepository.Insert(item.MapTo<ProjectAuditMember>());
            }

            #endregion
        }



        /// <summary>
        /// 新增或编辑评审组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateAuditGroupAndFinancial(CreateOrUpdateAuditGroupAndFinancialInput input)
        {
            var groupService =
                    Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                        .IocManager.IocContainer.Resolve<IProjectAuditGroupAppService>();
            var groupModel =
                await groupService.GetProjectAuditGroupForEdit(new NullableIdDto<Guid>() { Id = input.GroupId });
            if (groupModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "选择的评审组数据异常");
            var inputgroupMemberList = new List<CreateOrUpdateProjectAuditMembersInput>();
            var exit_groupMembers = await _projectAuditMemberRepository.GetAll().Where(r => r.ProjectBaseId == input.ProjectBaseId && r.GroupId.HasValue).ToListAsync();
            if (exit_groupMembers.Count() > 0)
            {
                #region 编辑评审组人员
                var groupUserCharge = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.项目负责人);
                if (groupUserCharge != null)
                {
                    var exit_MemberFZR = exit_groupMembers.FirstOrDefault(r => r.UserAuditRole == (int)AuditRoleEnum.项目负责人);
                    if (exit_MemberFZR == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取项目负责人失败");
                    exit_MemberFZR.UserId = groupUserCharge.UserId;
                    exit_MemberFZR.GroupId = groupUserCharge.GroupId;

                    var exit_MemberFH2 = exit_groupMembers.FirstOrDefault(r => r.UserAuditRole == (int)AuditRoleEnum.复核人三);
                    if (exit_MemberFH2 == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取三级复核人失败");
                    exit_MemberFH2.UserId = groupUserCharge.UserId;
                    exit_MemberFH2.GroupId = groupUserCharge.GroupId;
                }
                var groupUserContact1 = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.联系人一);
                if (groupUserContact1 != null)
                {
                    var exit_MemberLXR1 = exit_groupMembers.FirstOrDefault(r => r.UserAuditRole == (int)AuditRoleEnum.联系人一);
                    if (exit_MemberLXR1 == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取联系人一失败");
                    exit_MemberLXR1.UserId = groupUserContact1.UserId;
                    exit_MemberLXR1.GroupId = groupUserContact1.GroupId;

                }

                var groupUserContact2 = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.联系人二);
                if (groupUserContact2 != null)
                {
                    var exit_MemberLXR2 = exit_groupMembers.FirstOrDefault(r => r.UserAuditRole == (int)AuditRoleEnum.联系人二);
                    if (exit_MemberLXR2 == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取联系人二失败");
                    exit_MemberLXR2.UserId = groupUserContact2.UserId;
                    exit_MemberLXR2.GroupId = groupUserContact2.GroupId;

                }
                #endregion
            }
            else
            {
                #region 新增评审组人员
                var groupUserCharge = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.项目负责人);
                if (groupUserCharge != null)
                {
                    var entity = new CreateOrUpdateProjectAuditMembersInput()
                    {
                        UserAuditRole = 1,
                        IsGroup = true,
                        UserId = groupUserCharge.UserId,
                        GroupId = groupUserCharge.GroupId,
                        ProjectBaseId = input.ProjectBaseId
                    };
                    var entityFH2 = new CreateOrUpdateProjectAuditMembersInput()
                    {
                        UserAuditRole = (int)AuditRoleEnum.复核人三,
                        IsGroup = true,
                        UserId = groupUserCharge.UserId,
                        GroupId = groupUserCharge.GroupId,
                        ProjectBaseId = input.ProjectBaseId
                    };
                    inputgroupMemberList.Add(entity);
                    inputgroupMemberList.Add(entityFH2);
                }
                var groupUserContact1 = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.联系人一);
                if (groupUserContact1 != null)
                {
                    var entity = new CreateOrUpdateProjectAuditMembersInput()
                    {
                        UserAuditRole = (int)AuditRoleEnum.联系人一,
                        IsGroup = true,
                        UserId = groupUserContact1.UserId,
                        GroupId = groupUserContact1.GroupId,
                        ProjectBaseId = input.ProjectBaseId
                    };
                    inputgroupMemberList.Add(entity);
                }

                var groupUserContact2 = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.联系人二);
                if (groupUserContact2 != null)
                {
                    var entity = new CreateOrUpdateProjectAuditMembersInput()
                    {
                        UserAuditRole = (int)AuditRoleEnum.联系人二,
                        IsGroup = true,
                        UserId = groupUserContact2.UserId,
                        GroupId = groupUserContact2.GroupId,
                        ProjectBaseId = input.ProjectBaseId
                    };
                    inputgroupMemberList.Add(entity);
                }
                #endregion
            }
            foreach (var item in inputgroupMemberList)
            {
                _projectAuditMemberRepository.Insert(item.MapTo<ProjectAuditMember>());
            }
            var info = _singleProjectInfoRepository.Get(input.ProjectBaseId);
            _eventBus.Trigger(new ProjectLeaderChange()
            {
                ProjectId = input.ProjectBaseId,
                UserId = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.项目负责人).UserId
            });
            info.GroupId = input.GroupId;
        }
        /// <summary>
        /// 分派或设置评审部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SetDepartmentId(UpdateProjectDepartmentInput input)
        {
            var project = await _projectBaseRepository.GetAsync(input.ProjectId);
            var singleProjectModels = await _singleProjectInfoRepository.GetAll().Where(r => r.ProjectId == project.Id).ToListAsync();

            if (input.Single.Select(r => r.SingleProjectId).Distinct().Count() != input.Single.Count() || input.Single.Count != singleProjectModels.Count)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "需要指定所有单项的评审部门");
            foreach (var item in singleProjectModels)
            {
                var entity = input.Single.SingleOrDefault(r => r.SingleProjectId == item.Id);
                item.DeparmentId = "l_" + entity.OrgId;
                await _singleProjectInfoRepository.UpdateAsync(item);
            }

        }

        [RemoteService(IsEnabled = false)]
        public string GetPorjectDepartmentLeaders(Guid projectId)
        {
            var query = from a in _singleProjectInfoRepository.GetAll()
                        join b in _projectBaseRepository.GetAll() on a.ProjectId equals b.Id
                        where b.Id == projectId
                        select new { a.Id, a.DeparmentId };
            var data = query.ToList();
            if (data.Count == 0)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该项目不存在单项信息");
            if (data.Any(r => r.DeparmentId.IsNullOrEmpty()))
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "单项未指定评审部门");
            return string.Join(",", data.Select(r => r.DeparmentId));

        }


    }
}
