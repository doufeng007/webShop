using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Caching;
using ZCYX.FRMSCore.Model;
using Abp.Application.Navigation;
using Abp.Localization;

namespace ZCYX.FRMSCore.Authorization.Permissions
{
    public class AbpMenuBaseAppService : AsyncCrudAppService<AbpMenuBase, AbpMenuBaseDto, long, PagedResultRequestDto, CreateAbpMenuBaseInput, AbpMenuBaseDto>, IAbpMenuBaseAppService
    {
        private readonly IRepository<AbpMenuBase, long> _repository;
        private readonly INavigationManager _iNavigationManager;
        private readonly ICacheManager _cacheManager;
        public AbpMenuBaseAppService(IRepository<AbpMenuBase, long> repository, ICacheManager cacheManager, INavigationManager iNavigationManager)
            : base(repository)
        {
            _repository = repository;
            _cacheManager = cacheManager;
            _iNavigationManager = iNavigationManager;
        }

        [RemoteService(false)]
        public List<AbpMenuBaseDto> GetMenuFromCache()
        {
            var cacheName = "AbpAllMenus";
            return _cacheManager
               .GetCache(cacheName)
               .Get<string, List<AbpMenuBaseDto>>(cacheName, f => GetAllMenu());
        }


        public List<AbpMenuBaseDto> GetAllMenu()
        {
            var ret = new List<AbpMenuBaseDto>();
            var query = _repository.GetAll().OrderBy(r => r.ParentId).ThenBy(r => r.Order).ToList();
            foreach (var item in query)
            {
                var entity = new AbpMenuBaseDto()
                {
                    Code = item.Code,
                    CustomData = item.CustomData,
                    Description = item.Description,
                    DisplayName = item.DisplayName,
                    Icon = item.Icon,
                    Id = item.Id,
                    IsEnabled = item.IsEnabled,
                    IsVisible = item.IsVisible,
                    MoudleName = item.MoudleName,
                    Order = item.Order,
                    ParentId = item.ParentId,
                    RequiredPermissionName = item.RequiredPermissionName,
                    RequiresAuthentication = item.RequiresAuthentication,
                    Target = item.Target,
                    Url = item.Url,
                };
                ret.Add(entity);
            }
            return ret;
        }

        public async Task<List<AbpMenuBaseDto>> GetByMoudleName(string moudleName)
        {
            var ret = new List<AbpMenuBaseDto>();
            var data = await _repository.GetAll().Where(r => r.MoudleName == moudleName).OrderBy(r => r.Order).ToListAsync();
            foreach (var item in data)
            {
                var entity = new AbpMenuBaseDto()
                {
                    Code = item.Code,
                    Description = item.Description,
                    DisplayName = item.DisplayName,
                    Icon = item.Icon,
                    Id = item.Id,
                    MoudleName = item.MoudleName,
                    Order = item.Order,
                    ParentId = item.ParentId,
                    TenantId = item.TenantId
                };
                ret.Add(entity);
            }
            return ret;

        }

        public override async Task<AbpMenuBaseDto> Create(CreateAbpMenuBaseInput input)
        {
            var query = await _repository.FirstOrDefaultAsync(r => r.Code == input.Code);
            if (query != null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "菜单编码唯一");
            var info = await base.Create(input);
            await CurrentUnitOfWork.SaveChangesAsync();
            UpdateMenu();
            return info;
        }


        public override async Task<AbpMenuBaseDto> Update(AbpMenuBaseDto input)
        {
            var query = await _repository.FirstOrDefaultAsync(r => r.Code == input.Code && r.Id != input.Id);
            if (query != null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "菜单编码唯一");
            var info = await base.Update(input);
            await CurrentUnitOfWork.SaveChangesAsync();
            UpdateMenu();
            return info;
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }

        public void UpdateMenu()
        {
            foreach (var item in _iNavigationManager.MainMenu.Items.ToList())
            {
                _iNavigationManager.MainMenu.Items.Remove(item);
            }
            var list = GetAllMenu().Where(x => x.ParentId == null).OrderBy(x => x.Order).GroupBy(x => x.MoudleName).ToList();
            foreach (var item in list)
            {
                SetNavigationWithMouldName(item.Key);
            }
        }
        public void SetNavigationWithMouldName(string mouldName)
        {
            var menus = GetAllMenu();

            var moudleMenus = menus.Where(r => r.MoudleName == mouldName).OrderBy(x => x.Code);


            foreach (var item in moudleMenus)
            {
                if (!item.ParentId.HasValue)
                {
                    var oneMenu = new MenuItemDefinition(item.Code, L(item.DisplayName),
                      item.Icon, item.Url, item.RequiresAuthentication, item.RequiredPermissionName, item.Order, item.CustomData,
                      null, item.Target, item.IsEnabled, item.IsVisible, null);
                    _iNavigationManager.MainMenu.AddItem(oneMenu);

                }
                else
                {
                    var parentMenuData = menus.FirstOrDefault(r => r.Id == item.ParentId);
                    if (parentMenuData == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "父菜单不存在");
                    var allMenuItems = new List<MenuItemDefinition>();
                    GetItemMenuByCodeDiGui(allMenuItems, _iNavigationManager.MainMenu.Items.ToList());
                    // var parentMenu = context.Manager.MainMenu.Items.FirstOrDefault(ite => ite.Name == parentMenuData.Code);
                    var parentMenu = allMenuItems.FirstOrDefault(ite => ite.Name == parentMenuData.Code);
                    if (parentMenu == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到父菜单");
                    var oneMenu = new MenuItemDefinition(item.Code, L(item.DisplayName),
                        item.Icon, item.Url, item.RequiresAuthentication, item.RequiredPermissionName, item.Order, item.CustomData,
                        null, item.Target, item.IsEnabled, item.IsVisible, null);
                    parentMenu.AddItem(oneMenu);
                }


            }
        }
        public void GetItemMenuByCodeDiGui(List<MenuItemDefinition> item, List<MenuItemDefinition> sourceItem)
        {

            foreach (var entity in sourceItem)
            {
                item.Add(entity);
                if (entity.Items.Count > 0)
                {
                    GetItemMenuByCodeDiGui(item, entity.Items.ToList());
                }


            }
        }

        public override Task Delete(EntityDto<long> input)
        {
            var childs = GetSonID(input.Id);
            foreach (var item in childs)
                base.Delete(new EntityDto<long>() { Id = item.Id });
            var info = base.Delete(input);
            CurrentUnitOfWork.SaveChanges();
            UpdateMenu();
            return info;
        }


        public IEnumerable<AbpMenuBase> GetSonID(long p_id)
        {
            var query = from c in _repository.GetAll()
                        where c.ParentId == p_id
                        select c;
            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.Id)));
        }

    }
}
