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

namespace B_H5
{
    public class B_AgencyApplyAppService : FRMSCoreAppServiceBase, IB_AgencyApplyAppService
    {
        private readonly IRepository<B_AgencyApply, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<B_InviteUrl, Guid> _b_InviteUrlRepository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;

        public B_AgencyApplyAppService(IRepository<B_AgencyApply, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<B_InviteUrl, Guid> b_InviteUrlRepository, IRepository<B_Agency, Guid> b_AgencyRepository

        )
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _b_InviteUrlRepository = b_InviteUrlRepository;
            _b_AgencyRepository = b_AgencyRepository;

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
            ret.WaitAuditCount = await _repository.GetAll().Where(r => r.Status == B_AgencyApplyStatusEnum.待审核).CountAsync();
            ret.PassCount = await _repository.GetAll().Where(r => r.Status == B_AgencyApplyStatusEnum.已通过).CountAsync();
            ret.NoPassCount = await _repository.GetAll().Where(r => r.Status == B_AgencyApplyStatusEnum.待审核).CountAsync();
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
                        join b in _b_InviteUrlRepository.GetAll() on a.InviteUrlId equals b.Id
                        join u in UserManager.Users on b.CreatorUserId.Value equals u.Id
                        join b_a in _b_AgencyRepository.GetAll() on u.Id equals b_a.UserId
                        where a.Id == input.Id
                        select new B_AgencyApplyOutputDto
                        {
                            Id = a.Id,
                            Address = a.Address,
                            AgencyLevelCode = "",

                            AgencyLevelName = "",
                            BankName = a.BankName,
                            BankUserName = a.BankUserName,
                            City = a.City,
                            Country = a.Country,
                            County = a.County,
                            CreationTime = a.CreationTime,
                            InvitUserAddress = b_a.Address,
                            InvitUserName = u.Name,
                            InvitUserTel = u.PhoneNumber,
                            Name = a.Name,
                            PayAcount = a.PayAcount,
                            PayAmout = a.PayAmout,
                            PayDate = a.PayDate,
                            PayType = a.PayType,
                            PNumber = a.PNumber,
                            Provinces = a.Provinces,
                            Status = a.Status,
                            Tel = a.Tel,
                            WxId = a.WxId


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
        /// 添加一个B_AgencyApply
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_AgencyApplyInput input)
        {
            var newmodel = new B_AgencyApply()
            {
                Id = Guid.NewGuid(),
                AgencyLevelId = input.AgencyLevelId,
                AgencyLevel = input.AgencyLevel,
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

        }

        /// <summary>
        /// 审核代理申请
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Audit(AuditB_AgencyApplyInput input)
        {
            var model = _repository.Get(input.Id);
            if (input.IsPass)
            {
                model.Status = B_AgencyApplyStatusEnum.已通过;

            }
            else
            {
                model.Status = B_AgencyApplyStatusEnum.未通过;
            }
            model.Reason = input.Reason;
            model.Remark = input.Remark;

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