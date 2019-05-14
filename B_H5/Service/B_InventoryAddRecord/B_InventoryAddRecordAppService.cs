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
    public class B_InventoryAddRecordAppService : FRMSCoreAppServiceBase, IB_InventoryAddRecordAppService
    {
        private readonly IRepository<B_InventoryAddRecord, Guid> _repository;
        private readonly IRepository<B_Goods, Guid> _b_GoodsRepository;

        public B_InventoryAddRecordAppService(IRepository<B_InventoryAddRecord, Guid> repository, IRepository<B_Goods, Guid> b_GoodsRepository

        )
        {
            this._repository = repository;
            _b_GoodsRepository = b_GoodsRepository;

        }

        /// <summary>
        /// 后台 根据商品id获取库存新增记录
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_InventoryAddRecordListOutputDto>> GetList(GetB_InventoryAddRecordListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in UserManager.Users on a.ConfirmUserId equals b.Id into g
                        from c in g.DefaultIfEmpty()
                        where a.Goodsid == input.Goodsid
                        select new B_InventoryAddRecordListOutputDto()
                        {
                            Id = a.Id,
                            Goodsid = a.Goodsid,
                            Count = a.Count,
                            Status = a.Status,
                            ConfirmUserId = a.ConfirmUserId,
                            ConfirmUserName = c == null ? "" : c.Name,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_InventoryAddRecordListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_InventoryAddRecordOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_InventoryAddRecordOutputDto>();
        }


        /// <summary>
        /// 后台-新增库存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_InventoryAddRecordInput input)
        {
            if (input.Status == B_InventoryAddConfigEnum.是)
            {
                if (!input.ConfirmUserId.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "已回执新增库存请确定操作人员！");
            }
            var newmodel = new B_InventoryAddRecord()
            {
                Goodsid = input.Goodsid,
                Count = input.Count,
                Status = input.Status,
                ConfirmUserId = input.ConfirmUserId
            };

            if (input.Status == B_InventoryAddConfigEnum.是)
            {
                var goodModel = await _b_GoodsRepository.FirstOrDefaultAsync(r => r.Id == input.Goodsid);
                if (goodModel == null)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "商品不存在！");
                goodModel.Inventory = goodModel.Inventory + input.Count;
                await _b_GoodsRepository.UpdateAsync(goodModel);
            }
            await _repository.InsertAsync(newmodel);

        }



        /// <summary>
        /// 修改一个B_InventoryAddRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_InventoryAddRecordInput input)
        {
            //if (input.Id != Guid.Empty)
            //{
            //    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //    if (dbmodel == null)
            //    {
            //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //    }

            //    dbmodel.Goodsid = input.Goodsid;
            //    dbmodel.Count = input.Count;
            //    dbmodel.Status = input.Status;
            //    dbmodel.ConfirmUserId = input.ConfirmUserId;

            //    await _repository.UpdateAsync(dbmodel);

            //}
            //else
            //{
            //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //}
        }


        /// <summary>
        /// 后台-确认回执
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Confirm(EntityDto<Guid> input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            if (dbmodel.Status == B_InventoryAddConfigEnum.否)
            {
                dbmodel.Status = B_InventoryAddConfigEnum.是;
                var goodModel = await _b_GoodsRepository.FirstOrDefaultAsync(r => r.Id == dbmodel.Goodsid);
                if (goodModel == null)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "商品不存在！");
                goodModel.Inventory = goodModel.Inventory + dbmodel.Count;
                await _b_GoodsRepository.UpdateAsync(goodModel);
            }

        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            if (dbmodel.Status == B_InventoryAddConfigEnum.否)
                await _repository.DeleteAsync(x => x.Id == input.Id);
        }
    }
}