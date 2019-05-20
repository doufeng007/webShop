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
    public class B_PaymentPrepayAppService : FRMSCoreAppServiceBase, IB_PaymentPrepayAppService
    {
        private readonly IRepository<B_PaymentPrepay, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly WxTemplateMessageManager _wxTemplateMessageManager;
        private readonly IB_MessageAppService _b_MessageAppService;

        public B_PaymentPrepayAppService(IRepository<B_PaymentPrepay, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<B_Agency, Guid> b_AgencyRepository, WxTemplateMessageManager wxTemplateMessageManager, IB_MessageAppService b_MessageAppService

        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _b_AgencyRepository = b_AgencyRepository;
            _wxTemplateMessageManager = wxTemplateMessageManager;
            _b_MessageAppService = b_MessageAppService;

        }

        /// <summary>
        /// 管理后台充值审核列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_PaymentPrepayListOutputDto>> GetList(GetB_PaymentPrepayListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_AgencyRepository.GetAll() on a.CreatorUserId.Value equals b.UserId
                        join u in UserManager.Users on b.UserId equals u.Id
                        select new B_PaymentPrepayListOutputDto()
                        {
                            Id = a.Id,
                            UserName = u.Name,
                            AgencyLevelId = b.AgencyLevelId,
                            Status = a.Status,
                            Tel = u.PhoneNumber,
                            Code = a.Code,
                            PayType = a.PayType,
                            PayAmout = a.PayAmout,
                            PayDate = a.PayDate,
                            Remark = a.Remark,
                            CreationTime = a.CreationTime

                        };
            query = query.WhereIf(input.PayType.HasValue, r => r.PayType == input.PayType.Value).WhereIf(input.AgencyLevelId.HasValue, r => r.AgencyLevelId == input.AgencyLevelId.Value)
                .WhereIf(input.Status.HasValue, r => r.Status == input.Status.Value)
                .WhereIf(input.PayDateStart.HasValue, r => r.PayDate >= input.PayDateStart.Value)
                .WhereIf(input.PayDateEnd.HasValue, r => r.PayDate <= input.PayDateEnd.Value)
                .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.Code.Contains(input.SearchKey) ||
                      r.UserName.Contains(input.SearchKey) || r.Tel.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_PaymentPrepayListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 管理-充值审核列表 数量统计
        /// </summary>
        /// <returns></returns>
        public async Task<B_AgencyApplyCount> GetCount()
        {
            var ret = new B_AgencyApplyCount();
            ret.WaitAuditCount = await _repository.GetAll().Where(r => r.Status == B_PrePayStatusEnum.待审核).CountAsync();
            ret.PassCount = await _repository.GetAll().Where(r => r.Status == B_PrePayStatusEnum.已通过).CountAsync();
            ret.NoPassCount = await _repository.GetAll().Where(r => r.Status == B_PrePayStatusEnum.待审核).CountAsync();
            return ret;
        }



        /// <summary>
        /// wx货款充值记录-公众号查看
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<B_PaymentPrepayListForWxOutputDto>> GetListForWx(GetB_PaymentPrepayListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where a.CreatorUserId == AbpSession.UserId
                        select new B_PaymentPrepayListForWxOutputDto()
                        {
                            Id = a.Id,
                            Status = a.Status,
                            Code = a.Code,
                            PayType = a.PayType,
                            PayAmout = a.PayAmout,
                            PayDate = a.PayDate,
                            CreationTime = a.CreationTime
                        };
            query = query.WhereIf(input.PayType.HasValue, r => r.PayType == input.PayType.Value)
                .WhereIf(input.Status.HasValue, r => r.Status == input.Status.Value)
                .WhereIf(input.PayDateStart.HasValue, r => r.PayDate >= input.PayDateStart.Value)
                .WhereIf(input.PayDateEnd.HasValue, r => r.PayDate <= input.PayDateEnd.Value)
                .WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.Code.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_PaymentPrepayListForWxOutputDto>(toalCount, ret);
        }



        /// <summary>
        /// 获取贷款充值详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_PaymentPrepayOutputDto> Get(EntityDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<B_PaymentPrepayOutputDto>();
            var userModel = await UserManager.GetUserByIdAsync(model.CreatorUserId.Value);
            ret.Tel = userModel.PhoneNumber;

            ret.UserName = (await UserManager.GetUserByIdAsync(model.UserId)).Name;
            ret.CredentFiles = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.贷款充值打款凭证
            });


            return ret;
        }
        /// <summary>
        /// 公众号-贷款充值
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        [AbpAuthorize]
        public async Task Create(CreateB_PaymentPrepayInput input)
        {
            var newmodel = new B_PaymentPrepay()
            {
                UserId = AbpSession.UserId.Value,
                Code = DateTime.Now.ToString("yyyyMMddHHmmSS"),
                PayType = input.PayType,
                PayAmout = input.PayAmout,
                BankName = input.BankName,
                BankUserName = input.BankUserName,
                PayAcount = input.PayAcount,
                PayDate = input.PayDate,
                Status = B_PrePayStatusEnum.待审核,
                Reason = input.Reason,
                Remark = input.Remark,
                AuditRemark = input.AuditRemark,

            };

            await _repository.InsertAsync(newmodel);


            var fileList1 = new List<AbpFileListInput>();
            foreach (var item in input.CredentFiles)
            {
                fileList1.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
            }
            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = newmodel.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.贷款充值打款凭证,
                Files = fileList1
            });


            _b_MessageAppService.Create(new CreateB_MessageInput()
            {
                BusinessId = newmodel.Id,
                BusinessType = B_H5MesagessType.款项,
                Code = newmodel.Code,
                Title = $"您发起了{newmodel.PayAcount}元的充值申请，请等待审核结果",
                Content = $"充值单号：{newmodel.Code}",
                UserId = AbpSession.UserId.Value,
            });
        }

        /// <summary>
        /// 修改一个B_PaymentPrepay
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_PaymentPrepayInput input)
        {
            //if (input.Id != Guid.Empty)
            //{
            //    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //    if (dbmodel == null)
            //    {
            //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //    }

            //    dbmodel.UserId = input.UserId;
            //    dbmodel.PayType = input.PayType;
            //    dbmodel.PayAmout = input.PayAmout;
            //    dbmodel.BankName = input.BankName;
            //    dbmodel.BankUserName = input.BankUserName;
            //    dbmodel.PayAcount = input.PayAcount;
            //    dbmodel.PayDate = input.PayDate;
            //    dbmodel.Status = input.Status;
            //    dbmodel.Reason = input.Reason;
            //    dbmodel.Remark = input.Remark;
            //    dbmodel.AuditRemark = input.AuditRemark;

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
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }


        /// <summary>
        /// 审核充值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Audit(AuditB_PrePayInput input)
        {
            var model = _repository.Get(input.Id);
            var _agencyModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == model.CreatorUserId.Value);
            var userModel = await UserManager.GetUserByIdAsync(_agencyModel.UserId);
            if (_agencyModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理数据不存在！");
            var dic = new Dictionary<string, string>();
            if (input.IsPass)
            {
                model.Status = B_PrePayStatusEnum.已通过;

                _agencyModel.GoodsPayment = _agencyModel.GoodsPayment + model.PayAmout;
                await _b_AgencyRepository.UpdateAsync(_agencyModel);

                var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_OrderAppService>();
                await service.CreateAsync(new CreateB_OrderInput()
                {
                    Amout = model.PayAmout,
                    BusinessId = model.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.充值,
                    InOrOut = OrderAmoutEnum.入账,
                    OrderNo = model.Code,
                    UserId = _agencyModel.UserId,
                    IsBlance = false,
                    IsGoodsPayment = true,

                });


                _b_MessageAppService.Create(new CreateB_MessageInput()
                {
                    BusinessId = model.Id,
                    BusinessType = B_H5MesagessType.款项,
                    Code = model.Code,
                    Content = $"充值单号：{model.Code}已经审核通过，{model.PayAcount}元已经充入货款 ",
                    UserId = model.CreatorUserId.Value,
                    Title = ""

                });

                dic.Add("keyword1", userModel.Name);
                dic.Add("keyword2", model.PayAcount);
                dic.Add("keyword3", model.PayAmout.ToString());

                _wxTemplateMessageManager.SendWeChatMsg(model.Id.ToString(), Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.充值成功,
                _agencyModel.OpenId, "充值成功", dic, "");
            }
            else
            {
                model.Status = B_PrePayStatusEnum.未通过;
            }
            model.Reason = input.Reason;
            model.AuditRemark = input.Remark;






        }
    }
}