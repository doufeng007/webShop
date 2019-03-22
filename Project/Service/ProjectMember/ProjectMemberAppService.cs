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

namespace Project
{
    public class ProjectMemberAppService : FRMSCoreAppServiceBase, IProjectMemberAppService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectRepository;
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
        private readonly IBackgroudWorkJobWithHangFire _backgroudWorkJobWithHangFire;

        public ProjectMemberAppService(IProjectBaseRepository projectBaseRepository, IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<ProjectPersentFinish, Guid> projectPersentFinishRepository,
            IRepository<ProjectPersentFinishAllot, Guid> projectPersentFinishAllotRepository, IRepository<ProjectAuditRole, int> projectAuditRoleRepository, IRepository<User, long> userRepository
           , IRepository<ProjectBudgetControl, Guid> projectBudgetRepository, IRepository<ProjectBudgetControlAuditResult, Guid> projectBudgetControlAuditResultRepository
            , IRepository<ProjectAuditGroup, Guid> projectAuditGroupRepository
            , IWorkFlowTaskRepository workFlowTaskRepository, IRepository<WorkFlowOrganizationUnits, long> workFlowOrganizationUnitsRepository
            , IBackgroudWorkJobWithHangFire backgroudWorkJobWithHangFire, IRepository<SingleProjectInfo, Guid> singleProjectRepository)
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
            _backgroudWorkJobWithHangFire = backgroudWorkJobWithHangFire;
            _singleProjectRepository = singleProjectRepository;
        }


        public async Task UpdateProjectAuditMembers(ManagerProjectAuditMembersInput input)
        {
            var baseinfo = _projectBaseRepository.Get(input.ProjectId);
            var inputgroupMemberList = new List<CreateOrUpdateProjectAuditMembersInput>();
            var inputgroup = input.ProjectAuditMembersInput.FirstOrDefault(r => r.IsGroup);
            if (inputgroup != null)
            {
                var groupService =
                    Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                        .IocManager.IocContainer.Resolve<IProjectAuditGroupAppService>();
                var groupModel =
                    await groupService.GetProjectAuditGroupForEdit(new NullableIdDto<Guid>() { Id = inputgroup.GroupId });
                if (groupModel != null)
                {
                    var groupUserCharge = groupModel.Users.FirstOrDefault(r => r.UserRole == (int)ProjectAuditGroupRoleEnum.项目负责人);
                    if (groupUserCharge != null)
                    {
                        var entity = new CreateOrUpdateProjectAuditMembersInput()
                        {
                            UserAuditRole = 1,
                            IsGroup = true,
                            UserId = groupUserCharge.UserId,
                            GroupId = groupUserCharge.GroupId,
                            ProjectBaseId = input.ProjectId
                        };
                        var entityFH2 = new CreateOrUpdateProjectAuditMembersInput()
                        {
                            UserAuditRole = (int)AuditRoleEnum.复核人三,
                            IsGroup = true,
                            UserId = groupUserCharge.UserId,
                            GroupId = groupUserCharge.GroupId,
                            ProjectBaseId = input.ProjectId
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
                            ProjectBaseId = input.ProjectId
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
                            ProjectBaseId = input.ProjectId
                        };
                        inputgroupMemberList.Add(entity);
                    }
                    input.ProjectAuditMembersInput.Remove(inputgroup);
                    input.ProjectAuditMembersInput.AddRange(inputgroupMemberList);

                }

            }


            //原来的
            var oldaudit = _projectAuditMemberRepository.GetAll().Where(ite => ite.ProjectBaseId == input.ProjectId).ToList();

            var shouldadd = input.ProjectAuditMembersInput.Where(r => !r.Id.HasValue).ToList();
            var shouldupdate = input.ProjectAuditMembersInput.Where(r => r.Id.HasValue).ToList();
            var shoulddel_Ids = oldaudit.Select(r => r.Id).ToList().Except(shouldupdate.Select(r => r.Id.Value).ToList()).ToList();

            //删除
            shoulddel_Ids.ForEach(ite =>
            {
                var deleModel = oldaudit.FirstOrDefault(r => r.Id == ite);
                _projectAuditMemberRepository.Delete(deleModel);
                if (deleModel.UserAuditRole == (int)AuditRoleEnum.工程评审 || deleModel.UserAuditRole == (int)AuditRoleEnum.财务评审)
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
                a.AappraisalFileTypes = ite.AappraisalFileTypes;
                //a.FinishItems = ite.FinishItems;
                a.WorkDays = ite.WorkDays;
                a.WorkDes = ite.WorkDes;
                a.GroupId = ite.GroupId;
                if (a.UserAuditRole == (int)AuditRoleEnum.财务初审)   ///财务初审 通过orgid 找到部门的分管领导的userid； orgidxieru flowid
                {
                    var orgmodel = _workFlowOrganizationUnitsRepository.Get(ite.UserId);
                    if (orgmodel.ChargeLeader.IsNullOrWhiteSpace())
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "财务初审部门未指定分管领导");
                    var chargeleader = MemberPerfix.RemovePrefix(orgmodel.ChargeLeader).ToLong();
                    ite.UserId = chargeleader;
                    ite.FlowId = orgmodel.Id.ToString();
                }
                _projectAuditMemberRepository.Update(a);
                //if (string.IsNullOrWhiteSpace(ite.FinishItems) == false)
                //{
                //    //var ids = ite.FinishItems.Split(',');
                //    var ids = Array.ConvertAll(ite.FinishItems.Split(','), new Converter<string, Guid>(Guid.Parse));
                //    var items = _projectPersentFinishRepository.GetAll().Where(it => ids.Contains(it.Id)).ToList();
                //    var allpersent = items.Sum(it => it.Persent);
                //    foreach (var id in ids)
                //    {

                //        var finishitem = _projectPersentFinishRepository.Get(id);
                //        if (finishitem != null && finishitem.Name == "熟悉图纸")
                //        {
                //            double day = (double)(finishitem.Persent / allpersent) * ite.WorkDays.Value;
                //            _projectNoticeRepository.Insert(new ProjectNotice()
                //            {
                //                Content = "",
                //                FinishTime = DateTime.Now.AddDays(day),
                //                FinishId = id,
                //                ProjectId = input.BaseId.Value,
                //                UserId = ite.UserId
                //            });
                //            _backgroundWorkJobProjectPersentFinish.CreateJobForProjectPersentFinishSubmit(
                //                    input.BaseId.Value, id);
                //        }

                //    }
                //}
            });
            //新增
            shouldadd.ForEach(ite =>
            {
                ite.Id = Guid.NewGuid();
                ite.ProjectBaseId = input.ProjectId;
                if (ite.UserAuditRole == (int)AuditRoleEnum.工程评审)
                {
                    var finishId = Guid.NewGuid();
                    var finishModel = new ProjectPersentFinish() { Id = finishId, ProjectId = input.ProjectId };
                    var finishAllotModel = new ProjectPersentFinishAllot() { Id = Guid.NewGuid(), FinishId = finishId, AuditMembeId = ite.Id.Value, ProjectId = input.ProjectId, IsMain = true };
                    _projectPersentFinishRepository.Insert(finishModel);
                    _projectPersentFinishAllotRepository.Insert(finishAllotModel);
                }
                else if (ite.UserAuditRole == (int)AuditRoleEnum.财务初审)   ///财务初审 通过orgid 找到部门的分管领导的userid； orgidxieru flowid
                {
                    var orgmodel = _workFlowOrganizationUnitsRepository.Get(ite.UserId);
                    if (orgmodel.ChargeLeader.IsNullOrWhiteSpace())
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "财务初审部门未指定分管领导");
                    var chargeleader = MemberPerfix.RemovePrefix(orgmodel.ChargeLeader).ToLong();
                    ite.UserId = chargeleader;
                    ite.FlowId = orgmodel.Id.ToString();
                }
                _projectAuditMemberRepository.Insert(ite.MapTo<ProjectAuditMember>());
            });

            if (input.ProjectAuditMembersInput.Any(r => r.UserAuditRole == (int)AuditRoleEnum.财务评审))
            {
                baseinfo.HasFinancialReview = true;
                //await _projectBaseRepository.UpdateAsync(baseinfo);
            }
            //baseinfo.ProjectStatus = ProjectStatus.在审;
        }


        public async Task UpdateFinishAsync(ManagerProjectAuditMembersInput input)
        {
            //原来的
            var oldaudit = _projectAuditMemberRepository.GetAll().Where(ite => ite.ProjectBaseId == input.ProjectId && ite.UserAuditRole == (int)AuditRoleEnum.工程评审).ToList();
            //更新
            input.ProjectAuditMembersInput.ForEach(ite =>
            {
                var a = oldaudit.First(it => it.Id == ite.Id);
                a.AappraisalFileTypes = ite.AappraisalFileTypes;
                //a.FinishItems = ite.FinishItems;
                a.WorkDays = ite.WorkDays;
                a.WorkDes = ite.WorkDes;
                a.GroupId = ite.GroupId;
                _projectAuditMemberRepository.Update(a);
            });
        }

        public async Task UpdateProjectSpecifyReviewAsync(CreateProjectSpecifyReviewInput input)
        {
            var add_members = input.Reviews.Where(r => !r.Id.HasValue);
            var _singleProjectModel = await _singleProjectRepository.GetAsync(input.ProjectBaseId);
            _singleProjectModel.AuditAmount = input.AuditAmount;
            await _singleProjectRepository.UpdateAsync(_singleProjectModel);
            //var reviewerids = new List<int> { 6, 7, 8 };
            var reviewerids = new List<int> { 7 };
            var projectAuditMemberModel = await _projectAuditMemberRepository.GetAll().Where(r => r.ProjectBaseId == input.ProjectBaseId && reviewerids.Contains(r.UserAuditRole)).ToListAsync();
            var add_AuditMemberModel = input.Reviews.Where(r => r.Id == null).ToList();
            add_AuditMemberModel.ForEach(r => { _projectAuditMemberRepository.InsertAsync(r.MapTo<ProjectAuditMember>()); });
            var less_projectAuditMemberIds = projectAuditMemberModel.Select(r => r.Id).ToList().Except(input.Reviews.Where(r => r.Id.HasValue).Select(o => o.Id.Value).ToList()).ToList();
            less_projectAuditMemberIds.ForEach(r => { _projectAuditMemberRepository.Delete(projectAuditMemberModel.FirstOrDefault(o => o.Id == r)); });
            var update_projectAuditMembers = input.Reviews.Where(r => r.Id != null && !less_projectAuditMemberIds.Contains(r.Id.Value)).ToList();
            update_projectAuditMembers.ForEach(r =>
            {
                var db_projectAuditMember = projectAuditMemberModel.FirstOrDefault(o => o.Id == r.Id.Value);
                r.MapTo(db_projectAuditMember);
                _projectAuditMemberRepository.UpdateAsync(db_projectAuditMember);
            });
        }


        [AbpAuthorize]
        public async Task<GetProjectMemberForEditOutput> GetAsync(EntityDto<Guid> input)
        {
            var ret = new GetProjectMemberForEditOutput();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                            .IocManager.IocContainer.Resolve<IProjectAppService>();
            var projectBase = await service.GetSingleProject(new GetSingleProjectInput() { AppraisalTypeId = 8, Id = input.Id, });
            var membsers_query = from memeber in _projectAuditMemberRepository.GetAll()
                                 join auditRole in _projectAuditRoleRepository.GetAll() on memeber.UserAuditRole equals auditRole.Id
                                 join user in _userRepository.GetAll() on memeber.UserId equals user.Id
                                 join membergroups in _projectAuditGroupRepository.GetAll() on memeber.GroupId equals membergroups.Id into g
                                 from membergroup in g.DefaultIfEmpty()
                                 where memeber.ProjectBaseId == input.Id
                                 select new CreateOrUpdateAuditMemberOutput() { AuditRoleId = memeber.UserAuditRole, AuditRoleName = auditRole.Name, Id = memeber.Id, UserId = user.Id, UserName = user.Name, GroupId = memeber.GroupId, GroupName = membergroup.Name, WorkDes = memeber.WorkDes, WorkDays = memeber.WorkDays, FlowId = memeber.FlowId, Des = memeber.Des, Percentes = memeber.Percentes };
            ret.ProjectId = projectBase.BaseOutput.Id.Value;
            ret.SingleProjectId = input.Id;
            ret.ProjectInfo = projectBase;
            ret.Members = await membsers_query.ToListAsync();
            foreach (var item in ret.Members)
            {
                //财务初审的部门id 从flowId里面来
                if (item.AuditRoleId == (int)AuditRoleEnum.财务初审)
                {
                    item.UserId = item.FlowId.ToLong();
                }

            }

            var finishs_query = from finish in _projectPersentFinishRepository.GetAll()
                                join allot in _projectPersentFinishAllotRepository.GetAll() on finish.Id equals allot.FinishId into g
                                where finish.ProjectId == input.Id
                                select new
                                {
                                    F = finish,
                                    A = from a in g
                                        join memeber in _projectAuditMemberRepository.GetAll() on a.AuditMembeId equals memeber.Id
                                        join user in _userRepository.GetAll() on memeber.UserId equals user.Id
                                        select new { UserId = user.Id, UserName = user.Name, IsMain = a.IsMain }
                                };
            foreach (var item in finishs_query)
            {
                var entity = new CreateOrUpdateFinishOutput();
                entity.Id = item.F.Id;
                entity.Name = item.F.Name;
                entity.Persent = item.F.Persent;
                entity.WorkDay = item.F.WorkDay;
                foreach (var obj in item.A)
                {
                    var entity_user = new CreateOrUpdateFinishAllotOutput();
                    entity_user.UserId = obj.UserId;
                    entity_user.UserName = obj.UserName;
                    entity_user.IsMain = obj.IsMain;
                    entity.FinishMembers.Add(entity_user);
                }
                ret.Finishs.Add(entity);
            }

            return ret;

        }

        [AbpAuthorize]
        public async Task<List<ProjectControlAuditResultOutput>> GetProjectControlAsync(GetProjectControlAuditResultInput input)
        {
            var query = from a in _projectBudgetControlRepository.GetAll()
                        join b in _projectBudgetControlAuditResultRepository.GetAll() on a.Id equals b.ControlId into g
                        where a.Pro_Id == input.ProjectId
                        select new
                        {
                            a,
                            result = from r in g
                                     where r.UserId == AbpSession.UserId.Value && r.UserAuditRoleId == input.UserAuditRoleId
                                     select r
                        };
            var ret = new List<ProjectControlAuditResultOutput>();
            var data = await query.ToListAsync();
            foreach (var item in data)
            {
                var entity = new ProjectControlAuditResultOutput();
                entity.ApprovalMoney = item.a.ApprovalMoney;
                entity.Code = item.a.Code;
                entity.CodeName = item.a.CodeName;
                entity.ControlId = item.a.Id;
                entity.Name = item.a.Name;
                entity.SendMoney = item.a.SendMoney;
                if (item.result.Count() > 0)
                {
                    entity.ControlResultId = item.result.FirstOrDefault().Id;
                    entity.ValidationMoney = item.result.FirstOrDefault().ValidationMoney;
                }
                ret.Add(entity);
            }

            return ret;
        }

        [AbpAuthorize]
        public async Task UpdateProjectControlAuditResult(UpdateProjectControlAuditResultInput input)
        {
            input.UserId = AbpSession.UserId.Value;
            var query = _projectBudgetControlRepository.GetAll().Where(r => r.Pro_Id == input.ProjectId);
            foreach (var projectBudgetControl in query)
            {
                var item = input.ControlRsults.FirstOrDefault(r => r.ControlId == projectBudgetControl.Id);
                if (item != null)
                {
                    var entity = new ProjectBudgetControlAuditResult();
                    if (item.ControlResultId.HasValue)
                        entity.Id = item.ControlResultId.Value;

                    //entity.Id = item.ControlResultId.Value;
                    entity.ControlId = item.ControlId;
                    entity.UserId = input.UserId;
                    entity.UserAuditRoleId = input.UserAuditRoleId;
                    entity.Pro_Id = input.ProjectId;
                    entity.ValidationMoney = item.ValidationMoney;
                    await _projectBudgetControlAuditResultRepository.InsertOrUpdateAsync(entity);
                }
            }
        }

        [AbpAuthorize]
        public async Task<List<CreateOrUpdateAuditMemberOutput>> GetProjectAuditMembers(GetProjectAuditMembersInput input)
        {
            var query = from user in _userRepository.GetAll()
                        join memeber in _projectAuditMemberRepository.GetAll() on user.Id equals memeber.UserId
                        //join p in _projectBaseRepository.GetAll() on memeber.ProjectBaseId equals p.Id
                        where memeber.ProjectBaseId == input.ProjectId && (input.AuditRoleIds.IsNullOrWhiteSpace() || input.AuditRoleIds.Contains(memeber.UserAuditRole.ToString()))
                        select new { user, memeber };
            var ret = new List<CreateOrUpdateAuditMemberOutput>();
            var data = await query.ToListAsync();
            foreach (var item in data)
            {
                var entity = new CreateOrUpdateAuditMemberOutput()
                {
                    AuditRoleId = item.memeber.UserAuditRole,
                    AuditRoleName = ((AuditRoleEnum)item.memeber.UserAuditRole).ToString(),
                    Id = item.memeber.Id,
                    UserId = item.user.Id,
                    UserName = item.user.Name
                };
                ret.Add(entity);
            }
            return ret;
        }

        //[RemoteService(false)]
        public List<CreateOrUpdateAuditMemberOutput> GetProjectAuditMember(GetProjectAuditMembersInput input)
        {
            var roles = new string[0];
            if (!input.AuditRoleIds.IsNullOrWhiteSpace())
            {
                roles = input.AuditRoleIds.Split(",");
            }
            var query = from user in _userRepository.GetAll()
                        join memeber in _projectAuditMemberRepository.GetAll() on user.Id equals memeber.UserId
                        //join p in _projectBaseRepository.GetAll() on memeber.ProjectBaseId equals p.Id
                        where memeber.ProjectBaseId == input.ProjectId && (input.AuditRoleIds.IsNullOrWhiteSpace() || roles.Contains(memeber.UserAuditRole.ToString()))
                        select new { user, memeber };
            var ret = new List<CreateOrUpdateAuditMemberOutput>();
            var data = query.ToList();
            foreach (var item in data)
            {
                var entity = new CreateOrUpdateAuditMemberOutput()
                {
                    AuditRoleId = item.memeber.UserAuditRole,
                    AuditRoleName = ((AuditRoleEnum)item.memeber.UserAuditRole).ToString(),
                    Id = item.memeber.Id,
                    UserId = item.user.Id,
                    UserName = item.user.Name
                };
                ret.Add(entity);
            }
            return ret;
        }


        /// <summary>
        /// 项目评审人开始评审（并创建任务完成进度提醒）
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="taskId">任务id</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task BeginProjectAudit(Guid projectId, Guid taskId)
        {
            var projectModel = await _singleProjectRepository.GetAsync(projectId);
            if (projectModel.ProjectStatus.HasValue && projectModel.ProjectStatus != ProjectStatus.待审)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前项目已开始评审。请刷新页面。");
            }
            projectModel.ProjectStatus = ProjectStatus.在审;
            var task = await _workFlowTaskRepository.GetAsync(taskId);
            task.Status = 1;
            //task.TodoType = 1;//将任务改为我的事项
            //1.创建进度完成情况代办
            //2.计算时间间隔 创建higfire
            //3.时间到后调用方法将代办状态改为可见
            //4.进入待办，点击“已完成”后创建下一步隐藏待办，并计算下一步间隔时间创建higfire
            //重复34

            //改版先屏蔽
            //var ss = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectProgressComplateAppService>();
            //await ss.Send(projectId);//创建待办

        }

        /// <summary>
        /// 开始执行按钮对应api  暂时实现
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="instanceId"></param>
        /// <param name="type">1 开始项目评审 2开始任务 3开始维修固定资产</param>
        /// <returns></returns>
        public async Task BeginButtonApi(BeginFlowerDto input)
        {
            if (input.type == 1)
            {
                var ss = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
                await BeginProjectAudit(input.instanceId.ToGuid(), input.taskId);
            }
            else if (input.type == 2)
            {
                var serviceTask = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOATaskAppService>();
                await serviceTask.BeginOATask(input.instanceId.ToGuid(), input.taskId);
            }
            else if (input.type == 3)
            {
                var serviceRepaire = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOAFixedAssetsRepairAppService>();
                await serviceRepaire.BeginRepairOAFA(input.instanceId.ToGuid());
            }
        }
    }
}
