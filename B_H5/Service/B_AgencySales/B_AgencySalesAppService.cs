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
    public class B_AgencySalesAppService : FRMSCoreAppServiceBase, IB_AgencySalesAppService
    {
        private readonly IRepository<B_AgencySales, Guid> _repository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly IRepository<B_AgencyGroup, Guid> _b_AgencyGroupRepository;
        private readonly IRepository<B_AgencyGroupRelation, Guid> _b_AgencyGroupRelationRepository;
        private readonly IRepository<B_Categroy, Guid> _b_CategroyRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public B_AgencySalesAppService(IRepository<B_AgencySales, Guid> repository, IRepository<B_Agency, Guid> b_AgencyRepository
            , IRepository<B_AgencyGroup, Guid> b_AgencyGroupRepository, IRepository<B_AgencyGroupRelation, Guid> b_AgencyGroupRelationRepository
            , IRepository<B_Categroy, Guid> b_CategroyRepository, IAbpFileRelationAppService abpFileRelationAppService

        )
        {
            this._repository = repository;
            _b_AgencyRepository = b_AgencyRepository;
            _b_AgencyGroupRepository = b_AgencyGroupRepository;
            _b_AgencyGroupRelationRepository = b_AgencyGroupRelationRepository;
            _b_CategroyRepository = b_CategroyRepository;
            _abpFileRelationAppService = abpFileRelationAppService;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<B_AgencySalesCategroyListOutputDto>> GetListByCategroyId(GetB_AgencyCategroySalesListInput input)
        {
            var startDate = new DateTime(input.SalesDateYear, input.SalesDateMonth, 1, 0, 0, 0);
            var enddate = startDate.AddMonths(1);
            var agencyModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == AbpSession.UserId.Value);
            if (agencyModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");

            var query = from s in _repository.GetAll()
                        join a in _b_AgencyRepository.GetAll() on s.AgencyId equals a.Id
                        join u in UserManager.Users on a.UserId equals u.Id
                        where a.OriginalPid == agencyModel.Id && s.SalesDate >= startDate && s.SalesDate < enddate
                        && s.CategroyId == input.CategroyId
                        select new { s.AgencyId, s.Sales, u.Name, a.AgencyLevelId };
            var groupQuery = query.GroupBy(r => new { r.AgencyId, r.Name, r.AgencyLevelId }).Select(r =>
            new B_AgencySalesCategroyListOutputDto
            {
                AgencyId = r.Key.AgencyId,
                AgencyName = r.Key.Name,
                AgencyLeavelId = r.Key.AgencyLevelId,
                Sales = r.Sum(o => o.Sales)
            });
            var toalCount = await query.CountAsync();
            var ret = await groupQuery.OrderByDescending(r => r.AgencyName).PageBy(input).ToListAsync();

            var businessIds = ret.Select(r => r.AgencyId.ToString()).ToList();
            if (businessIds.Count > 0)
            {
                var fileGroups = await _abpFileRelationAppService.GetMultiListAsync(new GetMultiAbpFilesInput()
                {
                    BusinessIds = businessIds,
                    BusinessType = AbpFileBusinessType.代理头像
                });
                foreach (var item in ret)
                    if (fileGroups.Any(r => r.BusinessId == item.AgencyId.ToString()))
                    {
                        var fileModel = fileGroups.FirstOrDefault(r => r.BusinessId == item.AgencyId.ToString());
                        if (fileModel != null)
                        {
                            var files = fileModel.Files;
                            if (files.Count > 0)
                                item.File = files.FirstOrDefault();
                        }

                    }
            }

            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            foreach (var item in ret)
            {
                item.AgencyLeavelName = service.GetAgencyLevelFromCache(item.AgencyLeavelId).Name;
            }

            return new PagedResultDto<B_AgencySalesCategroyListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// H5 获取 我的销售额 类别 销售额明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<B_AgencySalesListOutputDto>> GetList(GetB_AgencySalesListInput input)
        {
            var startDate = new DateTime(input.SalesDateYear, input.SalesDateMonth, 1, 0, 0, 0);
            var enddate = startDate.AddMonths(1);

            var query = from a in _repository.GetAll()
                        where a.SalesDate >= startDate && a.SalesDate < enddate && a.UserId == AbpSession.UserId.Value
                        select a;
            var groupQuery = query.GroupBy(r => r.CategroyId).Select(r => new { CategroyId = r.Key, Sales = r.Sum(o => o.Sales) });


            var retQuery = from a in groupQuery
                           join b in _b_CategroyRepository.GetAll() on a.CategroyId equals b.Id
                           select new B_AgencySalesListOutputDto
                           {
                               CategroyId = a.CategroyId,
                               CategroyName = b.Name,
                               Discount = 0,
                               Sales = a.Sales
                           };

            var toalCount = await query.CountAsync();
            var ret = await retQuery.OrderByDescending(r => r.CategroyName).PageBy(input).ToListAsync();


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



            return new PagedResultDto<B_AgencySalesListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// H5获取当前登录用户指定月的销售额统计数据
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<B_AgencySalesOutputDto> Get(GetB_AgencySalesInput input)
        {
            var startDate = new DateTime(input.Year, input.Month, 1, 0, 0, 0);
            var enddate = startDate.AddMonths(1);
            var query = from a in _repository.GetAll()
                        where a.SalesDate >= startDate && a.SalesDate < enddate && a.UserId == AbpSession.UserId.Value
                        && (!input.CategroyId.HasValue || a.CategroyId == input.CategroyId.Value)
                        select a;
            var ret = new B_AgencySalesOutputDto();
            ret.Bonus = query.Sum(r => r.Bonus);
            ret.Profit = query.Sum(r => r.Profit);
            ret.TotalSales = query.Sum(r => r.Sales);

            var agencyModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == AbpSession.UserId.Value);
            if (agencyModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");
            if (agencyModel.AgencyLevel == 1)
            {
                var groupId = _b_AgencyGroupRelationRepository.FirstOrDefault(r => r.IsGroupLeader && r.AgencyId == agencyModel.Id).GroupId;

                var team_LeavelOneAgencyQuery = from s in _repository.GetAll()
                                                join a in _b_AgencyRepository.GetAll() on s.AgencyId equals a.Id
                                                join b in _b_AgencyGroupRelationRepository.GetAll() on a.Id equals b.AgencyId
                                                where b.GroupId == groupId && a.AgencyLevel == 1 && a.Id != agencyModel.Id
                                                &&
                                                s.SalesDate >= startDate && s.SalesDate < enddate && (!input.CategroyId.HasValue || s.CategroyId == input.CategroyId.Value)
                                                select s;
                var result = team_LeavelOneAgencyQuery.GroupBy(r => r.AgencyId).Select(r => new { AgencyId = r.Key, TotalSales = r.Sum(o => o.Sales) });

                var otherOneAgencyTotalSales = result.Sum(r => r.TotalSales);

                var teamSalse = ret.TotalSales + otherOneAgencyTotalSales;

                //ret.Discount =


            }


            return ret;
        }



        /// <summary>
        /// 添加一个B_AgencySales
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_AgencySalesInput input)
        {
            var newmodel = new B_AgencySales()
            {
                UserId = input.UserId,
                AgencyId = input.AgencyId,
                CategroyId = input.CategroyId,
                Sales = input.Sales,
                SalesDate = input.SalesDate,
                Profit = input.Profit,
                Discount = input.Discount
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个B_AgencySales
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_AgencySalesInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.UserId = input.UserId;
                dbmodel.AgencyId = input.AgencyId;
                dbmodel.CategroyId = input.CategroyId;
                dbmodel.Sales = input.Sales;
                dbmodel.SalesDate = input.SalesDate;
                dbmodel.Profit = input.Profit;
                dbmodel.Discount = input.Discount;

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