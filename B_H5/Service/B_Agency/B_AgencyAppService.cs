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
using Abp.Authorization;

namespace B_H5
{
    public class B_AgencyAppService : FRMSCoreAppServiceBase, IB_AgencyAppService
    {
        private readonly IRepository<B_Agency, Guid> _repository;
        private readonly IRepository<B_AgencyApply, Guid> _b_AgencyApplyRepository;
        private readonly IRepository<B_AgencyGroup, Guid> _b_AgencyGroupRepository;
        private readonly IRepository<B_AgencyGroupRelation, Guid> _b_AgencyGroupRelationRepository;

        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<B_AgencyLevel, Guid> _b_AgencyLevelepository;
        private readonly IRepository<ZCYX.FRMSCore.Authorization.Users.User, long> _userRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public B_AgencyAppService(IRepository<B_Agency, Guid> repository, IRepository<AbpDictionary, Guid> abpDictionaryrepository
            , IUnitOfWorkManager unitOfWorkManager, IAbpFileRelationAppService abpFileRelationAppService, IRepository<B_AgencyLevel, Guid> b_AgencyLevelepository
            , IRepository<ZCYX.FRMSCore.Authorization.Users.User, long> userRepository, IRepository<B_AgencyApply, Guid> b_AgencyApplyRepository
            , IRepository<B_AgencyGroup, Guid> b_AgencyGroupRepository, IRepository<B_AgencyGroupRelation, Guid> b_AgencyGroupRelationRepository

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
            _b_AgencyApplyRepository = b_AgencyApplyRepository;
            _b_AgencyGroupRepository = b_AgencyGroupRepository;
            _b_AgencyGroupRelationRepository = b_AgencyGroupRelationRepository;

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
                if (businessIds.Count > 0)
                {
                    var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
                    {
                        BusinessIds = businessIds,
                        BusinessType = AbpFileBusinessType.代理头像
                    });
                    foreach (var item in ret)
                        if (fileGroups.Any(r => r.BusinessId == item.Id.ToString()))
                        {
                            var fileModel = fileGroups.FirstOrDefault(r => r.BusinessId == item.Id.ToString());
                            if (fileModel != null)
                            {
                                var files = fileModel.Files;
                                if (files.Count > 0)
                                    item.File = files.FirstOrDefault();
                            }

                        }
                }
                return new PagedResultDto<B_AgencyListOutputDto>(toalCount, ret);
            }


        }


        /// <summary>
        /// 后台查看所有代理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyManagerListOutputDto>> GetManagerList(GetB_AgencyManagerListInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            join u in UserManager.Users on a.UserId equals u.Id
                            join b in _b_AgencyLevelepository.GetAll() on a.AgencyLevelId equals b.Id
                            select new B_AgencyManagerListOutputDto()
                            {
                                Id = a.Id,
                                AgenCyCode = a.AgenCyCode,
                                UserName = u.Name,
                                AgencyLevelName = b.Name,
                                AgencyLevelId = b.Id,
                                WxId = a.WxId,
                                Tel = u.PhoneNumber,
                                Status = a.Status,
                                CreationTime = a.CreationTime,

                            };

                query = query.WhereIf(input.AgencyLevelId.HasValue, r => r.AgencyLevelId == input.AgencyLevelId.Value)
                    .WhereIf(input.Status.HasValue, r => r.Status == input.Status)
                    .WhereIf(input.StartDate.HasValue, r => r.CreationTime >= input.StartDate.Value)
                    .WhereIf(input.EndDate.HasValue, r => r.CreationTime <= input.EndDate.Value)
                    .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.AgenCyCode.Contains(input.SearchKey) || r.UserName.Contains(input.SearchKey));

                var toalCount = await query.CountAsync();
                var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
                return new PagedResultDto<B_AgencyManagerListOutputDto>(toalCount, ret);
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
                            UserName = u.Name,
                            ApplyId = a.ApplyId,
                            PNumber = a.PNumber,


                        };
            var model = await query.FirstOrDefaultAsync();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var fielRet = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.ApplyId.ToString(),
                BusinessType = (int)AbpFileBusinessType.代理头像
            });
            if (fielRet.Count() > 0)
                model.File = fielRet.FirstOrDefault();


            model.CredentFiles = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.ApplyId.ToString(),
                BusinessType = (int)AbpFileBusinessType.申请代理打款凭证
            });


            model.HandleCredentFiles = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.ApplyId.ToString(),
                BusinessType = (int)AbpFileBusinessType.申请代理手持证件
            });
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
                Id = Guid.NewGuid(),
                UserId = ret.Id,
                AgencyLevel = level1Model.Level,
                AgencyLevelId = level1Model.Id,
                AgenCyCode = DateTime.Now.DateTimeToStamp().ToString(),
                Provinces = input.Provinces,
                County = input.County,
                City = input.City,
                Address = input.Address,
                Type = input.Type,
                SignData = input.SignData,
                Agreement = input.Agreement,
                WxId = input.WxId,
                P_Id = input.P_Id,
                OriginalPid = input.P_Id,
                PNumber = input.PNumber
            };



            await _repository.InsertAsync(newmodel);

            var group = new B_AgencyGroup()
            {
                Id = Guid.NewGuid(),
                LeaderAgencyId = newmodel.Id,
            };
            await _b_AgencyGroupRepository.InsertAsync(group);

            var groupRelation = new B_AgencyGroupRelation()
            {
                Id = Guid.NewGuid(),
                AgencyId = newmodel.Id,
                GroupId = group.Id,
                IsGroupLeader = true,

            };
            await _b_AgencyGroupRelationRepository.InsertAsync(groupRelation);


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
                dbmodel.PNumber = input.PNumber;
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

        /// <summary>
        /// 解封
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Enable(EntityDto<Guid> input)
        {
            var model = await _repository.GetAsync(input.Id);
            model.Status = B_AgencyAcountStatusEnum.正常;
            var user = await _userRepository.GetAsync(model.UserId);
            user.IsActive = true;

        }



        /// <summary>
        /// 绑定openId
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task RegistWxOpenId(string openId)
        {
            if (string.IsNullOrWhiteSpace(openId))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "用户openId不能为空！");
            var model = await _repository.GetAll().FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该代理不存在！");
            else
            {
                if (model.OpenId != openId)
                {
                    model.OpenId = openId;
                    await _repository.UpdateAsync(model);
                }
            }
        }





    }
}