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
using Abp;
using ZCYX.FRMSCore.Users;
using Microsoft.Extensions.Configuration;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using Abp.WeChat;
using Abp.Runtime.Caching;

namespace B_H5
{
    public class B_AgencyApplyAppService : FRMSCoreAppServiceBase, IB_AgencyApplyAppService
    {
        private readonly IRepository<B_AgencyApply, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<B_InviteUrl, Guid> _b_InviteUrlRepository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly IB_AgencyLevelAppService _b_AgencyLevelService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly WxTemplateMessageManager _wxTemplateMessageManager;
        private readonly IRepository<B_AgencyGroup, Guid> _b_AgencyGroupRepository;
        private readonly IRepository<B_AgencyGroupRelation, Guid> _b_AgencyGroupRelationRepository;
        private readonly string SmsCacheKey = "SmsCache";
        private readonly ICacheManager _cacheManager;

        public B_AgencyApplyAppService(IRepository<B_AgencyApply, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<B_InviteUrl, Guid> b_InviteUrlRepository, IRepository<B_Agency, Guid> b_AgencyRepository, IB_AgencyLevelAppService b_AgencyLevelService
            , WxTemplateMessageManager wxTemplateMessageManager, IRepository<B_AgencyGroup, Guid> b_AgencyGroupRepository
            , IRepository<B_AgencyGroupRelation, Guid> b_AgencyGroupRelationRepository, ICacheManager cacheManager

        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _b_InviteUrlRepository = b_InviteUrlRepository;
            _b_AgencyRepository = b_AgencyRepository;
            _b_AgencyLevelService = b_AgencyLevelService;
            var coreAssemblyDirectoryPath = typeof(B_AgencyAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            _wxTemplateMessageManager = wxTemplateMessageManager;
            _b_AgencyGroupRepository = b_AgencyGroupRepository;
            _b_AgencyGroupRelationRepository = b_AgencyGroupRelationRepository;
            _cacheManager = cacheManager;

        }

        /// <summary>
        /// 管理-代理审核列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_AgencyApplyListOutputDto>> GetList(GetB_AgencyApplyListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_InviteUrlRepository.GetAll() on a.InviteUrlId equals b.Id
                        join u in UserManager.Users on b.CreatorUserId.Value equals u.Id
                        select new B_AgencyApplyListOutputDto()
                        {
                            Id = a.Id,
                            //AgencyLevelName
                            AgencyLevelId = a.AgencyLevelId,
                            InvitUserName = u.Name,
                            Name = a.Name,
                            InvitUserTel = u.PhoneNumber,
                            Tel = a.Tel,
                            WxId = a.WxId,
                            PNumber = a.PNumber,
                            PayType = a.PayType,
                            PayAmout = a.PayAmout,
                            PayDate = a.PayDate,
                            Status = a.Status,
                            CreationTime = a.CreationTime

                        };
            if (input.PayType.HasValue)
                query = query.Where(r => r.PayType == input.PayType.Value);
            if (input.AgencyLevelId.HasValue)
                query = query.Where(r => r.AgencyLevelId == input.AgencyLevelId.Value);
            if (input.Status.HasValue)
                query = query.Where(r => r.Status == input.Status.Value);
            if (input.PayDateStart.HasValue)
                query = query.Where(r => r.PayDate >= input.PayDateStart.Value);
            if (input.PayDateEnd.HasValue)
                query = query.Where(r => r.PayDate <= input.PayDateEnd.Value);

            if (!input.SearchKey.IsNullOrEmpty())
            {
                query = query.Where(r => r.InvitUserName.Contains(input.SearchKey) || r.InvitUserTel.Contains(input.SearchKey) || r.Name.Contains(input.SearchKey)
                || r.Tel.Contains(input.SearchKey) || r.WxId.Contains(input.SearchKey) || r.PNumber.Contains(input.SearchKey));
            }

            var toalCount = await query.CountAsync();

            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            foreach (var item in ret)
            {
                item.AgencyLevelName = service.GetAgencyLevelFromCache(item.AgencyLevelId).Name;
                item.StatusTitle = item.Status.ToString();
            }

            return new PagedResultDto<B_AgencyApplyListOutputDto>(toalCount, ret);
        }







        /// <summary>
        /// 管理-代理审核列表 数量统计
        /// </summary>
        /// <returns></returns>
        public async Task<B_AgencyApplyCount> GetCount()
        {
            var ret = new B_AgencyApplyCount();

            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_InviteUrlRepository.GetAll() on a.InviteUrlId equals b.Id
                        join u in UserManager.Users on b.CreatorUserId.Value equals u.Id
                        select a;

            ret.WaitAuditCount = await query.Where(r => r.Status == B_AgencyApplyStatusEnum.待审核).CountAsync();
            ret.PassCount = await query.Where(r => r.Status == B_AgencyApplyStatusEnum.已通过).CountAsync();
            ret.NoPassCount = await query.Where(r => r.Status == B_AgencyApplyStatusEnum.未通过).CountAsync();
            return ret;
        }




        /// <summary>
        /// 管理-代理审核 查看详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<B_AgencyApplyOutputDto> Get(EntityDto<Guid> input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_InviteUrlRepository.GetAll() on a.InviteUrlId equals b.Id into g
                        from b in g.DefaultIfEmpty()
                        join u in UserManager.Users on b.CreatorUserId.Value equals u.Id into m
                        from u in m.DefaultIfEmpty()
                        join b_a in _b_AgencyRepository.GetAll() on u.Id equals b_a.UserId into n
                        from invitaAgency in n.DefaultIfEmpty()
                        where a.Id == input.Id
                        select new B_AgencyApplyOutputDto
                        {
                            Id = a.Id,
                            Address = a.Address,
                            AgencyCode = "",

                            AgencyLevelName = "",
                            BankName = a.BankName,
                            BankUserName = a.BankUserName,
                            City = a.City,
                            Country = a.Country,
                            County = a.County,
                            CreationTime = a.CreationTime,
                            InvitUserAddress = invitaAgency == null ? "" : invitaAgency.Address,
                            InvitUserName = u == null ? "" : u.Name,
                            InvitUserTel = u == null ? "" : u.PhoneNumber,
                            Name = a.Name,
                            PayAcount = a.PayAcount,
                            PayAmout = a.PayAmout,
                            PayDate = a.PayDate,
                            PayType = a.PayType,
                            PNumber = a.PNumber,
                            Provinces = a.Provinces,
                            Status = a.Status,
                            Tel = a.Tel,
                            WxId = a.WxId,
                            AgencyLevelId = a.AgencyLevelId,



                        };
            var model = await query.FirstOrDefaultAsync();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_AgencyLevelAppService>();
            model.AgencyLevelName = service.GetAgencyLevelFromCache(model.AgencyLevelId).Name;


            model.CredentFiles = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.申请代理打款凭证
            });


            model.HandleCredentFiles = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.申请代理手持证件
            });

            var fileRet = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.代理头像
            });

            model.TouxiangFile = fileRet.FirstOrDefault();

            return model;
        }



        /// <summary>
        /// 新增一个代理申请
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task<Guid> Create(CreateB_AgencyApplyInput input)
        {
            B_InviteUrl inviteUrlModel;
            B_AgencyLevel applyLeavelModel;
            if (_repository.GetAll().Where(r => r.Status != B_AgencyApplyStatusEnum.未通过).Any(r => r.Name == input.Name))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理名称重复！");

            var leavelRepository = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<B_AgencyLevel, Guid>>();
            if (input.InviteUrlId.HasValue)
            {
                inviteUrlModel = await _b_InviteUrlRepository.GetAsync(input.InviteUrlId.Value);
                applyLeavelModel = await leavelRepository.GetAsync(inviteUrlModel.AgencyLevel);
            }
            else
            {
                applyLeavelModel = await leavelRepository.FirstOrDefaultAsync(r => r.Level == 1);
                if (applyLeavelModel == null)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理级别不存在！");
            }


            if (input.PayAmout < (applyLeavelModel.FirstRechargeAmout + applyLeavelModel.Deposit))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "充值金额不足！");


            var newmodel = new B_AgencyApply()
            {
                Id = Guid.NewGuid(),
                AgencyLevelId = applyLeavelModel.Id,
                AgencyLevel = applyLeavelModel.Level,
                Tel = input.Tel,
                VCode = input.VCode,
                Pwd = input.Pwd,
                WxId = input.WxId,
                Country = input.Country,
                PNumber = input.PNumber,
                Provinces = input.Provinces,
                City = input.City,
                County = input.County,
                Address = input.Address,
                PayType = input.PayType,
                PayAmout = input.PayAmout,
                PayAcount = input.PayAcount,
                PayDate = input.PayDate,
                Status = B_AgencyApplyStatusEnum.待审核,
                InviteUrlId = input.InviteUrlId,
                BankName = input.BankName,
                BankUserName = input.BankUserName,
                Name = input.Name,
                AgencyApplyType = AgencyApplyEnum.代理邀请,
                OpenId = input.OpenId
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
                BusinessType = (int)AbpFileBusinessType.申请代理打款凭证,
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
                BusinessType = (int)AbpFileBusinessType.申请代理手持证件,
                Files = fileList2
            });


            if (input.TouxiangFile != null)
            {
                var fileList3 = new List<AbpFileListInput>();

                fileList3.Add(new AbpFileListInput() { Id = input.TouxiangFile.Id, Sort = input.TouxiangFile.Sort });

                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = newmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.代理头像,
                    Files = fileList3
                });
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未上传代理头像！");
            }


            return newmodel.Id;

        }




        /// <summary>
        /// 审核代理申请
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Audit(AuditB_AgencyApplyInput input)
        {
            var model = _repository.Get(input.Id);
            B_Agency invite_AgencyModel = null;
            if (model.InviteUrlId.HasValue)
            {
                var invitaUrlModel = _b_InviteUrlRepository.Get(model.InviteUrlId.Value);
                invite_AgencyModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == invitaUrlModel.CreatorUserId.Value);
            }
            if (input.IsPass)
            {
                model.Status = B_AgencyApplyStatusEnum.已通过;







                var agencyLeavelOne = _b_AgencyLevelService.GetAgencyLevelOneFromCache();

                var userService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IUserAppService>();
                var userCreateInput = new ZCYX.FRMSCore.Users.Dto.CreateUserDto()
                {
                    MainPostId = model.AgencyLevelId == agencyLeavelOne.Id ? new Guid(_appConfiguration["App:PrimaryAgencyOrgPostId"]) : new Guid(_appConfiguration["App:AgencyOrgPostId"]),
                    Name = model.Name,
                    OrganizationUnitId = model.AgencyLevelId == agencyLeavelOne.Id ? _appConfiguration["App:PrimaryAgencyOrgId"].ToLong() : _appConfiguration["App:AgencyOrgId"].ToLong(),
                    OrgPostIds = new List<Guid>()
                        {
                            model.AgencyLevelId == agencyLeavelOne.Id ? new Guid(_appConfiguration["App:PrimaryAgencyOrgPostId"])
                            : new Guid(_appConfiguration["App:AgencyOrgPostId"]), },
                    Password = model.Pwd,
                    PhoneNumber = model.Tel,
                    UserName = model.Tel,
                    Surname = model.Name,
                    Sex = null,
                    IsActive = true,
                    EmailAddress = $"{model.Tel}@abp.com",
                };

                var ret = await userService.Create(userCreateInput);
                var newmodel = new B_Agency()
                {
                    UserId = ret.Id,
                    AgencyLevel = model.AgencyLevel,
                    AgencyLevelId = model.AgencyLevelId,
                    AgenCyCode = DateTime.Now.DateTimeToStamp().ToString(),
                    Provinces = model.Provinces,
                    County = model.County,
                    City = model.City,
                    Address = model.Address,
                    Type = B_AgencyTypeEnum.直属代理,
                    SignData = model.CreationTime,
                    //Agreement = input.Agreement,
                    WxId = model.WxId,
                    ApplyId = model.Id,
                    OpenId = model.OpenId,
                    PNumber = model.PNumber,
                };
                if (invite_AgencyModel != null)
                {
                    newmodel.P_Id = invite_AgencyModel.Id;
                    newmodel.OriginalPid = invite_AgencyModel.Id;
                }
                var leavelRepository = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<B_AgencyLevel, Guid>>();
                var applyLeavelModel = await leavelRepository.GetAsync(model.AgencyLevelId);

                newmodel.GoodsPayment = model.PayAmout - applyLeavelModel.Deposit;

                await _b_AgencyRepository.InsertAsync(newmodel);


                var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IB_OrderAppService>();
                await service.CreateAsync(new CreateB_OrderInput()
                {
                    Amout = model.PayAmout - applyLeavelModel.Deposit,
                    BusinessId = model.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.充值,
                    InOrOut = OrderAmoutEnum.入账,
                    OrderNo = DateTime.Now.DateTimeToStamp().ToString(),
                    UserId = newmodel.UserId,
                    IsBlance = false,
                    IsGoodsPayment = true,
                });


                await service.CreateAsync(new CreateB_OrderInput()
                {
                    Amout = model.PayAmout - applyLeavelModel.Deposit,
                    BusinessId = model.Id,
                    BusinessType = OrderAmoutBusinessTypeEnum.保证金,
                    InOrOut = OrderAmoutEnum.入账,
                    OrderNo = DateTime.Now.DateTimeToStamp().ToString(),
                    UserId = newmodel.UserId,
                    IsBlance = false,
                    IsGoodsPayment = false,
                });



                if (invite_AgencyModel == null)
                {

                }




                if (model.AgencyLevelId == agencyLeavelOne.Id)   //邀请一级代理
                {
                    var group = new B_AgencyGroup()
                    {
                        Id = Guid.NewGuid(),
                        LeaderAgencyId = newmodel.Id,
                    };
                    await _b_AgencyGroupRepository.InsertAsync(group);

                    var groupOneRelation = new B_AgencyGroupRelation()
                    {
                        Id = Guid.NewGuid(),
                        AgencyId = newmodel.Id,
                        GroupId = group.Id,
                        IsGroupLeader = true,

                    };
                    await _b_AgencyGroupRelationRepository.InsertAsync(groupOneRelation);
                }
                var groupId = Guid.Empty;
                if (invite_AgencyModel != null)
                {
                    var groupRs = _b_AgencyGroupRelationRepository.GetAll().Where(r => r.AgencyId == invite_AgencyModel.Id);
                    if (groupRs.Count() == 0)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理团队不存在");
                    if (groupRs.Any(r => r.IsGroupLeader))
                        groupId = groupRs.FirstOrDefault(r => r.IsGroupLeader).GroupId;
                    else
                        groupId = groupRs.FirstOrDefault().GroupId;
                    var groupRelation = new B_AgencyGroupRelation()
                    {
                        Id = Guid.NewGuid(),
                        AgencyId = newmodel.Id,
                        GroupId = groupId,
                        IsGroupLeader = false,
                    };
                    await _b_AgencyGroupRelationRepository.InsertAsync(groupRelation);


                    await service.CreateAsync(new CreateB_OrderInput()
                    {
                        Amout = applyLeavelModel.RecommendAmout,
                        BusinessId = model.Id,
                        BusinessType = OrderAmoutBusinessTypeEnum.推荐奖金,
                        InOrOut = OrderAmoutEnum.入账,
                        OrderNo = DateTime.Now.DateTimeToStamp().ToString(),
                        UserId = invite_AgencyModel.UserId,
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
            dic.Add("keyword1", model.Name);
            dic.Add("keyword2", model.Status == B_AgencyApplyStatusEnum.已通过 ? "通过" : "不通过");
            _wxTemplateMessageManager.SendWeChatMsg(model.Id.ToString(), model.Status == B_AgencyApplyStatusEnum.已通过 ?
                Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.当前用户申请代理审核通过 :
                Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.当前用户申请代理审核不通过,
                model.OpenId, "代理审核通知", dic, model.Status == B_AgencyApplyStatusEnum.已通过 ? "欢迎加入" : $"审核不通过原因:{model.Reason}");

            if (invite_AgencyModel != null)
            {
                _wxTemplateMessageManager.SendWeChatMsg(model.Id.ToString(), model.Status == B_AgencyApplyStatusEnum.已通过 ?
                Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.邀请下级代理审核通过 :
                Abp.WeChat.Enum.TemplateMessageBusinessTypeEnum.邀请下级代理审核不通过,
                invite_AgencyModel.OpenId, "下级代理审核通知", dic, model.Status == B_AgencyApplyStatusEnum.已通过 ? "欢迎加入" : $"审核不通过原因:{model.Reason}");
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


        public async Task SendSms(string phone)
        {
            var cache = _cacheManager.GetCache(SmsCacheKey);
            var cacheValue = cache.GetOrDefault(phone).ToString();
            if (cacheValue.IsNullOrEmpty())
            {
                var smsManager = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<AliSms.AliSmsManager>();
                smsManager.SendSms("SMS_164860191", "乌生青",phone, "{\"code\":\"7777\"}");
            }
            else
            {

            }



        }
    }
}