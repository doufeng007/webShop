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
using AutoMapper;
using Abp.Extensions;
using ZCYX.FRMSCore.Model;
using Abp.Authorization.Users;

namespace ZCYX.FRMSCore.Users
{
    [RemoteService(IsEnabled = false)]
    public class UserManagerNotRemote : ApplicationService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<UserPosts, Guid> _userPostsRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<PostInfo, Guid> _postsRepository;
        private readonly IRepository<ContractWithSystem, Guid> _contractWithSystemRepository;
        private readonly IRepository<RealationSystem, Guid> _realationSystemRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;

        public UserManagerNotRemote(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher
            , IRepository<UserPosts, Guid> userPostsRepository, IRepository<PostInfo, Guid> postsRepository, IRepository<ContractWithSystem, Guid> contractWithSystemRepository
            , IRepository<RealationSystem, Guid> realationSystemRepository, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository, IRepository<UserRole, long> userRoleRepository)
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
            _userRoleRepository = userRoleRepository;
        }




        public async Task<long> Create(CreateUserDto input)
        {
            var user = ObjectMapper.Map<User>(input);
            user.TenantId = AbpSession.TenantId;
            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.IsEmailConfirmed = true;
            user.WorkNumber = user.WorkNumber?.Trim() ?? "";
            if (!user.WorkNumber.IsNullOrWhiteSpace())
            {
                //验证工号是否唯一
                var has = _userRepository.GetAll().FirstOrDefault(ite => ite.WorkNumber == user.WorkNumber);
                if (has != null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "工号重复，请换一个工号");
                }
            }
            
            (await _userManager.CreateAsync(user)).CheckErrors(LocalizationManager); ;

            if (input.RoleNames != null)
            {
                (await _userManager.SetRoles(user, input.RoleNames)).CheckErrors(LocalizationManager);
            }

            foreach (var item in input.OrgPostIds)
            {
                var orgPostModel = await _organizationUnitPostsRepository.GetAsync(item);
                var userPost = new UserPosts() { Id = Guid.NewGuid(), UserId = user.Id, PostId = orgPostModel.PostId, OrgPostId = orgPostModel.Id, OrgId = orgPostModel.OrganizationUnitId };
                await _userPostsRepository.InsertAsync(userPost);

            }
            foreach (var item in input.RelationSystemIds)
            {
                var r_s = new ContractWithSystem() { Id = Guid.NewGuid(), UserId = user.Id, SystemId = item };
                await _contractWithSystemRepository.InsertAsync(r_s);
            }

            CurrentUnitOfWork.SaveChanges();
            return user.Id;
        }


        /// <summary>
        ///获取name的中文首拼字符串 （唯一性验证）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<string> MakeUserName(string name)
        {
            var acount = name.ToChineseSpell();
            if (acount.IsNullOrWhiteSpace())
            {
                return "";
            }
            else
            {
                var doCount = 0;
                var preAcount = "";
                while (doCount < 50)
                {
                    var exitFlag = false;
                    var do_preAcount = "";

                    if (doCount == 0)
                        do_preAcount = acount;
                    else
                        do_preAcount = $"{acount}{doCount}";
                    exitFlag = await _userRepository.GetAll().AnyAsync(r => r.UserName == do_preAcount);
                    if (exitFlag)
                        doCount++;
                    else
                    {
                        preAcount = do_preAcount;
                        break;
                    }
                }
                return preAcount;
            }
        }
        /// <summary>
        /// 通过userid查找角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>角色列表</returns>
        public List<string> GetRoles(long userId)
        {
            var userRoles = _userRoleRepository.GetAll().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            return _roleRepository.GetAll().Where(x => userRoles.Contains(x.Id)).Select(x => x.Name).ToList();
        }
    }
}
