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

namespace B_H5
{
    public class B_GoodsAppService : FRMSCoreAppServiceBase, IB_GoodsAppService
    {
        private readonly IRepository<B_Goods, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public B_GoodsAppService(IRepository<B_Goods, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService

        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_GoodsListOutputDto>> GetList(GetB_GoodsListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new B_GoodsListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            CategroyId = a.CategroyId,
                            Price = a.Price,
                            Pirce1 = a.Pirce1,
                            Price2 = a.Price2,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_GoodsListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 根据商品类别获取商品类别----云仓提货 商品类别
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_GoodsListOutputDto>> GetListByCategroyId(GetB_GoodsListByCategroyIdInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where a.CategroyId == input.CategroyId
                        select new B_GoodsListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            CategroyId = a.CategroyId,
                            Price = a.Price,
                            Pirce1 = a.Pirce1,
                            Price2 = a.Price2,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            var businessIds = ret.Select(r => r.Id.ToString()).ToList();
            var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
            {
                BusinessIds = businessIds,
                BusinessType = AbpFileBusinessType.商品缩略图
            });
            foreach (var item in ret)
                if (fileGroups.Any(r => r.BusinessId == item.Id.ToString()))
                    item.File = fileGroups.FirstOrDefault(r => r.BusinessId == item.Id.ToString()).Files.FirstOrDefault();

            return new PagedResultDto<B_GoodsListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_GoodsOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_GoodsOutputDto>();
        }
        /// <summary>
        /// 添加一个B_Goods
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_GoodsInput input)
        {
            var newmodel = new B_Goods()
            {
                Name = input.Name,
                CategroyId = input.CategroyId,
                Price = input.Price,
                Pirce1 = input.Pirce1,
                Price2 = input.Price2
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个B_Goods
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_GoodsInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Name = input.Name;
                dbmodel.CategroyId = input.CategroyId;
                dbmodel.Price = input.Price;
                dbmodel.Pirce1 = input.Pirce1;
                dbmodel.Price2 = input.Price2;

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