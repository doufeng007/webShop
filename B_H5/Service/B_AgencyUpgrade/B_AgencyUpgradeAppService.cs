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
using Abp.WeChat;

namespace B_H5
{
    public class B_AgencyUpgradeAppService : FRMSCoreAppServiceBase, IB_AgencyUpgradeAppService
    {
        private readonly IRepository<B_AgencyUpgrade, Guid> _repository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly IRepository<B_AgencyLevel, Guid> _b_AgencyLevelRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WxTemplateMessageManager _wxTemplateMessageManager;
        private readonly IRepository<B_AgencyGroup, Guid> _b_AgencyGroupRepository;
        private readonly IRepository<B_AgencyGroupRelation, Guid> _b_AgencyGroupRelationRepository;
       // private readonly B_AgencyManager _b_AgencyManager;
        private readonly IRepository<B_AgencyApply, Guid> _b_AgencyApplyRepository;
        private readonly IRepository<B_InviteUrl, Guid> _b_InviteUrlRepository;

        public B_AgencyUpgradeAppService(IRepository<B_AgencyUpgrade, Guid> repository, IRepository<B_Agency, Guid> b_AgencyRepository
            , IRepository<B_AgencyLevel, Guid> b_AgencyLevelRepository, IAbpFileRelationAppService abpFileRelationAppService
            , WxTemplateMessageManager wxTemplateMessageManager, IRepository<B_AgencyGroup, Guid> b_AgencyGroupRepository
            , IRepository<B_AgencyGroupRelation, Guid> b_AgencyGroupRelationRepository
            //, B_AgencyManager b_AgencyManager
            , IRepository<B_AgencyApply, Guid> b_AgencyApplyRepository, IRepository<B_InviteUrl, Guid> b_InviteUrlRepository
        )
        {
            this._repository = repository;
            _b_AgencyRepository = b_AgencyRepository;
            _b_AgencyLevelRepository = b_AgencyLevelRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _wxTemplateMessageManager = wxTemplateMessageManager;
            _b_AgencyGroupRepository = b_AgencyGroupRepository;
            _b_AgencyGroupRelationRepository = b_AgencyGroupRelationRepository;
          //  _b_AgencyManager = b_AgencyManager;
            _b_AgencyApplyRepository = b_AgencyApplyRepository;
            _b_InviteUrlRepository = b_InviteUrlRepository;

        }

        /// <summary>
        /// 获取代理升级审核列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyUpgradeListOutputDto>> GetList(GetB_AgencyApplyListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_AgencyRepository.GetAll() on a.AgencyId equals b.Id
                        join u in UserManager.Users on b.UserId equals u.Id
                        select new B_AgencyUpgradeListOutputDto()
                        {
                            Id = a.Id,
                            AgencyName = u.Name,
                            OldAgencyLevelId = b.AgencyLevelId,
                            PayAmout = a.PayAmout,
                            PayDate = a.PayDate,
                            PayType = a.PayType,
                            PNumber = b.PNumber,
                            Tel = u.PhoneNumber,
                            WxId = b.WxId,
                            ToAgencyLevelId = a.ToAgencyLevelId,
                            NeedPrePayAmout = a.NeedPrePayAmout,
                            NeedDeposit = a.NeedDeposit,
                            Status = a.Status,
                            CreationTime = a.CreationTime
                        };

            if (input.PayType.HasValue)
                query = query.Where(r => r.PayType == input.PayType.Value);
            if (input.AgencyLevelId.HasValue)
                query = query.Where(r => r.OldAgencyLevelId == input.AgencyLevelId.Value || r.ToAgencyLevelId == input.AgencyLevelId.Value);
            if (input.Status.HasValue)
                query = query.Where(r => r.Status == input.Status.Value);
            if (input.PayDateStart.HasValue)
                query = query.Where(r => r.PayDate >= input.PayDateStart.Value);
            if (input.PayDateEnd.HasValue)
                query = query.Where(r => r.PayDate <= input.PayDateEnd.Value);

            if (!input.SearchKey.IsNullOrEmpty())
            {
                query = query.Where(r => r.AgencyName.Contains(input.SearchKey)
                || r.Tel.Contains(input.SearchKey) || r.WxId.Contains(input.SearchKey) || r.PNumber.Contains(input.SearchKey));
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            foreach (var item in ret)
            {
                item.OldAgencyLevelName = service.GetAgencyLevelFromCache(item.OldAgencyLevelId).Name;
                item.ToAgencyLevelName = service.GetAgencyLevelFromCache(item.ToAgencyLevelId).Name;
            }

            return new PagedResultDto<B_AgencyUpgradeListOutputDto>(toalCount, ret);
        }



        /// <summary>
        /// 管理-代理升级审核列表 数量统计
        /// </summary>
        /// <returns></returns>
        public async Task<B_AgencyApplyCount> GetCount()
        {
            var ret = new B_AgencyApplyCount();
            ret.WaitAuditCount = await _repository.GetAll().Where(r => r.Status == B_AgencyApplyStatusEnum.待审核).CountAsync();
            ret.PassCount = await _repository.GetAll().Where(r => r.Status == B_AgencyApplyStatusEnum.已通过).CountAsync();
            ret.NoPassCount = await _repository.GetAll().Where(r => r.Status == B_AgencyApplyStatusEnum.未通过).CountAsync();
            return ret;
        }

        /// <summary>
        /// 获取代理升级详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_AgencyUpgradeOutputDto> Get(EntityDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<B_AgencyUpgradeOutputDto>();
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            ret.ToAgencyLevelName = service.GetAgencyLevelFromCache(model.ToAgencyLevelId).Name;


            ret.CredentFiles = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.升级代理打款凭证
            });


            ret.HandleCredentFiles = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.升级代理手持证件
            });

            return ret;
        }



        /// <summary>
        /// 代理升级申请
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Create(CreateB_AgencyUpgradeInput input)
        {
            var agencyModel = await _b_AgencyRepository.FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value);
            if (agencyModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");
            var oldLeavelModel = await _b_AgencyLevelRepository.GetAsync(agencyModel.AgencyLevelId);
            if (oldLeavelModel.Level == 1)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "一级代理不能升级！");
            var toLeavelModel = await _b_AgencyLevelRepository.FirstOrDefaultAsync(input.ToAgencyLevelId);
            if (toLeavelModel == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理级别不存在！");
            if (toLeavelModel.Level <= oldLeavelModel.Level)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不能降级！");
            var minPayAmout = toLeavelModel.FirstRechargeAmout + (toLeavelModel.Deposit - oldLeavelModel.Deposit);
            if (input.PayAmout < minPayAmout)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "充值金额不足！");

            var newmodel = new B_AgencyUpgrade()
            {
                AgencyId = agencyModel.Id,
                ToAgencyLevelId = input.ToAgencyLevelId,
                NeedPrePayAmout = toLeavelModel.FirstRechargeAmout,
                NeedDeposit = toLeavelModel.Deposit - oldLeavelModel.Deposit,
                Status = B_AgencyApplyStatusEnum.待审核,
                PayType = input.PayType,
                PayAmout = input.PayAmout,
                PayAcount = input.PayAcount,
                PayDate = input.PayDate,
                BankName = input.BankName,
                BankUserName = input.BankUserName,
            };

            await _repository.InsertAsync(newmodel);

            if (input.CredentFiles == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未上传打款凭证！");

            if (input.HandleCredentFiles == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未上传手持证件！");



            var fileList1 = new List<AbpFileListInput>();
            foreach (var item in input.CredentFiles)
            {
                fileList1.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
            }
            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = newmodel.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.升级代理打款凭证,
                Files = fileList1
            });



            var fileList2 = new List<AbpFileListInput>();
            foreach (var item in input.HandleCredentFiles)
            {
                fileList2.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
            }
            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = newmodel.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.升级代理手持证件,
                Files = fileList2
            });

        }

        /// <summary>
        /// 修改一个B_AgencyUpgrade
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_AgencyUpgradeInput input)
        {

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
        /// 审核代理升级
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Audit(AuditB_AgencyApplyInput input)
        {
            var model = _repository.Get(input.Id);
            var b_AgencyModel = await _b_AgencyRepository.GetAsync(model.AgencyId);
            var userModel = await UserManager.GetUserByIdAsync(b_AgencyModel.UserId);
            if (!b_AgencyModel.P_Id.HasValue)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "一级代理不能升级！");
            var p_AgencyModel = await _b_AgencyRepository.GetAsync(b_AgencyModel.P_Id.Value);
            if (input.IsPass)
            {
                model.Status = B_AgencyApplyStatusEnum.已通过;


                var toLeavelModel = await _b_AgencyLevelRepository.GetAsync(model.ToAgencyLevelId);
                if (toLeavelModel.Level == 1)
                {
                    var group = new B_AgencyGroup()
                    {
                        Id = Guid.NewGuid(),
                        LeaderAgencyId = model.AgencyId,
                    };
                    await _b_AgencyGroupRepository.InsertAsync(group);

                    var groupRelation = new B_AgencyGroupRelation()
                    {
                        Id = Guid.NewGuid(),
                        AgencyId = model.AgencyId,
                        GroupId = group.Id,
                        IsGroupLeader = true,
                    };
                    await _b_AgencyGroupRelationRepository.InsertAsync(groupRelation);

                    b_AgencyModel.P_Id = null;
                    await _b_AgencyRepository.UpdateAsync(b_AgencyModel);
                }
                else
                {
                    //var newParentId = _b_AgencyManager.GetParentAgencyId(b_AgencyModel.Id, toLeavelModel.Level - 1);

                    //b_AgencyModel.P_Id = newParentId;

                    //await _b_AgencyRepository.UpdateAsync(b_AgencyModel);




                }

                var oldLeavelModel = await _b_AgencyLevelRepository.GetAsync(b_AgencyModel.AgencyLevelId);
                var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_OrderAppService>();
                await service.CreateAsync(new CreateB_OrderInput()
                {
                    Amout = model.PayAmout - (toLeavelModel.Deposit - oldLeavelModel.Deposit),
                    BusinessId = model.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.充值,
                    InOrOut = OrderAmoutEnum.入账,
                    OrderNo = DateTime.Now.DateTimeToStamp().ToString(),
                    UserId = b_AgencyModel.UserId,
                    IsBlance = false,
                    IsGoodsPayment = true,
                });


                await service.CreateAsync(new CreateB_OrderInput()
                {
                    Amout = toLeavelModel.Deposit - oldLeavelModel.Deposit,
                    BusinessId = model.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.保证金,
                    InOrOut = OrderAmoutEnum.入账,
                    OrderNo = DateTime.Now.DateTimeToStamp().ToString(),
                    UserId = b_AgencyModel.UserId,
                    IsBlance = false,
                    IsGoodsPayment = false,
                });

                var applyModel = await _b_AgencyApplyRepository.FirstOrDefaultAsync(r => r.Id == b_AgencyModel.ApplyId);
                if (applyModel == null)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "数据异常！");
                if (applyModel.InviteUrlId != null)
                {
                    var inviteUrlModel = await _b_InviteUrlRepository.GetAsync(applyModel.InviteUrlId.Value);


                    await service.CreateAsync(new CreateB_OrderInput()
                    {
                        Amout = toLeavelModel.RecommendAmout - oldLeavelModel.RecommendAmout,
                        BusinessId = model.Id,
                        BusinessType = OrderAmoutBusinessTypeEnum.推荐奖金,
                        InOrOut = OrderAmoutEnum.入账,
                        OrderNo = DateTime.Now.DateTimeToStamp().ToString(),
                        UserId = inviteUrlModel.CreatorUserId.Value,
                        IsBlance = true,
                        IsGoodsPayment = false,
                    });
                }





            }
            else
            {
                model.Status = B_AgencyApplyStatusEnum.未通过;
            }
            model.Reason = input.Reason;
            model.Remark = input.Remark;

            var dic = new Dictionary<string, string>();
            dic.Add("keyword1", userModel.Name);
            dic.Add("keyword2", model.Status == B_AgencyApplyStatusEnum.已通过 ? "通过" : "不通过");
            _wxTemplateMessageManager.SendWeChatMsg(model.Id.ToString(), model.Status == B_AgencyApplyStatusEnum.已通过 ?
                Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.当前用户升级成功 :
                Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.当前用户升级失败,
                b_AgencyModel.OpenId, "代理升级审核通知", dic, model.Status == B_AgencyApplyStatusEnum.已通过 ? "欢迎加入" : $"审核不通过原因:{model.Reason}");


            _wxTemplateMessageManager.SendWeChatMsg(model.Id.ToString(), model.Status == B_AgencyApplyStatusEnum.已通过 ?
            Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.下级代理升级成功 :
            Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.下级代理升级失败,
            p_AgencyModel.OpenId, "下级代理升级审核通知", dic, model.Status == B_AgencyApplyStatusEnum.已通过 ? "欢迎加入" : $"审核不通过原因:{model.Reason}");


        }
    }
}