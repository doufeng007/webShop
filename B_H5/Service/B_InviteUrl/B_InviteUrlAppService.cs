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
using Abp;
using Abp.Authorization;

namespace B_H5
{
    public class B_InviteUrlAppService : FRMSCoreAppServiceBase, IB_InviteUrlAppService
    {
        private readonly IRepository<B_InviteUrl, Guid> _repository;
        private readonly IRepository<B_AgencyLevel, Guid> _B_AgencyLevelRepository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;

        public B_InviteUrlAppService(IRepository<B_InviteUrl, Guid> repository, IRepository<B_AgencyLevel, Guid> B_AgencyLevelRepository, IRepository<B_Agency, Guid> b_AgencyRepository

        )
        {
            this._repository = repository;
            _B_AgencyLevelRepository = B_AgencyLevelRepository;
            _b_AgencyRepository = b_AgencyRepository;


        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<B_InviteUrlListOutputDto>> GetList(GetB_InviteUrlListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _B_AgencyLevelRepository.GetAll() on a.AgencyLevel equals b.Id
                        where a.CreatorUserId.Value == AbpSession.UserId.Value
                        select new B_InviteUrlListOutputDto()
                        {
                            Id = a.Id,
                            AgencyLevel = a.AgencyLevel,
                            AgencyLevelName = b.Name,
                            ValidityDataType = a.ValidityDataType,
                            AvailableCount = a.AvailableCount,
                            Url = a.Url,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_InviteUrlListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 获取代理邀请详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_InviteUrlOutputDto> Get(EntityDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var query = from a in _repository.GetAll()
                        join b in _b_AgencyRepository.GetAll() on a.CreatorUserId.Value equals b.UserId
                        join u in UserManager.Users on b.UserId equals u.Id
                        where a.Id == input.Id
                        select new B_InviteUrlOutputDto()
                        {
                            Id = a.Id,
                            CreateAgencyTel = u.PhoneNumber,
                            Url = a.Url,
                            AgencyLevelId = a.AgencyLevel,
                            CreateAgencyAddress = b.Address,
                            AvailableCount = a.AvailableCount,
                            CreateAgencyLevelId = b.AgencyLevelId,
                            CreateAgencyName = u.Name,
                        };
            var ret = await query.FirstOrDefaultAsync();
            if (ret == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            ret.AgencyLevelName = service.GetAgencyLevelFromCache(ret.AgencyLevelId).Name;
            ret.CreateAgencyLevelName = service.GetAgencyLevelFromCache(ret.CreateAgencyLevelId).Name;
            return ret;
        }
        /// <summary>
        /// 添加一个B_InviteUrl
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Create(CreateB_InviteUrlInput input)
        {
            var newmodel = new B_InviteUrl()
            {
                AgencyLevel = input.AgencyLevel,
                ValidityDataType = input.ValidityDataType,
                AvailableCount = input.AvailableCount,
                Url = input.Url
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个B_InviteUrl
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_InviteUrlInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.AgencyLevel = input.AgencyLevel;
                dbmodel.ValidityDataType = input.ValidityDataType;
                dbmodel.AvailableCount = input.AvailableCount;
                dbmodel.Url = input.Url;

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