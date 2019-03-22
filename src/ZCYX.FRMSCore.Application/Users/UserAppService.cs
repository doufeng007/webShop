using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Session;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Users.Dto;
using ZCYX.FRMSCore.Roles.Dto;
using Abp.AutoMapper;
using ZCYX.FRMSCore.Authorization.Permissions.Dto;
using ZCYX.FRMSCore.Authorization.Permissions;
using System.Diagnostics;
using ZCYX.FRMSCore.Application;
using System;
using Abp.UI;
using Abp;
using Abp.Authorization.Users;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Model;
using Abp.IM;
using ZCYX.FRMSCore.Entity;

namespace ZCYX.FRMSCore.Users
{
    //[AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits)]
    [AbpAuthorize]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<UserPosts, Guid> _userPostsRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<ContractWithSystem, Guid> _contractWithSystemRepository;
        private readonly IRepository<RealationSystem, Guid> _realationSystemRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workflowOrganizationUnitsRepository;
        private readonly UserStore _abpStore;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<RoleRelation, Guid> _roleRelationRepository;

        private readonly IRepository<WorkFlowUserOrganizationUnits,long> _userOrganizationUnitsRepository;
        private readonly IRepository<OrganizationUnitPostsRole, Guid> _organizationUnitPostsRoleRepository;
        private readonly IRepository<UserPermissionSetting, long> _abpPermissionsRepository;
        private readonly IRepository<AbpPermissionBase,long> _abpPermissionBaseRepository;
        private readonly AbpOrganizationUnitsManager _abpOrganizationUnitsManager;
        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager, IRepository<UserPermissionSetting, long> abpPermissionsRepository,
            IRepository<User, long> userRepository, IRepository<AbpPermissionBase, long> abpPermissionBaseRepository,
            IRepository<Role> roleRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitsRepository,
            IPasswordHasher<User> passwordHasher
            , IRepository<UserPosts, Guid> userPostsRepository, IRepository<PostInfo, Guid> postsRepository, IRepository<ContractWithSystem, Guid> contractWithSystemRepository
            , IRepository<RealationSystem, Guid> realationSystemRepository, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository
            , UserStore abpStore, IRepository<OrganizationUnitPostsRole, Guid> organizationUnitPostsRoleRepository
            , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, AbpOrganizationUnitsManager abpOrganizationUnitsManager
            , IRepository<UserRole, long> userRoleRepository, IRepository<WorkFlowOrganizationUnits, long> workflowOrganizationUnitsRepository, IRepository<RoleRelation, Guid> roleRelationRepository)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _userPostsRepository = userPostsRepository;
            _postsRepository = postsRepository;
            _contractWithSystemRepository = contractWithSystemRepository;
            _realationSystemRepository = realationSystemRepository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _userRepository = userRepository;
            _abpStore = abpStore;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _userRoleRepository = userRoleRepository;
            _workflowOrganizationUnitsRepository = workflowOrganizationUnitsRepository;
            _userOrganizationUnitsRepository = userOrganizationUnitsRepository;
            _organizationUnitPostsRoleRepository = organizationUnitPostsRoleRepository;
            _abpOrganizationUnitsManager = abpOrganizationUnitsManager;
            _abpPermissionsRepository = abpPermissionsRepository;
            _abpPermissionBaseRepository = abpPermissionBaseRepository;
            _roleRelationRepository = roleRelationRepository;
        }
        public async Task<List<UserOutput>> GetXmfzrUserList() {

            var role = _roleRepository.GetAll().FirstOrDefault(x=>x.Name== "XMFZR");
            if(role==null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未设置项目负责人角色");
            var query = from a in _userRepository.GetAll()
                        join b in  _userRoleRepository.GetAll() on a.Id equals b.UserId
                        where b.RoleId==role.Id
                        select new UserOutput
                        {
                            Id = a.Id,
                            UserName = a.UserName
                        };
            return  query.OrderBy(ite => ite.Id).ToList();
        }


        public override async Task<UserDto> Get(EntityDto<long> input)
        {
            var user = await base.Get(input);
            var post = from a in _postsRepository.GetAll()
                       join b in _userPostsRepository.GetAll() on a.Id equals b.PostId
                       join c in _organizationUnitPostsRepository.GetAll() on b.OrgPostId equals c.Id
                       where b.UserId == input.Id
                       select new UserPostDto { Id = b.Id, OrgPostId = c.Id, PostId = a.Id, PostName = a.Name , IsMain=b.IsMain };
            if (post.Count() > 0)
            {
                user.Posts = post.ToList();
            }

            var sys_realtion = from a in _realationSystemRepository.GetAll()
                               join b in _contractWithSystemRepository.GetAll() on a.Id equals b.SystemId
                               where b.UserId == user.Id
                               select a;
            if (sys_realtion.Count() > 0)
            {
                user.RealationSystems = sys_realtion.MapTo<List<RealationSystem>>();
            }

            return user;
        }
        public async Task CreateImUserList()
        {
            var users = await _userRepository.GetAll().Where(x => !x.IsDeleted).ToListAsync();
            foreach (var item in users)
            {
                try
                {
                   await CreateImUser(new CreateImUserDto() { Id = item.Id, PassWord = "123qwe" });
                }
                catch (Exception ex)
                {
                }
            }
        }
        public async Task CreateImUser(CreateImUserDto input)
        {
            try
            {
                var user = await _userManager.GetUserByIdAsync(input.Id);
                if (_passwordHasher.VerifyHashedPassword(user, user.Password,input.PassWord)!= PasswordVerificationResult.Success)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "密码错误");
                var im_Service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IM_UserManager>();
               var isExistence= im_Service.GetImIsExistenceUser(user.Id.ToString());
                var exit_UserCount = _userRepository.GetAll().Count();
                if (isExistence)
                {
                    im_Service.UpdateIMUserPassword(user.Id.ToString(), input.PassWord);
                    im_Service.UpdateIMUserNickname(user.Id.ToString(),user.Name);
                }
                else
                {
                    if (exit_UserCount == 0)
                    {
                        var firesOrg = _workflowOrganizationUnitsRepository.FirstOrDefault(r => !r.ParentId.HasValue);
                        var companyName = "公司全体";
                        if (firesOrg != null)
                            companyName = firesOrg.DisplayName;
                        im_Service.CreateIMUsers(new List<CreateIMUserInput>()
                    {
                        new CreateIMUserInput() { nickname= user.Name, password = input.PassWord, username = user.Id.ToString() }
                    }, true, companyName);
                    }
                    else
                    {
                        im_Service.CreateIMUsers(new List<CreateIMUserInput>()
                                            {
                                                new CreateIMUserInput() { nickname= user.Name, password = input.PassWord, username = user.Id.ToString() }
                                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "错误:"+ex.Message);
            }
        }

        public override async Task<UserDto> Create(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.IsEmailConfirmed = true;
            user.WorkNumber = user.WorkNumber.Trim();
            //验证工号是否唯一
            var has = _userRepository.GetAll().FirstOrDefault(ite => ite.WorkNumber == user.WorkNumber);
            if (has != null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "工号重复，请换一个工号");
            }
            CheckErrors(await _userManager.CreateAsync(user));

            //改为由岗位继承角色
            //if (input.RoleNames != null)
            //{
            //    CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            //}
            if (input.OrganizationUnitId == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请至少指定一个部门。");
            }
            if (input.OrgPostIds == null || input.OrgPostIds.Count == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请至少指定一个岗位。");
            }
            //设置所在部门
            await _abpOrganizationUnitsManager.AddToOrganizationUnitAsync(user.Id, input.OrganizationUnitId);
            //角色
            var roles = new List<string>();
            foreach (var item in input.OrgPostIds)
            {
                var orgPostModel = await _organizationUnitPostsRepository.GetAsync(item);
                var userPost = new UserPosts() { Id = Guid.NewGuid(), UserId = user.Id, PostId = orgPostModel.PostId, OrgPostId = orgPostModel.Id, OrgId = orgPostModel.OrganizationUnitId };
                if (item == input.MainPostId)
                {
                    userPost.IsMain = true;// 设置主岗位
                }
                var rs = _organizationUnitPostsRoleRepository.GetAll().Where(ite => ite.OrgPostId == item).Select(ite => ite.RoleName).ToList();
                foreach (var r in rs) {
                    var existrole = await _roleManager.GetRoleByNameAsync(r);
                    if (existrole!=null) {
                        roles.Add(r);
                    }
                }
                await _userPostsRepository.InsertAsync(userPost);

                if (orgPostModel.Level ==  Level.分管领导)//如果是领导岗位 则更新部门领导
                {
                    var org = _workflowOrganizationUnitsRepository.Get(orgPostModel.OrganizationUnitId);
                    var tmp = new List<string>();
                    var users = _userPostsRepository.GetAll().Where(ite => ite.OrgPostId == item).ToList();
                    var us = new List<User>();
                    foreach (var u in users)
                    {
                        var us1 = _userRepository.FirstOrDefault(u.UserId);
                        if (us1 != null)
                        {
                            us.Add(us1);
                        }
                    }
                    us.Add(user);
                    tmp = us.Select(ite => $"u_{ite.Id}").ToList();
                    org.ChargeLeader = string.Join(",", tmp);
                }
                if (orgPostModel.Level == Level.部门领导)//如果是领导岗位 则更新部门领导
                {
                    var org = _workflowOrganizationUnitsRepository.Get(orgPostModel.OrganizationUnitId);
                    var tmp = new List<string>();
                    var users = _userPostsRepository.GetAll().Where(ite => ite.OrgPostId == item).ToList();
                    var us = new List<User>();
                    foreach (var u in users)
                    {
                        var us1 = _userRepository.FirstOrDefault(u.UserId);
                        if (us1 != null)
                        {
                            us.Add(us1);
                        }
                    }
                    us.Add(user);
                    tmp = us.Select(ite => $"u_{ite.Id}").ToList();
                    org.Leader = string.Join(",", tmp);
                }
            }
            if (roles == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "所选岗位角色为空，请先为岗位指定角色。");
            }
            CheckUserRole(roles);
            CheckErrors(await _userManager.SetRoles(user, roles.ToArray()));
            foreach (var item in input.RelationSystemIds)
            {
                var r_s = new ContractWithSystem() { Id = Guid.NewGuid(), UserId = user.Id, SystemId = item };
                await _contractWithSystemRepository.InsertAsync(r_s);
            }


            #region  创建IM账号
            try
            {

                var im_Service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IM_UserManager>();
                var exit_UserCount = _userRepository.GetAll().Count();
                if (exit_UserCount == 0)
                {
                    var firesOrg = _workflowOrganizationUnitsRepository.FirstOrDefault(r => !r.ParentId.HasValue);
                    var companyName = "公司全体";
                    if (firesOrg != null)
                        companyName = firesOrg.DisplayName;
                    im_Service.CreateIMUsers(new List<CreateIMUserInput>()
                    {
                        new CreateIMUserInput() { nickname= input.Name, password = input.Password, username = user.Id.ToString() }
                    }, true, companyName);
                }
                else
                {
                    im_Service.CreateIMUsers(new List<CreateIMUserInput>()
                                            {
                                                new CreateIMUserInput() { nickname= input.Name, password = input.Password, username = user.Id.ToString() }
                                            });
                }
            }
            catch (Exception ex)
            {
                //处理im报错
            }

            #endregion

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckUserRole(input.RoleNames.ToList());
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }


            return await Get(input);
        }


        public async Task UpdateUserAsyn(UpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await _userManager.GetUserByIdAsync(input.User.Id.Value);
            var oldName = user.Name;
            //Update user properties
            //input.User.MapTo(user); //Passwords is not mapped (see mapping configuration)
            user.Name = input.User.Name;
            user.UserName = input.User.UserName;
            user.EmailAddress = input.User.EmailAddress;
            user.PhoneNumber = input.User.PhoneNumber;
            user.Sex = input.User.Sex;
            user.EnterTime = input.User.EnterTime;
            user.IdCard = input.User.IdCard;


            if (!string.IsNullOrWhiteSpace(input.User.Password))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.User.Password));
            }

            CheckErrors(await _userManager.UpdateAsync(user));




            //Update roles 改为不能直接设置角色，而通过岗位继承角色
            //if (input.AssignedRoleNames != null && input.AssignedRoleNames.Length != 0)
            //{
            //    var ret = await SetRoles(user, input.AssignedRoleNames);
            //    CheckErrors(ret);
            //}


            //if (input.SetRandomPassword)
            //{
            //    input.User.Password = User.CreateRandomPassword();
            //}


            if (input.PostIds != null && input.PostIds.Count == 0) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请选择至少一个岗位");
            }
            if (input.PostIds.Count != input.PostIds.Distinct().Count()) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请不要重复选择同一岗位");
            }
            if (input.MainPostId == Guid.Empty || input.PostIds.Contains(input.MainPostId) == false) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "主岗位不在所选岗位中，请重新选择。");
            }
            var exit_post = _userPostsRepository.GetAll().Where(r => r.UserId == user.Id);

            foreach (var item in exit_post)
            {
                await _userPostsRepository.DeleteAsync(item);
            }
            var orgids = new List<long>();//所在部门集合
            long mainorg = 0;
            foreach (var item in input.PostIds)
            {
                var orgPostModel = await _organizationUnitPostsRepository.GetAsync(item);
                if (orgids.Exists(ite => ite == orgPostModel.OrganizationUnitId) == false) {
                    orgids.Add(orgPostModel.OrganizationUnitId);
                }
                var userPost = new UserPosts() { Id = Guid.NewGuid(), UserId = user.Id, PostId = orgPostModel.PostId, OrgPostId = orgPostModel.Id, OrgId = orgPostModel.OrganizationUnitId };
                if (item == input.MainPostId)
                {
                    userPost.IsMain = true;// 设置主岗位
                    mainorg = orgPostModel.OrganizationUnitId;
                }
                await _userPostsRepository.InsertAsync(userPost);
            }
            //部门更新
            var oldorgs = _userOrganizationUnitsRepository.GetAll().Where(ite => ite.UserId == input.User.Id).ToList();
            var oldids = oldorgs.Select(ite => ite.OrganizationUnitId).ToList();
            var addorg = orgids.Except(oldids).ToList();
            var updateorg = orgids.Intersect(oldids).ToList();
            var delorg = oldids.Except(orgids).ToList();
            foreach (var x in addorg)
            {
                _userOrganizationUnitsRepository.Insert(new WorkFlowUserOrganizationUnits()
                {
                    IsMain = mainorg == x,
                    OrganizationUnitId = x,
                    UserId = user.Id,
                });
            }
            foreach (var x in updateorg) {
                var t = oldorgs.First(ite => ite.OrganizationUnitId == x);
                if (x == mainorg) {
                    t.IsMain = true;
                }
            }
            foreach (var x in delorg) {
                var t= oldorgs.First(ite => ite.OrganizationUnitId == x);
                _userOrganizationUnitsRepository.Delete(ite => ite.Id == t.Id);
            }

            CurrentUnitOfWork.SaveChanges();
            foreach (var item in input.PostIds) {
                var orgPostModel = await _organizationUnitPostsRepository.GetAsync(item);
                var org = _workflowOrganizationUnitsRepository.Get(orgPostModel.OrganizationUnitId);
                var tmp = new List<string>();
                if (orgPostModel.Level == Level.分管领导)
                {
                    var users = _userPostsRepository.GetAll().Where(ite => ite.OrgPostId == item).ToList();
                    var us = new List<User>();
                    foreach (var u in users)
                    {
                        var us1 = _userRepository.FirstOrDefault(u.UserId);
                        if (us1 != null)
                        {
                            us.Add(us1);
                        }
                    }
                    tmp = us.Select(ite => $"u_{ite.Id}").ToList();
                    org.ChargeLeader = string.Join(",", tmp);
                }
                if (orgPostModel.Level == Level.部门领导)
                {
                    var users = _userPostsRepository.GetAll().Where(ite => ite.OrgPostId == item).ToList();
                    var us = new List<User>();
                    foreach (var u in users)
                    {
                        var us1 = _userRepository.FirstOrDefault(u.UserId);
                        if (us1 != null)
                        {
                            us.Add(us1);
                        }
                    }
                    tmp = us.Select(ite => $"u_{ite.Id}").ToList();
                    org.Leader = string.Join(",", tmp);
                }
                _workflowOrganizationUnitsRepository.Update(org);
            }
            //调整岗位后 更新角色
            var newroles = _organizationUnitPostsRoleRepository.GetAll().Where(ite => input.PostIds.Contains(ite.OrgPostId)).Select(ite => ite.RoleName).Distinct().ToArray();
            CheckUserRole(newroles.ToList());
            await _userManager.SetRoles(user, newroles);
            //联系人关联系统
            var exit_sys_realtion = _contractWithSystemRepository.GetAll().Where(r => r.UserId == user.Id);
            foreach (var item in exit_sys_realtion)
            {
                await _contractWithSystemRepository.DeleteAsync(item);
            }
            if (input.RealtionSystemIds != null)
            {
                foreach (var item in input.RealtionSystemIds)
                {
                    var entity = new ContractWithSystem() { Id = Guid.NewGuid(), SystemId = item, UserId = user.Id };
                    await _contractWithSystemRepository.InsertAsync(entity);
                }
            }

            CurrentUnitOfWork.SaveChanges();
            #region  更新im用户信息
            try
            {
                var im_Service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IM_UserManager>();
                if (!string.IsNullOrWhiteSpace(input.User.Password))
                    im_Service.UpdateIMUserPassword(input.User.Id.ToString(), input.User.Password);
                if (input.User.Name != oldName)
                    im_Service.UpdateIMUserNickname(input.User.Id.ToString(), input.User.Name);
            }
            catch (Exception)
            {
            }
            #endregion
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
            #region  删除IM用户  
            try
            {
                var im_Service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IM_UserManager>();
                im_Service.DeleteIMUser(input.Id.ToString());
            }
            catch (Exception)
            {

            }
            #endregion


            #region 删除组织架构领导
            var org = _workflowOrganizationUnitsRepository.GetAll().Where(ite => ite.ChargeLeader.Contains($"u_{user.Id}")).ToList();
            foreach (var r in org)
            {
                var users = r.ChargeLeader.Split(",").ToList();
                users.Remove($"u_{user.Id}");
                r.ChargeLeader = string.Join(",", users);
                _workflowOrganizationUnitsRepository.Update(r);
            }
            var org2 = _workflowOrganizationUnitsRepository.GetAll().Where(ite => ite.Leader.Contains($"u_{user.Id}")).ToList();
            foreach (var r in org2)
            {
                var users = r.Leader.Split(",").ToList();
                users.Remove($"u_{user.Id}");
                r.Leader = string.Join(",", users);
                _workflowOrganizationUnitsRepository.Update(r);
            }
            #endregion
        }

        public async Task Enable(EntityDto<long> input)
        {
            var user = _userRepository.Get(input.Id);
            user.IsActive = !user.IsActive;
            await _userRepository.UpdateAsync(user);
            var im_Service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IM_UserManager>();
            try
            {
                if (user.IsActive)
                    im_Service.EnableIMUser(input.Id.ToString());
                else
                {
                   
                    //如果是部门领导 则取消部门领导负责人
                    var org = _workflowOrganizationUnitsRepository.GetAll().Where(ite => ite.Leader.Contains(input.Id.ToString()) || ite.ChargeLeader.Contains(input.Id.ToString())).ToList();
                    foreach (var o in org) {
                        if (o.Leader.Contains(input.Id.ToString())) {
                            var users = o.Leader.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var u in users) {
                                if (u == "u_" + input.Id.ToString()) {
                                    users.Remove(u);
                                    break;
                                }
                            }
                            o.Leader= string.Join(",", users);
                        }
                        if (o.ChargeLeader.Contains(input.Id.ToString()))
                        {
                            var users = o.ChargeLeader.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var u in users)
                            {
                                if (u == "u_" + input.Id.ToString())
                                {
                                    users.Remove(u);
                                    break;
                                }
                            }
                            o.ChargeLeader = string.Join(",", users);
                        }
                        _workflowOrganizationUnitsRepository.Update(o);
                    }
                    CurrentUnitOfWork.SaveChanges();
                    im_Service.DisableIMUser(input.Id.ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            return await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = await _userManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }
        /// <summary>
        /// 根据角色编码找到该角色人员
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<List<UserDto>> GetUserByRoleCode(string code)
        {
            var us = (await _userManager.GetUsersInRoleAsync(code)).ToList();
            if (us == null || us.Count == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"未找到具有角色{code}的人员。");
            }
            return us.MapTo<List<UserDto>>();
        }

        public async Task ResetUserSpecificPermissions(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.ResetAllPermissionsAsync(user);
        }


        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await _userManager.SetGrantedPermissionsAsync(user, grantedPermissions);
            if (_roleRelationRepository.GetAll().Count(x => x.UserId == user.Id) > 0)
            {
                var list = _roleRelationRepository.GetAll().Where(x => x.UserId == user.Id).ToList();
                foreach (var item in list)
                {
                    _workFlowOrganizationUnitsManager.AddRelationPermission(item.RelationId, item.UserId, item.RelationUserId);
                }
            }
        }



        public virtual async Task<IdentityResult> SetRoles(User user, string[] roleNames)
        {

            await _abpStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles);

            //Remove from removed roles
            foreach (var userRole in user.Roles.ToList())
            {
                var role = await _roleManager.FindByIdAsync(userRole.RoleId.ToString());
                if (roleNames.All(roleName => role.Name != roleName))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            //Add to added roles
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                if (user.Roles.All(ur => ur.RoleId != role.Id))
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            return IdentityResult.Success;
        }



        /// <summary>
        /// 获取某个角色下的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserUnderRoleDto>> GetAbpUsersByRoleId(GetAbpUsersByRoleIdInput input)
        {
            var ret = new List<UserUnderRoleDto>();
            var query = from user in _userRepository.GetAll()
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId
                        where ur.RoleId == input.RoleId
                        select user;
            var toalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            foreach (var item in data)
            {
                var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
                var entity = new UserUnderRoleDto()
                {
                    UserId = item.Id,
                    UserName = item.Name,
                    OrgName = userOrgModel.OrgId_Name,
                };

                ret.Add(entity);
            }
            return new PagedResultDto<UserUnderRoleDto>(toalCount, ret);
        }


        /// <summary>
        /// 管理角色下用户，同时新增+删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateUsersUnderRole(UpdateUsersUnderRoleInput input)
        {

            var roleModel = await _roleRepository.GetAsync(input.RoleId);
            var oldUserIds = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleId(input.RoleId);
            ////add 
            var add_UserIds = input.UserIds.Except(oldUserIds.Select(r => r.Id));
            foreach (var add_UserId in add_UserIds)
            {
                var user = await _userManager.GetUserByIdAsync(add_UserId);
                await _abpStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles);
                if (user.Roles.All(ur => ur.RoleId != roleModel.Id))
                {
                    var result = await _userManager.AddToRoleAsync(user, roleModel.Name);

                    if (!result.Succeeded)
                    {
                        CheckErrors(result);
                    }
                }
            }

            ////remove

            var remove_UserIds = oldUserIds.Select(r => r.Id).Except(input.UserIds);
            foreach (var remove_UserId in remove_UserIds)
            {
                var user = await _userManager.GetUserByIdAsync(remove_UserId);
                var role = await _roleManager.FindByIdAsync(input.RoleId.ToString());
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    CheckErrors(result);
                }


            }


        }


        /// <summary>
        /// 管理角色下用户，新增或者删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateUsersUnderRoleOneWay(UpdateUsersUnderRoleOneWayInput input)
        {

            var roleModel = await _roleRepository.GetAsync(input.RoleId);
            var oldUserIds = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleId(input.RoleId);
            ////add 
            if (input.ActionType == 1)
            {
                var add_UserIds = input.UserIds.Except(oldUserIds.Select(r => r.Id));
                foreach (var add_UserId in add_UserIds)
                {
                    var user = await _userManager.GetUserByIdAsync(add_UserId);
                    await _abpStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles);
                    if (user.Roles.All(ur => ur.RoleId != roleModel.Id))
                    {
                        var result = await _userManager.AddToRoleAsync(user, roleModel.Name);

                        if (!result.Succeeded)
                        {
                            CheckErrors(result);
                        }
                    }
                }
            }
            ////remove
            else if (input.ActionType == 2)
            {
                var remove_UserIds = oldUserIds.Select(r => r.Id).Intersect(input.UserIds);
                foreach (var remove_UserId in remove_UserIds)
                {
                    var user = await _userManager.GetUserByIdAsync(remove_UserId);
                    var role = await _roleManager.FindByIdAsync(input.RoleId.ToString());
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        CheckErrors(result);
                    }
                }

            }







        }



        /// <summary>
        /// ,分隔的userid
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<IMUserInfoDto>> GetUserNameByIds(EntityDto<string> input)
        {
            var ret = new List<IMUserInfoDto>();
            if (string.IsNullOrWhiteSpace(input.Id))
                return ret;
            var userIds = input.Id.Split(',').ToList();
            var usermodels = _userRepository.GetAll().Where(r => userIds.Contains(r.Id.ToString()));
            foreach (var item in usermodels)
            {
                ret.Add(new IMUserInfoDto() { NickName = item.Name, UserName = item.Id.ToString() });
            }
            return ret;
        }
        /// <summary>
        /// 获取用户特殊设定的权限列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SpecialPermissionDto> GetSpecialPermiss(long userId) {
            var list = from a in _abpPermissionsRepository.GetAll()
                       join b in _abpPermissionBaseRepository.GetAll() on a.Name equals b.Code
                       where a.UserId == userId
                       select new SpecialPermissionDto()
                       {
                           Id = a.Id,
                           DisplayName = b.DisplayName,
                           IsGranted = a.IsGranted,
                           Name = a.Name,
                           UserId = a.UserId
                       };
            return list.ToList();
        }
        /// <summary>
        /// 移除特殊指定的权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveSpecialPermiss(long id) {
            var p = _abpPermissionsRepository.Get(id);
            _abpPermissionsRepository.Delete(p);
            return true;
        }


        private void CheckUserRole(List<string> roles)
        {
            if (roles.Contains("ZJL") && roles.Contains("BGSZR"))
                throw new UserFriendlyException("不能同时拥有总经理【内置】和办公室主任【内置】角色");

        }
    }
}
