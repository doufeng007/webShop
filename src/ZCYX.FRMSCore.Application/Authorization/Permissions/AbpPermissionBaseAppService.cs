using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Authorization
{
    public class AbpPermissionBaseAppService : AsyncCrudAppService<AbpPermissionBase, AbpPermissionBaseDto, long, PagedResultRequestDto, CreateAbpPermissionBaseInput, AbpPermissionBaseDto>, IAbpPermissionBaseAppService
    {
        private readonly IRepository<AbpPermissionBase, long> _repository;
        public AbpPermissionBaseAppService(IRepository<AbpPermissionBase, long> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public List<AbpPermissionBaseDto> GetByMoudleName(string moudleName)
        {
            var ret = new List<AbpPermissionBaseDto>();
            var data = _repository.GetAll().IgnoreQueryFilters().Where(r => r.TenantId == null && r.MoudleName == moudleName).OrderBy(r => r.Order).ToList();
            foreach (var item in data)
            {
                var entity = new AbpPermissionBaseDto()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Description = item.Description,
                    DisplayName = item.DisplayName,
                    MoudleName = item.MoudleName,
                    ParentId = item.ParentId,
                    TenantId = item.TenantId,
                    Order = item.Order,

                };
                ret.Add(entity);
            }

            return ret;

        }

        public override async Task<AbpPermissionBaseDto> Create(CreateAbpPermissionBaseInput input)
        {
            var query = await _repository.FirstOrDefaultAsync(r => r.Code == input.Code);
            if (query != null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr,"权限编码唯一");
            return await base.Create(input);
        }


        public override async Task<AbpPermissionBaseDto> Update(AbpPermissionBaseDto input)
        {
            var query = await _repository.FirstOrDefaultAsync(r => r.Code == input.Code && r.Id != input.Id);
            if (query != null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "权限编码唯一");
            return await base.Update(input);
        }


        public override Task Delete(EntityDto<long> input)
        {
            var childs = GetSonID(input.Id);
            foreach (var item in childs)
                base.Delete(new EntityDto<long>() { Id = item.Id });
            return base.Delete(input);
        }


        public IEnumerable<AbpPermissionBase> GetSonID(long p_id)
        {
            var query = from c in _repository.GetAll()
                        where c.ParentId == p_id
                        select c;
            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.Id)));
        }


    }
}
