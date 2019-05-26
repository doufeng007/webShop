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
using Abp.WeChat;
using Abp;

namespace B_H5
{
    public class B_WithdrawalAppService : FRMSCoreAppServiceBase, IB_WithdrawalAppService
    {
        private readonly IRepository<B_Withdrawal, Guid> _repository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly WxTemplateMessageManager _wxTemplateMessageManager;

        public B_WithdrawalAppService(IRepository<B_Withdrawal, Guid> repository, IRepository<B_Agency, Guid> b_AgencyRepository, WxTemplateMessageManager wxTemplateMessageManager

        )
        {
            this._repository = repository;
            _b_AgencyRepository = b_AgencyRepository;
            _wxTemplateMessageManager = wxTemplateMessageManager;

        }

        /// <summary>
        /// 提现审核/打款-列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_WithdrawalListOutputDto>> GetList(GetB_WithdrawalListInput input)
        {

            var statusArry = new List<B_WithdrawalStatusEnum>();
            if (input.ListType == 1)
            {
                statusArry.Add(B_WithdrawalStatusEnum.待审核);
                statusArry.Add(B_WithdrawalStatusEnum.未通过);
            }
            else if (input.ListType == 2)
            {
                statusArry.Add(B_WithdrawalStatusEnum.待打款);
                statusArry.Add(B_WithdrawalStatusEnum.已打款);
                statusArry.Add(B_WithdrawalStatusEnum.打款异常);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "参数异常！");
            }

            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_AgencyRepository.GetAll() on a.CreatorUserId.Value equals b.UserId
                        join u in UserManager.Users on b.UserId equals u.Id
                        where statusArry.Contains(a.Status)
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
                            Status = a.Status


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
        /// H5- 我的提款记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<B_WithdrawalListOutputDto>> GetMyList(GetB_WithdrawalListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_AgencyRepository.GetAll() on a.CreatorUserId.Value equals b.UserId
                        join u in UserManager.Users on b.UserId equals u.Id
                        where a.CreatorUserId==AbpSession.UserId
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
                            Status = a.Status


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
                Code = DateTime.Now.ToString("yyyyMMddHHmmSS"),
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
            var dic = new Dictionary<string, string>();
            dic.Add("keyword1", model.Code);
            dic.Add("keyword2", model.Amout.ToString());
            dic.Add("keyword3", model.CreationTime.ToString());
            var title = "";
            var remark = "";
            var btype = Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.提现申请审核通过;

            if (input.IsPass)
            {

                model.Status = B_WithdrawalStatusEnum.待打款;
                model.AuditRemark = input.Remark;

                title = "您的提现申请已审核通过";
                remark = "提现金额7个工作日内到账（节假日顺延），请知晓并耐心等待！";

            }
            else
            {
                model.Status = B_WithdrawalStatusEnum.未通过;
                model.Reason = input.Reason;
                model.AuditRemark = input.Remark;
                agencyModel.Balance = agencyModel.Balance + model.Amout;

                title = "您的提现申请审核未通过";
                remark = $"审核不通过原因{model.Reason}";
                btype = Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.提现申请审核不通过;
            }
            await _repository.UpdateAsync(model);

            _wxTemplateMessageManager.SendWeChatMsg(model.Id.ToString(), btype,
                agencyModel.OpenId, title, dic, remark);


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


                    var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_OrderAppService>();
                    await service.CreateAsync(new CreateB_OrderInput()
                    {
                        Amout = model.Amout,
                        BusinessId = model.Id,
                        BusinessType = OrderAmoutBusinessTypeEnum.提现,
                        InOrOut = OrderAmoutEnum.出账,
                        OrderNo = model.Code,
                        UserId = model.CreatorUserId.Value,
                        IsBlance = true,
                        IsGoodsPayment = false,
                    });

                    var _b_MessageAppService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_MessageAppService>();
                    _b_MessageAppService.Create(new CreateB_MessageInput()
                    {
                        BusinessId = model.Id,
                        BusinessType = B_H5MesagessType.订单,
                        Code = model.Code,
                        Content = $"提现单号：{model.Code}已经审核通过，{model.Amout}元已经打入银行账户，请注意查收",
                        UserId = AbpSession.UserId.Value,
                    });
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