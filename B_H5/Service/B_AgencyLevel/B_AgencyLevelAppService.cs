using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp.Runtime.Caching;

namespace B_H5
{
    public class B_AgencyLevelAppService : FRMSCoreAppServiceBase, IB_AgencyLevelAppService
    {
        private readonly IRepository<B_AgencyLevel, Guid> _repository;
        private readonly ICacheManager _cacheManager;

        public B_AgencyLevelAppService(IRepository<B_AgencyLevel, Guid> repository, ICacheManager cacheManager

        )
        {
            this._repository = repository;
            _cacheManager = cacheManager;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyLevelListOutputDto>> GetListAsync(GetB_AgencyLevelListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new B_AgencyLevelListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Level = a.Level,
                            IsDefault = a.IsDefault,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_AgencyLevelListOutputDto>(toalCount, ret);
        }


        public PagedResultDto<B_AgencyLevelListOutputDto> GetList(GetB_AgencyLevelListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new B_AgencyLevelListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Level = a.Level,
                            IsDefault = a.IsDefault,
                            CreationTime = a.CreationTime

                        };
            var toalCount = query.Count();
            var ret = query.OrderBy(r => r.Level).PageBy(input).ToList();
            return new PagedResultDto<B_AgencyLevelListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_AgencyLevelOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_AgencyLevelOutputDto>();
        }
        /// <summary>
        /// 添加一个B_AgencyLevel
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_AgencyLevelInput input)
        {
            var query = _repository.GetAll();
            if (query.Count() == 0)
                input.Level = 1;
            else
                input.Level = query.Max(r => r.Level) + 1;
            var newmodel = new B_AgencyLevel()
            {
                Name = input.Name,
                Level = input.Level,
            };

            await _repository.InsertAsync(newmodel);

            _cacheManager.GetCache("B_AgencyLevelList").Remove("B_AgencyLevelList");
            //_cacheManager.GetCache(cacheName).SetAsync(flowVersionNumStr, data);

        }

        /// <summary>
        /// 修改一个B_AgencyLevel
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_AgencyLevelInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Name = input.Name;
                //dbmodel.Level = input.Level;
                //dbmodel.IsDefault = input.IsDefault;

                await _repository.UpdateAsync(dbmodel);

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            //await _repository.DeleteAsync(x => x.Id == input.Id);
        }

        private List<B_AgencyLevelListOutputDto> GetAgencyLevelFromCache()
        {
            return _cacheManager
               .GetCache("B_AgencyLevelList")
               .Get<string, List<B_AgencyLevelListOutputDto>>("B_AgencyLevelList", f =>
               {
                   var ret = GetList(new GetB_AgencyLevelListInput() { MaxResultCount = 1000, SkipCount = 0 });
                   return ret.Items.ToList();
               });
        }


        public B_AgencyLevelListOutputDto GetAgencyLevelFromCache(Guid id)
        {
            var list = GetAgencyLevelFromCache();
            var ret = list.FirstOrDefault(r => r.Id == id);
            if (ret == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "获取代理级别失败");
            return ret;

        }
    }
}