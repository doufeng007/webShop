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
    public class B_WithdrawalAppService : FRMSCoreAppServiceBase, IB_WithdrawalAppService
    {
        private readonly IRepository<B_Withdrawal, Guid> _repository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;

        public B_WithdrawalAppService(IRepository<B_Withdrawal, Guid> repository, IRepository<B_Agency, Guid> b_AgencyRepository

        )
        {
            this._repository = repository;
            _b_AgencyRepository = b_AgencyRepository;

        }

        /// <summary>
        /// 提现审核/打款-列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_WithdrawalListOutputDto>> GetList(GetB_WithdrawalListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_AgencyRepository.GetAll() on a.CreatorUserId.Value equals b.UserId
                        join u in UserManager.Users on b.UserId equals u.Id
                        select new B_WithdrawalListOutputDto()
                        {
                            Id = a.Id,
                            Code = a.Code,
                            PayTime = a.PayTime,
                            Tel = u.PhoneNumber,
                            UserName = u.Name,
                            BankName = a.BankName,
                            BankBranchName = a.BankBranchName,
                            BankUserName = a.BankUserName,
                            BankNumber = a.BankNumber,
                            Amout = a.Amout,
                            CreationTime = a.CreationTime,
                            AgencyLevelId = b.AgencyLevelId,


                        };
            query = query
                //.WhereIf(input.PayType.HasValue, r => r.PayType == input.PayType.Value)
                .WhereIf(input.AgencyLevelId.HasValue, r => r.AgencyLevelId == input.AgencyLevelId.Value)
                .WhereIf(input.Status.HasValue, r => r.Status == input.Status.Value)
                .WhereIf(input.PayDateStart.HasValue, r => r.PayTime >= input.PayDateStart.Value)
                .WhereIf(input.PayDateEnd.HasValue, r => r.PayTime <= input.PayDateEnd.Value)
                .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.Code.Contains(input.SearchKey) ||
                      r.UserName.Contains(input.SearchKey) || r.Tel.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_WithdrawalListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 管理-提现审核列表 数量统计
        /// </summary>
        /// <returns></returns>
        public async Task<B_AgencyApplyCount> GetAuditCount()
        {
            var ret = new B_AgencyApplyCount();
            ret.WaitAuditCount = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.待审核).CountAsync();
            ret.PassCount = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.待打款).CountAsync();
            ret.NoPassCount = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.未通过).CountAsync();
            return ret;
        }

        /// <summary>
        /// 管理-提现打款列表 数量统计
        /// </summary>
        /// <returns></returns>
        public async Task<B_WithdrawalCount> GetCount()
        {
            var ret = new B_WithdrawalCount();
            ret.WaitAuditCount = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.待打款).CountAsync();
            ret.PassCount = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.已打款).CountAsync();
            ret.NoPassCount = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.打款异常).CountAsync();
            ret.PassAmout = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.已打款).SumAsync(r => r.Amout);
            ret.WaitAmout = await _repository.GetAll().Where(r => r.Status == B_WithdrawalStatusEnum.待打款).SumAsync(r => r.Amout);
            return ret;
        }

        /// <summary>
        /// 获取提现申请详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_WithdrawalOutputDto> Get(EntityDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<B_WithdrawalOutputDto>();
            ret.UserName = (await UserManager.GetUserByIdAsync(model.CreatorUserId.Value)).Name;
            return ret;
        }


        /// <summary>
        /// 新增提现申请
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]

        public async Task Create(CreateB_WithdrawalInput input)
        {
            var agencyModel = await _b_AgencyRepository.FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value);
            if (agencyModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理数据不存在！");
            if (agencyModel.Balance < input.Amout)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"余额不足，不能提现{input.Amout}元！");
            agencyModel.Balance = agencyModel.Balance - input.Amout;
            await _b_AgencyRepository.UpdateAsync(agencyModel);
            var newmodel = new B_Withdrawal()
            {
                BankName = input.BankName,
                BankBranchName = input.BankBranchName,
                BankUserName = input.BankUserName,
                BankNumber = input.BankNumber,
                Amout = input.Amout,
                //Reason = input.Reason,
                //Remark = input.Remark
                Status = B_WithdrawalStatusEnum.待审核
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个B_Withdrawal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_WithdrawalInput input)
        {
            //if (input.Id != Guid.Empty)
            //{
            //    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //    if (dbmodel == null)
            //    {
            //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //    }

            //    dbmodel.BankName = input.BankName;
            //    dbmodel.BankBranchName = input.BankBranchName;
            //    dbmodel.BankUserName = input.BankUserName;
            //    dbmodel.BankNumber = input.BankNumber;
            //    dbmodel.Amout = input.Amout;
            //    await _repository.UpdateAsync(dbmodel);

            //}
            //else
            //{
            //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //}
        }

        /// <summary>
        /// 审核打款申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Audit(AuditB_WithdrawalInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            var agencyModel = await _b_AgencyRepository.FirstOrDefaultAsync(r => r.UserId == model.CreatorUserId.Value);
            if (agencyModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理数据不存在！");
            if (input.IsPass)
            {

                model.Status = B_WithdrawalStatusEnum.待打款;
                model.AuditRemark = input.Remark;
            }
            else
            {
                model.Status = B_WithdrawalStatusEnum.未通过;
                model.Reason = input.Reason;
                model.AuditRemark = input.Remark;
                agencyModel.Balance = agencyModel.Balance + model.Amout;
            }
            await _repository.UpdateAsync(model);
        }


        /// <summary>
        /// 完成打款
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Remit(RemitInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            if (model.Status != B_WithdrawalStatusEnum.待打款)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "审核结果为不通过，不能打款！");
            else if (model.Status != B_WithdrawalStatusEnum.已打款)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "已完成打款，不能多次打款！");
            else if (model.Status != B_WithdrawalStatusEnum.待审核)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "还未审核完成，不能打款！");
            else
            {
                if (input.IsSucce)
                {
                    model.Status = B_WithdrawalStatusEnum.已打款;
                    model.PayTime = DateTime.Now;
                    model.Remark = input.Remark;
                }
                else
                {
                    model.Status = B_WithdrawalStatusEnum.打款异常;
                    model.Remark = input.Remark;
                }

                await _repository.UpdateAsync(model);
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