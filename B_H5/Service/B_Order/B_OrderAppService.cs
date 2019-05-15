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
    public class B_OrderAppService : FRMSCoreAppServiceBase, IB_OrderAppService
    {
        private readonly IRepository<B_Order, Guid> _repository;
        private readonly IRepository<B_OrderIn, Guid> _b_OrderInRepository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly IRepository<B_AgencyLevel, Guid> _b_AgencyLevelRepository;

        public B_OrderAppService(IRepository<B_Order, Guid> repository, IRepository<B_OrderIn, Guid> b_OrderInRepository
            , IRepository<B_Agency, Guid> b_AgencyRepository, IRepository<B_AgencyLevel, Guid> b_AgencyLevelRepository

        )
        {
            this._repository = repository;
            _b_OrderInRepository = b_OrderInRepository;
            _b_AgencyRepository = b_AgencyRepository;
            _b_AgencyLevelRepository = b_AgencyLevelRepository;

        }

        /// <summary>
        /// 获取用户余额详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<GetUserBlanceListOutput>> GetBlanceList(GetB_OrderListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where a.UserId == AbpSession.UserId.Value && a.IsBlance
                        select a;
            var toalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<GetUserBlanceListOutput>();
            foreach (var item in data)
            {
                var entity = new GetUserBlanceListOutput()
                {
                    Amout = item.Amout,
                    CreateTime = item.CreationTime,
                    Id = item.Id,
                    InOrOut = item.InOrOut,
                    OrderNo = item.OrderNo,
                    Status = "已完成",
                };
                if (entity.InOrOut == OrderAmoutEnum.入账)
                {
                    switch (item.BusinessType)
                    {
                        case OrderAmoutBusinessTypeEnum.进货:
                            entity.Type = "订单货款";
                            break;
                        case OrderAmoutBusinessTypeEnum.提货:
                            entity.Type = "下级提货奖金";
                            break;
                        case OrderAmoutBusinessTypeEnum.提现:
                            entity.Type = "";
                            break;
                        case OrderAmoutBusinessTypeEnum.充值:
                            entity.Type = "";
                            break;
                        case OrderAmoutBusinessTypeEnum.团队管理奖金:
                            entity.Type = "团队管理奖";
                            break;
                        case OrderAmoutBusinessTypeEnum.保证金:
                            break;
                        case OrderAmoutBusinessTypeEnum.推荐奖金:
                            entity.Type = "推荐奖金";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (item.BusinessType)
                    {
                        case OrderAmoutBusinessTypeEnum.进货:
                            entity.Type = "下单支付";
                            break;
                        case OrderAmoutBusinessTypeEnum.提货:
                            break;
                        case OrderAmoutBusinessTypeEnum.提现:
                            entity.Type = "提现";
                            break;
                        case OrderAmoutBusinessTypeEnum.充值:
                            break;
                        case OrderAmoutBusinessTypeEnum.团队管理奖金:
                            break;
                        case OrderAmoutBusinessTypeEnum.保证金:
                            break;
                        case OrderAmoutBusinessTypeEnum.推荐奖金:
                            break;
                        default:
                            break;
                    }
                }

                ret.Add(entity);
            }

            return new PagedResultDto<GetUserBlanceListOutput>(toalCount, ret);
        }


        /// <summary>
        /// 获取用户货款详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<GetUserGoodPaymentListOutput>> GetGoodPaymentList(GetB_OrderListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where a.UserId == AbpSession.UserId.Value && a.IsGoodsPayment
                        select a;
            var toalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<GetUserGoodPaymentListOutput>();
            foreach (var item in data)
            {
                var entity = new GetUserGoodPaymentListOutput()
                {
                    Amout = item.Amout,
                    CreateTime = item.CreationTime,
                    Id = item.Id,
                    InOrOut = item.InOrOut,
                    OrderNo = item.OrderNo,
                    Status = "已完成",
                };
                if (entity.InOrOut == OrderAmoutEnum.入账)
                {
                    switch (item.BusinessType)
                    {
                        case OrderAmoutBusinessTypeEnum.进货:
                            entity.Type = "订单货款";
                            break;
                        case OrderAmoutBusinessTypeEnum.提货:
                            entity.Type = "下级提货奖金";
                            break;
                        case OrderAmoutBusinessTypeEnum.提现:
                            entity.Type = "";
                            break;
                        case OrderAmoutBusinessTypeEnum.充值:
                            entity.Type = "";
                            break;
                        case OrderAmoutBusinessTypeEnum.团队管理奖金:
                            entity.Type = "团队管理奖";
                            break;
                        case OrderAmoutBusinessTypeEnum.保证金:
                            break;
                        case OrderAmoutBusinessTypeEnum.推荐奖金:
                            entity.Type = "推荐奖金";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (item.BusinessType)
                    {
                        case OrderAmoutBusinessTypeEnum.进货:
                            entity.Type = "下单支付";
                            break;
                        case OrderAmoutBusinessTypeEnum.提货:
                            break;
                        case OrderAmoutBusinessTypeEnum.提现:
                            entity.Type = "提现";
                            break;
                        case OrderAmoutBusinessTypeEnum.充值:
                            break;
                        case OrderAmoutBusinessTypeEnum.团队管理奖金:
                            break;
                        case OrderAmoutBusinessTypeEnum.保证金:
                            break;
                        case OrderAmoutBusinessTypeEnum.推荐奖金:
                            break;
                        default:
                            break;
                    }
                }

                ret.Add(entity);
            }

            return new PagedResultDto<GetUserGoodPaymentListOutput>(toalCount, ret);
        }


        /// <summary>
        /// 后台-代理金额列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize("WebShop.Manager")]
        public async Task<PagedResultDto<AgencyMoneyStaticDto>> GetAgencyMoneyStatic(GetAgencyMoneyStaticInput input)
        {
            var searchFlag = false;
            if (!input.SearchKey.IsNullOrEmpty())
                searchFlag = true;
            var query = from a in _repository.GetAll()
                        join b in _b_AgencyRepository.GetAll() on a.UserId equals b.UserId
                        join c in UserManager.Users on b.UserId equals c.Id
                        where (!input.LeavelId.HasValue || b.AgencyLevelId == input.LeavelId.Value)
                        && (!searchFlag || (b.AgenCyCode.Contains(input.SearchKey) || c.Name.Contains(input.SearchKey)))
                        group new { a, b, c } by new { a.UserId, b.AgencyLevelId, c.Name, b.AgenCyCode, b.Balance, b.GoodsPayment, b.CreationTime } into g
                        select new AgencyMoneyStaticDto
                        {
                            UserId = g.Key.UserId,
                            AgencyCode = g.Key.AgenCyCode,
                            AgencyLevelId = g.Key.AgencyLevelId,
                            AgencyName = g.Key.Name,
                            Blance = g.Key.Balance,
                            GoodsPayment = g.Key.GoodsPayment,
                            InviteAmout = g.Where(r => r.a.BusinessType == OrderAmoutBusinessTypeEnum.推荐奖金).Sum(r => r.a.Amout),
                            ChildOrderinOutAmout = g.Where(r => r.a.BusinessType == OrderAmoutBusinessTypeEnum.提货 && r.a.InOrOut == OrderAmoutEnum.入账).Sum(r => r.a.Amout),
                            OrderInAmout = g.Where(r => r.a.BusinessType == OrderAmoutBusinessTypeEnum.进货 && r.a.InOrOut == OrderAmoutEnum.出账).Sum(r => r.a.Amout),
                            TeamBonus = g.Where(r => r.a.BusinessType == OrderAmoutBusinessTypeEnum.团队管理奖金 && r.a.InOrOut == OrderAmoutEnum.入账).Sum(r => r.a.Amout),
                            WithdrawalAmout = g.Where(r => r.a.BusinessType == OrderAmoutBusinessTypeEnum.提现).Sum(r => r.a.Amout),
                            AgencyCreateTime = g.Key.CreationTime

                        };

            var totalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.AgencyCreateTime).PageBy(input).ToListAsync();

            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            foreach (var item in data)
            {
                var agencyLeavelModel = service.GetAgencyLevelFromCache(item.AgencyLevelId);
                item.AgencyLevelName = agencyLeavelModel.Name;
                item.Deposite = item.Deposite;
            }


            return new PagedResultDto<AgencyMoneyStaticDto>(totalCount, data);



        }



        /// <summary>
        /// 后台- 获取一个代理金额详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AgencyMoneyDetailListDto>> GetAgencyMoneyDetailList(GetAgencyMoneyDetailListInput input)
        {
            var query = from a in _repository.GetAll()
                        where a.UserId == input.UserId
                        select new AgencyMoneyDetailListDto
                        {
                            Amout = a.Amout,
                            BusinessType = a.BusinessType,
                            CreationTime = a.CreationTime
                        };

            query = query.WhereIf(input.BusinessType.HasValue, r => r.BusinessType == input.BusinessType.Value);
            var totalCount = await query.CountAsync();

            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in data)
            {
                item.BusinessTitle = item.BusinessType.ToString();
            }


            return new PagedResultDto<AgencyMoneyDetailListDto>(totalCount, data);
        }








        /// <summary>
        /// 获取我的钱包 信息
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<UserBlanceListDto> Get()
        {

            var agencyModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == AbpSession.UserId.Value);
            if (agencyModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");


            var ret = new UserBlanceListDto()
            {
                Blance = agencyModel.Balance,
                GoodPayment = agencyModel.GoodsPayment,
            };

            var leavelModel = await _b_AgencyLevelRepository.GetAsync(agencyModel.AgencyLevelId);
            ret.Deposit = leavelModel.Deposit;

            return ret;

        }
        /// <summary>
        /// 添加一个B_Order
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task CreateAsync(CreateB_OrderInput input)
        {
            var newmodel = new B_Order()
            {
                UserId = input.UserId,
                Amout = input.Amout,
                Stauts = input.Stauts,
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType,
                InOrOut = input.InOrOut,
                OrderNo = input.OrderNo,
            };

            await _repository.InsertAsync(newmodel);

        }


        public void Create(CreateB_OrderInput input)
        {
            var newmodel = new B_Order()
            {
                UserId = input.UserId,
                Amout = input.Amout,
                Stauts = input.Stauts,
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType,
                InOrOut = input.InOrOut,
                OrderNo = input.OrderNo,
            };
            _repository.Insert(newmodel);

        }

        /// <summary>
        /// 修改一个B_Order
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_OrderInput input)
        {
            //if (input.Id != Guid.Empty)
            //{
            //    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //    if (dbmodel == null)
            //    {
            //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //    }

            //    dbmodel.UserId = input.UserId;
            //    dbmodel.Amout = input.Amout;
            //    dbmodel.Stauts = input.Stauts;

            //    await _repository.UpdateAsync(dbmodel);

            //}
            //else
            //{
            //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //}
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            //await _repository.DeleteAsync(x => x.Id == input.Id);
        }



    }
}