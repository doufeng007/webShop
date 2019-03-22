using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.Application.Services;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.Authorization;
using Abp;
using Abp.Domain.Uow;

namespace ZCYX.FRMSCore.Application
{
    [RemoteService(IsEnabled = false)]
    public class AbpOrganizationUnitsManager : ApplicationService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IOrganizationUnitSettings _organizationUnitSettings;
        private readonly UserManager _userManager;

        public AbpOrganizationUnitsManager(IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<User, long> userRepository,
            OrganizationUnitManager organizationUnitManager, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository
            , IOrganizationUnitSettings organizationUnitSettings, UserManager userManager)
        {
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitSettings = organizationUnitSettings;
            _userManager = userManager;
        }

        public virtual async Task AddToOrganizationUnitAsync(long userId, long ouId)
        {
            var isMain = true;
            await AddToOrganizationUnitAsync(await _userManager.GetUserByIdAsync(userId), await _organizationUnitRepository.GetAsync(ouId), isMain);
        }

        public virtual async Task AddToOrganizationUnitAsync(User user, OrganizationUnit ou, bool isMain = false)
        {
            var currentOus = await GetOrganizationUnitsAsync(user);

            if (currentOus.Any(cou => cou.Id == ou.Id))
            {
                return;
            }

            await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, currentOus.Count + 1);

            await _userOrganizationUnitRepository.InsertAsync(new WorkFlowUserOrganizationUnits() { TenantId = user.TenantId, UserId = user.Id, OrganizationUnitId = ou.Id, IsMain = isMain });
            if (isMain)
            {
                var currentUous = await GetUserOrganizationUnitsAsync(user);
                foreach (var item in currentUous)
                {
                    item.IsMain = false;
                }
            }
        }


        public virtual async Task SetOrganizationUnitsAsync(long userId, params long[] organizationUnitIds)
        {
            await SetOrganizationUnitsAsync(
                await _userManager.GetUserByIdAsync(userId),
                organizationUnitIds
                );
        }

        public virtual async Task SetOrganizationUnitsAsync(User user, params long[] organizationUnitIds)
        {
            if (organizationUnitIds == null)
            {
                organizationUnitIds = new long[0];
            }

            await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, organizationUnitIds.Length);

            var currentOus = await GetOrganizationUnitsAsync(user);

            //Remove from removed OUs
            foreach (var currentOu in currentOus)
            {
                if (!organizationUnitIds.Contains(currentOu.Id))
                {
                    await RemoveFromOrganizationUnitAsync(user, currentOu);
                }
            }

            //Add to added OUs
            foreach (var organizationUnitId in organizationUnitIds)
            {
                if (currentOus.All(ou => ou.Id != organizationUnitId))
                {
                    await AddToOrganizationUnitAsync(
                        user,
                        await _organizationUnitRepository.GetAsync(organizationUnitId)
                        );
                }
            }
        }


        [UnitOfWork]
        public virtual Task<List<WorkFlowOrganizationUnits>> GetOrganizationUnitsAsync(User user)
        {
            var query = from uou in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                        where uou.UserId == user.Id
                        select ou;

            return Task.FromResult(query.ToList());
        }

        [UnitOfWork]
        public virtual Task<List<WorkFlowUserOrganizationUnits>> GetUserOrganizationUnitsAsync(User user)
        {
            var query = from uou in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                        where uou.UserId == user.Id
                        select uou;

            return Task.FromResult(query.ToList());
        }

        private async Task CheckMaxUserOrganizationUnitMembershipCountAsync(int? tenantId, int requestedCount)
        {
            var maxCount = await _organizationUnitSettings.GetMaxUserMembershipCountAsync(tenantId);
            if (requestedCount > maxCount)
            {
                throw new AbpException($"Can not set more than {maxCount} organization unit for a user!");
            }
        }



        public virtual async Task<bool> IsInOrganizationUnitAsync(long userId, long ouId)
        {
            return await IsInOrganizationUnitAsync(
                await _userManager.GetUserByIdAsync(userId),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }

        public virtual async Task<bool> IsInOrganizationUnitAsync(User user, OrganizationUnit ou)
        {
            return await _userOrganizationUnitRepository.CountAsync(uou =>
                uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id
                ) > 0;
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(long userId, long ouId)
        {
            await RemoveFromOrganizationUnitAsync(
                await _userManager.GetUserByIdAsync(userId),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(User user, OrganizationUnit ou)
        {
            await _userOrganizationUnitRepository.DeleteAsync(uou => uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id);
        }


    }



}
