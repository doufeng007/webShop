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
using B_H5.Service.B_Agency.Dto;

namespace B_H5
{
    public class B_OrderAppService : FRMSCoreAppServiceBase, IB_OrderAppService
    {
        private readonly IRepository<B_Order, Guid> _repository;
        private readonly IRepository<B_OrderIn, Guid> _b_OrderInRepository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly IRepository<B_AgencyLevel, Guid> _b_AgencyLevelRepository;
        private readonly IRepository<B_AgencySales, Guid> _b_AgencySalesRepository;
        private readonly IRepository<B_PaymentPrepay, Guid> _b_PaymentPrepayRepository;
        private readonly IRepository<B_Withdrawal, Guid> _b_WithdrawalRepository;
        private readonly IRepository<B_AgencyGroup, Guid> _b_AgencyGroupRepository;
        private readonly IRepository<B_AgencyGroupRelation, Guid> _b_AgencyGroupRelationRepository;

        public B_OrderAppService(IRepository<B_Order, Guid> repository, IRepository<B_OrderIn, Guid> b_OrderInRepository
            , IRepository<B_Agency, Guid> b_AgencyRepository, IRepository<B_AgencyLevel, Guid> b_AgencyLevelRepository
            , IRepository<B_AgencySales, Guid> _repository, IRepository<B_AgencySales, Guid> b_AgencySalesRepository
            , IRepository<B_PaymentPrepay, Guid> b_PaymentPrepayRepository, IRepository<B_Withdrawal, Guid> b_WithdrawalRepository
            , IRepository<B_AgencyGroup, Guid> b_AgencyGroupRepository, IRepository<B_AgencyGroupRelation, Guid> b_AgencyGroupRelationRepository


        )
        {
            this._repository = repository;
            _b_OrderInRepository = b_OrderInRepository;
            _b_AgencyRepository = b_AgencyRepository;
            _b_AgencyLevelRepository = b_AgencyLevelRepository;
            _b_AgencySalesRepository = b_AgencySalesRepository;
            _b_PaymentPrepayRepository = b_PaymentPrepayRepository;
            _b_WithdrawalRepository = b_WithdrawalRepository;
            _b_AgencyGroupRepository = b_AgencyGroupRepository;
            _b_AgencyGroupRelationRepository = b_AgencyGroupRelationRepository;

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
        /// 后台-获取平台金额
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize("WebShop.Manager")]
        public async Task<PagedResultDto<AmoutManagerStatisDto>> GetAmoutManagerStatis(GetAmoutManagerStatisInput input)
        {
            var query1 = from a in _b_OrderInRepository.GetAll()
                         join b in _b_AgencyRepository.GetAll() on a.UserId equals b.UserId
                         join u in UserManager.Users on a.UserId equals u.Id
                         where a.IsOneLeavel == true
                         select new AmoutManagerStatisDto()
                         {
                             AgencyCode = b.AgenCyCode,
                             AgencyLeavelId = b.AgencyLevelId,
                             AgencyName = u.Name,
                             Amout = a.Amout,
                             BusinessType = SystemAmoutType.订单,
                             CreationTime = a.CreationTime,
                             InOrOut = OrderAmoutEnum.入账,
                         };
            var btypelist = new List<OrderAmoutBusinessTypeEnum>();
            btypelist.Add(OrderAmoutBusinessTypeEnum.提现);
            btypelist.Add(OrderAmoutBusinessTypeEnum.充值);
            btypelist.Add(OrderAmoutBusinessTypeEnum.团队管理奖金);
            btypelist.Add(OrderAmoutBusinessTypeEnum.保证金);
            btypelist.Add(OrderAmoutBusinessTypeEnum.推荐奖金);

            var query2 = from a in _repository.GetAll()
                         join u in UserManager.Users on a.UserId equals u.Id
                         join b in _b_AgencyRepository.GetAll() on u.Id equals b.UserId
                         where btypelist.Contains(a.BusinessType)
                         select new AmoutManagerStatisDto
                         {
                             AgencyCode = b.AgenCyCode,
                             AgencyLeavelId = b.AgencyLevelId,
                             AgencyName = u.Name,
                             Amout = a.Amout,
                             BusinessType = (SystemAmoutType)a.BusinessType,
                             CreationTime = a.CreationTime,
                             InOrOut = a.InOrOut == OrderAmoutEnum.入账 ? OrderAmoutEnum.出账 : OrderAmoutEnum.入账,
                         };
            var query = query1.Union(query2);

            query = query.WhereIf(input.BusinessType.HasValue, r => r.BusinessType == input.BusinessType.Value).WhereIf(input.AgencyLeavlId.HasValue, r => r.AgencyLeavelId == input.AgencyLeavlId.Value)
                .WhereIf(input.InOrOut.HasValue, r => r.InOrOut == input.InOrOut.Value).WhereIf(input.StartDate.HasValue, r => r.CreationTime >= input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, r => r.CreationTime <= input.EndDate.Value)
                .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.AgencyName.Contains(input.SearchKey) || r.AgencyCode.Contains(input.SearchKey));

            var totalCount = await query.CountAsync();

            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            foreach (var item in data)
            {
                item.BusinessTypeTitle = item.BusinessType.ToString();
                item.AgencyLeavelName = service.GetAgencyLevelFromCache(item.AgencyLeavelId).Name;
            }


            return new PagedResultDto<AmoutManagerStatisDto>(totalCount, data);
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


        /// <summary>
        /// 获取平台金额统计
        /// </summary>
        /// <returns></returns>
        public async Task<OrderMoneyStatisDto> GetOrderMoneyStatis()
        {
            var ret = new OrderMoneyStatisDto();
            var query1 = from a in _b_AgencySalesRepository.GetAll()
                         select a;
            ret.TotalSaleAmout = query1.Where(r => r.BusinessType == B_AgencySalesBusinessTypeEnum.销售额).Sum(r => r.Sales);
            ret.TotalPrePayAmout = _b_PaymentPrepayRepository.GetAll().Where(r => r.Status == B_PrePayStatusEnum.已通过).Sum(r => r.PayAmout);
            ret.TotalWithDrawalAmout = _b_WithdrawalRepository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.已打款).Sum(r => r.Amout);
            ret.TotalDeposite = _repository.GetAll().Where(r => r.BusinessType == OrderAmoutBusinessTypeEnum.保证金).Sum(r => r.Amout);
            ret.TotalInviteAmout = _repository.GetAll().Where(r => r.BusinessType == OrderAmoutBusinessTypeEnum.推荐奖金).Sum(r => r.Amout);
            ret.TotalOrderOutBonusAmout = _repository.GetAll().Where(r => r.BusinessType == OrderAmoutBusinessTypeEnum.提货).Sum(r => r.Amout);

            ret.TotalTeamDisAmout = _repository.GetAll().Where(r => r.BusinessType == OrderAmoutBusinessTypeEnum.团队管理奖金).Sum(r => r.Amout);
            ret.TotalRewardAmount = ret.TotalInviteAmout + ret.TotalOrderOutBonusAmout + ret.TotalTeamDisAmout;

            ret.TotalAgencyBlance = _b_AgencyRepository.GetAll().Sum(r => r.Balance);
            ret.TotalAgencyPayment = _b_AgencyRepository.GetAll().Sum(r => r.GoodsPayment);

            ret.TotalSystemBlance = _b_OrderInRepository.GetAll().Where(r => r.IsOneLeavel).Sum(r => r.Amout);

            ret.TotalSystemAmout = ret.TotalAgencyBlance + ret.TotalAgencyPayment + ret.TotalSystemBlance;

            return ret;

        }



        /// <summary>
        /// 获取平台金额统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_SyatemAmoutStatisDto>> GetSyatemAmoutStatis(B_SyatemAmoutStatisInput input)
        {
            if (input.BType == B_SyatemAmoutStatisType.销售额)
            {
                var query = from a in _b_AgencySalesRepository.GetAll()
                            where a.BusinessType == B_AgencySalesBusinessTypeEnum.销售额 && a.CreationTime >= input.StartDate && a.CreationTime <= input.EndDate
                            select new
                            {
                                a.Sales,
                                CreatDay = a.CreationTime.ToString("yyyyMMdd"),
                                CreatMonth = a.CreationTime.ToString("yyyyMM"),
                            };
                if (input.DayOrMonth == 1)
                {
                    var groupQuery = query.GroupBy(r => r.CreatDay).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Sales), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
                else
                {
                    var groupQuery = query.GroupBy(r => r.CreatMonth).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Sales), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
            }
            else if (input.BType == B_SyatemAmoutStatisType.充值金额)
            {
                var query = from a in _b_PaymentPrepayRepository.GetAll()
                            where a.Status == B_PrePayStatusEnum.已通过 && a.CreationTime >= input.StartDate && a.CreationTime <= input.EndDate
                            select new
                            {
                                a.PayAmout,
                                CreatDay = a.CreationTime.ToString("yyyyMMdd"),
                                CreatMonth = a.CreationTime.ToString("yyyyMM"),
                            };
                if (input.DayOrMonth == 1)
                {
                    var groupQuery = query.GroupBy(r => r.CreatDay).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.PayAmout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
                else
                {
                    var groupQuery = query.GroupBy(r => r.CreatMonth).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.PayAmout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
            }

            else if (input.BType == B_SyatemAmoutStatisType.提现金额)
            {
                var query = from a in _b_WithdrawalRepository.GetAll()
                            where a.Status == B_WithdrawalStatusEnum.已打款 && a.CreationTime >= input.StartDate && a.CreationTime <= input.EndDate
                            select new
                            {
                                a.Amout,
                                CreatDay = a.CreationTime.ToString("yyyyMMdd"),
                                CreatMonth = a.CreationTime.ToString("yyyyMM"),
                            };
                if (input.DayOrMonth == 1)
                {
                    var groupQuery = query.GroupBy(r => r.CreatDay).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
                else
                {
                    var groupQuery = query.GroupBy(r => r.CreatMonth).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
            }


            else if (input.BType == B_SyatemAmoutStatisType.保证金)
            {
                var query = from a in _repository.GetAll()
                            where a.BusinessType == OrderAmoutBusinessTypeEnum.保证金 && a.CreationTime >= input.StartDate && a.CreationTime <= input.EndDate
                            select new
                            {
                                a.Amout,
                                CreatDay = a.CreationTime.ToString("yyyyMMdd"),
                                CreatMonth = a.CreationTime.ToString("yyyyMM"),
                            };
                if (input.DayOrMonth == 1)
                {
                    var groupQuery = query.GroupBy(r => r.CreatDay).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
                else
                {
                    var groupQuery = query.GroupBy(r => r.CreatMonth).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
            }

            else if (input.BType == B_SyatemAmoutStatisType.提货奖)
            {
                var query = from a in _repository.GetAll()
                            where a.BusinessType == OrderAmoutBusinessTypeEnum.提货 && a.CreationTime >= input.StartDate && a.CreationTime <= input.EndDate
                            select new
                            {
                                a.Amout,
                                CreatDay = a.CreationTime.ToString("yyyyMMdd"),
                                CreatMonth = a.CreationTime.ToString("yyyyMM"),
                            };
                if (input.DayOrMonth == 1)
                {
                    var groupQuery = query.GroupBy(r => r.CreatDay).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
                else
                {
                    var groupQuery = query.GroupBy(r => r.CreatMonth).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
            }

            else if (input.BType == B_SyatemAmoutStatisType.推荐奖)
            {
                var query = from a in _repository.GetAll()
                            where a.BusinessType == OrderAmoutBusinessTypeEnum.推荐奖金 && a.CreationTime >= input.StartDate && a.CreationTime <= input.EndDate
                            select new
                            {
                                a.Amout,
                                CreatDay = a.CreationTime.ToString("yyyyMMdd"),
                                CreatMonth = a.CreationTime.ToString("yyyyMM"),
                            };
                if (input.DayOrMonth == 1)
                {
                    var groupQuery = query.GroupBy(r => r.CreatDay).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
                else
                {
                    var groupQuery = query.GroupBy(r => r.CreatMonth).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
            }

            else if (input.BType == B_SyatemAmoutStatisType.销售返点奖)
            {
                var query = from a in _repository.GetAll()
                            where a.BusinessType == OrderAmoutBusinessTypeEnum.团队管理奖金 && a.CreationTime >= input.StartDate && a.CreationTime <= input.EndDate
                            select new
                            {
                                a.Amout,
                                CreatDay = a.CreationTime.ToString("yyyyMMdd"),
                                CreatMonth = a.CreationTime.ToString("yyyyMM"),
                            };
                if (input.DayOrMonth == 1)
                {
                    var groupQuery = query.GroupBy(r => r.CreatDay).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
                else
                {
                    var groupQuery = query.GroupBy(r => r.CreatMonth).Select(r => new B_SyatemAmoutStatisDto { Amout = r.Sum(o => o.Amout), Date = r.Key });
                    var totalCount = await groupQuery.CountAsync();
                    var ret = await groupQuery.OrderBy(r => r.Date).PageBy(input).ToListAsync();
                    return new PagedResultDto<B_SyatemAmoutStatisDto>(totalCount, ret);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "参数不正确！");
            }


        }




        ///// <summary>
        ///// 销售排行榜
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<PagedResultDto<SyatemAmoutOrderDto>> GetInViteCountStatis(TeamCountStatisInput input)
        //{
        //    var query = from a in _b_AgencyGroupRepository.GetAll()
        //                join b in _b_AgencyGroupRelationRepository.GetAll() on a.Id equals b.GroupId
        //                join agency in _repository.GetAll() on b.AgencyId equals agency.Id
        //                join u in UserManager.Users on agency.UserId equals u.Id
        //                let c = from agency in _repository.GetAll()
        //                        join r in _b_AgencyGroupRelationRepository.GetAll() on agency.Id equals r.AgencyId
        //                        where r.GroupId == a.Id
        //                        select agency
        //                where b.IsGroupLeader == true
        //                select new TeamCountStatisDto
        //                {
        //                    AgencyCount = c.Count(),
        //                    Name = u.Name,
        //                };

        //    var totalCount = await query.CountAsync();
        //    var data = await query.OrderByDescending(r => r.AgencyCount).PageBy(input).ToListAsync();
        //    var ret = new PagedResultDto<TeamCountStatisDto>(totalCount, data);
        //    return ret;
        //}


        //public async Task<PagedResultDto<SyatemAmoutOrderDto>> GetAgencySaleOrderStatis(TeamCountStatisInput input)
        //{

        //}



    }
}