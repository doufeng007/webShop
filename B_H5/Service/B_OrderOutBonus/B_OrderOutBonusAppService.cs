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
    public class B_OrderOutBonusAppService : FRMSCoreAppServiceBase, IB_OrderOutBonusAppService
    {
        private readonly IRepository<B_OrderOutBonus, Guid> _repository;

        public B_OrderOutBonusAppService(IRepository<B_OrderOutBonus, Guid> repository

        )
        {
            this._repository = repository;

        }

        /// <summary>
        /// 提货奖金规则
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_OrderOutBonusListOutputDto>> GetList(GetB_OrderOutBonusListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join u in UserManager.Users on a.CreatorUserId equals u.Id
                        select new B_OrderOutBonusListOutputDto()
                        {
                            Id = a.Id,
                            Amout = a.Amout,
                            EffectTime = a.EffectTime,
                            FailureTime = a.FailureTime,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                            CreateUserName = u.Name

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_OrderOutBonusListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 获取提货奖金规则
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_OrderOutBonusOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_OrderOutBonusOutputDto>();
        }


        /// <summary>
        /// 新增一个提货奖金
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Create(CreateB_OrderOutBonusInput input)
        {
            var hasEffectModel = await _repository.FirstOrDefaultAsync(r => r.Status == BonusRuleStatusEnum.有效);
            if (hasEffectModel == null)
            {
                var newmodel = new B_OrderOutBonus()
                {
                    Amout = input.Amout,
                    EffectTime = DateTime.Now,
                    Status = BonusRuleStatusEnum.有效
                };
                await _repository.InsertAsync(newmodel);
            }
            else
            {
                hasEffectModel.Status = BonusRuleStatusEnum.失效;
                var failDate = DateTime.Now.AddDays(1);
                hasEffectModel.FailureTime = new DateTime(failDate.Year, failDate.Month, failDate.Day, 0, 0, 0);
                await _repository.UpdateAsync(hasEffectModel);

                var newmodel = new B_OrderOutBonus()
                {
                    Amout = input.Amout,
                    EffectTime = new DateTime(failDate.Year, failDate.Month, failDate.Day, 0, 0, 0),
                    Status = BonusRuleStatusEnum.有效
                };
                await _repository.InsertAsync(newmodel);

            }



        }



        public async Task<decimal> GetEffectAmoutAsync()
        {
            var nowTime = DateTime.Now;
            var query = from a in _repository.GetAll()
                        where nowTime >= a.EffectTime && nowTime < a.FailureTime
                        select a;
            var count = await query.CountAsync();
            if (count > 1)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提货奖金重复。设置异常！");
            else if (count == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未找到提货奖金设置！");
            else
            {
                var rule = await query.FirstOrDefaultAsync();
                return rule.Amout;
            }


        }


        public decimal GetEffectAmout()
        {
            var nowTime = DateTime.Now;
            var query = from a in _repository.GetAll()
                        where nowTime >= a.EffectTime && nowTime < a.FailureTime
                        select a;
            var count = query.Count();
            if (count > 1)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提货奖金重复。设置异常！");
            else if (count == 0)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未找到提货奖金设置！");
            else
            {
                var rule = query.FirstOrDefault();
                return rule.Amout;
            }


        }






    }
}