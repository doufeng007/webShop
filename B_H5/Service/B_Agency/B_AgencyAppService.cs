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
using Abp;
using ZCYX.FRMSCore.Users;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using Abp.Reflection.Extensions;

namespace B_H5
{
    public class B_AgencyAppService : FRMSCoreAppServiceBase, IB_AgencyAppService
    {
        private readonly IRepository<B_Agency, Guid> _repository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<B_AgencyLevel, Guid> _b_AgencyLevelepository;
        private readonly IRepository<ZCYX.FRMSCore.Authorization.Users.User, long> _userRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public B_AgencyAppService(IRepository<B_Agency, Guid> repository, IRepository<AbpDictionary, Guid> abpDictionaryrepository
            , IUnitOfWorkManager unitOfWorkManager, IAbpFileRelationAppService abpFileRelationAppService, IRepository<B_AgencyLevel, Guid> b_AgencyLevelepository
            , IRepository<ZCYX.FRMSCore.Authorization.Users.User, long> userRepository

        )
        {
            this._repository = repository;
            _abpDictionaryrepository = abpDictionaryrepository;
            _unitOfWorkManager = unitOfWorkManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _b_AgencyLevelepository = b_AgencyLevelepository;
            _userRepository = userRepository;
            var coreAssemblyDirectoryPath = typeof(B_AgencyAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);

        }

        /// <summary>
        /// 微信端-渠道列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyListOutputDto>> GetList(GetB_AgencyListInput input)
        {
            var b_AgencyId = Guid.Empty;
            if (input.UserId.HasValue)
            {
                var model = _repository.FirstOrDefault(r => r.UserId == input.UserId);
                if (model == null)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "查询用户不存在");
                else
                {
                    b_AgencyId = model.Id;
                }
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            join u in UserManager.Users on a.UserId equals u.Id
                            join b in _b_AgencyLevelepository.GetAll() on a.AgencyLevelId equals b.Id
                            where (!input.UserId.HasValue || a.P_Id == b_AgencyId)
                            && a.Type == input.Type
                            && (!input.AgencyLevelId.HasValue || a.AgencyLevelId == input.AgencyLevelId.Value)
                            select new B_AgencyListOutputDto()
                            {
                                Id = a.Id,
                                UserId = a.UserId,
                                UserName = u.Name,
                                AgencyLevelName = b.Name,
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
                        join b in _b_AgencyLevelepository.GetAll() on a.AgencyLevelId equals b.Id
                        where a.Id == input.Id
                        select new B_AgencyOutputDto()
                        {
                            Address = a.Address,
                            AgenCyCode = a.AgenCyCode,
                            AgencyLevelName = b.Name,
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
            var fielRet = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.代理头像
            });
            if (fielRet.Count() > 0)
                model.File = fielRet.FirstOrDefault();
            return model;
        }


        /// <summary>
        /// 后台管理员添加一级代理
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_AgencyInput input)
        {
            var level1Query = _b_AgencyLevelepository.GetAll().Where(r => r.Level == 1);
            if (level1Query.Count() == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理级别未添加，请先添加代理级别");
            else if (level1Query.Count() > 1)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理级别错误");
            var level1Model = level1Query.FirstOrDefault();
            var userService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IUserAppService>();
            var userCreateInput = new ZCYX.FRMSCore.Users.Dto.CreateUserDto()
            {
                MainPostId = new Guid(_appConfiguration["App:PrimaryAgencyOrgPostId"]),
                Name = input.Name,
                OrganizationUnitId = _appConfiguration["App:PrimaryAgencyOrgId"].ToLong(),
                OrgPostIds = new List<Guid>() { new Guid(_appConfiguration["App:PrimaryAgencyOrgPostId"]), },
                Password = "123qwe",
                PhoneNumber = input.Tel,
                UserName = input.Tel,
                Surname = input.Name,
                Sex = null,
                IsActive = true,
                EmailAddress = $"{input.Tel}@abp.com",
            };

            var ret = await userService.Create(userCreateInput);
            var newmodel = new B_Agency()
            {
                UserId = ret.Id,
                AgencyLevel = level1Model.Level,
                AgencyLevelId = level1Model.Id,
                AgenCyCode = input.AgenCyCode,
                Provinces = input.Provinces,
                County = input.County,
                City = input.City,
                Address = input.Address,
                Type = input.Type,
                SignData = input.SignData,
                Agreement = input.Agreement,
                WxId = input.WxId,
                P_Id = input.P_Id,
                OriginalPid = input.P_Id
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
                var user = _userRepository.FirstOrDefault(r => r.Id == dbmodel.UserId);
                user.Name = input.Name;
                user.PhoneNumber = input.Tel;
                await _userRepository.UpdateAsync(user);

                dbmodel.AgenCyCode = input.AgenCyCode;
                dbmodel.Provinces = input.Provinces;
                dbmodel.County = input.County;
                dbmodel.City = input.City;
                dbmodel.Address = input.Address;
                dbmodel.Type = input.Type;
                dbmodel.SignData = input.SignData;
                dbmodel.Agreement = input.Agreement;
                //dbmodel.Status = input.Status;
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
            var model = _repository.Get(input.Id);
            await _repository.DeleteAsync(model);
            await _userRepository.DeleteAsync(r => r.Id == model.UserId);
        }

        public async Task Disable(EntityDto<Guid> input)
        {
            var model = await _repository.GetAsync(input.Id);
            model.Status = (int)B_AgencyAcountStatusEnum.封号;
            var user = await _userRepository.GetAsync(model.UserId);
            user.IsActive = false;

        }

    }
}