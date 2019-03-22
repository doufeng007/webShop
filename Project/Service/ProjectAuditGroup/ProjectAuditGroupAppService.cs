using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Diagnostics;
using Abp.Domain.Repositories;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Linq;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectAuditGroupAppService : ApplicationService, IProjectAuditGroupAppService
    {
        private readonly IRepository<ProjectAuditGroup, Guid> _projectAuditGroupRepository;
        private readonly IRepository<ProjectAuditGroupUser, Guid> _projectAuditGroupUserRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly UserManager _userManager;
        public ProjectAuditGroupAppService(IRepository<ProjectAuditGroup, Guid> projectAuditGroupRepository, IRepository<ProjectAuditGroupUser, Guid> projectAuditGroupUserRepository, IRepository<User, long> userRepository
            , IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IWorkFlowTaskRepository workFlowTaskRepository, UserManager userManager)
        {
            _projectAuditGroupRepository = projectAuditGroupRepository;
            _projectAuditGroupUserRepository = projectAuditGroupUserRepository;
            _userRepository = userRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _userManager = userManager;
        }

        public async Task CreateOrUpdateProjectAuditGroup(CreateOrUpdateProjectAuditGroupInput input)
        {
            if (input.Users.Count(x => x.UserRole == 1) < 1)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请选择项目负责人。");
            if (input.Users.Count(x => x.UserRole == 2) < 1)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请选择第一联系人。");
            if (input.Users.Count(x => x.UserRole == 3) < 1)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请选择成员");
            if (input.Id.HasValue)
            {
                await UpdateProjectAuditGroupAsync(input);
            }
            else
            {
                await CreateProjectAuditGroupAsync(input);
            }
        }

        public async Task<Guid> CreateOrUpdate(CreateOrUpdateProjectAuditGroupInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateProjectAuditGroupAsync(input);
                return input.Id.Value;
            }
            else
            {
                return await CreateProjectAuditGroup(input);
            }
        }



        public async Task<GetProjectAuditGroupForEditOutput> GetProjectAuditGroupForEdit(NullableIdDto<Guid> input)
        {
            if (!input.Id.HasValue)
                return new GetProjectAuditGroupForEditOutput();
            var projectAuditGroup = await _projectAuditGroupRepository.GetAsync(input.Id.Value);
            var output = projectAuditGroup.MapTo<GetProjectAuditGroupForEditOutput>();
            var users = await _projectAuditGroupUserRepository.GetAll().Where(r => r.GroupId == projectAuditGroup.Id).ToListAsync();
            var usersquery = from a in _projectAuditGroupUserRepository.GetAll()
                             join b in _userRepository.GetAll() on a.UserId equals b.Id
                             where a.GroupId == input.Id
                             select new { Group = a, UserName = b.Name };
            var usersList = new List<CreateOrUpdateProjectAuditGroupUserInput>();
            foreach (var item in usersquery)
            {
                var entity = item.Group.MapTo<CreateOrUpdateProjectAuditGroupUserInput>();
                entity.UserName = item.UserName;
                usersList.Add(entity);
            }
            output.Users = usersList;
            return output;
        }

        public async Task<PagedResultDto<ProjectAuditGroupListDto>> GetProjectAuditGroups(GetProjectAuditGroupListInput input)
        {
            try
            {
                var query = _projectAuditGroupRepository.GetAll();
                var count = await query.CountAsync();
                var projectAuditGroups = await query
                 .OrderBy(input.Sorting)
                 .PageBy(input)
                 .ToListAsync();
                var projectAuditGroupDtos = projectAuditGroups.MapTo<List<ProjectAuditGroupListDto>>();

                return new PagedResultDto<ProjectAuditGroupListDto>(count, projectAuditGroupDtos);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }




        public async Task DeleteProjectAuditGroup(EntityDto<Guid> input)
        {
            var projectAuditGroup = await _projectAuditGroupRepository.GetAsync(input.Id);
            await _projectAuditGroupRepository.DeleteAsync(projectAuditGroup);

            var list = await _projectAuditGroupUserRepository.GetAll().Where(r => r.GroupId == projectAuditGroup.Id).ToListAsync();
            var userid = list.First(x => x.UserRole == 1).UserId;
            await UpdateRole(userid, 0, projectAuditGroup.Id);
            list.ForEach(r => _projectAuditGroupUserRepository.DeleteAsync(r));
        }




        public void CopyForProjectAuditGroup(CopyForProjectAuditGroupInput input)
        {
            var groupMemberUserRole = new List<int>() { 1, 9, 10 };
            if (input.HasFinancialReviewMember)
                groupMemberUserRole.Add(5);
            var projectId = input.InstanceID.ToGuid();
            var querygroupusers = _projectAuditMemberRepository.GetAll().Where(r => r.ProjectBaseId == projectId && groupMemberUserRole.Contains(r.UserAuditRole));
            var groupusers = querygroupusers.ToList();
            var abpUserIds = groupusers.Select(r => r.UserId).ToList();
            var taskModel = _workFlowTaskRepository.Get(input.TaskID);
            var nextTask = _workFlowTaskRepository.FirstOrDefault(r => r.PrevID == taskModel.Id);
            if (nextTask != null)
            {
                var currentReviceUser = abpUserIds.FirstOrDefault(r => r == nextTask.ReceiveID);
                if (currentReviceUser != 0)
                {
                    abpUserIds.Remove(currentReviceUser);
                }
                var copyUsersStr = "";
                foreach (var item in abpUserIds)
                {
                    if (copyUsersStr != "")
                        copyUsersStr = copyUsersStr + ",";
                    copyUsersStr = copyUsersStr + $"u_{item}";
                }
                var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
                service.FlowCopyForAsync(new FlowCopyForInput()
                {
                    FlowId = input.FlowID,
                    GroupId = input.GroupID,
                    InstanceId = input.InstanceID,
                    StepId = nextTask.StepID,
                    TaskId = nextTask.Id,
                    UserIds = copyUsersStr
                });
            }



        }




        protected virtual async Task UpdateProjectAuditGroupAsync(CreateOrUpdateProjectAuditGroupInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var projectAuditGroup = await _projectAuditGroupRepository.GetAsync(input.Id.Value);
            projectAuditGroup.Name = input.Name;
            projectAuditGroup.Description = input.Description;
            var list = await _projectAuditGroupUserRepository.GetAll().Where(r => r.GroupId == input.Id.Value).ToListAsync();
            var userid = list.First(x => x.UserRole == 1).UserId;
            var adduserid = input.Users.First(x => x.UserRole == 1).UserId;
            list.ForEach(r => _projectAuditGroupUserRepository.DeleteAsync(r));
            input.Users.ForEach(r =>
            {
                var entity = new ProjectAuditGroupUser();
                entity.Id = Guid.NewGuid(); entity.GroupId = input.Id.Value;
                entity.UserId = r.UserId;
                entity.UserRole = r.UserRole;
                _projectAuditGroupUserRepository.InsertAsync(entity);
            });
            await _projectAuditGroupRepository.UpdateAsync(projectAuditGroup);
            if (userid != adduserid)
            {
                await UpdateRole(userid, 0, input.Id.Value);
                await UpdateRole(adduserid, 1, input.Id.Value);
            }
        }
        /// <summary>
        /// 项目负责人角色新增删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type">0,删除，1新增</param>
        /// <returns></returns>
        private async Task UpdateRole(long userId, int type, Guid guid)
        {
            var user = _userRepository.GetAll().FirstOrDefault(ite => ite.Id == userId);
            if (user == null)
                return;
            var list = new List<string>();
            list.Add("XMFZR");
            if (type == 0)
            {
                var groupUserCount = await _projectAuditGroupUserRepository.GetAll().Where(x => x.UserId == userId && x.UserRole == 1 && x.GroupId != guid).CountAsync();
                if (groupUserCount > 0)
                    return;
                await _userManager.RemoveFromRolesAsync(user, list);
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count(x => string.Compare(x, "XMFZR", true) == 0) == 0)
                    await _userManager.AddToRolesAsync(user, list);
            }
        }
        protected virtual async Task CreateProjectAuditGroupAsync(CreateOrUpdateProjectAuditGroupInput input)
        {
            var projectAuditGroup = new ProjectAuditGroup()
            {
                Name = input.Name,
                Description = input.Description
                //Sort_id = input.Sort_id
            };
            var groupId = await _projectAuditGroupRepository.InsertAndGetIdAsync(projectAuditGroup);
            input.Users.ForEach(r =>
            {
                var entity = new ProjectAuditGroupUser();
                entity.Id = Guid.NewGuid();
                entity.GroupId = groupId;
                entity.UserId = r.UserId;
                entity.UserRole = r.UserRole;
                _projectAuditGroupUserRepository.InsertAsync(entity);
            });

            var adduserid = input.Users.First(x => x.UserRole == 1).UserId;
            await UpdateRole(adduserid, 1, groupId);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.
        }

        protected virtual async Task<Guid> CreateProjectAuditGroup(CreateOrUpdateProjectAuditGroupInput input)
        {
            var projectAuditGroup = new ProjectAuditGroup()
            {
                Name = input.Name,
                Description = input.Description,
            };
            var retid = await _projectAuditGroupRepository.InsertAndGetIdAsync(projectAuditGroup);
            input.Users.ForEach(r =>
            {
                var entity = new ProjectAuditGroupUser();
                entity.Id = Guid.NewGuid(); entity.GroupId = retid;
                entity.UserId = r.UserId;
                entity.UserRole = r.UserRole;
                _projectAuditGroupUserRepository.InsertAsync(entity);
            });
            await CurrentUnitOfWork.SaveChangesAsync();
            return retid;
        }



    }
}
