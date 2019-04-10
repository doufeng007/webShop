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


        public B_InOrderAppService(IRepository<B_Agency, Guid> b_AgencyRepository, B_CategroyManager b_CategroyManager
            , IRepository<B_CWUserInventory, Guid> b_CWUserInventoryRepository, IRepository<AbpDictionary, Guid> abpDictionaryRepository
            , IRepository<B_OrderIn, Guid> repository)
        {
            _b_AgencyRepository = b_AgencyRepository;
            _b_CategroyManager = b_CategroyManager;
            _b_CWUserInventoryRepository = b_CWUserInventoryRepository;
            _abpDictionaryRepository = abpDictionaryRepository;
            _repository = repository;
        }
        /// <summary>
        /// 获取进货单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<B_InOrderListOutputDto>> GetB_InOrderListAsync(GetB_InOrderListInput input)
        {
            throw new NotImplementedException();
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
            var categroyPrice = _b_CategroyManager.GetCategroyPriceForUser(AbpSession.UserId.Value, input.CategroyId);
            var totalAmout = categroyPrice * input.Number;

            if ((bModel.GoodsPayment + bModel.Balance) < totalAmout)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "余额不足，无法支付");
            else
            {
                if (bModel.GoodsPayment < totalAmout)
                {
                    bModel.GoodsPayment = 0;
                    bModel.Balance = bModel.Balance - (totalAmout - bModel.GoodsPayment);
                }
                else
                {
                    bModel.GoodsPayment = bModel.GoodsPayment - totalAmout;
                }
            }


            var orderInmodel = new B_OrderIn()
            {
                Id = Guid.NewGuid(),
                Amout = totalAmout,
                CategroyId = input.CategroyId,
                Number = input.Number,
                UserId = AbpSession.UserId.Value,
            };

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
                                    userInventory.Count = userInventory.Count + input.Number;
                                else  //递归往下级发货 修改下级的count和lessCount
                                    OrderInForChildeAgency(bModel.UserId, input.CategroyId, input.Number, userInventory);
                            }

                        }
                    }

                }
            }



            await _repository.InsertAsync(orderInmodel);




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
                    var meetingChildeLessOrderIns = childeLessOrderIns.OrderBy(r => r.Number).ThenBy(r => r.CreationTime).ToList();
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
                                    OrderInForChildeAgency(agencyInventoryModel.UserId, categroyId, item.Number);
                                }
                                else
                                {
                                    agencyInventoryModel.LessCount = agencyInventoryModel.LessCount - item.Number;
                                    currentNumber = currentNumber - item.Number;
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
