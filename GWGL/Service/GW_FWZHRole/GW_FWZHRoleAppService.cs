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

namespace GWGL
{
    public class GW_FWZHRoleAppService : FRMSCoreAppServiceBase, IGW_FWZHRoleAppService
    {
        private readonly IRepository<GW_FWZHRole, Guid> _repository;

        public GW_FWZHRoleAppService(IRepository<GW_FWZHRole, Guid> repository

        )
        {
            this._repository = repository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<GW_FWZHRoleListOutputDto>> GetList(GetGW_FWZHRoleListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new GW_FWZHRoleListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Code = a.Code,
                            StartIndex = a.StartIndex,
                            AutoCoding = a.AutoCoding,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<GW_FWZHRoleListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<GW_FWZHRoleOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<GW_FWZHRoleOutputDto>();
        }
        /// <summary>
        /// 添加一个GW_FWZHRole
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateGW_FWZHRoleInput input)
        {
            if (_repository.GetAll().Any(r => r.Name == input.Name))
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "发文机关重复。");
            if (_repository.GetAll().Any(r => r.Code == input.Code))
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "发文机关代字重复。");
            var newmodel = new GW_FWZHRole()
            {
                Name = input.Name,
                Code = input.Code,
                StartIndex = input.StartIndex,
                AutoCoding = input.AutoCoding
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个GW_FWZHRole
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateGW_FWZHRoleInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                if (_repository.GetAll().Any(r => r.Id != input.Id && r.Name == input.Name))
                    throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "发文机关重复。");
                if (_repository.GetAll().Any(r => r.Id != input.Id && r.Code == input.Code))
                    throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "发文机关代字重复。");

                dbmodel.Name = input.Name;
                dbmodel.Code = input.Code;
                dbmodel.StartIndex = input.StartIndex;
                dbmodel.AutoCoding = input.AutoCoding;

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