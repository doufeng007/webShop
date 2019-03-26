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
using Abp.WorkFlowDictionary;
using Abp.Domain.Uow;

namespace B_H5
{
    public class B_AgencyAppService : FRMSCoreAppServiceBase, IB_AgencyAppService
    {
        private readonly IRepository<B_Agency, Guid> _repository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;


        public B_AgencyAppService(IRepository<B_Agency, Guid> repository, IRepository<AbpDictionary, Guid> abpDictionaryrepository
            , IUnitOfWorkManager unitOfWorkManager, IAbpFileRelationAppService abpFileRelationAppService

        )
        {
            this._repository = repository;
            _abpDictionaryrepository = abpDictionaryrepository;
            _unitOfWorkManager = unitOfWorkManager;
            _abpFileRelationAppService = abpFileRelationAppService;

        }

        /// <summary>
        /// 代理人列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyListOutputDto>> GetList(GetB_AgencyListInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            join u in UserManager.Users on a.UserId equals u.Id
                            join b in _abpDictionaryrepository.GetAll() on a.AgencyLevel equals b.Id
                            select new B_AgencyListOutputDto()
                            {
                                Id = a.Id,
                                UserId = a.UserId,
                                UserName = u.Name,
                                AgencyLevelName = b.Title,
                                AgenCyCode = a.AgenCyCode,
                                CreationTime = a.CreationTime,
                            };
                var toalCount = await query.CountAsync();
                var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
                var businessIds = ret.Select(r => r.Id.ToString()).ToList();
                var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
                {
                    BusinessIds = businessIds,
                    BusinessType = AbpFileBusinessType.代理头像
                });
                foreach (var item in ret)
                    if (fileGroups.Any(r => r.BusinessId == item.Id.ToString()))
                        item.File = fileGroups.FirstOrDefault(r => r.BusinessId == item.Id.ToString()).Files.FirstOrDefault();
                return new PagedResultDto<B_AgencyListOutputDto>(toalCount, ret);
            }


        }

        /// <summary>
        /// 获取代理人详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_AgencyOutputDto> Get(EntityDto<Guid> input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join u in UserManager.Users on a.UserId equals u.Id
                        join b in _abpDictionaryrepository.GetAll() on a.AgencyLevel equals b.Id
                        where a.Id == input.Id
                        select new B_AgencyOutputDto()
                        {
                            Address = a.Address,
                            AgenCyCode = a.AgenCyCode,
                            AgencyLevelName = b.Title,
                            City = a.City,
                            County = a.County,
                            Id = a.Id,
                            PhoneNumber = u.PhoneNumber,
                            Provinces = a.Provinces,
                            SignData = a.SignData,
                            UserId = a.UserId,
                            UserName = u.Name
                        };
            var model = await query.FirstOrDefaultAsync();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model;
        }


        /// <summary>
        /// 添加一个代理
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_AgencyInput input)
        {
            var newmodel = new B_Agency()
            {
                UserId = 1,
                AgencyLevel = input.AgencyLevel,
                AgenCyCode = input.AgenCyCode,
                Provinces = input.Provinces,
                County = input.County,
                City = input.City,
                Address = input.Address,
                Type = input.Type,
                SignData = input.SignData,
                Agreement = input.Agreement,
                Status = input.Status,
            };

            await _repository.InsertAsync(newmodel);

        }


        /// <summary>
        /// 修改一个代理
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_AgencyInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                dbmodel.AgencyLevel = input.AgencyLevel;
                dbmodel.AgenCyCode = input.AgenCyCode;
                dbmodel.Provinces = input.Provinces;
                dbmodel.County = input.County;
                dbmodel.City = input.City;
                dbmodel.Address = input.Address;
                dbmodel.Type = input.Type;
                dbmodel.SignData = input.SignData;
                dbmodel.Agreement = input.Agreement;
                dbmodel.Status = input.Status;
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
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }
    }
}