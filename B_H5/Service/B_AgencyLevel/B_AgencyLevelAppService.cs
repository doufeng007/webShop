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
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly IRepository<B_AgencyLevel, Guid> _repository;
        private readonly ICacheManager _cacheManager;

        public B_AgencyLevelAppService(IRepository<B_AgencyLevel, Guid> repository, ICacheManager cacheManager, IRepository<B_Agency, Guid> b_AgencyRepository

        )
        {
            this._repository = repository;
            _cacheManager = cacheManager;
            _b_AgencyRepository = b_AgencyRepository;

        }

        /// <summary>
        /// 后台-等级管理
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyLevelListOutputDto>> GetListAsync(GetB_AgencyLevelListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_AgencyRepository.GetAll() on a.Id equals b.AgencyLevelId into g
                        select new B_AgencyLevelListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Level = a.Level,
                            IsDefault = a.IsDefault,
                            CreationTime = a.CreationTime,
                            AgencyCount = g.Count(),

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_AgencyLevelListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 后台-等级金额
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyLevelAmoutListOutputDto>> GetAmoutListAsync(GetB_AgencyLevelListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        select new B_AgencyLevelAmoutListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Level = a.Level,
                            CreationTime = a.CreationTime,
                            Deposit = a.Deposit,
                            Discount = a.Discount,
                            FirstRechargeAmout = a.FirstRechargeAmout,
                            RecommendAmout = a.RecommendAmout

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_AgencyLevelAmoutListOutputDto>(toalCount, ret);
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
        /// 新增代理级别
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_AgencyLevelInput input)
        {
            if (_repository.GetAll().Any(r => r.Level == input.Level))
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"已存在{input.Level}级代理名称");
            }


            if (input.Level != 1)
            {
                if (!_repository.GetAll().Any(r => r.Level == 1))
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"请先创建一级代理名称");
                }
            }



            await _repository.InsertAsync(new B_AgencyLevel()
            {
                Id = Guid.NewGuid(),
                Level = input.Level,
                Name = input.Name,

            });

            _cacheManager.GetCache("B_AgencyLevelList").Remove("B_AgencyLevelList");

        }

        /// <summary>
        /// 修改代理等级 -设置等级金额
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
                dbmodel.RecommendAmout = input.RecommendAmout;
                dbmodel.Deposit = input.Deposit;
                dbmodel.Discount = input.Discount;
                dbmodel.FirstRechargeAmout = input.FirstRechargeAmout;

                await _repository.UpdateAsync(dbmodel);

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 逻辑代理等级
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            if (_b_AgencyRepository.GetAll().Any(r => r.AgencyLevelId == input.Id))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "不能删除有代理的代理等级");
            else
                await _repository.DeleteAsync(x => x.Id == input.Id);
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
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "获取代理等级失败");
            return ret;

        }

        public B_AgencyLevelListOutputDto GetAgencyLevelOneFromCache()
        {
            var list = GetAgencyLevelFromCache();
            var ret = list.FirstOrDefault(r => r.Level == 1);
            if (ret == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "获取代理等级失败");
            return ret;
        }


    }
}