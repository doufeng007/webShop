using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
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
using Abp.UI;
using Abp.Authorization;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Extensions;
using ZCYX.FRMSCore.Model;

namespace Supply.Service.SupplyBack
{
    [AbpAuthorize]
    public class SupplyBackAppService : ApplicationService, ISupplyBackAppService
    {
        private readonly IRepository<SupplyBackMain, Guid> _supplyBackMainRepository;
        private readonly IRepository<SupplyBackSub, Guid> _supplyBackSubRepository;
        private readonly ISupplyBaseRepository _supplyBaseRepository;
        private readonly IRepository<UserSupply, Guid> _userSupplyRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<WorkFlowTask, Guid> _workflowtaskRepository;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository2;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;

        public SupplyBackAppService(IRepository<SupplyBackMain, Guid> supplyBackMainRepository, IRepository<User, long> userRepository, IRepository<UserSupply, Guid> userSupplyRepository,
            IRepository<SupplyBackSub, Guid> supplyBackSubRepository, ISupplyBaseRepository supplyBaseRepository, IRepository<WorkFlowTask, Guid> workflowtaskRepository
            , IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, IWorkFlowTaskRepository workFlowTaskRepository2, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _userRepository = userRepository;
            _supplyBackMainRepository = supplyBackMainRepository;
            _userSupplyRepository = userSupplyRepository;
            _workflowtaskRepository = workflowtaskRepository;
            _supplyBackSubRepository = supplyBackSubRepository;
            _supplyBaseRepository = supplyBaseRepository;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _workFlowTaskRepository2 = workFlowTaskRepository2;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;

        }
        /// <summary>
        /// 申请退还
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(CreateSupplyBackMainInput input)
        {
            var model = new SupplyBackMain();
            model.Id = Guid.NewGuid();
            if (input.SupplyBackSub != null)
            {
                foreach (var x in input.SupplyBackSub)
                {
                    var t = new SupplyBackSub();
                    t.MainId = model.Id;
                    t.SupplyId = x;
                    t.Id = Guid.NewGuid();
                    _supplyBackSubRepository.Insert(t);
                    var s = _supplyBaseRepository.Get(x);
                    s.Status = (int)SupplyStatus.退还中;
                    _supplyBaseRepository.Update(s);
                    var u = _userSupplyRepository.GetAll().FirstOrDefault(ite => ite.Status == (int)UserSupplyStatus.使用中 && ite.SupplyId == x);
                    if (u != null)
                    {
                        u.Status = (int)UserSupplyStatus.退还中;
                        _userSupplyRepository.Update(u);
                    }

                }
            }
            _supplyBackMainRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SupplyBackMainDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = new SupplyBackMainDto();
            var model = _supplyBackMainRepository.Get(id);
            if (model != null)
            {
                ret.Status = model.Status;
                var sub = from a in _supplyBackSubRepository.GetAll()
                          join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                          where a.MainId == id
                          select new SupplyBackSubDto()
                          {
                              EndTime = a.CreationTime,
                              Id = a.Id,
                              MainId = a.MainId,
                              StartTime = b.CreationTime,
                              Status = a.Status,
                              Code = b.Code,
                              Money = b.Money,
                              Name = b.Name,
                              SupplyId = b.Id,
                              Type = b.Type,
                              UserId = a.CreatorUserId.Value,
                              UserId_Name = "",
                              Version = b.Version
                          };
                ret.SupplyBackSub = sub.ToList();
            }
            return ret;
        }

        /// <summary>
        /// 获取我申请的退还记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplyBackMainDto>> GetAll(GetListInput input) {

            var query = from a in _supplyBackMainRepository.GetAll()
                        join b in _userRepository.GetAll() on a.CreatorUserId equals b.Id
                        let openModel = (from c in _workFlowTaskRepository2.GetAll().Where(x =>
                          x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                          x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                       
                        //join sub in _supplyBackSubRepository.GetAll() on a.Id equals sub.MainId into subs
                        where a.CreatorUserId == AbpSession.UserId.Value
                        //SupplyApplyListDto
                        select new 
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            UserId = a.CreatorUserId.Value,
                            Status = a.Status,
                            // SupplyBackSub=subs,
                            UserId_Name=b.Name,
                            
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                        ? 1
                                        : 2
                        };
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var model = await query.OrderByDescending(ite => ite.CreationTime).OrderBy(ite => ite.OpenModel).PageBy(input).ToListAsync();

            var ret = new List<SupplyBackMainDto>();
            foreach (var r in model)
            {
                var item = new SupplyBackMainDto()
                {
                    Id = r.Id,
                     
                    UserId_Name  = r.UserId_Name,
                    CreationTime = r.CreationTime,
                    UserId = r.UserId,
                    Status = r.Status,
                    OpenModel = r.OpenModel,
                    
                };
                item.InstanceId = r.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
                var querysub =( from a in _supplyBackSubRepository.GetAll()
                            join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                            join u in _userRepository.GetAll() on a.CreatorUserId equals u.Id
                            where a.MainId==r.Id
                            select new SupplyBackSubDto()
                            {
                                Code = b.Code,
                                EndTime = a.LastModificationTime,
                                Id = a.Id,
                                MainId = a.MainId,
                                Money = b.Money,
                                Name = b.Name,
                                StartTime = b.CreationTime,
                                Status = a.Status,
                                SupplyId = a.SupplyId,
                                Type = b.Type,
                                TypeName = ((SupplyType)b.Type).ToString(),
                                UserId = a.CreatorUserId.Value,
                                UserId_Name = u.Name,
                                Version = b.Version,
                                StatusTitle = ((SupplyBackSubStatus)a.Status).ToString()
                            }).ToList();
                item.SupplyBackSub = querysub;
                ret.Add(item);
            }
            return new PagedResultDto<SupplyBackMainDto>(count, ret);

        }
        public async Task<PagedResultDto<SupplyBackSubDto>> GetSub(PageSubDto input)
        {
            var query = from a in _supplyBackSubRepository.GetAll()
                        join b in _supplyBaseRepository.GetAll() on a.SupplyId equals b.Id
                        join u in _userRepository.GetAll() on a.CreatorUserId equals u.Id

                        select new SupplyBackSubDto()
                        {
                            Code = b.Code,
                            EndTime = a.CreationTime,
                            Id = a.Id,
                            MainId = a.MainId,
                            Money = b.Money,
                            Name = b.Name,
                            StartTime = b.CreationTime,
                            Status = a.Status,
                            SupplyId = a.SupplyId,
                            Type = b.Type,
                            TypeName = ((SupplyType)b.Type).ToString(),
                            UserId = a.CreatorUserId.Value,
                            UserId_Name = u.Name,
                            Version = b.Version,
                            StatusTitle = ((SupplyBackSubStatus)a.Status).ToString()
                        };

          
            var count = await query.CountAsync();
            var model = await query.OrderByDescending(ite=>ite.EndTime).OrderBy(ite => ite.Status).PageBy(input).ToListAsync();
            return new PagedResultDto<SupplyBackSubDto>(count, model);
        }

        [UnitOfWork]
        public async Task Sure(SupplyBackInput input)
        {
            var model = _supplyBackSubRepository.Get(input.Id);
            if (model.Status == 1)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该退还申请已处理。");
            }
            switch (input.Type)
            {
                case 0:
                    model.Status =  (int)SupplyBackSubStatus.已退还;
                    break;
                case 1:
                    model.Status = (int)SupplyBackSubStatus.驳回;
                    break;
            }
            _supplyBackSubRepository.Update(model);

            var supply = _supplyBaseRepository.Get(model.SupplyId);
            if (supply == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "要退还的物品不存在。");
            }
            switch (input.Type)
            {
                case 0:
                    supply.Status = (int)SupplyStatus.在库;
                    supply.UserId = "";
                    supply.PutInDate = DateTime.Now;
                    break;
                case 1:
                    supply.Status = (int)SupplyStatus.被领用;
                    break;
            }
            
            _supplyBaseRepository.Update(supply);
            var u = _userSupplyRepository.GetAll().FirstOrDefault(ite => ite.Status == (int)UserSupplyStatus.退还中 && ite.SupplyId == model.SupplyId);
            if (u != null)
            {
                switch (input.Type)
                {
                    case 0:
                        u.Status = (int)UserSupplyStatus.已退还;
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

            var task = _workflowtaskRepository.GetAll().OrderByDescending(ite => ite.SenderTime).FirstOrDefault(ite => ite.InstanceID == model.MainId.ToString() && ite.ReceiveID==AbpSession.UserId.Value);
            if (task == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前待办任务。");
            }
            var hasnoback = _supplyBackSubRepository.GetAll().Count(ite => ite.MainId == model.MainId && ite.Status == 0);
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

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
