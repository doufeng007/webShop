using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Extensions;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.File;
using Abp.UI;
using Supply.Service;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;
using Abp.Application.Services;
using Abp.Domain.Uow;
using CWGL;
using ZCYX.FRMSCore.Extensions;

namespace Supply
{
    public class SupplyScrapAppService : FRMSCoreAppServiceBase, ISupplyScrapAppService
    {
        private readonly IRepository<SupplyScrapMain, Guid> _supplyScrapMainRepository;
        private readonly IRepository<SupplyScrapSub, Guid> _supplyScrapSubRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly ISupplyBaseRepository _supplyBaseRepository;
        private readonly IRepository<UserSupply, Guid> _userSupplyRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<WorkFlowTask, Guid> _workflowtaskRepository;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository2;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;

        public SupplyScrapAppService(IRepository<SupplyScrapMain, Guid> supplyScrapMainRepository, IRepository<SupplyScrapSub, Guid> supplyScrapSubRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IRepository<User, long> userRepository, IAbpFileRelationAppService abpFileRelationAppService
            , ISupplyBaseRepository supplyBaseRepository, WorkFlowTaskManager workFlowTaskManager, IRepository<UserSupply, Guid> userSupplyRepository, IRepository<WorkFlowTask, Guid> workflowtaskRepository
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, IWorkFlowTaskRepository workFlowTaskRepository2
            , IWorkFlowTaskRepository workFlowTaskRepository, IUnitOfWorkManager unitOfWorkManager
            , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager)
        {
            _supplyScrapMainRepository = supplyScrapMainRepository;
            _supplyScrapSubRepository = supplyScrapSubRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _supplyBaseRepository = supplyBaseRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _userSupplyRepository = userSupplyRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _userRepository = userRepository;
            _workflowtaskRepository = workflowtaskRepository;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _workFlowTaskRepository2 = workFlowTaskRepository2;
            _workFlowTaskRepository = workFlowTaskRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
        }

        public List<InitWorkFlowOutput> Create(CreateSupplyScrapMainInput input)
        {
            var ret = new List<InitWorkFlowOutput>();

            foreach (var x in input.SupplyScrapSub)
            {
                var data = CreateV2(new CreateSupplyScrapMainInput() { FlowId = input.FlowId, FlowTitle = input.FlowTitle, SupplyScrapSub = new List<CreateSupplyScrapSubInput>() { x } });
                var entity = _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput() { FlowId = input.FlowId, FlowTitle = input.FlowTitle, InStanceId = data.InStanceId });
                ret.Add(entity);
            }
            return ret;
        }


        /// <summary>
        /// 申请报废
        /// </summary>
        /// <param name="input">报废物品id列表</param>
        /// <returns></returns>
        public InitWorkFlowOutput CreateV2(CreateSupplyScrapMainInput input)
        {
            var x = input.SupplyScrapSub.FirstOrDefault();
            var t = new SupplyScrapSub();
            //t.MainId = model.Id;
            t.SupplyId = x.SupplyId;
            t.UserSupplyId = x.UserSupplyId;
            t.Reason = x.Reason;
            t.Id = Guid.NewGuid();
            _supplyScrapSubRepository.Insert(t);
            var s = _supplyBaseRepository.Get(x.SupplyId);
            s.Status = (int)SupplyStatus.报废中;
            _supplyBaseRepository.Update(s);
            var u = _userSupplyRepository.Get(x.UserSupplyId);
            u.Status = (int)UserSupplyStatus.报废中;
            return new InitWorkFlowOutput() { InStanceId = t.Id.ToString() };
        }



        /// <summary>
        /// 报废管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplyScrapSubDto>> GetList(SupplyScrapListInput input)
        {


            var query = from a in _supplyScrapSubRepository.GetAll()
                        join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                        join u in _userRepository.GetAll() on a.CreatorUserId equals u.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                        x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new SupplyScrapSubDto()
                        {
                            Code = b.Code,
                            CreationTime = a.CreationTime,
                            ExpiryDate = b.ExpiryDate,
                            ProductDate = b.CreationTime,
                            Id = a.Id,
                            MainId = a.MainId,
                            Money = b.Money,
                            Name = b.Name,
                            Status = a.Status,
                            SupplyId = a.SupplyId,
                            Type = b.Type,
                            TypeName = ((SupplyType)b.Type).ToString(),
                            UserId = a.CreatorUserId.Value,
                            UserId_Name = u.Name,
                            Version = b.Version,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2,
                            Reason = a.Reason,
                        };

            if (!string.IsNullOrWhiteSpace(input.Status))
            {
                var statusArrty = input.Status.Split(",");
                query = query.Where(r => statusArrty.Contains(r.Status.ToString()));
            }

            var count = await query.CountAsync();
            var model = await query.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();
            var list = new List<SupplyScrapSubDto>();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                if (model.Count > 0)
                {
                    foreach (var tmp in model)
                    {
                        var firstTask = new FirstTaskModelScrap();
                        tmp.InstanceId = tmp.Id.ToString();
                        _workFlowBusinessTaskManager.SupplementWorkFlowBusinessListForSupplyScrap(input.FlowId, tmp as BusinessWorkFlowListOutput, out firstTask);
                        tmp.FirstTaskId = firstTask.TaskId;
                        tmp.FirstGroupId = firstTask.GroupId;
                        tmp.FirstStepId = firstTask.StepId;

                        list.Add(tmp);
                    }
                }
            }
            return new PagedResultDto<SupplyScrapSubDto>(count, list);
        }



        /// <summary>
        /// 提交报废
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SubmitSupplyScrap(List<SubmitSupplyScrapInput> input)
        {
            if (input.Count == 0)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数错误");
            var model = new SupplyScrapMain()
            {
                Id = Guid.NewGuid(),
                Status = 0,
            };

            var items = _supplyScrapSubRepository.GetAll().Where(r => input.Select(o => o.SubId).Contains(r.Id));
            foreach (var item in items)
            {
                item.MainId = model.Id;
                item.PreResidueValue = input.FirstOrDefault(r => r.SubId == item.Id).PreValue;
                _supplyScrapSubRepository.Update(item);
            }

            await _supplyScrapMainRepository.InsertAsync(model);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public GetSupplyScrapMainDto GetMain(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = new GetSupplyScrapMainDto();
            var model = _supplyScrapMainRepository.Get(id);
            ret.Id = model.Id;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var sub = from a in _supplyScrapSubRepository.GetAll()
                          join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                          join c in _userSupplyRepository.GetAll() on a.UserSupplyId equals c.Id
                          join cu in UserManager.Users on c.UserId equals cu.Id
                          where a.MainId == id
                          select new SupplyScrapSubDto()
                          {
                              CreationTime = a.CreationTime,
                              ExpiryDate = b.ExpiryDate,
                              ProductDate = b.ProductDate,
                              Id = a.Id,
                              MainId = a.MainId,
                              Status = a.Status,
                              Code = b.Code,
                              Money = b.Money,
                              Name = b.Name,
                              SupplyId = b.Id,
                              Type = b.Type,
                              UserId = a.CreatorUserId.Value,
                              UserId_Name = "",
                              Version = b.Version,
                              Reason = a.Reason,
                              PreResidueValue = a.PreResidueValue,
                              UserSupply_UserId = c.UserId,
                              UserSupply_UserName = cu.Name
                          };
                ret.SupplyScrapSub = sub.ToList();
            }

            return ret;
        }






        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]

        public GetSupplyScrapSubDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var sub = from a in _supplyScrapSubRepository.GetAll()
                      join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                      where a.Id == id
                      select new GetSupplyScrapSubDto()
                      {
                          CreationTime = a.CreationTime,
                          ExpiryDate = b.ExpiryDate,
                          ProductDate = b.ProductDate,
                          Id = a.Id,
                          MainId = a.MainId,
                          Code = b.Code,
                          Money = b.Money,
                          Name = b.Name,
                          SupplyId = b.Id,
                          Type = b.Type,
                          UserId = a.CreatorUserId.Value,
                          UserId_Name = "",
                          Version = b.Version,
                          Reason = a.Reason
                      };
            return sub.FirstOrDefault();
        }
        /// <summary>
        /// 行政获取报废申请列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplyScrapSubDto>> GetSub(PageSubDto input)
        {
            var strflowid = input.FlowId.ToString();
            var query = from a in _supplyScrapSubRepository.GetAll()
                        join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                        join u in _userRepository.GetAll() on a.CreatorUserId equals u.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                             x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                             x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                         select c).Any()
                        select new SupplyScrapSubDto()
                        {
                            Code = b.Code,
                            CreationTime = a.CreationTime,
                            ExpiryDate = b.ExpiryDate,
                            ProductDate = b.CreationTime,
                            Id = a.Id,
                            MainId = a.MainId,
                            Money = b.Money,
                            Name = b.Name,
                            Status = a.Status,
                            SupplyId = a.SupplyId,
                            Type = b.Type,
                            TypeName = ((SupplyType)b.Type).ToString(),
                            UserId = a.CreatorUserId.Value,
                            UserId_Name = u.Name,
                            Version = b.Version,
                            Reason = a.Reason,
                            OpenModel = openModel ? 1 : 2,
                        };

            var count = await query.CountAsync();
            var model = await query.OrderBy(ite => ite.OpenModel).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();
            var retdata = new List<SupplyScrapSubDto>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as SupplyScrapSubDto);
                retdata.Add(item);

            }
            return new PagedResultDto<SupplyScrapSubDto>(count, retdata);
        }

        public async Task Sure(SupplyScrapInput input)
        {
            var model = _supplyScrapSubRepository.Get(input.Id);
            if (model.Status == 1)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该报废申请已处理。");
            }
            switch (input.Type)
            {
                case 0:
                    model.Status = (int)SupplyScrapSubStatus.已报废;
                    break;
                case 1:
                    model.Status = (int)SupplyScrapSubStatus.驳回;
                    break;
            }
            _supplyScrapSubRepository.Update(model);

            var supply = _supplyBaseRepository.Get(model.SupplyId);
            if (supply == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "要报废的物品不存在。");
            }
            switch (input.Type)
            {
                case 0:
                    supply.Status = (int)SupplyStatus.已报废;
                    break;
                case 1:
                    supply.Status = (int)SupplyStatus.被领用;
                    break;
            }
            _supplyBaseRepository.Update(supply);
            var u = _userSupplyRepository.GetAll().FirstOrDefault(ite => ite.Status == (int)UserSupplyStatus.报废中 && ite.SupplyId == model.SupplyId);
            if (u != null)
            {
                switch (input.Type)
                {
                    case 0:
                        u.Status = (int)UserSupplyStatus.已报废;
                        break;
                    case 1:
                        u.Status = (int)UserSupplyStatus.使用中;
                        break;
                }
                _userSupplyRepository.Update(u);
            }

            /*
            判断当前申请的列表是否都已处理完毕，是则调用流程结束方法
            */

            var task = _workflowtaskRepository.GetAll().OrderByDescending(ite => ite.SenderTime).FirstOrDefault(ite => ite.InstanceID == model.MainId.ToString() && ite.ReceiveID == AbpSession.UserId.Value);
            if (task == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前待办任务。");
            }
            var hasnoback = _supplyScrapSubRepository.GetAll().Count(ite => ite.MainId == model.MainId && ite.Status == 0);
            if (hasnoback == 1)
            {
                // var _taskAppService=  Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
                await _workFlowWorkTaskAppService.ExecuteTask(new ExecuteWorkFlowInput()
                {
                    ActionType = "completed",
                    FlowId = task.FlowID,
                    GroupId = task.GroupID,
                    StepId = task.StepID,
                    TaskId = task.Id,
                    Title = "退还申请已处理"
                });
            }
        }
        /// <summary>
        /// 获取我申请的报废记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplyScrapMainDto>> GetAll(GetListInput input)
        {

            var query = from a in _supplyScrapMainRepository.GetAll()
                        join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                        let openModel = (from c in _workFlowTaskRepository2.GetAll().Where(x =>
                          x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                          x.ReceiveID == AbpSession.UserId.Value)
                                         select c)

                        //join sub in _supplyScrapSubRepository.GetAll() on a.Id equals sub.MainId into subs
                        where a.CreatorUserId == AbpSession.UserId.Value
                        select new
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            UserId = a.CreatorUserId.Value,
                            Status = a.Status,
                            //SupplyBackSub = subs,
                            UserId_Name = b.Name,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                        ? 1
                                        : 2
                        };
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var model = await query.OrderByDescending(ite => ite.CreationTime).OrderBy(ite => ite.OpenModel).PageBy(input).ToListAsync();

            var ret = new List<SupplyScrapMainDto>();
            foreach (var r in model)
            {
                var item = new SupplyScrapMainDto()
                {
                    Id = r.Id,

                    UserId_Name = r.UserId_Name,
                    CreationTime = r.CreationTime,
                    UserId = r.UserId,
                    Status = r.Status,
                    OpenModel = r.OpenModel,

                };
                item.InstanceId = r.Id.ToString();
                item.SupplyScrapSub = new List<SupplyScrapSubDto>();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
                var querysub = (from a in _supplyScrapSubRepository.GetAll()
                                join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                                join u in _userRepository.GetAll() on a.CreatorUserId equals u.Id
                                where a.MainId == r.Id
                                select new SupplyScrapSubDto()
                                {
                                    Code = b.Code,
                                    CreationTime = a.CreationTime,
                                    ExpiryDate = b.ExpiryDate,
                                    ProductDate = b.CreationTime,
                                    Id = a.Id,
                                    MainId = a.MainId,
                                    Money = b.Money,
                                    Name = b.Name,

                                    Status = a.Status,
                                    SupplyId = a.SupplyId,
                                    Type = b.Type,
                                    TypeName = ((SupplyType)b.Type).ToString(),
                                    UserId = a.CreatorUserId.Value,
                                    UserId_Name = u.Name,
                                    Version = b.Version,
                                    StatusTitle = ((SupplyScrapSubStatus)a.Status).ToString(),
                                    Reason = a.Reason,
                                }).ToList();

                item.SupplyScrapSub = querysub;
                ret.Add(item);
            }
            return new PagedResultDto<SupplyScrapMainDto>(count, ret);

        }


        public async Task Update(SupplyScrapUpdateInput input)
        {
            var query = from a in _supplyScrapMainRepository.GetAll()
                        join b in _supplyScrapSubRepository.GetAll() on a.Id equals b.MainId
                        join c in _supplyBaseRepository.GetAll() on b.SupplyId equals c.Id
                        where a.Id == input.Id
                        select new { b, c };
            if (query.Count() != input.SubList.Count())
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请指定报废处置结果");
            var data = await query.ToListAsync();
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICW_PersonalTodoAppService>();
            var first_User = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("XZRY").FirstOrDefault();
            if (first_User == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到行政人员");
            foreach (var item in input.SubList)
            {
                var entity = data.FirstOrDefault(r => r.b.Id == item.SubId);
                if (entity == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "参数错误");
                if (item.CWType == CWGL.Enums.RefundResultType.无退无报)
                    continue;
                await service.Create(new CreateCW_PersonalTodoInput()
                {
                    BusinessId = item.SubId.ToString(),
                    BusinessType = CW_PersonalType.用品报废,
                    CWType = item.CWType,
                    FlowId = input.FlowId,
                    Amout_Gather = item.Amout_Gather,
                    Amout_Pay = item.Amout_Pay,
                    Status = CW_PersonalToStatus.未处理,
                    Title = $"用品：{entity.c.Code}-报废",
                    UserId = first_User.Id

                });
            }
        }





        [RemoteService(IsEnabled = false)]
        public string CreateSubFlowSupplyScrapInstance(string instanceId)
        {
            var id = instanceId.ToGuid();
            var model = _supplyScrapSubRepository.Get(id);
            var mainId = model.MainId;
            if (!mainId.HasValue)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "创建合并子流程实例失败");
            return mainId.Value.ToString();
        }

        [RemoteService(IsEnabled = false)]
        public void AfterScrapAction(string instanceId, Guid flowId)
        {
            var id = instanceId.ToGuid();
            var model = _supplyScrapMainRepository.Get(id);
            var subModels = _supplyScrapSubRepository.GetAll().Where(r => r.MainId == model.Id).ToList();
            var supplyIds = subModels.Select(r => r.SupplyId).ToList();
            var supplyModels = _supplyBaseRepository.GetAll().Where(r => supplyIds.Contains(r.Id));
            foreach (var item in supplyModels)
            {
                item.Status = (int)SupplyStatus.已报废;
            }


        }
    }
}

