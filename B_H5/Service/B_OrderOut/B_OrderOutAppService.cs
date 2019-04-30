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
using Abp.Authorization;
using Abp;

namespace B_H5
{
    public class B_OrderOutAppService : FRMSCoreAppServiceBase, IB_OrderOutAppService
    {
        private readonly IRepository<B_OrderOut, Guid> _repository;
        private readonly IRepository<B_Categroy, Guid> _b_CategroyRepository;
        private readonly IRepository<B_CWUserInventory, Guid> _b_CWUserInventoryRepository;
        private readonly IRepository<B_OrderDetail, Guid> _b_OrderDetailRepository;
        private readonly IRepository<B_OrderCourier, Guid> _b_OrderCourierRepository;
        private readonly IRepository<B_Goods, Guid> _b_GoodsRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<B_MyAddress, Guid> _b_MyAddressRepository;
        private readonly IRepository<B_Order, Guid> _b_OrderRepository;

        public B_OrderOutAppService(IRepository<B_OrderOut, Guid> repository, IRepository<B_Categroy, Guid> b_CategroyRepository
            , IRepository<B_CWUserInventory, Guid> b_CWUserInventoryRepository, IRepository<B_OrderDetail, Guid> b_OrderDetailRepository
            , IRepository<B_OrderCourier, Guid> b_OrderCourierRepository, IRepository<B_Goods, Guid> b_GoodsRepository
            , IAbpFileRelationAppService abpFileRelationAppService, IRepository<B_MyAddress, Guid> b_MyAddressRepository
            , IRepository<B_Order, Guid> b_OrderRepository
        )
        {
            this._repository = repository;
            _b_CategroyRepository = b_CategroyRepository;
            _b_CWUserInventoryRepository = b_CWUserInventoryRepository;
            _b_OrderDetailRepository = b_OrderDetailRepository;
            _b_OrderCourierRepository = b_OrderCourierRepository;
            _b_GoodsRepository = b_GoodsRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _b_MyAddressRepository = b_MyAddressRepository;
            _b_OrderRepository = b_OrderRepository;

        }

        /// <summary>
        /// 后台-获取提货订单列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_OrderOutListOutputDto>> GetList(GetB_OrderOutListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join m in _b_OrderRepository.GetAll() on a.Id equals m.BusinessId
                        join b in UserManager.Users on a.UserId equals b.Id
                        join c in _b_MyAddressRepository.GetAll() on a.AddressId equals c.Id
                        join d in _b_OrderDetailRepository.GetAll() on a.Id equals d.BId into g
                        select new B_OrderOutListOutputDto()
                        {
                            Id = a.Id,
                            Address = c.Addres,
                            AddressCity = c.City,
                            AddressProvinces = c.Provinces,
                            AddressTel = c.Tel,
                            AddressUserName = c.Name,
                            CourierNum = "",
                            GoodsNumber = g.Sum(r => r.Number),
                            OrderNo = m.OrderNo,
                            Amout = a.Amout,
                            DeliveryFee = a.DeliveryFee,
                            GoodsPayment = a.GoodsPayment,
                            Balance = a.Balance,
                            Status = a.Stauts,
                            CreationTime = a.CreationTime,
                            UserName = b.Name

                        };


            query = query.WhereIf(input.Status.HasValue, r => r.Status == input.Status.Value)
                .WhereIf(input.StartDate.HasValue, r => r.CreationTime >= input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, r => r.CreationTime <= input.EndDate.Value)
              .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.OrderNo.Contains(input.SearchKey) || r.UserName.Contains(input.SearchKey));

            var toalCount = await query.CountAsync();

            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_OrderOutListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// H5 我的提货订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_OrderOutMyListOutputDto>> GetMyList(GetB_OrderOutListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join m in _b_OrderRepository.GetAll() on a.Id equals m.BusinessId
                        join c in _b_MyAddressRepository.GetAll() on a.AddressId equals c.Id
                        join d in _b_OrderDetailRepository.GetAll() on a.Id equals d.BId into g
                        select new B_OrderOutMyListOutputDto()
                        {
                            Id = a.Id,
                            AddressUserName = c.Name,
                            OrderNo = m.OrderNo,
                            GoodsNumber = g.Sum(r => r.Number),
                            Amout = a.Amout,
                            Status = a.Stauts,
                            CreationTime = a.CreationTime,
                        };


            query = query.WhereIf(input.Status.HasValue, r => r.Status == input.Status.Value);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            var bidList = ret.Select(r => r.Id).ToList();

            var orderDetailQuery = from a in _b_OrderDetailRepository.GetAll()
                                   join b in _b_GoodsRepository.GetAll() on a.GoodsId equals b.Id
                                   where bidList.Contains(a.BId)
                                   select new B_OrderDetailOutputDto()
                                   {
                                       Id = a.Id,
                                       GoodsId = a.GoodsId,
                                       BId = a.BId,
                                       Amout = a.Amout,
                                       CategroyId = a.CategroyId,
                                       CreationTime = a.CreationTime,
                                       GoodsTitle = b.Name,
                                       Number = a.Number,
                                   };

            var orderDetailList = orderDetailQuery.ToList();

            var businessIds = orderDetailList.Select(r => r.GoodsId.ToString()).ToList();
            var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
            {
                BusinessIds = businessIds,
                BusinessType = AbpFileBusinessType.商品缩略图
            });
            foreach (var item in orderDetailList)
                if (fileGroups.Any(r => r.BusinessId == item.GoodsId.ToString()))
                {
                    var fileModel = fileGroups.FirstOrDefault(r => r.BusinessId == item.Id.ToString());
                    if (fileModel != null)
                    {
                        var files = fileModel.Files;
                        if (files.Count > 0)
                            item.File = files.FirstOrDefault();
                    }

                }

            foreach (var item in ret)
            {
                var entityList = orderDetailList.Where(r => r.BId == item.Id).ToList();
                item.GoodsList = entityList;
            }

            return new PagedResultDto<B_OrderOutMyListOutputDto>(toalCount, ret);
        }





        /// <summary>
        /// H5获取提货单详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_OrderOutOutputDto> Get(EntityDto<Guid> input)
        {
            var ret = new B_OrderOutOutputDto();
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }



            var addRessModel = await _b_MyAddressRepository.GetAsync(model.AddressId);
            ret.Address = addRessModel.Addres;
            ret.AddressCity = addRessModel.City;
            ret.AddressProvinces = addRessModel.Provinces;
            ret.AddressTel = addRessModel.Tel;
            ret.AddressUserName = addRessModel.Name;
            ret.Amout = model.Amout;
            ret.Balance = model.Balance;
            ret.DeliveryFee = model.DeliveryFee;
            //ret.GoodsNumber = 
            ret.GoodsPayment = model.GoodsPayment;
            ret.Stauts = model.Stauts;
            ret.Id = model.Id;


            var orderDetailQuery = from a in _b_OrderDetailRepository.GetAll()
                                   join b in _b_GoodsRepository.GetAll() on a.GoodsId equals b.Id
                                   where a.BId == input.Id
                                   select new B_OrderDetailOutputDto()
                                   {
                                       Id = a.Id,
                                       GoodsId = a.GoodsId,
                                       BId = a.BId,
                                       Amout = a.Amout,
                                       CategroyId = a.CategroyId,
                                       CreationTime = a.CreationTime,
                                       GoodsTitle = b.Name,
                                       Number = a.Number,
                                   };
            ret.GoodsList = await orderDetailQuery.ToListAsync();

            var businessIds = ret.GoodsList.Select(r => r.GoodsId.ToString()).ToList();
            var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
            {
                BusinessIds = businessIds,
                BusinessType = AbpFileBusinessType.商品缩略图
            });
            foreach (var item in ret.GoodsList)
                if (fileGroups.Any(r => r.BusinessId == item.GoodsId.ToString()))
                {
                    var fileModel = fileGroups.FirstOrDefault(r => r.BusinessId == item.Id.ToString());
                    if (fileModel != null)
                    {
                        var files = fileModel.Files;
                        if (files.Count > 0)
                            item.File = files.FirstOrDefault();
                    }

                }


            ret.GoodsNumber = ret.GoodsList.Sum(r => r.Number);


            var couriers = _b_OrderCourierRepository.GetAll().Where(r => r.OrderId == input.Id).Select(r => new B_OrderCourierOutputDto()
            {
                Id = r.Id,
                OrderId = r.OrderId,
                CourierName = r.CourierName,
                CourierNum = r.CourierNum
            });

            ret.Couriers = await couriers.ToListAsync();


            return ret;
        }
        /// <summary>
        /// H5创建一个提货订单
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]

        public async Task Create(CreateB_OrderOutInput input)
        {

            var categroyList = input.Goods.Select(r => r.CategroyId).ToList();
            var query = from a in _b_CategroyRepository.GetAll()
                        where categroyList.Contains(a.Id) && a.FirestLevelCategroyPropertyId == FirestLevelCategroyProperty.进提货
                        select a;

            var newmodel = new B_OrderOut()
            {
                Id = Guid.NewGuid(),
                UserId = input.UserId,
                Amout = input.Amout,
                DeliveryFee = input.DeliveryFee,
                PayAmout = input.PayAmout,
                GoodsPayment = input.GoodsPayment,
                Balance = input.Balance,
                AddressId = input.AddressId,
                Stauts = OrderOutStauts.待发货
            };


            await _repository.InsertAsync(newmodel);

            foreach (var item in input.Goods)
            {
                if (query.Any(r => r.Id == item.CategroyId))
                {
                    var cwUserInventory = await _b_CWUserInventoryRepository.FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value && r.CategroyId == item.CategroyId);
                    if (cwUserInventory == null)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "云仓货物不足，请先在云仓进货");
                    else
                    {
                        if (cwUserInventory.Count < item.Number)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "云仓货物不足，请先在云仓进货");
                        else
                        {
                            cwUserInventory.Count = cwUserInventory.Count - item.Number;
                            if (cwUserInventory.LessCount > 0)
                                cwUserInventory.LessCount = cwUserInventory.LessCount + item.Number;
                            await _b_CWUserInventoryRepository.UpdateAsync(cwUserInventory);
                        }
                    }
                }

                var newDetail = new B_OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    Amout = item.Amout,
                    BId = newmodel.Id,
                    BType = 0,
                    CategroyId = item.CategroyId,
                    GoodsId = item.GoodsId,
                    Number = item.Number,
                    UserId = AbpSession.UserId.Value,
                };
                await _b_OrderDetailRepository.InsertAsync(newDetail);

            }


            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_OrderAppService>();
            await service.Create(new CreateB_OrderInput()
            {
                Amout = newmodel.Amout,
                BusinessId = newmodel.Id,
                BusinessType = OrderAmoutBusinessTypeEnum.提货,
                InOrOut = OrderAmoutEnum.入账,
                OrderNo = "",
                UserId = AbpSession.UserId.Value
            });
        }

        /// <summary>
        /// 修改一个B_OrderOut
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_OrderOutInput input)
        {

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