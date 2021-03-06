﻿using System;
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
using ZCYX.FRMSCore.Model;

namespace Supply
{
    public class CuringProcurementPlanAppService : FRMSCoreAppServiceBase, ICuringProcurementPlanAppService
    {
        private readonly IRepository<CuringProcurementPlan, Guid> _repository;

        public CuringProcurementPlanAppService(IRepository<CuringProcurementPlan, Guid> repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CuringProcurementPlanListOutputDto>> GetList(GetCuringProcurementPlanListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        select new CuringProcurementPlanListOutputDto()
                        {
                            Id = a.Id,
                            MainId = a.MainId,
                            Name = a.Name,
                            Version = a.Version,
                            Number = a.Number,
                            Unit = a.Unit,
                            Money = a.Money,
                            Des = a.Des,
                            Type = a.Type,
                            Remark = a.Remark,
                            Status = a.Status,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<CuringProcurementPlanListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<CuringProcurementPlanOutputDto> Get(NullableIdDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<CuringProcurementPlanOutputDto>();
        }
        /// <summary>
        /// 添加一个CuringProcurementPlan
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Create(CreateCuringProcurementPlanInput input)
        {
            var newmodel = new CuringProcurementPlan()
            {
                MainId = input.MainId,
                Name = input.Name,
                Version = input.Version,
                Number = input.Number,
                Unit = input.Unit,
                Money = input.Money,
                Des = input.Des,
                Type = input.Type,
                Remark = input.Remark,
                Status = input.Status,
                BusinessType = input.BusinessType,
            };
            await _repository.InsertAsync(newmodel);
        }

        /// <summary>
        /// 修改一个CuringProcurementPlan
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateCuringProcurementPlanInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                dbmodel.MainId = input.MainId;
                dbmodel.Name = input.Name;
                dbmodel.Version = input.Version;
                dbmodel.Number = input.Number;
                dbmodel.Unit = input.Unit;
                dbmodel.Money = input.Money;
                dbmodel.Des = input.Des;
                dbmodel.Type = input.Type;
                dbmodel.Remark = input.Remark;
                dbmodel.Status = input.Status;
                dbmodel.BusinessType = input.BusinessType;

                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

        // <summary>
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