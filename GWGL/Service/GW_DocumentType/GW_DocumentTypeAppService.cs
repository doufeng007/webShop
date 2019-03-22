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
    public class GW_DocumentTypeAppService : FRMSCoreAppServiceBase, IGW_DocumentTypeAppService
    {
        private readonly IRepository<GW_DocumentType, Guid> _repository;

        public GW_DocumentTypeAppService(IRepository<GW_DocumentType, Guid> repository

        )
        {
            this._repository = repository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<GW_DocumentTypeListOutputDto>> GetList(GetGW_DocumentTypeListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new GW_DocumentTypeListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Type = a.Type,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                            Type_Name = ((GW_DocumentTypeEnmu)a.Type).ToString(),

                        };
            if (input.Status.HasValue)
                query = query.Where(r => r.Status == (int)input.Status.Value);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<GW_DocumentTypeListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<GW_DocumentTypeOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<GW_DocumentTypeOutputDto>();
            ret.Type_Name = ((GW_DocumentTypeEnmu)ret.Type).ToString();
            ret.StatusTitle = ((GW_EmployeesSignStatusEnmu)ret.Status).ToString();
            return ret;

        }
        /// <summary>
        /// 添加一个GW_DocumentType
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateGW_DocumentTypeInput input)
        {
            if (_repository.GetAll().Any(r => r.Name == input.Name))
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "标题重复。");
            var newmodel = new GW_DocumentType()
            {
                Name = input.Name,
                Type = (int)input.Type,
                Status = (int)input.Status
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个GW_DocumentType
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateGW_DocumentTypeInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                if (_repository.GetAll().Any(r => r.Id != input.Id && r.Name == input.Name))
                    throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "标题重复。");

                dbmodel.Name = input.Name;
                dbmodel.Type = (int)input.Type;
                dbmodel.Status = (int)input.Status;

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