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

namespace B_H5
{
    public class B_TeamSaleBonusAppService : FRMSCoreAppServiceBase, IB_TeamSaleBonusAppService
    {
        private readonly IRepository<B_TeamSaleBonus, Guid> _repository;
        private readonly IRepository<B_TeamSaleBonusDetail, Guid> _detailRepository;


        public B_TeamSaleBonusAppService(IRepository<B_TeamSaleBonus, Guid> repository, IRepository<B_TeamSaleBonusDetail, Guid> detailRepository

        )
        {
            this._repository = repository;
            _detailRepository = detailRepository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_TeamSaleBonusListOutputDto>> GetList(GetB_TeamSaleBonusListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join u in UserManager.Users on a.CreatorUserId equals u.Id
                        let d =
                        from de in _detailRepository.GetAll()
                        where de.Pid == a.Id
                        select new B_TeamSaleBonusDetailListOutputDto()
                        {
                            Id = de.Id,
                            MaxSale = de.MaxSale,
                            MinSale = de.MinSale,
                            Pid = de.Pid,
                            Scale = de.Scale
                        }
                        select new B_TeamSaleBonusListOutputDto()
                        {
                            Id = a.Id,
                            EffectTime = a.EffectTime,
                            FailureTime = a.FailureTime,
                            CreationTime = a.CreationTime,
                            CreatorUserName = u.Name,
                            Details = d.ToList()
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_TeamSaleBonusListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_TeamSaleBonusOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_TeamSaleBonusOutputDto>();
        }
        /// <summary>
        /// 新增一个团结奖金系数
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Create(CreateB_TeamSaleBonusInput input)
        {
            var hasEffectModel = await _repository.FirstOrDefaultAsync(r => r.Status == BonusRuleStatusEnum.有效);
            if (hasEffectModel == null)
            {
                var newmodel = new B_TeamSaleBonus()
                {
                    Id = Guid.NewGuid(),
                    EffectTime = DateTime.Now,
                    Status = BonusRuleStatusEnum.有效
                };
                await _repository.InsertAsync(newmodel);
                foreach (var item in input.Details)
                {
                    var entity = new B_TeamSaleBonusDetail()
                    {
                        Id = Guid.NewGuid(),
                        Pid = newmodel.Id,
                        MaxSale = item.MaxSale,
                        MinSale = item.MinSale,
                        Scale = item.Scale
                    };

                    await _detailRepository.InsertAsync(entity);
                }


            }
            else
            {
                hasEffectModel.Status = BonusRuleStatusEnum.失效;
                var failDate = DateTime.Now.AddMonths(1);
                hasEffectModel.FailureTime = new DateTime(failDate.Year, failDate.Month, 1, 0, 0, 0);
                await _repository.UpdateAsync(hasEffectModel);

                var newmodel = new B_TeamSaleBonus()
                {
                    EffectTime = new DateTime(failDate.Year, failDate.Month, 1, 0, 0, 0),
                    Status = BonusRuleStatusEnum.有效
                };
                await _repository.InsertAsync(newmodel);
                foreach (var item in input.Details)
                {
                    var entity = new B_TeamSaleBonusDetail()
                    {
                        Id = Guid.NewGuid(),
                        Pid = newmodel.Id,
                        MaxSale = item.MaxSale,
                        MinSale = item.MinSale,
                        Scale = item.Scale
                    };

                    await _detailRepository.InsertAsync(entity);
                }

            }

        }


        /// <summary>
        /// 获取有效团队奖金设置
        /// </summary>
        /// <param name="amout"></param>
        /// <returns></returns>
        public decimal GetEffectScale(decimal amout)
        {
            var dateNow = DateTime.Now;
            var query = from a in _repository.GetAll()
                        join b in _detailRepository.GetAll() on a.Id equals b.Pid
                        where dateNow >= a.EffectTime && dateNow < a.FailureTime
                        select b;
            var item = query.Where(r => amout >= r.MinSale && amout < r.MaxSale).ToList();
            if (item.Count > 1)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "团队奖金重复。设置异常！");
            else if (item.Count == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未找到团队奖金设置！");
            else
            {
                var rule = item.FirstOrDefault();
                return rule.Scale;
            }
        }



    }
}