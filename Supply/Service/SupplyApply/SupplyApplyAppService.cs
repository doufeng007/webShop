using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.File;
using Abp.UI;
using ZCYX.FRMSCore.Application;
using Abp.Extensions;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Supply.Service.SupplyApply
{
    [AbpAuthorize]
    public class SupplyApplyAppService : FRMSCoreAppServiceBase, ISupplyApplyAppService
    {
        private readonly IRepository<SupplyApplyMain, Guid> _supplyApplyMainRepository;
        private readonly IRepository<SupplyApplySub, Guid> _supplyApplySubRepository;
        private readonly IRepository<SupplyApplySubBak, Guid> _supplyApplySubBakRepository;
        private readonly IRepository<SupplyApplyResult, Guid> _supplyApplyResultRepository;
        private readonly ISupplyBaseRepository _supplyBaseRepository;
        private readonly IRepository<SupplyPurchaseMain, Guid> _supplyPurchaseMainRepository;
        private readonly IRepository<SupplyPurchasePlan, Guid> _supplyPurchasePlaneRepository;
        private readonly IRepository<SupplyPurchaseResult, Guid> _supplyPurchaseResultRepository;

        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<UserSupply, Guid> _userSupplyRepository;

        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        private readonly IRepository<PostInfo, Guid> _postInfoRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;

        public SupplyApplyAppService(IRepository<SupplyApplyMain, Guid> supplyApplyMainRepository, IAbpFileRelationAppService abpFileRelationAppService,
            IRepository<User, long> userRepository, IRepository<SupplyApplySub, Guid> supplyApplySubRepository, IRepository<SupplyApplySubBak, Guid> supplyApplySubBakRepository, ISupplyBaseRepository supplyBaseRepository
            , IRepository<SupplyPurchaseMain, Guid> supplyPurchaseMainRepository, IRepository<SupplyPurchasePlan, Guid> supplyPurchasePlaneRepository
            , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , IRepository<SupplyApplyResult, Guid> supplyApplyResultRepository, IRepository<UserSupply, Guid> userSupplyRepository
            , IRepository<SupplyPurchaseResult, Guid> supplyPurchaseResultRepository
            , IRepository<PostInfo, Guid> postInfoRepository, IRepository<UserPosts, Guid> userPostRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository
            , IWorkFlowTaskRepository workFlowTaskRepository, IWorkFlowWorkTaskAppService workFlowWorkTaskAppService
            , WorkFlowCacheManager workFlowCacheManager, ProjectAuditManager projectAuditManager)
        {
            _supplyApplyMainRepository = supplyApplyMainRepository;
            _supplyApplySubRepository = supplyApplySubRepository;
            _supplyBaseRepository = supplyBaseRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _userRepository = userRepository;
            _supplyPurchaseMainRepository = supplyPurchaseMainRepository;
            _supplyPurchasePlaneRepository = supplyPurchasePlaneRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _supplyApplyResultRepository = supplyApplyResultRepository;
            _userSupplyRepository = userSupplyRepository;
            _supplyPurchaseResultRepository = supplyPurchaseResultRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _postInfoRepository = postInfoRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userPostRepository = userPostRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _workFlowCacheManager = workFlowCacheManager;
            _projectAuditManager = projectAuditManager;
            _supplyApplySubBakRepository = supplyApplySubBakRepository;
        }


        /// <summary>
        /// 个人领取用品  对一个申领下的所有申领明细的领取
        /// </summary>
        /// <param name="input">对申领明细的领取</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task Apply(Guid input)
        {
            var model = await _supplyApplySubRepository.GetAsync(input);
            if (model.Status == (int)SupplyApplySubStatusType.已发放 || model.Status == (int)SupplyApplySubStatusType.采购入库)
            {
                model.Status = (int)SupplyApplySubStatusType.已领取;
                var applyResult = await _supplyApplyResultRepository.GetAll().Where(r => r.ApplySubId == model.Id && r.Status == (int)SupplyApplyResultStatus.已发放).ToListAsync();
                foreach (var item in applyResult)
                {
                    item.Status = (int)SupplyApplyResultStatus.已领取;
                    var s = _supplyBaseRepository.Get(item.SupplyId);
                    s.Status = (int)SupplyStatus.被领用;
                    s.UserId = model.UserId;
                    s.ProductDate = DateTime.Now;
                    _supplyBaseRepository.Update(s);
                    var userSupply = new UserSupply()
                    {
                        Id = Guid.NewGuid(),
                        SupplyId = item.SupplyId,
                        UserId = AbpSession.UserId.Value,
                        CreationTime = DateTime.Now,
                        StartTime = DateTime.Now,
                        EndTime = s.ExpiryDate
                    };
                    await _userSupplyRepository.InsertAsync(userSupply);

                }
                _supplyApplySubRepository.Update(model);
            }
            else
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该物品申领暂不能领用");
        }


        /// <summary>
        /// 个人领取用品  对一个申领下一个申领明显的领取
        /// </summary>
        /// <param name="subApplyId"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task ApplyOne(Guid subApplyId, Guid resultId)
        {
            var model = await _supplyApplySubRepository.GetAsync(subApplyId);
            if (model.Status != (int)SupplyApplySubStatusType.已发放 && model.Status != (int)SupplyApplySubStatusType.采购入库) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该物品申领暂不能领用");
            var applyResult = await _supplyApplyResultRepository.GetAsync(resultId);
            applyResult.Status = (int)SupplyApplyResultStatus.已领取;
            var s = _supplyBaseRepository.Get(applyResult.SupplyId);
            s.Status = (int)SupplyStatus.被领用;
            s.UserId = model.UserId;
            s.ProductDate = DateTime.Now;
            _supplyBaseRepository.Update(s);
            _supplyApplySubRepository.Update(model);
            var userSupply = new UserSupply()
            {
                Id = Guid.NewGuid(),
                SupplyId = applyResult.SupplyId,
                UserId = AbpSession.UserId.Value,
                CreationTime = DateTime.Now,
                StartTime = DateTime.Now,
            };
            await _userSupplyRepository.InsertAsync(userSupply);
            var needTodoRecevieCount = await _supplyApplyResultRepository.CountAsync(r => r.ApplySubId == subApplyId && r.Status == (int)SupplyApplyResultStatus.已发放);
            if (needTodoRecevieCount == 1)
            {
                model.Status = (int)SupplyApplySubStatusType.已领取;
            }

        }



        /// <summary>
        /// 申领个人用品
        /// </summary>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(CreateApplyDto input)
        {
            var main = new SupplyApplyMain();
            main.Id = Guid.NewGuid();
            main.IsImportant = input.IsImportant;
            if (input.SupplyApplySub != null && input.SupplyApplySub.Count > 0)
            {
                foreach (var item in input.SupplyApplySub)
                {
                    var sub = item.MapTo<SupplyApplySub>();
                    sub.MainId = main.Id;
                    sub.Id = Guid.NewGuid();
                    if (item.FileList != null)
                    {
                        var fileList = new List<AbpFileListInput>();
                        foreach (var ite in item.FileList)
                        {
                            fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                        }
                        _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                        {
                            BusinessId = sub.Id.ToString(),
                            BusinessType = (int)AbpFileBusinessType.申领用品附件,
                            Files = fileList
                        });
                    }
                    _supplyApplySubRepository.Insert(sub);
                    var subBak = item.MapTo<SupplyApplySubBak>();
                    subBak.MainId = main.Id;
                    subBak.Id = Guid.NewGuid();
                    if (item.FileList != null)
                    {
                        var fileList = new List<AbpFileListInput>();
                        foreach (var ite in item.FileList)
                        {
                            fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                        }
                        _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                        {
                            BusinessId = subBak.Id.ToString(),
                            BusinessType = (int)AbpFileBusinessType.申领用品附件,
                            Files = fileList
                        });
                    }
                    _supplyApplySubBakRepository.Insert(subBak);
                }
            }
            _supplyApplyMainRepository.Insert(main);
            return new InitWorkFlowOutput() { InStanceId = main.Id.ToString() };
        }


        /// <summary>
        /// 行政获取申领列表
        /// </summary>
        public async Task<PagedResultDto<SupplyApplyListDto>> GetAll(GetSupplyApplyListInput input)
        {
            var query = from a in _supplyApplyMainRepository.GetAll()
                        join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                          x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                          x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new SupplyApplyListDto
                        {
                            Id = a.Id,
                            IsImportant = a.IsImportant,
                            CreateUserName = b.Name,
                            CreationTime = a.CreationTime,
                            CreatorUserId = a.CreatorUserId.Value,
                            Status = a.Status,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                        ? 1
                                        : 2
                        };
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var model = await query.OrderBy(ite => ite.OpenModel).ThenByDescending(ite => ite.IsImportant).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();

            foreach (var r in model)
            {
                r.InstanceId = r.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, r as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<SupplyApplyListDto>(count, model);
        }


        /// <summary>
        /// 获取个人申领列表
        /// </summary>
        [AbpAuthorize]
        public async Task<PagedResultDto<SupplyApplyListDto>> GetMyAll(GetSupplyApplyListInput input)
        {

            var query = from a in _supplyApplyMainRepository.GetAll()
                        join b in _supplyApplySubRepository.GetAll() on a.Id equals b.MainId into subs
                        join u in _userRepository.GetAll() on a.CreatorUserId equals u.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                          x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                          x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        where a.CreatorUserId == AbpSession.UserId.Value
                        select new
                        {
                            a,
                            subs,
                            u,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                        ? 1
                                        : 2
                        };
            //var query = from a in _supplyApplyMainRepository.GetAll()
            //            join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
            //            let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
            //              x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
            //              x.ReceiveID == AbpSession.UserId.Value)
            //                             select c)
            //            join sub in _supplyApplySubRepository.GetAll() on a.Id equals sub.MainId into subs
            //            where a.CreatorUserId == AbpSession.UserId.Value
            //            //SupplyApplyListDto
            //            select new
            //            {
            //                Id = a.Id,
            //                IsImportant = a.IsImportant,
            //                CreateUserName = b.Name,
            //                CreationTime = a.CreationTime,
            //                CreatorUserId = a.CreatorUserId.Value,
            //                Status = a.Status,
            //                Subs = subs,
            //                OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
            //                            ? 1
            //                            : 2
            //            };
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.a.Status.ToString()));
            var count = await query.CountAsync();
            var model = await query.OrderBy(ite => ite.OpenModel).ThenByDescending(ite => ite.a.IsImportant).ThenByDescending(ite => ite.a.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<SupplyApplyListDto>();
            foreach (var r in model)
            {
                var item = new SupplyApplyListDto()
                {
                    Id = r.a.Id,
                    IsImportant = r.a.IsImportant,
                    CreateUserName = r.u.Name,
                    CreationTime = r.a.CreationTime,
                    CreatorUserId = r.a.CreatorUserId == null ? 0 : r.a.CreatorUserId.Value,
                    Status = r.a.Status,
                    OpenModel = r.OpenModel
                };
                item.InstanceId = r.a.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
                foreach (var subiten in r.subs)
                {
                    var subItem = new SupplyApplySubBaseDto()
                    {
                        ApplyDateTime = subiten.CreationTime,
                        CreationTime = subiten.CreationTime,
                        Des = subiten.Des,
                        GetTime = subiten.GetTime,
                        Id = subiten.Id,
                        MainId = item.Id,
                        Money = subiten.Money,
                        Name = subiten.Name,
                        Number = subiten.Number,
                        Result = subiten.Result,
                        Status = subiten.Status,
                        Type = subiten.Type,
                        Unit = subiten.Unit,
                        UserId = subiten.UserId,
                        Version = subiten.Version,
                    };
                    //subItem.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = subiten.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申领用品附件 });
                    subItem.Result_Name = ((SupplyApplySubResultType)subiten.Result).ToString();
                    subItem.StatusTitle = ((SupplyApplySubStatusType)subiten.Status).ToString();
                    subItem.TypeName = ((SupplyType)subiten.Type).ToString();
                    subItem.UserId_Name = _workFlowOrganizationUnitsManager.GetNames(subiten.UserId);
                    item.Subs.Add(subItem);
                }
                ret.Add(item);
            }
            return new PagedResultDto<SupplyApplyListDto>(count, ret);

        }



        [AbpAuthorize]
        public async Task<PagedResultDto<SupplyApplyListDto>> GetMyAllV2(GetSupplyApplyListInput input)
        {

            var query = from a in _supplyApplyMainRepository.GetAll()
                        join u in _userRepository.GetAll() on a.CreatorUserId equals u.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                          x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                          x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        where a.CreatorUserId == AbpSession.UserId.Value
                        select new
                        {
                            a,
                            u,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                        ? 1
                                        : 2
                        };
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.a.Status.ToString()));
            var count = await query.CountAsync();
            var model = await query.OrderBy(ite => ite.OpenModel).ThenByDescending(ite => ite.a.IsImportant).ThenByDescending(ite => ite.a.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<SupplyApplyListDto>();
            foreach (var r in model)
            {
                var item = new SupplyApplyListDto()
                {
                    Id = r.a.Id,
                    IsImportant = r.a.IsImportant,
                    CreateUserName = r.u.Name,
                    CreationTime = r.a.CreationTime,
                    CreatorUserId = r.a.CreatorUserId == null ? 0 : r.a.CreatorUserId.Value,
                    Status = r.a.Status,
                    OpenModel = r.OpenModel
                };
                item.InstanceId = r.a.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
                ret.Add(item);
            }
            return new PagedResultDto<SupplyApplyListDto>(count, ret);

        }



        public async Task<PagedResultDto<SupplyApplySubBaseDto>> GetSupplySubsByMainId(GetSupplyApplySubListInput input)
        {
            var query = from a in _supplyApplySubRepository.GetAll()
                        where a.MainId == input.MainId && a.CreatorUserId == AbpSession.UserId.Value
                        select new SupplyApplySubBaseDto()
                        {
                            CreationTime = a.CreationTime,
                            Des = a.Des,
                            DoPurchaseStatus = 0,
                            GetTime = a.GetTime,
                            Id = a.Id,
                            MainId = a.MainId,
                            Money = a.Money,
                            Name = a.Name,
                            Number = a.Number,
                            Result = a.Result,
                            ResultRemark = a.ResultRemark,
                            Result_Name = "",
                            Status = a.Status,
                            SupplyId = a.SupplyId,
                            Type = a.Type,
                            Unit = a.Unit,
                            UserId = a.UserId,
                            Version = a.Version,
                        };
            var count = await query.CountAsync();
            var model = await query.OrderBy(ite => ite.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in model)
            {
                item.ApplyDateTime = item.CreationTime;
                item.Result_Name = ((SupplyApplySubResultType)item.Result).ToString();
                item.StatusTitle = ((SupplyApplySubStatusType)item.Status).ToString();
                item.TypeName = ((SupplyType)item.Type).ToString();
                item.UserId_Name = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
            }
            return new PagedResultDto<SupplyApplySubBaseDto>(count, model);
        }

        /// <summary>
        /// 获取待领取的申领用品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<ApplyResultDto>> GetMy(PagedAndSortedInputDto input)
        {
            var query = from a in _supplyApplySubRepository.GetAll()
                        join b in _supplyApplyResultRepository.GetAll() on a.Id equals b.ApplySubId
                        join c in _supplyBaseRepository.GetAll() on b.SupplyId equals c.Id
                        where a.CreatorUserId == AbpSession.UserId.Value && (a.Status == (int)SupplyApplySubStatusType.已发放 || a.Status == (int)SupplyApplySubStatusType.采购入库) && b.Status == (int)SupplyApplyResultStatus.已发放
                        select new ApplyResultDto()
                        {
                            ApplyResultId = b.Id,
                            ApplySubId = a.Id,
                            Des = a.Des,
                            GetTime = a.GetTime,
                            Money = c.Money,
                            Name = c.Name,
                            SupplyId = c.Id,
                            Number = a.Number,
                            Type = c.Type,
                            Unit = a.Unit,
                            UserId = a.UserId,
                            Version = c.Version,
                            Code = c.Code,
                            CreationTime = a.CreationTime,

                        };
            var count = await query.CountAsync();
            var model = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            foreach (var m in model)
            {
                m.StatusTitle = SupplyApplySubResultType.已发放.ToString();
                m.UserId_Name = _workFlowOrganizationUnitsManager.GetNames(m.UserId);
                m.TypeName = ((SupplyType)m.Type).ToString();
                m.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = m.SupplyId.ToString(), BusinessType = (int)AbpFileBusinessType.用品附件 });
            }
            return new PagedResultDto<ApplyResultDto>(count, model);
        }
        /// <summary>
        /// 行政人员获取可以发放的用品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplySelectDto>> GetCanApplySupply(PagedAndSortedInputDto input)
        {
            var query = _supplyBaseRepository.GetAll().Where(r => r.Status == (int)SupplyStatus.在库);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey));
            }
            var count = await query.CountAsync();
            var model = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var datas = model.MapTo<List<SupplySelectDto>>();
            foreach (var item in datas)
            {
                item.TypeName = ((SupplyType)item.Type).ToString();
            }
            return new PagedResultDto<SupplySelectDto>(count, datas);
        }
        /// <summary>
        /// 行政人员对用品申领的处理
        /// </summary>
        /// <param name="input"></param>
        public async Task Grant(GrantDto input)
        {
            if (input.ActionType == SupplyApplySubResultType.处理中 || input.ActionType == SupplyApplySubResultType.已领取)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "用品申领处置方式异常");
            var model = _supplyApplySubRepository.Get(input.ApplySubId);
            model.Status = (int)input.ActionType;
            model.Result = (int)input.ActionType;
            if (input.ActionType == SupplyApplySubResultType.已发放)
            {
                if (input.SupplyIds == null || input.SupplyIds.Count() == 0)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未对用品申领发放用品");
                await _supplyApplyResultRepository.DeleteAsync(r => r.ApplySubId == model.Id);
                foreach (var item in input.SupplyIds)
                {
                    var supplyBaseModel = await _supplyBaseRepository.GetAsync(item);
                    supplyBaseModel.Status = (int)SupplyStatus.待领取;
                    var supplyApplyResult = new SupplyApplyResult()
                    {
                        ApplyMainId = model.MainId,
                        ApplySubId = model.Id,
                        Id = Guid.NewGuid(),
                        SupplyId = item,

                    };
                    await _supplyApplyResultRepository.InsertAsync(supplyApplyResult);
                }
            }
            else if (input.ActionType == SupplyApplySubResultType.申购)  //加入申购计划 只是先把处理结果改为 申购中  2018.5.20 改为： 不使用子流程处理，申购中把数据写入申购清单
            {


            }
            _supplyApplySubRepository.Update(model);
        }


        /// <summary>
        /// 获取申领详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<SupplyApplyDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var query = from a in _supplyApplyMainRepository.GetAll()
                        join u in UserManager.Users on a.CreatorUserId equals u.Id
                        join b in _supplyApplySubRepository.GetAll() on a.Id equals b.MainId into sub
                        from b in sub.DefaultIfEmpty()
                        join d in _supplyApplyResultRepository.GetAll() on b.Id equals d.ApplySubId into r
                        from d in r.DefaultIfEmpty()
                        join supply in _supplyBaseRepository.GetAll() on d.SupplyId equals supply.Id into h
                        from supply in h.DefaultIfEmpty()
                        where a.Id == id
                        select new
                        {
                            ApplyMain = a,
                            CreationUserNmae = u.Name,
                            b,
                            d,
                            supply
                        };


            var date = await query.ToListAsync();
            var model = date.FirstOrDefault();
            var ret = new SupplyApplyDto()
            {
                CreateUserName = model.CreationUserNmae,
                CreatorUserId = model.ApplyMain.CreatorUserId.Value,
                Id = model.ApplyMain.Id,
                IsImportant = model.ApplyMain.IsImportant,
                Status = model.ApplyMain.Status,
                CreationTime = model.ApplyMain.CreationTime,
            };

            var subs = date.Select(r => r.b).OrderByDescending(ite => ite.CreationTime).Distinct();



            foreach (var item in subs)
            {
                var itemModel = new SupplyApplySubDto()
                {
                    Des = item.Des,
                    Id = item.Id,
                    SupplyId = item.SupplyId,
                    GetTime = item.GetTime,
                    MainId = item.MainId,
                    Money = item.Money,
                    Name = item.Name,
                    Number = item.Number,
                    Result = item.Result,
                    ResultRemark = item.ResultRemark,
                    Type = item.Type,
                    Unit = item.Unit,
                    UserId = item.UserId,
                    Version = item.Version,
                    FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申领用品附件 }),
                    UserId_Name = _workFlowOrganizationUnitsManager.GetNames(item.UserId),
                    CreationTime = item.CreationTime,
                    Result_Name = ((SupplyApplySubResultType)item.Result).ToString(),
                    Status = item.Status,
                    StatusTitle = ((SupplyApplySubStatusType)item.Status).ToString(),
                    ApplyDateTime = item.CreationTime,
                };

                itemModel.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = itemModel.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申领用品附件 });
                var result = date.Where(r => r.b.Id == item.Id);
                foreach (var entity in result)
                {
                    if (entity.d != null && entity.supply != null)
                    {
                        var resultdata = new SupplyApplyResultDto()
                        {
                            Id = entity.d.Id,
                            ApplyMainId = entity.ApplyMain.Id,
                            ApplySubId = entity.b.Id,
                            SupplyCode = entity.supply.Code,
                            SupplyId = entity.supply.Id,
                            SupplyName = entity.supply.Name,
                            SupplyMoney = entity.supply.Money,
                            SupplyVersion = entity.supply.Version,

                        };
                        itemModel.SupplyApplyResult.Add(resultdata);
                    }
                }
                ret.SupplyApplySub.Add(itemModel);
            }
            var queryBak = (from a in _supplyApplySubBakRepository.GetAll()
                            where a.MainId == id
                            select new SupplyApplySubBakDto()
                            {
                                Des = a.Des,
                                Id = a.Id,
                                SupplyId = a.SupplyId,
                                GetTime = a.GetTime,
                                MainId = a.MainId,
                                Money = a.Money,
                                Name = a.Name,
                                Number = a.Number,
                                Type = a.Type,
                                Unit = a.Unit,
                                UserId = a.UserId,
                                Version = a.Version,
                                CreationTime = a.CreationTime,
                                ApplyDateTime = a.CreationTime,
                            }).ToList();
            for (int i = 0; i < queryBak.Count; i++)
            {
                queryBak[i].UserId_Name = _workFlowOrganizationUnitsManager.GetNames(queryBak[i].UserId);
                queryBak[i].FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = queryBak[i].Id.ToString(), BusinessType = (int)AbpFileBusinessType.申领用品附件 });
            }
            ret.SupplyApplySubBak = queryBak;
            return ret;
        }

        public void Update(CreateApplyDto input)
        {
            var model = _supplyApplyMainRepository.Get(input.Id.Value);
            var old_model = model.DeepClone();
            var query_oldSub = from sub in _supplyApplySubRepository.GetAll()
                               join u in UserManager.Users on sub.UserId equals ("u_" + u.Id.ToString()) into g
                               from user in g.DefaultIfEmpty()
                               where sub.MainId == model.Id
                               select new { Subs = sub, UserName = user == null ? "" : user.Name };
            var old_ChangeModel = new SupplyApplyChangeDto() { IsImportant = old_model.IsImportant };
            foreach (var item in query_oldSub)
            {
                var entity = new SupplyApplySubChangeDto()
                {
                    Des = item.Subs.Des,
                    GetTime = item.Subs.GetTime,
                    Money = item.Subs.Money,
                    Id = item.Subs.Id,
                    Name = item.Subs.Name,
                    Number = item.Subs.Number,
                    Type_Name = ((SupplyType)item.Subs.Type).ToString(),
                    Unit = item.Subs.Unit,
                    UserId_Name = item.UserName,
                    Version = item.Subs.Version,
                };
                var files = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Subs.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申领用品附件 });
                if (files.Count > 0)
                    entity.Files = files.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();
                old_ChangeModel.SubSubpply.Add(entity);
            }
            model = input.MapTo(model);
            if (input.SupplyApplySub != null && input.SupplyApplySub.Count > 0)
            {
                foreach (var item in input.SupplyApplySub)
                {
                    if (item.FileList != null)
                    {
                        var fileList = new List<AbpFileListInput>();
                        foreach (var ite in item.FileList)
                        {
                            fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                        }
                        _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                        {
                            BusinessId = item.Id.ToString(),
                            BusinessType = (int)AbpFileBusinessType.申领用品附件,
                            Files = fileList
                        });
                    }
                    if (item.Id.HasValue == false)
                    {
                        var sub = item.MapTo<SupplyApplySub>();
                        sub.MainId = model.Id;
                        sub.Id = Guid.NewGuid();
                        _supplyApplySubRepository.Insert(sub);
                    }
                    else
                    {
                        var sub = _supplyApplySubRepository.Get(item.Id.Value);
                        sub = item.MapTo(sub);
                        _supplyApplySubRepository.Update(sub);
                    }
                }
            }
            _supplyApplyMainRepository.Update(model);
            var new_ChangeModel = new SupplyApplyChangeDto() { IsImportant = model.IsImportant };
            foreach (var item in input.SupplyApplySub)
            {
                var entity = new SupplyApplySubChangeDto()
                {
                    Des = item.Des,
                    GetTime = item.GetTime,
                    Money = item.Money,
                    Id = item.Id ?? Guid.Empty,
                    Name = item.Name,
                    Number = item.Number,
                    Type_Name = ((SupplyType)item.Type).ToString(),
                    Unit = item.Unit,
                    //UserId_Name = item.UserName,
                    Version = item.Version,
                };
                if (!item.UserId.IsNullOrWhiteSpace())
                    entity.UserId_Name = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                if (item.FileList.Count > 0)
                {
                    entity.Files = item.FileList.Select(r => new AbpFileChangeDto { FileName = r.FileName, Id = r.Id }).ToList();
                }
                new_ChangeModel.SubSubpply.Add(entity);
            }

            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = old_ChangeModel.GetColumnAllLogs(new_ChangeModel);
                _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }

        /// <summary>
        /// 修改申领用品明细数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateOne(UpdateApplySubDto input)
        {
            var old_model = await _supplyApplySubRepository.GetAsync(input.Id);
            input.MapTo(old_model);
            await _supplyApplySubRepository.UpdateAsync(old_model);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var ite in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = input.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.申领用品附件,
                    Files = fileList
                });
            }
        }


        /// <summary>
        /// 新增申领用品明细数据  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOneSupplyApplySub(CreateOneSupplyApplySubInput input)
        {
            var mainModel = await _supplyApplyMainRepository.GetAsync(input.MainId);
            var subModel = new SupplyApplySub()
            {
                Id = Guid.NewGuid(),
                GetTime = input.GetTime,
                Des = input.Des,
                MainId = input.MainId,
                Money = input.Money,
                Name = input.Name,
                Number = input.Number,
                Result = input.Result,
                ResultRemark = input.ResultRemark,
                Type = input.Type,
                Unit = input.Unit,
                UserId = input.UserId,
                Version = input.Version,
                CreatorUserId = mainModel.CreatorUserId,

            };
            await _supplyApplySubRepository.InsertAsync(subModel);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var ite in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = subModel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.申领用品附件,
                    Files = fileList
                });
            }
        }




        /// <summary>
        /// 判断申领处理结果，是否包含申购
        /// </summary>
        /// <param name="id">申领id</param>
        /// <returns></returns>
        [RemoteService(false)]
        public bool HasSupplyApplyPurchase(Guid id)
        {
            var query = from a in _supplyApplyMainRepository.GetAll()
                        join b in _supplyApplySubRepository.GetAll() on a.Id equals b.MainId into g
                        where a.Id == id
                        select new { a, Subs = g };
            var model = query.FirstOrDefault();
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到申领信息");
            if (model.Subs.Any(r => r.Result == (int)SupplyApplySubResultType.申购))
                return true;
            else
                return false;
        }




        [RemoteService(false)]
        public string CreateSupplyPurchaseMainForSubFlow(Guid instacneId)
        {
            var query = from a in _supplyApplyMainRepository.GetAll()
                        join b in _supplyApplySubRepository.GetAll() on a.Id equals b.MainId into g
                        where a.Id == instacneId
                        select new { a, Subs = g.Where(r => r.Result == (int)SupplyApplySubResultType.申购) };
            var model = query.FirstOrDefault();
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到申领信息");

            if (model.Subs.Count() == 0)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到申购信息");
            var purchaseCode = DateTime.Now.ToString("yyyyMMdd");
            lock (purchaseCode)
            {
                var exit_PurchaseModels = _supplyPurchaseMainRepository.GetAll().Where(r => r.Code.Contains(purchaseCode));
                if (exit_PurchaseModels.Count() == 0)
                {
                    var entity_pMainId = Guid.NewGuid();
                    var current_Code = purchaseCode + "001";
                    foreach (var item in model.Subs)
                    {
                        var entity = new SupplyPurchasePlan()
                        {
                            Id = Guid.NewGuid(),
                            SupplyApplySubId = item.Id,
                            SupplyApplyMainId = instacneId,
                            SupplyPurchaseCode = current_Code,
                            Name = item.Name,
                            Des = item.Des,
                            GetTime = item.GetTime,
                            Money = item.Money,
                            Number = item.Number,
                            SupplyPurchaseId = entity_pMainId,
                            Unit = item.Unit,
                            UserId = item.UserId,
                            Version = item.Version,
                            Type = item.Type,
                        };
                        _supplyPurchasePlaneRepository.Insert(entity);
                    }
                    var entity_pMain = new SupplyPurchaseMain()
                    {
                        Id = entity_pMainId,
                        Code = current_Code,
                    };
                    _supplyPurchaseMainRepository.Insert(entity_pMain);
                    return current_Code;
                }
                else
                {
                    var near_exit_PurchaseModel = exit_PurchaseModels.OrderByDescending(r => r.CreationTime).First();
                    if (near_exit_PurchaseModel.Status == 0)
                    {
                        foreach (var item in model.Subs)
                        {
                            var entity = new SupplyPurchasePlan()
                            {
                                Id = Guid.NewGuid(),
                                SupplyApplySubId = item.Id,
                                SupplyApplyMainId = instacneId,
                                SupplyPurchaseCode = near_exit_PurchaseModel.Code,
                                Name = item.Name,
                                Des = item.Des,
                                GetTime = item.GetTime,
                                Money = item.Money,
                                Number = item.Number,
                                SupplyPurchaseId = near_exit_PurchaseModel.Id,
                                Unit = item.Unit,
                                UserId = item.UserId,
                                Version = item.Version,
                                Type = item.Type
                            };
                            _supplyPurchasePlaneRepository.Insert(entity);
                        }
                        return near_exit_PurchaseModel.Code;
                    }
                    else
                    {
                        var entity_pMainId = Guid.NewGuid();
                        var currentCount = exit_PurchaseModels.Count() + 1;
                        var current_Code = "";
                        if (currentCount < 10)
                            current_Code = purchaseCode + $"00{currentCount}";
                        else if (currentCount < 99)
                            current_Code = purchaseCode + $"0{currentCount}";
                        foreach (var item in model.Subs)
                        {
                            var entity = new SupplyPurchasePlan()
                            {
                                Id = Guid.NewGuid(),
                                SupplyApplySubId = item.Id,
                                SupplyApplyMainId = instacneId,
                                SupplyPurchaseCode = current_Code,
                                Name = item.Name,
                                Des = item.Des,
                                GetTime = item.GetTime,
                                Money = item.Money,
                                Number = item.Number,
                                SupplyPurchaseId = entity_pMainId,
                                Unit = item.Unit,
                                UserId = item.UserId,
                                Version = item.Version,
                                Type = item.Type,
                            };
                            _supplyPurchasePlaneRepository.Insert(entity);
                        }
                        var entity_pMain = new SupplyPurchaseMain()
                        {
                            Id = entity_pMainId,
                            Code = current_Code,
                        };
                        _supplyPurchaseMainRepository.Insert(entity_pMain);
                        return current_Code;
                    }
                }
            }




        }


        /// <summary>
        /// 申领流程中 把处理为申购的数据 加入 申购清单
        /// </summary>
        /// <param name="instacneId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public void CreateSupplyPurchaseQD(Guid instacneId)
        {
            var query = from a in _supplyApplyMainRepository.GetAll()
                        join b in _supplyApplySubRepository.GetAll() on a.Id equals b.MainId into g
                        where a.Id == instacneId
                        select new { a, Subs = g.Where(r => r.Result == (int)SupplyApplySubResultType.申购) };
            var model = query.FirstOrDefault();
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到申领信息");
            foreach (var item in model.Subs)
            {
                var entity = new SupplyPurchasePlan()
                {
                    Id = Guid.NewGuid(),
                    SupplyApplySubId = item.Id,
                    SupplyApplyMainId = instacneId,
                    SupplyPurchaseCode = "",
                    Name = item.Name,
                    Des = item.Des,
                    GetTime = item.GetTime,
                    Money = item.Money,
                    Number = item.Number,
                    SupplyPurchaseId = Guid.Empty,
                    Unit = item.Unit,
                    UserId = item.UserId,
                    Version = item.Version,
                    Type = item.Type,
                };
                _supplyPurchasePlaneRepository.Insert(entity);
            }
        }



        /// <summary>
        /// 采购处理采购待办获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<SupplyPurchaseMainDto> GetSupplyPurchase(GetWorkFlowTaskCommentInput input)
        {
            var ret = new SupplyPurchaseMainDto();
            var mainModel = await _supplyPurchaseMainRepository.GetAll().SingleOrDefaultAsync(r => r.Code == input.InstanceId);
            ret.Id = mainModel.Id;
            ret.Code = mainModel.Code;
            var plans = await _supplyPurchasePlaneRepository.GetAll().Where(r => r.SupplyPurchaseId == mainModel.Id).ToListAsync();
            foreach (var item in plans)
            {
                var data = new SupplyPurchasePlanDto();
                var resultQuery = from r in _supplyPurchaseResultRepository.GetAll()
                                  join s in _supplyBaseRepository.GetAll() on r.SupplyId equals s.Id
                                  orderby r.CreationTime descending
                                  select new SupplyPurchaseResultDto
                                  {
                                      Id = r.Id,
                                      SupplyCode = s.Code,
                                      SupplyId = s.Id,
                                      SupplyName = s.Name,
                                      SupplyPurchasePlanId = item.Id,
                                      Money = s.Money,
                                      Type = s.Type,
                                      //UserId = s.UserId,
                                      Version = s.Version,
                                      Unit = s.Unit,
                                  };

                var resultModel = await resultQuery.ToListAsync();

                data.Result = resultModel;
                if (data.Result != null && data.Result.Count() > 0)
                {
                    foreach (var item_result in data.Result)
                    {
                        item_result.Type_Name = ((SupplyType)item_result.Type).ToString();
                    }
                }
                data.Id = item.Id;
                data.Des = item.Des;
                data.GetTime = item.GetTime;
                data.CreationTime = item.CreationTime;
                data.Money = item.Money;
                data.Name = item.Name;
                data.Number = item.Number;
                data.SupplyApplyMainId = item.SupplyApplyMainId;
                data.SupplyApplySubId = item.SupplyApplySubId;
                data.SupplyPurchaseCode = item.SupplyPurchaseCode;
                data.SupplyPurchaseId = item.SupplyPurchaseId;
                data.DoPurchaseStatus = item.DoPurchaseStatus;
                data.DoPurchaseStatusTitle = ((SupplyPurchaseQOneDStatus)item.DoPurchaseStatus).ToString();
                data.Unit = item.Unit;
                data.UserId = item.UserId;
                data.Version = item.Version;
                data.User_Name = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                data.Type = item.Type;
                data.TypeName = ((SupplyType)item.Type).ToString();
                ret.Plans.Add(data);
            }
            return ret;
        }


        /// <summary>
        ///新增或编辑采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateSupplyPurchase(CreateOrUpdateSupplyPurchasePlanInput input)
        {
            if (input.Id.HasValue)
            {
                var entity = await _supplyPurchasePlaneRepository.GetAsync(input.Id.Value);
                entity.Des = input.Des;
                entity.GetTime = input.GetTime;
                entity.Money = input.Money;
                entity.Name = input.Name;
                entity.Number = input.Number;
                entity.Unit = input.Unit;
                entity.UserId = input.UserId;
                entity.Version = input.Version;
                entity.Type = input.Type;
            }
            else
            {
                var supplyPurchaseMain = await _supplyPurchaseMainRepository.GetAsync(input.MainId);
                var entity = new SupplyPurchasePlan()
                {
                    Des = input.Des,
                    GetTime = input.GetTime,
                    Money = input.Money,
                    Name = input.Name,
                    Number = input.Number,
                    SupplyApplyMainId = input.SupplyApplyMainId,
                    SupplyApplySubId = input.SupplyApplySubId,
                    SupplyPurchaseCode = supplyPurchaseMain.Code,
                    SupplyPurchaseId = supplyPurchaseMain.Id,
                    Unit = input.Unit,
                    UserId = input.UserId,
                    Version = input.Version,
                    Type = input.Type,
                };

                await _supplyPurchasePlaneRepository.InsertAsync(entity);
            }
        }


        /// <summary>
        /// 删除采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteSupplyPurchase(EntityDto<Guid> input)
        {
            var model = await _supplyPurchasePlaneRepository.GetAsync(input.Id);
            if (model.SupplyApplySubId.HasValue)
                throw new UserFriendlyException((int)ErrorCode.HttpPortErr, "申领生成的申购数据，不能删除");
            else
            {
                await _supplyPurchasePlaneRepository.DeleteAsync(model);
            }
        }


        /// <summary>
        /// 更新采购清单
        /// </summary>
        /// <returns></returns>
        public async Task UpdateSupplyPurchase(UpdateSupplyPurchaseInput input)
        {
            var model = await _supplyPurchaseMainRepository.GetAsync(input.Id);
            var updateModel = input.Plans.Where(r => r.Id.HasValue);
            var planModels = await _supplyPurchasePlaneRepository.GetAll().Where(r => r.SupplyPurchaseId == model.Id).ToListAsync();
            foreach (var item in updateModel)
            {
                var entity = planModels.FirstOrDefault(r => r.Id == item.Id);
                if (entity != null)
                {
                    entity.Des = item.Des;
                    entity.GetTime = item.GetTime;
                    entity.Money = item.Money;
                    entity.Name = item.Name;
                    entity.Number = item.Number;
                    //entity.SupplyApplyMainId = item.SupplyApplyMainId;
                    //entity.SupplyApplySubId = item.SupplyApplySubId;
                    //entity.SupplyPurchaseCode = item.SupplyPurchaseCode;
                    //entity.SupplyPurchaseId = item.SupplyPurchaseId;
                    entity.Unit = item.Unit;
                    entity.UserId = item.UserId;
                    entity.Version = item.Version;
                    entity.Type = item.Type;
                }

            }
            var insertModels = input.Plans.Where(r => !r.Id.HasValue);
            foreach (var item in insertModels)
            {
                var entity = new SupplyPurchasePlan()
                {
                    Des = item.Des,
                    GetTime = item.GetTime,
                    Money = item.Money,
                    Name = item.Name,
                    Number = item.Number,
                    SupplyApplyMainId = item.SupplyApplyMainId,
                    SupplyApplySubId = item.SupplyApplySubId,
                    SupplyPurchaseCode = model.Code,
                    SupplyPurchaseId = model.Id,
                    Unit = item.Unit,
                    UserId = item.UserId,
                    Version = item.Version,
                    Type = item.Type,
                };

                await _supplyPurchasePlaneRepository.InsertAsync(entity);
            }

        }



        /// <summary>
        /// 入库采购的用品One
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterSupplyPurchaseOutput> RegisterSupplyPurchase(RegisterSupplyPurchaseInput input)
        {
            var ret = new RegisterSupplyPurchaseOutput();
            ret.ResultId = input.ResultId.Value;
            ret.Name = input.Name;
            ret.Version = input.Version;
            ret.Unit = input.Unit;
            ret.Money = input.Money;
            //ret.Des = input.Des;
            ret.UserId = input.UserId;
            var model = await _supplyPurchasePlaneRepository.GetAsync(input.SupplyPurchasePlanId);
            if (input.ResultId.HasValue)
            {

                var purchaseResultModel = await _supplyPurchaseResultRepository.GetAsync(input.ResultId.Value);
                var supplyModel = await _supplyBaseRepository.GetAsync(purchaseResultModel.SupplyId);
                supplyModel.Name = input.Name;
                supplyModel.Version = input.Version;
                supplyModel.Money = input.Money;
                supplyModel.UserId = input.UserId;
                supplyModel.Type = input.Type;
                supplyModel.PutInDate = DateTime.Now;

                ret.Code = supplyModel.Code;
                if (model.SupplyApplyMainId.HasValue)
                {
                    var exit_subApplyResult = await _supplyApplyResultRepository.GetAll().AnyAsync(r => r.ApplySubId == model.SupplyApplySubId && r.SupplyId == supplyModel.Id);
                    if (exit_subApplyResult)
                        ret.IsUseApply = true;
                }

            }
            else
            {
                var supplyModel = new SupplyBase()
                {
                    Id = Guid.NewGuid(),
                    Code = "",
                    Name = input.Name,
                    Version = input.Version,
                    Money = input.Money,
                    UserId = input.UserId,
                    Type = input.Type,
                    Unit = input.Unit,
                    PutInDate = DateTime.Now,
                };

                var supplyPurchaseResult = new SupplyPurchaseResult()
                {
                    Id = Guid.NewGuid(),
                    SupplyId = supplyModel.Id,
                    SupplyPurchasePlanId = input.SupplyPurchasePlanId,
                };

                await _supplyPurchaseResultRepository.InsertAsync(supplyPurchaseResult);
                if (model.SupplyApplySubId.HasValue)
                {
                    var supplyApplySubModel = await _supplyApplySubRepository.GetAsync(model.SupplyApplySubId.Value);
                    var supplyApplyResultModels = await _supplyApplyResultRepository.GetAll().Where(r => r.ApplySubId == supplyApplySubModel.Id).ToListAsync();
                    if (supplyApplyResultModels.Count() < supplyApplySubModel.Number)
                    {
                        var supplyApplyResult = new SupplyApplyResult()
                        {
                            Id = Guid.NewGuid(),
                            ApplyMainId = model.SupplyApplyMainId.Value,
                            ApplySubId = model.SupplyApplyMainId.Value,
                            SupplyId = supplyModel.Id
                        };
                        await _supplyApplyResultRepository.InsertAsync(supplyApplyResult);
                        supplyModel.Status = (int)SupplyStatus.待领取;
                        ret.IsUseApply = true;
                    }
                    else
                    {
                        supplyModel.Status = (int)SupplyStatus.在库;
                    }

                }
                await _supplyBaseRepository.InsertAsync(supplyModel);
            }

            return ret;

        }


        /// <summary>
        /// 入库采购的用品 按采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateRegisterSupplyPurchaseAll(RegisterSupplyPurchasePlanInput input)
        {
            var ret = new RegisterSupplyPurchaseOutput();
            var model = await _supplyPurchasePlaneRepository.GetAsync(input.PlanId);
            model.DoPurchaseStatus = (int)SupplyPurchaseQOneDStatus.已入库;
            model.PutInDate = DateTime.Now;

            if (model.SupplyApplySubId.HasValue)
            {
                var supplyApplySubModel = await _supplyApplySubRepository.GetAsync(model.SupplyApplySubId.Value);
                supplyApplySubModel.Status = (int)SupplyApplySubStatusType.采购入库;
                if (input.Supplys.Count() < supplyApplySubModel.Number)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "采购的数据不能少于申购的数量");
                for (int i = 0; i < supplyApplySubModel.Number; i++)
                {
                    var item = input.Supplys[i];
                    var supplyModel = new SupplyBase()
                    {
                        Id = Guid.NewGuid(),
                        Code = "",
                        Name = item.Name,
                        Version = item.Version,
                        Money = item.Money,
                        UserId = model.UserId,
                        Type = item.Type,
                        Unit = item.Unit,
                        CreatorUserId = AbpSession.UserId,
                        ExpiryDate = item.ExpiryDate,
                        PutInDate = DateTime.Now,
                    };

                    var supplyPurchaseResult = new SupplyPurchaseResult()
                    {
                        Id = Guid.NewGuid(),
                        SupplyId = supplyModel.Id,
                        SupplyPurchasePlanId = input.PlanId,
                    };
                    await _supplyPurchaseResultRepository.InsertAsync(supplyPurchaseResult);
                    var supplyApplyResult = new SupplyApplyResult()
                    {
                        Id = Guid.NewGuid(),
                        ApplyMainId = model.SupplyApplyMainId.Value,
                        ApplySubId = model.SupplyApplySubId.Value,
                        SupplyId = supplyModel.Id,


                    };
                    await _supplyApplyResultRepository.InsertAsync(supplyApplyResult);


                    supplyModel.Status = (int)SupplyStatus.待领取;
                    await _supplyBaseRepository.InsertAsync(supplyModel);
                    //input.Supplys.Remove(item);

                    item.HasDo = true;
                }
            }
            foreach (var item in input.Supplys.Where(r => !r.HasDo))
            {
                var supplyModel = new SupplyBase()
                {
                    Id = Guid.NewGuid(),
                    Code = "",
                    Name = item.Name,
                    Version = item.Version,
                    Money = item.Money,
                    //UserId = item.UserId,
                    Type = item.Type,
                    Unit = item.Unit,
                    CreatorUserId = AbpSession.UserId,
                    ExpiryDate = item.ExpiryDate,
                    PutInDate = DateTime.Now,
                };

                var supplyPurchaseResult = new SupplyPurchaseResult()
                {
                    Id = Guid.NewGuid(),
                    SupplyId = supplyModel.Id,
                    SupplyPurchasePlanId = input.PlanId,
                };

                await _supplyPurchaseResultRepository.InsertAsync(supplyPurchaseResult);
                supplyModel.Status = (int)SupplyStatus.在库;
                await _supplyBaseRepository.InsertAsync(supplyModel);
            }
        }


        public async Task UpdateRegisterSupplyPurchaseAll(RegisterSupplyPurchasePlanInput input)
        {
            var ret = new RegisterSupplyPurchaseOutput();
            var model = await _supplyPurchasePlaneRepository.GetAsync(input.PlanId);
            //var supplyApplySubModel = await _supplyApplySubRepository.GetAsync(model.SupplyApplySubId.Value);
            var exit_Result = await _supplyPurchaseResultRepository.GetAll().Where(r => r.SupplyPurchasePlanId == input.PlanId).ToListAsync();
            var delete_Result = exit_Result.Select(r => r.Id).Except(input.Supplys.Where(r => r.ResultId.HasValue).Select(r => r.ResultId.Value));
            foreach (var item in delete_Result)
            {
                var delete_model = exit_Result.FirstOrDefault(r => r.Id == item);
                await _supplyPurchaseResultRepository.DeleteAsync(delete_model);
                await _supplyBaseRepository.DeleteAsync(delete_model.SupplyId);
            }

            foreach (var item in input.Supplys)
            {

                if (item.ResultId.HasValue)
                {
                    var purchaseResultModel = await _supplyPurchaseResultRepository.GetAsync(item.ResultId.Value);
                    var supplyModel = await _supplyBaseRepository.GetAsync(purchaseResultModel.SupplyId);
                    supplyModel.Name = item.Name;
                    supplyModel.Version = item.Version;
                    supplyModel.Money = item.Money;
                    //supplyModel.UserId = item.UserId;
                    supplyModel.Type = item.Type;
                    if (model.SupplyApplyMainId.HasValue)
                    {
                        var exit_subApplyResult = await _supplyApplyResultRepository.GetAll().AnyAsync(r => r.ApplySubId == model.SupplyApplySubId && r.SupplyId == supplyModel.Id);
                        if (exit_subApplyResult)
                            ret.IsUseApply = true;
                    }

                }
                else
                {
                    var supplyModel = new SupplyBase()
                    {
                        Id = Guid.NewGuid(),
                        Code = "",
                        Name = item.Name,
                        Version = item.Version,
                        Money = item.Money,
                        //UserId = item.UserId,
                        Type = item.Type,
                        Unit = item.Unit,
                        CreatorUserId = AbpSession.UserId,
                        PutInDate = DateTime.Now,
                    };

                    var supplyPurchaseResult = new SupplyPurchaseResult()
                    {
                        Id = Guid.NewGuid(),
                        SupplyId = supplyModel.Id,
                        SupplyPurchasePlanId = input.PlanId,
                    };

                    await _supplyPurchaseResultRepository.InsertAsync(supplyPurchaseResult);
                    supplyModel.Status = (int)SupplyStatus.在库;
                    await _supplyBaseRepository.InsertAsync(supplyModel);
                }
            }



        }


        /// <summary>
        /// 获取入库结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<RegisterSupplyPurchaseOutput>> GetSupplyPurchaseResult(GetSupplyPurchaseResultInput input)
        {

            var query = from a in _supplyPurchaseResultRepository.GetAll()
                        join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                        join d in _supplyPurchasePlaneRepository.GetAll() on a.SupplyPurchasePlanId equals d.Id
                        join c in _supplyApplyResultRepository.GetAll() on new { A = d.SupplyApplySubId.Value, B = b.Id } equals new { A = c.ApplySubId, B = c.SupplyId } into g
                        from c in g.DefaultIfEmpty()
                        where a.SupplyPurchasePlanId == input.PlanId
                        select new RegisterSupplyPurchaseOutput()
                        {
                            Code = b.Code,
                            Money = b.Money,
                            Name = b.Name,
                            ResultId = a.Id,
                            UserId = b.UserId,
                            Version = b.Version,
                            IsUseApply = c == null ? false : true,
                            Unit = b.Unit,

                        };
            var count = await query.CountAsync();
            var datas = await query.OrderBy(r => r.IsUseApply).PageBy(input).ToListAsync();
            var ret = new PagedResultDto<RegisterSupplyPurchaseOutput>(count, datas);
            return ret;
        }


        [RemoteService(false)]
        public void ChangeApplyStatusAfterPurchase(string instanceId)
        {
            var model = _supplyPurchaseMainRepository.FirstOrDefault(r => r.Code == instanceId);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取采购数据失败");
            var planModels = _supplyPurchasePlaneRepository.GetAll().Where(r => r.SupplyPurchaseId == model.Id && r.SupplyApplySubId.HasValue).ToList();
            foreach (var item in planModels)
            {
                item.Status = (int)SupplyApplySubResultType.已发放;
            }
        }


        public async Task<PagedResultDto<SupplyPurchaseListDto>> GetAllPurchase(GetSupplyPurchaseListInput input)
        {

            var query = from a in _supplyPurchaseMainRepository.GetAll()
                        join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                x.FlowID == input.FlowId && x.InstanceID == a.Code &&
                                x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new SupplyPurchaseListDto
                        {
                            Id = a.Id,
                            Code = a.Code,
                            CreateUserName = b.Name,
                            CreationTime = a.CreationTime,
                            CreatorUserId = a.CreatorUserId.Value,
                            Status = a.Status,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                        ? 1
                                        : 2
                        };
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(),
                r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var model = await query.OrderBy(x => x.OpenModel).ThenByDescending(x => x.CreationTime).PageBy(input).ToListAsync();

            foreach (var r in model)
            {
                r.InstanceId = r.Code;
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId,
                    r as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<SupplyPurchaseListDto>(count, model);

        }



        /// <summary>
        /// 获取用品申领处理的申购清单
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplyPurchaseQDFromApplyListOutput>> GetSupplyPurchaseQDFromApplyList(SupplyPurchaseQDFromApplyListInput input)
        {
            var query = from a in _supplyApplyMainRepository.GetAll()
                        join b in _supplyApplySubRepository.GetAll() on new { MainId = a.Id, ApplyStatus = (int)SupplyApplySubResultType.申购 }
                        equals new { MainId = b.MainId, ApplyStatus = b.Result } into g
                        join p in _supplyPurchasePlaneRepository.GetAll() on a.Id equals p.SupplyApplyMainId into plans
                        join c in UserManager.Users on a.CreatorUserId equals c.Id
                        join d in _userOrganizationUnitRepository.GetAll() on a.CreatorUserId.Value equals d.UserId
                        join e in _organizationUnitRepository.GetAll() on d.OrganizationUnitId equals e.Id
                        let cc = (from p in _userPostRepository.GetAll()
                                  join o in _postInfoRepository.GetAll() on p.PostId equals o.Id
                                  where p.UserId == c.Id && p.OrgId == d.OrganizationUnitId
                                  select new { PostId = o.Id, PostName = o.Name }).ToList()
                        where g.Count() > 0 && plans.Count() > 0 && d.IsMain == true
                        select new
                        {
                            SupplyAppyId = a.Id,
                            SupplyUserName = c.Name,
                            Post = cc,
                            OrgName = e.DisplayName,
                            a.CreationTime,
                            IsImportant = a.IsImportant,
                            Status = a.DoPurchaseStatus
                        };
            var count = query.Count();
            var list = query.OrderBy(input.Sorting).PageBy(input).ToList();
            var ret = new List<SupplyPurchaseQDFromApplyListOutput>();
            foreach (var item in list)
            {
                var entity = new SupplyPurchaseQDFromApplyListOutput();
                entity.SupplyAppyId = item.SupplyAppyId;
                entity.SupplyUserName = item.SupplyUserName;
                if (item.Post == null || item.Post.Count() == 0)
                {
                }
                else
                {
                    var potsList = item.Post.Select(r => r.PostName);
                    entity.PostName = string.Join(",", potsList);
                }

                entity.OrgName = item.OrgName;
                entity.ApplyDateTime = item.CreationTime;
                entity.IsImportant = item.IsImportant;
                entity.Status = item.Status;
                entity.StatusTile = ((SupplyPurchaseQDStatus)item.Status).ToString();
                ret.Add(entity);
            }

            return new PagedResultDto<SupplyPurchaseQDFromApplyListOutput>(count, ret);

        }


        /// <summary>
        /// 获取一次申领的申购清单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplyPurchasePlanDto>> GetSupplyPurchaseQDOne(SupplyPurchaseQDOneFromApplyListInput input)
        {
            var query = from a in _supplyPurchasePlaneRepository.GetAll()
                        join b in _supplyApplyMainRepository.GetAll() on a.SupplyApplyMainId equals b.Id
                        where b.Id == input.SupplyApplyMainId
                        select new SupplyPurchasePlanDto
                        {
                            Des = a.Des,
                            GetTime = a.GetTime,
                            Id = a.Id,
                            Money = a.Money,
                            Name = a.Name,
                            Number = a.Number,
                            SupplyApplyMainId = a.SupplyApplyMainId,
                            SupplyApplySubId = a.SupplyApplySubId,
                            SupplyPurchaseCode = a.SupplyPurchaseCode,
                            SupplyPurchaseId = a.SupplyPurchaseId,
                            Type = a.Type,
                            Unit = a.Unit,
                            UserId = a.UserId,
                            Version = a.Version,
                            DoPurchaseStatus = a.DoPurchaseStatus,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                        };
            var count = await query.CountAsync();
            var data = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            foreach (var item in data)
            {
                item.TypeName = ((SupplyType)item.Type).ToString();
                item.User_Name = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                item.DoPurchaseStatusTitle = ((SupplyPurchaseQOneDStatus)item.DoPurchaseStatus).ToString();
            }
            return new PagedResultDto<SupplyPurchasePlanDto>(count, data);
        }


        /// <summary>
        /// 加入采购计划
        /// </summary>
        /// <returns></returns>
        public async Task AddSupplyPurchase(AddSupplyPurchasePlanInput input)
        {
            if (input.PlanId.HasValue)
            {
                var planModel = _supplyPurchasePlaneRepository.Get(input.PlanId.Value);
                if (planModel.SupplyApplyMainId.HasValue)
                {
                    var all_Plans = _supplyPurchasePlaneRepository.GetAll().Where(r => r.SupplyApplyMainId == planModel.SupplyApplyMainId);
                    var noDoCount = all_Plans.Count(r => r.DoPurchaseStatus == (int)SupplyPurchaseQOneDStatus.未处理);
                    var applyMainModel = await _supplyApplyMainRepository.GetAsync(planModel.SupplyApplyMainId.Value);
                    if (noDoCount == 1)
                    {
                        applyMainModel.DoPurchaseStatus = (int)SupplyPurchaseQDStatus.已完成;
                    }
                    else if (noDoCount > 1)
                    {
                        applyMainModel.DoPurchaseStatus = (int)SupplyPurchaseQDStatus.未完成;
                    }
                }
                planModel.DoPurchaseStatus = (int)SupplyPurchaseQOneDStatus.已加入采购计划;
                if (planModel.SupplyApplySubId.HasValue)
                {
                    var applySubMdeol = await _supplyApplySubRepository.GetAsync(planModel.SupplyApplySubId.Value);
                    applySubMdeol.Status = (int)SupplyApplySubStatusType.已加入采购计划;
                }


            }
            else
            {
                var entity = new SupplyPurchasePlan()
                {
                    Des = input.Des,
                    GetTime = input.GetTime,
                    Money = input.Money,
                    Name = input.Name,
                    Number = input.Number,
                    SupplyApplyMainId = input.SupplyApplyMainId,
                    SupplyApplySubId = input.SupplyApplySubId,
                    Unit = input.Unit,
                    UserId = input.UserId,
                    Version = input.Version,
                    Type = input.Type,
                    DoPurchaseStatus = (int)SupplyPurchaseQOneDStatus.已加入采购计划,
                };
                _supplyPurchasePlaneRepository.Insert(entity);
            }
        }

        /// <summary>
        /// 批量新增采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddSupplyPurchases(List<CreateSupplyPurchasePlanInput> input)
        {
            foreach (var item in input)
            {
                var entity = new SupplyPurchasePlan()
                {
                    Des = item.Des,
                    GetTime = item.GetTime,
                    Money = item.Money,
                    Name = item.Name,
                    Number = item.Number,
                    //SupplyApplyMainId = item.SupplyApplyMainId,
                    //SupplyApplySubId = item.SupplyApplySubId,
                    Unit = item.Unit,
                    UserId = item.UserId,
                    Version = item.Version,
                    Type = item.Type,
                    DoPurchaseStatus = (int)SupplyPurchaseQOneDStatus.已加入采购计划,
                };
                _supplyPurchasePlaneRepository.Insert(entity);
            }
        }


        /// <summary>
        /// 提交采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<InitWorkFlowOutput> SubmitSupplyPurchasePlans(SubmitSupplyPurchasePlansInput input)
        {
            var ret = new InitWorkFlowOutput();
            var purchaseCode = DateTime.Now.ToString("yyyyMMdd");
            var retCode = "";
            var retId = Guid.Empty;
            lock (purchaseCode)
            {
                var exit_PurchaseModels = _supplyPurchaseMainRepository.GetAll().Where(r => r.Code.Contains(purchaseCode));
                if (exit_PurchaseModels.Count() == 0)
                {
                    var entity_pMainId = Guid.NewGuid();
                    var current_Code = purchaseCode + "001";
                    ret = CreateSupplyPruchasePlanInstance(new CreateSupplyPruchasePlanInstanceInput() { Id = entity_pMainId, Code = current_Code, FlowId = input.FlowId, FlowTitle = input.FlowTitle });
                    retCode = current_Code;
                    retId = entity_pMainId;
                }
                else
                {
                    var near_exit_PurchaseModel = exit_PurchaseModels.OrderByDescending(r => r.CreationTime).First();
                    if (near_exit_PurchaseModel.Status == 0)
                    {
                        retCode = near_exit_PurchaseModel.Code;
                        retId = near_exit_PurchaseModel.Id;
                        var exit_taskModel = _workFlowTaskRepository.GetAll().SingleOrDefault(r => r.InstanceID == near_exit_PurchaseModel.Code);
                        ret.FlowId = exit_taskModel.FlowID;
                        ret.GroupId = exit_taskModel.GroupID;
                        ret.InStanceId = exit_taskModel.InstanceID;
                        ret.StepId = exit_taskModel.StepID;
                        ret.StepName = exit_taskModel.StepName;
                        ret.TaskId = exit_taskModel.Id;
                    }
                    else
                    {
                        var entity_pMainId = Guid.NewGuid();
                        var currentCount = exit_PurchaseModels.Count() + 1;
                        var current_Code = "";
                        if (currentCount < 10)
                            current_Code = purchaseCode + $"00{currentCount}";
                        else if (currentCount < 99)
                            current_Code = purchaseCode + $"0{currentCount}";
                        ret = CreateSupplyPruchasePlanInstance(new CreateSupplyPruchasePlanInstanceInput() { Id = entity_pMainId, Code = current_Code, FlowId = input.FlowId, FlowTitle = input.FlowTitle });
                        retCode = current_Code;
                        retId = entity_pMainId;
                    }
                }
                var planModels = _supplyPurchasePlaneRepository.GetAll().Where(r => input.PlanId.Contains(r.Id));

                foreach (var item in planModels)
                {

                    item.SupplyPurchaseCode = retCode;
                    item.SupplyPurchaseId = retId;
                    item.DoPurchaseStatus = (int)SupplyPurchaseQOneDStatus.待审批;
                    if (item.SupplyApplySubId.HasValue)
                    {
                        var applySubMdeol = _supplyApplySubRepository.Get(item.SupplyApplySubId.Value);
                        applySubMdeol.Status = (int)SupplyApplySubStatusType.已提交采购计划;
                    }
                }
            }
            return ret;

        }



        /// <summary>
        /// 加入采购计划时，创建采购流程实例
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput CreateSupplyPruchasePlanInstance(CreateSupplyPruchasePlanInstanceInput input)
        {
            var entity_pMain = new SupplyPurchaseMain()
            {
                Id = input.Id,
                Code = input.Code,
            };
            _supplyPurchaseMainRepository.Insert(entity_pMain);
            var ret = _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput() { FlowId = input.FlowId, FlowTitle = input.FlowTitle, InStanceId = input.Code });
            return ret;
        }

        /// <summary>
        /// 采购是否存在申领
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public bool IsSupplyPruchaseExitApply(string code)
        {
            var model = _supplyPurchaseMainRepository.GetAll().SingleOrDefault(r => r.Code == code);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "编码获取采购数据失败");
            var flag = _supplyPurchasePlaneRepository.GetAll().Any(r => r.SupplyPurchaseId == model.Id && r.SupplyApplySubId.HasValue);
            return flag;
        }


        /// <summary>
        /// 获取采购里的用品申领人员
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public string FindSupplyPruchaseRecipientsUsers(string code)
        {
            var query = from a in _supplyPurchaseMainRepository.GetAll()
                        join b in _supplyPurchasePlaneRepository.GetAll() on a.Id equals b.SupplyPurchaseId
                        join c in _supplyApplyMainRepository.GetAll() on b.SupplyApplyMainId equals c.Id
                        where a.Code == code && b.SupplyApplySubId.HasValue && b.SupplyApplyMainId.HasValue
                        select c.CreatorUserId;
            var users = query.ToList().Select(r => { return $"u_{r}"; });
            return string.Join(',', users);
        }


        /// <summary>
        /// 获取采购计划列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplyPurchasePlansListOutput>> GetSupplyPurchasePlans(GetSupplyPurchasePlansInput input)
        {
            var query = from a in _supplyPurchasePlaneRepository.GetAll()
                            //join r in _supplyPurchaseResultRepository.GetAll() on a.Id equals r.SupplyPurchasePlanId into h
                            //let rr = h.FirstOrDefault()
                        join b in _supplyApplyMainRepository.GetAll() on a.SupplyApplyMainId equals b.Id into g
                        from supply in g.DefaultIfEmpty()
                        where a.DoPurchaseStatus > (int)SupplyPurchaseQOneDStatus.未处理
                        select new SupplyPurchasePlansListOutput
                        {
                            ApplyUserName = supply != null ? (from u in UserManager.Users
                                                              where u.Id == supply.CreatorUserId.Value
                                                              select u.Name).SingleOrDefault() :
                                                              (from u in UserManager.Users
                                                               where u.Id == a.CreatorUserId.Value
                                                               select u.Name).SingleOrDefault(),
                            SupplyApply = supply,
                            //SupplyApplyApplyName = supply != null ? (from u in UserManager.Users
                            //                                         where u.Id == supply.CreatorUserId.Value
                            //                                         select u.Name).SingleOrDefault() : "",
                            SupplyApplyTime = supply != null ? supply.CreationTime.ToString() : "",
                            Des = a.Des,
                            DoPurchaseStatus = a.DoPurchaseStatus,
                            Name = a.Name,
                            GetTime = a.GetTime,
                            Id = a.Id,
                            DoPurchaseStatusTitle = "",
                            Money = a.Money,
                            Number = a.Number,
                            SupplyApplyMainId = a.SupplyApplyMainId,
                            SupplyApplySubId = a.SupplyApplySubId,
                            SupplyPurchaseCode = a.SupplyPurchaseCode,
                            SupplyPurchaseId = a.SupplyPurchaseId,
                            Type = a.Type,
                            TypeName = "",
                            Unit = a.Unit,
                            UserId = a.UserId,
                            User_Name = "",
                            Version = a.Version,
                            PutInData = a.PutInDate == null ? "" : a.PutInDate.Value.ToString(),
                            CreationTime = a.CreationTime,
                        };

            if (input.ActionType == 1)
            {
                query = query.Where(r => r.DoPurchaseStatus == (int)SupplyPurchaseQOneDStatus.已加入采购计划);
            }
            else if (input.ActionType == 2)
            {
                query = query.Where(r => r.DoPurchaseStatus == (int)SupplyPurchaseQOneDStatus.已入库);
            }
            else
            {
                query = query.Where(r => r.DoPurchaseStatus > (int)SupplyPurchaseQOneDStatus.待审批);
            }
            var count = await query.CountAsync();
            var data = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            foreach (var item in data)
            {
                item.TypeName = ((SupplyType)item.Type).ToString();
                item.User_Name = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                item.DoPurchaseStatusTitle = ((SupplyPurchaseQOneDStatus)item.DoPurchaseStatus).ToString();
            }

            return new PagedResultDto<SupplyPurchasePlansListOutput>(count, data);
        }

        /// <summary>
        /// 编辑采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateSupplyPurchasePlan(UpdateSupplyPurchasePlanInput input)
        {
            if (!input.Id.HasValue)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "参数错误");
            var entity = await _supplyPurchasePlaneRepository.GetAsync(input.Id.Value);
            entity.Des = input.Des;
            entity.GetTime = input.GetTime;
            entity.Money = input.Money;
            entity.Name = input.Name;
            entity.Number = input.Number;
            entity.Unit = input.Unit;
            entity.UserId = input.UserId;
            entity.Version = input.Version;
            entity.Type = input.Type;

        }

        /// <summary>
        /// 领导审批采购清单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdaterPurchasePlanStatus(List<UpdaterPurchasePlanStatusInput> input)
        {
            foreach (var item in input)
            {
                var model = await _supplyPurchasePlaneRepository.GetAsync(item.PlanId);
                model.DoPurchaseStatus = item.ActionType == 1 ? (int)SupplyPurchaseQOneDStatus.同意 : (int)SupplyPurchaseQOneDStatus.驳回;
                if (model.SupplyApplySubId.HasValue)
                {
                    var applySubMdeol = _supplyApplySubRepository.Get(model.SupplyApplySubId.Value);
                    applySubMdeol.Status = item.ActionType == 1 ? (int)SupplyApplySubStatusType.采购申请同意 : (int)SupplyApplySubStatusType.采购申请驳回;
                }

            }

        }
        /// <summary>
        /// 采购用品领用
        /// </summary>
        /// <param name="input">采购编码</param>
        /// <returns></returns>
        public async Task<List<SupplyApplyDto>> GetPlansForRecipients(EntityDto<string> input)
        {
            var purchaseModel = await _supplyPurchaseMainRepository.FirstOrDefaultAsync(r => r.Code == input.Id);
            if (purchaseModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数错误");
            var planModels = from a in _supplyPurchasePlaneRepository.GetAll()
                             join r in _supplyPurchaseResultRepository.GetAll() on a.Id equals r.SupplyPurchasePlanId into result
                             join b in _supplyApplyMainRepository.GetAll() on a.SupplyApplyMainId.Value equals b.Id
                             where a.SupplyPurchaseId == purchaseModel.Id && a.SupplyApplyMainId.HasValue && b.CreatorUserId == AbpSession.UserId.Value
                             select new { a, result };


            var supplyApplyQuery = from a in _supplyApplyMainRepository.GetAll()
                                   join b in _supplyPurchasePlaneRepository.GetAll() on a.Id equals b.SupplyApplyMainId.Value
                                   join c in _supplyApplySubRepository.GetAll() on new { MainId = a.Id, SubId = b.SupplyApplySubId.Value } equals new { MainId = c.MainId, SubId = c.Id } into g
                                   where b.SupplyPurchaseId == purchaseModel.Id && b.SupplyApplyMainId.HasValue && b.SupplyApplySubId.HasValue && a.CreatorUserId == AbpSession.UserId.Value
                                   select new { a, sub = g };
            var ret = new List<SupplyApplyDto>();
            foreach (var item in supplyApplyQuery)
            {
                var entity = new SupplyApplyDto();
                entity.Id = item.a.Id;
                entity.IsImportant = item.a.IsImportant;
                foreach (var subItem in item.sub)
                {
                    var subEntity = new SupplyApplySubDto()
                    {
                        Des = subItem.Des,
                        Id = subItem.Id,
                        GetTime = subItem.GetTime,
                        MainId = subItem.MainId,
                        Money = subItem.Money,
                        Name = subItem.Name,
                        Number = subItem.Number,
                        Result = subItem.Result,
                        ResultRemark = subItem.ResultRemark,
                        SupplyId = subItem.SupplyId,
                        Type = subItem.Type,
                        UserId = subItem.UserId,
                        Unit = subItem.Unit,
                        Version = subItem.Version,
                        Result_Name = ((SupplyApplySubResultType)subItem.Result).ToString(),
                        UserId_Name = _workFlowOrganizationUnitsManager.GetNames(subItem.UserId),
                        TypeName = ((SupplyType)subItem.Type).ToString(),
                        Status = subItem.Status,
                        StatusTitle = ((SupplyApplySubStatusType)subItem.Status).ToString(),
                        ApplyDateTime = subItem.CreationTime,

                    };
                    var results_Query = from r in _supplyApplyResultRepository.GetAll()
                                        join s in _supplyBaseRepository.GetAll() on r.SupplyId equals s.Id
                                        where r.ApplySubId == subItem.Id
                                        select new SupplyApplyResultDto
                                        {
                                            Id = r.Id,
                                            ApplyMainId = subItem.MainId,
                                            ApplySubId = subItem.Id,
                                            SupplyCode = s.Code,
                                            SupplyId = s.Id,
                                            SupplyMoney = s.Money,
                                            SupplyName = s.Name,
                                            SupplyVersion = s.Version,

                                        };
                    foreach (var entity_result in results_Query)
                    {

                        subEntity.SupplyApplyResult.Add(entity_result);

                    }

                    entity.SupplyApplySub.Add(subEntity);
                }
                ret.Add(entity);
            }


            return ret;
        }



        /// <summary>
        /// 申领是否处理完成
        /// </summary>
        /// <returns></returns>
        public bool IsComplateApply(Guid mainId)
        {
            var subApplyModels = _supplyApplySubRepository.GetAll().Where(r => r.MainId == mainId);
            if (subApplyModels.Count() == 0)
                return true;
            else
            {
                if (subApplyModels.Any(r => r.Status == (int)SupplyApplySubStatusType.处理中))
                    return false;
                if (subApplyModels.Any(r => r.Status == (int)SupplyApplySubStatusType.已加入采购计划))
                    return false;
                if (subApplyModels.Any(r => r.Status == (int)SupplyApplySubStatusType.已提交采购计划))
                    return false;
                if (subApplyModels.Any(r => r.Status == (int)SupplyApplySubStatusType.申购中))
                    return false;
                if (subApplyModels.Any(r => r.Status == (int)SupplyApplySubStatusType.采购入库))
                    return false;
                if (subApplyModels.Any(r => r.Status == (int)SupplyApplySubStatusType.采购申请同意))
                    return false;
                if (subApplyModels.Any(r => r.Status == (int)SupplyApplySubStatusType.已发放))
                    return false;

            }
            return true;
        }


        /// <summary>
        /// 采购是否入库完
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public ExecuteWorkFlowOutput IsComplatePutin(string instanceId)
        {
            var plans = _supplyPurchasePlaneRepository.GetAll().Any(r => r.SupplyPurchaseCode == instanceId && r.DoPurchaseStatus != (int)SupplyPurchaseQOneDStatus.已入库 && r.DoPurchaseStatus
            != (int)SupplyPurchaseQOneDStatus.驳回);
            var ret = new ExecuteWorkFlowOutput();
            ret.IsSuccesefull = !plans;
            if (!ret.IsSuccesefull)
                ret.ErrorMsg = "采购计划暂未入库完成";
            return ret;
        }


        /// <summary>
        /// 采购计划-领导审核是否完成
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public ExecuteWorkFlowOutput IsComplateAuditByZJL(string instanceId)
        {
            var plans = _supplyPurchasePlaneRepository.GetAll().Any(r => r.SupplyPurchaseCode == instanceId && r.DoPurchaseStatus == (int)SupplyPurchaseQOneDStatus.待审批);
            var ret = new ExecuteWorkFlowOutput();
            ret.IsSuccesefull = !plans;
            if (!ret.IsSuccesefull)
                ret.ErrorMsg = "采购计划暂未审批完成";
            return ret;
        }





    }
}
