using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Project.Service.ProjectMemberV3.Dto;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;

namespace Project.Service.ProjectMemberV3
{
    public class ProjectMemberV3AppService : FRMSCoreAppServiceBase, IProjectMemberV3AppService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectRepository;
        private readonly IRepository<ProjectPersentFinish, Guid> _projectPersentFinishRepository;
        private readonly IRepository<ProjectPersentFinishAllot, Guid> _projectPersentFinishAllotRepository;
        private readonly IRepository<ProjectAuditRole, int> _projectAuditRoleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workFlowOrganizationUnitsRepository;

        public ProjectMemberV3AppService(IProjectBaseRepository projectBaseRepository, IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<ProjectPersentFinish, Guid> projectPersentFinishRepository,
            IRepository<ProjectPersentFinishAllot, Guid> projectPersentFinishAllotRepository,
            IRepository<WorkFlowOrganizationUnits, long> workFlowOrganizationUnitsRepository,
            IRepository<ProjectAuditRole, int> projectAuditRoleRepository, IRepository<User, long> userRepository, IRepository<SingleProjectInfo, Guid> singleProjectRepository)
        {
            _projectBaseRepository = projectBaseRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _projectPersentFinishRepository = projectPersentFinishRepository;
            _projectPersentFinishAllotRepository = projectPersentFinishAllotRepository;
            _projectAuditRoleRepository = projectAuditRoleRepository;
            _userRepository = userRepository;
            _workFlowOrganizationUnitsRepository = workFlowOrganizationUnitsRepository;
            _singleProjectRepository = singleProjectRepository;

        }
        public async Task UpdateAsync(CreateOrUpdateProjectMemberInput input)
        {
            var singleProjectModel = await _singleProjectRepository.GetAsync(input.ProjectId);
            if (singleProjectModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "项目不存在");
            var exit_allot = await _projectPersentFinishAllotRepository.GetAll().Where(r => r.ProjectId == input.ProjectId).ToListAsync();
            foreach (var item in exit_allot)
            {
                await _projectPersentFinishAllotRepository.DeleteAsync(item);
            }


            var exit_Finishs = await _projectPersentFinishRepository.GetAll().Where(r => r.ProjectId == input.ProjectId).ToListAsync();
            foreach (var item in exit_Finishs)
            {
                await _projectPersentFinishRepository.DeleteAsync(item);
            }


            var exit_memebers = await _projectAuditMemberRepository.GetAll().Where(r => r.ProjectBaseId == input.ProjectId && r.UserAuditRole == 2).ToListAsync();
            foreach (var item in exit_memebers)
            {
                await _projectAuditMemberRepository.DeleteAsync(item);
            }

            foreach (var item in input.Members)
            {
                var entity = new ProjectAuditMember();
                entity.UserAuditRole = item.AuditRoleId;
                entity.Id = Guid.NewGuid();
                entity.ProjectBaseId = input.ProjectId;
                entity.UserId = item.UserId;
                await _projectAuditMemberRepository.InsertAsync(entity);
                item.Id = entity.Id;
            }
            var auditUsers = input.Members.Where(r => r.AuditRoleId == (int)AuditRoleEnum.工程评审);
            var allfinishmerber = new List<long>();//所有事项的参与人
            foreach (var item in input.Finishs)
            {
                allfinishmerber = allfinishmerber.Union(item.FinishMembers.Select(ite => ite.UserId)).ToList();
                var entity = new ProjectPersentFinish();
                entity.Id = Guid.NewGuid();
                entity.Name = item.Name;
                entity.ProjectId = input.ProjectId;
                entity.Industry = "";
                entity.Persent = item.Persent;
                entity.WorkDay = item.WorkDay;
                await _projectPersentFinishRepository.InsertAsync(entity);
                foreach (var entity_user in item.FinishMembers)
                {
                    var allot = new ProjectPersentFinishAllot();
                    allot.Id = Guid.NewGuid();
                    allot.FinishId = entity.Id;
                    allot.IsMain = entity_user.IsMain;
                    allot.ProjectId = input.ProjectId;
                    var auditMember = auditUsers.FirstOrDefault(r => r.UserId == entity_user.UserId);
                    if (auditMember == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "评审事务人员分派异常");
                    allot.AuditMembeId = auditMember.Id.Value;
                    await _projectPersentFinishAllotRepository.InsertAsync(allot);
                }
            }
            var re = auditUsers.Select(ite => ite.UserId).Except(allfinishmerber);//验证分派的评审人员必须都分派有事项。
            if (re != null && re.Count() > 0)
            {
                var unames = new List<string>();
                foreach (var u in re)
                {
                    unames.Add(_userRepository.Get(u).Name);
                }
                var str = string.Join("、", unames);
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"【工程评审】人员“{str}”还未安排事项。");
            }
            CurrentUnitOfWork.SaveChanges();
            #region  对财务评审人员的更新
            singleProjectModel.HasFinancialReview = input.HasFinancialReview;
            var inputgroupMemberList = new List<CreateOrUpdateProjectAuditMembersInput>();
            if (input.HasFinancialReview)
            {
                if (input.OrgFinancial1 == 0 || input.OrgFinancial2 == 0)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "财务评审人员未指定");
                var orgmodel = _workFlowOrganizationUnitsRepository.Get(input.OrgFinancial1);
                if (orgmodel.ChargeLeader.IsNullOrWhiteSpace())
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "财务初审部门未指定分管领导");
                var t = orgmodel.ChargeLeader.Split(",", StringSplitOptions.RemoveEmptyEntries);//部门领导可能有多个
                foreach (var x in t)
                {
                    var chargeleader = MemberPerfix.RemovePrefix(x).ToLong();
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
                }

                var orgmodel2 = _workFlowOrganizationUnitsRepository.Get(input.OrgFinancial2);
                if (orgmodel2.ChargeLeader.IsNullOrWhiteSpace())
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "财务终审部门未指定分管领导");
                var t2 = orgmodel2.ChargeLeader.Split(",", StringSplitOptions.RemoveEmptyEntries);//部门领导可能有多个
                foreach (var x in t2)
                {
                    var chargeleader2 = MemberPerfix.RemovePrefix(x).ToLong();
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


    }
}
