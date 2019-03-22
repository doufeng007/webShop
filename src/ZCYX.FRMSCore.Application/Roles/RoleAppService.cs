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
using Abp.UI;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Authorization;
using ZCYX.FRMSCore.Roles.Dto;
using Abp.AutoMapper;
using ZCYX.FRMSCore.Authorization.Permissions.Dto;
using System;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Roles
{
    [AbpAuthorize]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IRepository<OrganizationUnitPostsRole, Guid> _organizationUnitPostsRoleRepository;
        private readonly IRepository<Role> _roleRepository;

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager, IRepository<Role> roleRepository, IRepository<OrganizationUnitPostsRole, Guid> organizationUnitPostsRoleRepository)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _organizationUnitPostsRoleRepository = organizationUnitPostsRoleRepository;
        }
        public override async Task<RoleDto> Create(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            if (string.IsNullOrWhiteSpace(role.Name)) {
                role.Name = role.DisplayName.GetCnFirstChart() + DateTime.Now.ToString("MMddHHmm");//获取拼音
            }
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.Permissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }
        /// <summary>
        /// 获取内置角色
        /// </summary>
        /// <returns></returns>
        public async Task<List< RoleDto>> GetStaticRole()
        {

            var roles =await _roleRepository.GetAll().Where(ite => ite.IsStatic && ite.IsSelect).ToListAsync();
            return roles.MapTo<List<RoleDto>>();
        }
        public override async Task<RoleDto> Update(RoleDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var isstatic = role.IsStatic;
            var isselect = role.IsSelect;
            ObjectMapper.Map(input, role);
            role.IsStatic = isstatic;
            role.IsSelect = isselect;
            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.Permissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }
            _organizationUnitPostsRoleRepository.Delete(ite => ite.RoleName == role.Name);
            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions)
            ));
        }
        protected override IQueryable<Role> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions);
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto;

            if (input.Id.HasValue) //Editing existing role?
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = role.MapTo<RoleEditDto>();
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = permissions.MapTo<List<FlatPermissionDto>>().OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }
    }
}
