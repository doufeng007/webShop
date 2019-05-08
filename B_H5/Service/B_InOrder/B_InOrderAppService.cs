using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.Domain.Repositories;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ZCYX.FRMSCore.Model;
using Abp.WorkFlowDictionary;
using Abp;
using Abp.File;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.WeChat.Enum;
using Abp.WeChat;
using ZCYX.FRMSCore.Authorization.Users;

namespace B_H5
{
    /// <summary>
    /// 进货
    /// </summary>
    public class B_InOrderAppService : FRMSCoreAppServiceBase, IB_InOrderAppService
    {

        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly B_CategroyManager _b_CategroyManager;
        private readonly IRepository<B_CWUserInventory, Guid> _b_CWUserInventoryRepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        private readonly IRepository<B_OrderIn, Guid> _repository;
        private readonly IRepository<B_Order, Guid> _b_OrderRepository;
        private readonly IRepository<B_Categroy, Guid> _b_CategroyRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IB_CWDetailAppService _b_CWDetailAppService;
        private readonly WxTemplateMessageManager _wxTemplateMessageManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<B_AgencySales, Guid> _b_AgencySalesRepository;
        private readonly IRepository<B_AgencySalesDetail, Guid> _b_AgencySalesDetailRepository;


        public B_InOrderAppService(IRepository<B_Agency, Guid> b_AgencyRepository, B_CategroyManager b_CategroyManager
            , IRepository<B_CWUserInventory, Guid> b_CWUserInventoryRepository, IRepository<AbpDictionary, Guid> abpDictionaryRepository
            , IRepository<B_OrderIn, Guid> repository, IRepository<B_Order, Guid> b_OrderRepository, IRepository<B_Categroy, Guid> b_CategroyRepository
            , IAbpFileRelationAppService abpFileRelationAppService, IB_CWDetailAppService b_CWDetailAppService, WxTemplateMessageManager wxTemplateMessageManager
            , IRepository<User, long> userRepository, IRepository<B_AgencySales, Guid> b_AgencySalesRepository, IRepository<B_AgencySalesDetail, Guid> b_AgencySalesDetailRepository)
        {
            _b_AgencyRepository = b_AgencyRepository;
            _b_CategroyManager = b_CategroyManager;
            _b_CWUserInventoryRepository = b_CWUserInventoryRepository;
            _abpDictionaryRepository = abpDictionaryRepository;
            _repository = repository;
            _b_OrderRepository = b_OrderRepository;
            _b_CategroyRepository = b_CategroyRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _b_CWDetailAppService = b_CWDetailAppService;
            _wxTemplateMessageManager = wxTemplateMessageManager;
            _userRepository = userRepository;
            _b_AgencySalesRepository = b_AgencySalesRepository;
            _b_AgencySalesDetailRepository = b_AgencySalesDetailRepository;

        }
        /// <summary>
        /// 获取进货单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<B_InOrderListOutputDto>> GetB_InOrderListAsync(GetB_InOrderListInput input)
        {
            var userIdList = new List<long>();
            if (input.LowerUsers)
            {
                if (!AbpSession.UserId.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "获取下级进货订单请先登录！");
                else
                {
                    var currentAgencyModel = await _b_AgencyRepository.FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value);
                    if (currentAgencyModel == null)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理人数据不存在！");
                    else
                    {
                        //userIdList = _b_AgencyRepository.GetAll().Where(r=>r.P_Id)
                    }
                }
            }

            var query = from a in _repository.GetAll()
                        join u in UserManager.Users on a.UserId equals u.Id
                        join b in _b_OrderRepository.GetAll() on a.Id equals b.BusinessId
                        join c in _b_CategroyRepository.GetAll() on a.CategroyId equals c.Id
                        select new B_InOrderListOutputDto
                        {
                            Id = a.Id,
                            Amout = a.Amout,
                            Balance = a.Balance,
                            CreationTime = a.CreationTime,
                            GoodsPayment = a.GoodsPayment,
                            Number = a.Number,
                            OrderNo = b.OrderNo,
                            CategroyId = a.CategroyId,
                            CategroyTitle = c.Name,
                            UserName = u.Name,
                            UserId = a.UserId,
                            Status = a.Status,

                        };



            query = query.WhereIf(input.Status.HasValue, r => r.Status == input.Status.Value)
                .WhereIf(input.UserId.HasValue, r => r.UserId == input.UserId.Value)
                .WhereIf(input.StartDate.HasValue, r => r.CreationTime >= input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, r => r.CreationTime <= input.EndDate.Value)
              .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.OrderNo.Contains(input.SearchKey) || r.UserName.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            var businessIds = ret.Select(r => r.CategroyId.ToString()).Distinct().ToList();
            if (businessIds.Count > 0)
            {
                var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
                {
                    BusinessIds = businessIds,
                    BusinessType = AbpFileBusinessType.商品类别图
                });
                foreach (var item in ret)
                    if (fileGroups.Any(r => r.BusinessId == item.CategroyId.ToString()))
                    {
                        var fileModel = fileGroups.FirstOrDefault(r => r.BusinessId == item.CategroyId.ToString());
                        if (fileModel != null)
                        {
                            var files = fileModel.Files;
                            if (files.Count > 0)
                                item.File = files.FirstOrDefault();
                        }

                    }
            }

            return ret;
        }


        public async Task<OrderInDto> Get(EntityDto<Guid> input)
        {
            var query = from a in _repository.GetAll()
                        join b in _b_OrderRepository.GetAll() on a.Id equals b.BusinessId
                        join c in _b_CategroyRepository.GetAll() on a.CategroyId equals c.Id
                        where a.Id == input.Id
                        select new OrderInDto
                        {
                            Id = a.Id,
                            Amout = a.Amout,
                            Balance = a.Balance,
                            CreationTime = a.CreationTime,
                            GoodsPayment = a.GoodsPayment,
                            Number = a.Number,
                            OrderNo = b.OrderNo,
                            Price = c.Price,
                            CategroyId = a.CategroyId,
                            CategroyTitle = c.Name



                        };
            var ret = await query.FirstOrDefaultAsync();
            if (ret == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            var files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = ret.CategroyId.ToString(),
                BusinessType = (int)AbpFileBusinessType.商品类别图
            });

            if (files.Count() > 0)
                ret.File = files.FirstOrDefault();


            return ret;

        }

        /// <summary>
        /// 我的云仓-进货
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task OrderIn(OrderInInput input)
        {
            if (input.Number <= 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "进货数量必须大于0");
            var bModel = await _b_AgencyRepository.GetAll().FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value);
            if (bModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");
            var categroyModel = await _b_CategroyRepository.GetAsync(input.CategroyId);
            var categroyPrice = _b_CategroyManager.GetCategroyPriceForUser(AbpSession.UserId.Value, categroyModel.Price);
            var totalAmout = categroyPrice * input.Number;


            var orderInmodel = new B_OrderIn()
            {
                Id = Guid.NewGuid(),
                Amout = totalAmout,
                CategroyId = input.CategroyId,
                Number = input.Number,
                UserId = AbpSession.UserId.Value,
                OrderNo = DateTime.Now.ToString("yyyyMMddHHmmSS"),
            };


            if ((bModel.GoodsPayment + bModel.Balance) < totalAmout)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "余额不足，无法支付");
            else
            {
                if (bModel.GoodsPayment < totalAmout)
                {
                    bModel.GoodsPayment = 0;
                    bModel.Balance = bModel.Balance - (totalAmout - bModel.GoodsPayment);
                    orderInmodel.GoodsPayment = bModel.GoodsPayment;
                    orderInmodel.Balance = totalAmout - bModel.GoodsPayment;
                }
                else
                {
                    bModel.GoodsPayment = bModel.GoodsPayment - totalAmout;
                    orderInmodel.GoodsPayment = totalAmout;
                }
            }




            var userInventory = await _b_CWUserInventoryRepository.FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value && r.CategroyId == input.CategroyId);

            if (bModel.AgencyLevel == 1)  // 1级代理直接从云仓里进货
            {

                if (userInventory == null)
                {
                    var newModel = new B_CWUserInventory()
                    {
                        CategroyId = input.CategroyId,
                        Count = input.Number,
                        Id = Guid.NewGuid(),
                        LessCount = 0,
                        UserId = AbpSession.UserId.Value,

                    };
                    await _b_CWUserInventoryRepository.InsertAsync(newModel);
                    orderInmodel.Status = InOrderStatusEnum.已完成;
                }
                else
                {
                    if (userInventory.LessCount > 0)
                    {
                        orderInmodel.Status = InOrderStatusEnum.已完成;
                        OrderInForChildeAgency(bModel.UserId, input.CategroyId, input.Number, userInventory);
                    }
                    else
                    {
                        userInventory.Count = userInventory.Count + input.Number;
                        await _b_CWUserInventoryRepository.UpdateAsync(userInventory);
                        orderInmodel.Status = InOrderStatusEnum.已完成;


                    }
                }
            }
            else
            {
                var parent_AgencyModel = await _b_AgencyRepository.FirstOrDefaultAsync(r => r.Id == bModel.P_Id);
                if (parent_AgencyModel == null)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "非一级代理找不到上级代理");
                else
                {
                    var parent_Agency_Inventory = await _b_CWUserInventoryRepository.FirstOrDefaultAsync(r => r.UserId == parent_AgencyModel.UserId
                   && r.CategroyId == input.CategroyId);
                    if (parent_Agency_Inventory == null)
                    {
                        orderInmodel.Status = InOrderStatusEnum.上级缺货;

                        var newParentModel = new B_CWUserInventory()
                        {
                            CategroyId = input.CategroyId,
                            Count = 0,
                            Id = Guid.NewGuid(),
                            LessCount = input.Number,
                            UserId = parent_AgencyModel.UserId,
                        };
                        await _b_CWUserInventoryRepository.InsertAsync(newParentModel);

                        if (userInventory == null)
                        {
                            var newModel = new B_CWUserInventory()
                            {
                                CategroyId = input.CategroyId,
                                Count = 0,
                                Id = Guid.NewGuid(),
                                LessCount = 0,
                                UserId = AbpSession.UserId.Value,
                            };
                            await _b_CWUserInventoryRepository.InsertAsync(newModel);
                        }
                        else
                        {
                            //不做任何操作
                        }


                    }
                    else
                    {
                        if (parent_Agency_Inventory.Count >= input.Number)   ///上级有货， 往下发货的逻辑
                        {
                            if (parent_Agency_Inventory.LessCount > 0)
                                parent_Agency_Inventory.LessCount = parent_Agency_Inventory.LessCount + input.Number;

                            parent_Agency_Inventory.Count = parent_Agency_Inventory.Count - input.Number;
                            parent_AgencyModel.Balance = parent_AgencyModel.Balance + totalAmout;
                            orderInmodel.Status = InOrderStatusEnum.已完成;
                            if (userInventory == null)
                            {
                                var newModel = new B_CWUserInventory()
                                {
                                    CategroyId = input.CategroyId,
                                    Count = input.Number,
                                    Id = Guid.NewGuid(),
                                    LessCount = 0,
                                    UserId = AbpSession.UserId.Value,
                                };
                                await _b_CWUserInventoryRepository.InsertAsync(newModel);
                            }
                            else
                            {
                                if (userInventory.LessCount <= 0)
                                {
                                    userInventory.Count = userInventory.Count + input.Number;


                                }
                                else  //递归往下级发货 修改下级的count和lessCount
                                    OrderInForChildeAgency(bModel.UserId, input.CategroyId, input.Number, userInventory);
                            }

                        }
                        else
                        {
                            parent_Agency_Inventory.LessCount = parent_Agency_Inventory.LessCount + input.Number;
                            orderInmodel.Status = InOrderStatusEnum.上级缺货;
                        }
                        await _b_CWUserInventoryRepository.UpdateAsync(parent_Agency_Inventory);

                    }

                }
            }



            await _repository.InsertAsync(orderInmodel);
            OrderInForCurrentUser(orderInmodel);
        }

        /// <summary>
        /// 处理上级代理自动往下级代理发货
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="categroyId"></param>
        /// <param name="number"></param>
        /// <param name="agencyInventoryModel"></param>
        private void OrderInForChildeAgency(long userId, Guid categroyId, int number, B_CWUserInventory agencyInventoryModel = null)
        {
            var agencyModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == userId);
            var org_AgencyBlance = agencyModel.Balance;
            if (agencyInventoryModel == null)
                agencyInventoryModel = _b_CWUserInventoryRepository.FirstOrDefault(r => r.UserId == agencyModel.UserId && r.CategroyId == categroyId);
            if (agencyInventoryModel == null)
                return;
            else
            {
                if (agencyInventoryModel.LessCount > 0)
                {

                    var childeLessOrderIns = from a in _repository.GetAll()
                                             join b in _b_AgencyRepository.GetAll() on a.UserId equals b.UserId
                                             where a.Status == InOrderStatusEnum.上级缺货 && b.P_Id == agencyModel.Id && a.CategroyId == categroyId
                                             select a;
                    //var meetingChildeLessOrderIns = childeLessOrderIns.OrderBy(r => r.Number).ThenBy(r => r.CreationTime).ToList();
                    var meetingChildeLessOrderIns = childeLessOrderIns.OrderBy(r => r.CreationTime).ToList();
                    if (meetingChildeLessOrderIns.Count == 0)  // 下级代理的  上级缺货订单数量为0 ； leseeCount>0  逻辑上不会进入这个if
                    {
                        agencyInventoryModel.Count = agencyInventoryModel.Count + number;
                        if (agencyInventoryModel.LessCount - number > 0)
                            agencyInventoryModel.LessCount = agencyInventoryModel.LessCount - number;
                        else
                            agencyInventoryModel.LessCount = 0;
                    }
                    else
                    {
                        var currentAgencyInventoryCount = agencyInventoryModel.Count;
                        var currentNumber = number;
                        foreach (var item in meetingChildeLessOrderIns)
                        {
                            if (item.Number > (currentAgencyInventoryCount + currentNumber))
                            {
                                agencyInventoryModel.Count = currentAgencyInventoryCount + currentNumber;
                                break;
                            }
                            else if (item.Number == (currentAgencyInventoryCount + currentNumber))
                            {
                                agencyInventoryModel.Count = 0;
                                agencyInventoryModel.LessCount = agencyInventoryModel.LessCount - (item.Number - currentAgencyInventoryCount);
                                item.Status = InOrderStatusEnum.已完成;
                                _repository.Update(item);
                                ///上级往下发货成功 余额往下
                                var sale = OrderInForChildDetail(item, categroyId, item.Number, item.UserId, agencyInventoryModel.UserId);
                                org_AgencyBlance = org_AgencyBlance + sale;
                                OrderInForChildeAgency(agencyInventoryModel.UserId, categroyId, item.Number);
                                break;
                            }
                            else if (item.Number < (currentAgencyInventoryCount + currentNumber))
                            {
                                if (currentAgencyInventoryCount != 0)
                                {
                                    ///第一个
                                    currentNumber = currentNumber - (item.Number - currentAgencyInventoryCount);
                                    agencyInventoryModel.LessCount = agencyInventoryModel.LessCount - (item.Number - currentAgencyInventoryCount);
                                    currentAgencyInventoryCount = 0;

                                    item.Status = InOrderStatusEnum.已完成;
                                    _repository.Update(item);
                                    ///上级往下发货成功 余额往下
                                    ///上级往下发货成功 余额往下
                                    var sale = OrderInForChildDetail(item, categroyId, item.Number, item.UserId, agencyInventoryModel.UserId);
                                    org_AgencyBlance = org_AgencyBlance + sale;
                                    OrderInForChildeAgency(agencyInventoryModel.UserId, categroyId, item.Number);
                                }
                                else
                                {
                                    agencyInventoryModel.LessCount = agencyInventoryModel.LessCount - item.Number;
                                    currentNumber = currentNumber - item.Number;

                                    item.Status = InOrderStatusEnum.已完成;
                                    _repository.Update(item);
                                    ///上级往下发货成功 余额往下
                                    var sale = OrderInForChildDetail(item, categroyId, item.Number, item.UserId, agencyInventoryModel.UserId);
                                    org_AgencyBlance = org_AgencyBlance + sale;
                                    OrderInForChildeAgency(agencyInventoryModel.UserId, categroyId, item.Number);
                                }

                            }
                        }
                    }
                }
                else
                {
                    agencyInventoryModel.Count = agencyInventoryModel.Count + number;
                }
            }

            if (agencyModel.Balance != org_AgencyBlance)
            {
                agencyModel.Balance = org_AgencyBlance;
                _b_AgencyRepository.Update(agencyModel);
            }

        }


        private decimal OrderInForChildDetail(B_OrderIn orderInmodel, Guid categroyId, int number, long userId, long p_UserId)
        {

            _b_CWDetailAppService.CreateAsync(new CreateB_CWDetailInput()
            {
                BusinessType = CWDetailBusinessTypeEnum.自身进货入仓,
                CategroyId = categroyId,
                Number = number,
                Type = CWDetailTypeEnum.入仓,
                UserId = userId
            });

            _b_CWDetailAppService.CreateAsync(new CreateB_CWDetailInput()
            {
                BusinessType = CWDetailBusinessTypeEnum.下级进货自身出仓,
                CategroyId = categroyId,
                Number = number,
                Type = CWDetailTypeEnum.出仓,
                UserId = p_UserId,
                RelationUserId = userId,
            });

            var categroyModel = _b_CategroyRepository.Get(orderInmodel.CategroyId);

            var p_Agency = _b_AgencyRepository.FirstOrDefault(r => r.UserId == p_UserId);
            var agency = _b_AgencyRepository.FirstOrDefault(r => r.UserId == userId);
            var profit = _b_CategroyManager.GetProfitForUser(p_Agency.AgencyLevelId, agency.AgencyLevelId, orderInmodel.CategroyId);
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_OrderAppService>();
            #region 销售额

            var new_parent_Sale = new B_AgencySales()
            {
                Id = Guid.NewGuid(),
                AgencyId = p_Agency.Id,
                CategroyId = orderInmodel.CategroyId,
                Profit = 0,
                Sales = orderInmodel.Amout,
                SalesDate = DateTime.Now,
                UserId = p_Agency.UserId,
                FromUserId = userId,
            };

            if (agency.OriginalPid == agency.P_Id)
            {
                new_parent_Sale.Profit = profit;
            }
            else
            {
                var oldParentAgency = _b_AgencyRepository.Get(agency.OriginalPid.Value);
                new_parent_Sale.Profit = profit / 2;
                new_parent_Sale.Sales = orderInmodel.Amout - profit / 2;
                var new_Oldparent_Sale = new B_AgencySales()
                {
                    Id = Guid.NewGuid(),
                    AgencyId = oldParentAgency.Id,
                    UserId = oldParentAgency.UserId,
                    CategroyId = orderInmodel.CategroyId,
                    Profit = profit / 2,
                    Sales = 0,
                    SalesDate = DateTime.Now,
                    FromUserId = userId,
                };
                _b_AgencySalesRepository.Insert(new_Oldparent_Sale);


                //原始上家的进钱
                service.Create(new CreateB_OrderInput()
                {
                    Amout = new_Oldparent_Sale.Profit,
                    BusinessId = orderInmodel.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.进货,
                    InOrOut = OrderAmoutEnum.入账,
                    OrderNo = orderInmodel.OrderNo,
                    UserId = oldParentAgency.UserId,
                    IsBlance = true,
                    IsGoodsPayment = false,
                });
            }
            _b_AgencySalesRepository.Insert(new_parent_Sale);

            #endregion


            #region  消费记录


            //进货者的花钱
            if (orderInmodel.Balance > 0)
            {
                service.Create(new CreateB_OrderInput()
                {
                    Amout = orderInmodel.Balance,
                    BusinessId = orderInmodel.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.进货,
                    InOrOut = OrderAmoutEnum.出账,
                    OrderNo = orderInmodel.OrderNo,
                    UserId = orderInmodel.UserId,
                    IsBlance = true,
                    IsGoodsPayment = false,
                });
            }
            if (orderInmodel.GoodsPayment > 0)
            {
                service.Create(new CreateB_OrderInput()
                {
                    Amout = orderInmodel.GoodsPayment,
                    BusinessId = orderInmodel.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.进货,
                    InOrOut = OrderAmoutEnum.出账,
                    OrderNo = orderInmodel.OrderNo,
                    UserId = orderInmodel.UserId,
                    IsBlance = false,
                    IsGoodsPayment = true
                });
            }


            ///上家的进钱

            service.Create(new CreateB_OrderInput()
            {
                Amout = new_parent_Sale.Sales,
                BusinessId = orderInmodel.Id,
                BusinessType = OrderAmoutBusinessTypeEnum.进货,
                InOrOut = OrderAmoutEnum.入账,
                OrderNo = orderInmodel.OrderNo,
                UserId = p_Agency.UserId,
                IsBlance = true,
                IsGoodsPayment = false,
            });


            //原始上家的进钱 放前面




            #endregion



            SendWeChatMessage(orderInmodel.Id.ToString(), TemplateMessageBusinessTypeEnum.当前用户进货订单完成, userId, $"进货订单{orderInmodel.OrderNo}"
                            , categroyModel.Name, "货物已转入云仓", orderInmodel.Amout, InOrderStatusEnum.已完成);


            var parent_User = _userRepository.Get(p_UserId);

            SendWeChatMessage(orderInmodel.Id.ToString(), TemplateMessageBusinessTypeEnum.下级代理进货订单完成, p_UserId, $"进货订单{orderInmodel.OrderNo}"
                            , categroyModel.Name, $"货物已转入代理（{parent_User.Name}）云仓", orderInmodel.Amout, InOrderStatusEnum.已完成);

            return new_parent_Sale.Sales;
        }




        public void OrderInForCurrentUser(B_OrderIn orderInmodel)
        {
            var categroyModel = _b_CategroyRepository.Get(orderInmodel.CategroyId);
            var b_AgencyModel = _b_AgencyRepository.GetAll().FirstOrDefault(r => r.UserId == AbpSession.UserId.Value);

            if (orderInmodel.Status == InOrderStatusEnum.已完成)
            {
                //创建入仓记录
                _b_CWDetailAppService.Create(new CreateB_CWDetailInput()
                {
                    BusinessType = CWDetailBusinessTypeEnum.自身进货入仓,
                    CategroyId = orderInmodel.CategroyId,
                    Number = orderInmodel.Number,
                    Type = CWDetailTypeEnum.入仓,
                    UserId = orderInmodel.UserId
                });


                //发送微信模板消息
                SendWeChatMessage(orderInmodel.Id.ToString(), TemplateMessageBusinessTypeEnum.当前用户进货订单完成, AbpSession.UserId.Value, $"进货订单{orderInmodel.OrderNo}"
                            , categroyModel.Name, "货物已转入云仓", orderInmodel.Amout, InOrderStatusEnum.已完成);
                var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_OrderAppService>();
                if (b_AgencyModel.AgencyLevel != 1)
                {
                    var parent_AgencyModel = _b_AgencyRepository.FirstOrDefault(r => r.Id == b_AgencyModel.P_Id);
                    if (parent_AgencyModel == null)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "非一级代理找不到上级代理");
                    _b_CWDetailAppService.CreateAsync(new CreateB_CWDetailInput()
                    {
                        BusinessType = CWDetailBusinessTypeEnum.下级进货自身出仓,
                        CategroyId = orderInmodel.CategroyId,
                        Number = orderInmodel.Number,
                        Type = CWDetailTypeEnum.出仓,
                        UserId = parent_AgencyModel.UserId,
                        RelationUserId = orderInmodel.UserId,
                    });






                    #region  销售额

                    var new_parent_Sale = new B_AgencySales()
                    {
                        Id = Guid.NewGuid(),
                        AgencyId = parent_AgencyModel.Id,
                        UserId = parent_AgencyModel.UserId,
                        CategroyId = orderInmodel.CategroyId,
                        Profit = 0,
                        Sales = orderInmodel.Amout,
                        SalesDate = DateTime.Now,
                        FromUserId = orderInmodel.UserId,
                    };
                    var profit = _b_CategroyManager.GetProfitForUser(parent_AgencyModel.AgencyLevelId, b_AgencyModel.AgencyLevelId, orderInmodel.CategroyId);
                    if (b_AgencyModel.OriginalPid == b_AgencyModel.P_Id)
                    {
                        new_parent_Sale.Profit = profit;
                    }
                    else
                    {
                        var oldParentAgency = _b_AgencyRepository.Get(b_AgencyModel.OriginalPid.Value);
                        new_parent_Sale.Profit = profit / 2;
                        new_parent_Sale.Sales = new_parent_Sale.Sales - new_parent_Sale.Profit;
                        var new_Oldparent_Sale = new B_AgencySales()
                        {
                            Id = Guid.NewGuid(),
                            AgencyId = oldParentAgency.Id,
                            UserId = oldParentAgency.UserId,
                            CategroyId = orderInmodel.CategroyId,
                            Profit = profit / 2,
                            Sales = 0,
                            SalesDate = DateTime.Now,
                            FromUserId = orderInmodel.UserId
                        };
                        _b_AgencySalesRepository.Insert(new_Oldparent_Sale);

                        //原始上家的进钱
                        service.Create(new CreateB_OrderInput()
                        {
                            Amout = new_Oldparent_Sale.Profit,
                            BusinessId = orderInmodel.Id,
                            BusinessType = OrderAmoutBusinessTypeEnum.进货,
                            InOrOut = OrderAmoutEnum.入账,
                            OrderNo = orderInmodel.OrderNo,
                            UserId = oldParentAgency.UserId,
                            IsBlance = true,
                            IsGoodsPayment = false,
                        });
                    }
                    _b_AgencySalesRepository.Insert(new_parent_Sale);
                    #endregion


                    #region  消费记录


                    //进货者的花钱
                    if (orderInmodel.Balance > 0)
                    {
                        service.Create(new CreateB_OrderInput()
                        {
                            Amout = orderInmodel.Balance,
                            BusinessId = orderInmodel.Id,
                            BusinessType = OrderAmoutBusinessTypeEnum.进货,
                            InOrOut = OrderAmoutEnum.出账,
                            OrderNo = orderInmodel.OrderNo,
                            UserId = orderInmodel.UserId,
                            IsBlance = true,
                            IsGoodsPayment = false,
                        });
                    }
                    if (orderInmodel.GoodsPayment > 0)
                    {
                        service.Create(new CreateB_OrderInput()
                        {
                            Amout = orderInmodel.GoodsPayment,
                            BusinessId = orderInmodel.Id,
                            BusinessType = OrderAmoutBusinessTypeEnum.进货,
                            InOrOut = OrderAmoutEnum.出账,
                            OrderNo = orderInmodel.OrderNo,
                            UserId = orderInmodel.UserId,
                            IsBlance = false,
                            IsGoodsPayment = true
                        });
                    }


                    ///上家的进钱

                    service.Create(new CreateB_OrderInput()
                    {
                        Amout = new_parent_Sale.Sales,
                        BusinessId = orderInmodel.Id,
                        BusinessType = OrderAmoutBusinessTypeEnum.进货,
                        InOrOut = OrderAmoutEnum.入账,
                        OrderNo = orderInmodel.OrderNo,
                        UserId = parent_AgencyModel.UserId,
                        IsBlance = true,
                        IsGoodsPayment = false,
                    });


                    //原始上家的进钱 放前面




                    #endregion




                    var parent_User = _userRepository.Get(parent_AgencyModel.UserId);
                    SendWeChatMessage(orderInmodel.Id.ToString(), TemplateMessageBusinessTypeEnum.下级代理进货订单完成, parent_AgencyModel.UserId, $"进货订单{orderInmodel.OrderNo}"
                            , categroyModel.Name, $"货物已转入代理（{parent_User.Name}）云仓", orderInmodel.Amout, InOrderStatusEnum.已完成);
                }
                else
                {
                    ///一级代理进货 不会产生谁的销售额；

                    #region   消费记录
                    //进货者的花钱
                    if (orderInmodel.Balance > 0)
                    {
                        service.Create(new CreateB_OrderInput()
                        {
                            Amout = orderInmodel.Balance,
                            BusinessId = orderInmodel.Id,
                            BusinessType = OrderAmoutBusinessTypeEnum.进货,
                            InOrOut = OrderAmoutEnum.出账,
                            OrderNo = orderInmodel.OrderNo,
                            UserId = orderInmodel.UserId,
                            IsBlance = true,
                            IsGoodsPayment = false,
                        });
                    }
                    if (orderInmodel.GoodsPayment > 0)
                    {
                        service.Create(new CreateB_OrderInput()
                        {
                            Amout = orderInmodel.GoodsPayment,
                            BusinessId = orderInmodel.Id,
                            BusinessType = OrderAmoutBusinessTypeEnum.进货,
                            InOrOut = OrderAmoutEnum.出账,
                            OrderNo = orderInmodel.OrderNo,
                            UserId = orderInmodel.UserId,
                            IsBlance = false,
                            IsGoodsPayment = true
                        });
                    }
                    #endregion
                }



            }
            else
            {
                //发送微信模板消息
                SendWeChatMessage(orderInmodel.Id.ToString(), TemplateMessageBusinessTypeEnum.当前用户进货订单上级缺货, AbpSession.UserId.Value, $"进货订单{orderInmodel.OrderNo}"
                            , categroyModel.Name, "请尽快补货！", orderInmodel.Amout, InOrderStatusEnum.上级缺货);
                if (b_AgencyModel.AgencyLevel != 1)
                {
                    var parent_AgencyModel = _b_AgencyRepository.FirstOrDefault(r => r.Id == b_AgencyModel.P_Id);
                    if (parent_AgencyModel == null)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "非一级代理找不到上级代理");
                    var parent_User = _userRepository.Get(parent_AgencyModel.UserId);
                    SendWeChatMessage(orderInmodel.Id.ToString(), TemplateMessageBusinessTypeEnum.下级代理进货订单上级缺货, parent_AgencyModel.UserId, $"进货订单{orderInmodel.OrderNo}"
                            , categroyModel.Name, "请尽快补货！", orderInmodel.Amout, InOrderStatusEnum.上级缺货);
                }
            }

        }
















        public void SendWeChatMessage(string bid, TemplateMessageBusinessTypeEnum bType, long userId, string title, string categroyName, string remark, decimal amout, InOrderStatusEnum status)
        {
            var bModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == userId);
            if (bModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");
            var dic = new Dictionary<string, string>();
            dic.Add("keyword1", categroyName);
            dic.Add("keyword2", amout.ToString());
            dic.Add("keyword3", status.ToString());
            _wxTemplateMessageManager.SendWeChatMsg(bid, bType, bModel.OpenId, title, dic, remark);

        }




        /// <summary>
        /// 获取云仓商品提货数据
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<UserBlanceDto> GetOrderIn(Guid categroyId)
        {
            var bModel = await _b_AgencyRepository.GetAll().FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value);
            if (bModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");
            var ret = new UserBlanceDto() { UserBalance = bModel.Balance, UserGoodsPayment = bModel.GoodsPayment };
            return ret;
        }






    }
}
