using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.File;
using Abp.UI;
using Abp.WorkFlow;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Authorization.Users;
using System.Linq;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Application;
using CWGL;
using ZCYX.FRMSCore.Extensions;

namespace Supply.Service
{
    public class SupplyRepairAppService : ApplicationService, ISupplyRepairAppService
    {
        private readonly IRepository<SupplyRepair, Guid> _supplyRepairRepository;
        private readonly ISupplyBaseRepository _supplyBaseRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<User, long> _userAppService;
        private readonly IRepository<UserSupply, Guid> _userSupplyRepository;
        private readonly IRepository<WorkFlowTask, Guid> _workflowtaskRepository;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly IWorkFlowAppService _workFlowAppService;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<CW_PersonalTodo, Guid> _cW_PersonalTodoRepository;
        private readonly IRepository<SupplyScrapMain, Guid> _supplyScrapMainRepository;
        private readonly IRepository<SupplyScrapSub, Guid> _supplyScrapSubRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<SupplySupplier, Guid> _supplySupplierRepository;

        public SupplyRepairAppService(IRepository<SupplyRepair, Guid> supplyRepairRepository, IRepository<UserSupply, Guid> userSupplyRepository, IRepository<User, long> userAppService,
            IAbpFileRelationAppService abpFileRelationAppService, ISupplyBaseRepository supplyBaseRepository, IRepository<WorkFlowTask, Guid> workflowtaskRepository,
            IWorkFlowAppService workFlowAppService, WorkFlowBusinessTaskManager workFlowBusinessTaskManager,
            IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, IRepository<CW_PersonalTodo, Guid> cW_PersonalTodoRepository
            , IRepository<SupplyScrapMain, Guid> supplyScrapMainRepository, IRepository<SupplyScrapSub, Guid> supplyScrapSubRepository
            , IWorkFlowTaskRepository workFlowTaskRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , IRepository<SupplySupplier, Guid> supplySupplierRepository)
        {
            _workflowtaskRepository = workflowtaskRepository;
            _workFlowAppService = workFlowAppService;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _supplyRepairRepository = supplyRepairRepository;
            _supplyBaseRepository = supplyBaseRepository;
            _userSupplyRepository = userSupplyRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _userAppService = userAppService;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _cW_PersonalTodoRepository = cW_PersonalTodoRepository;
            _supplyScrapMainRepository = supplyScrapMainRepository;
            _supplyScrapSubRepository = supplyScrapSubRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _supplySupplierRepository = supplySupplierRepository;
        }
        /// <summary>
        /// 用品修好后，个人进入待办事项确认领取
        /// </summary>
        /// <param name="input">维修id</param>
        public async Task ApplyForFlow(Guid input)
        {
            var r = _supplyRepairRepository.Get(input);
            var u = _userSupplyRepository.GetAll().FirstOrDefault(ite => ite.SupplyId == r.SupplyId && ite.Status == (int)UserSupplyStatus.已修好);
            if (u != null)
            {
                u.Status = (int)UserSupplyStatus.使用中;
                _userSupplyRepository.Update(u);
            }
            var s = _supplyBaseRepository.Get(r.SupplyId);
            s.Status = (int)SupplyStatus.被领用;
            _supplyBaseRepository.Update(s);

            /*
            判断当前申请是否都已处理完毕，是则调用流程结束方法
            */

            var task = _workflowtaskRepository.GetAll().OrderByDescending(ite => ite.SenderTime).FirstOrDefault(ite => ite.InstanceID == input.ToString());
            if (task == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前待办任务。");
            }

            // var _taskAppService=  Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            await _workFlowWorkTaskAppService.ExecuteTask(new ExecuteWorkFlowInput()
            {
                ActionType = "completed",
                FlowId = task.FlowID,
                GroupId = task.GroupID,
                StepId = task.StepID,
                TaskId = task.Id,
                Title = "报修用品已认领"
            });

        }
        /// <summary>
        /// 用品修好后，个人进入个人用品确认领取
        /// </summary>
        /// <param name="input">用品id</param>
        /// <returns></returns>
        public async Task Apply(Guid input)
        {
            var u = _userSupplyRepository.Get(input);
            if (u != null)
            {
                u.Status = (int)UserSupplyStatus.使用中;
                _userSupplyRepository.Update(u);
            }
            var s = _supplyBaseRepository.Get(u.SupplyId);
            s.Status = (int)SupplyStatus.被领用;
            _supplyBaseRepository.Update(s);

            /*
            判断当前申请是否都已处理完毕，是则调用流程结束方法
            */
            var r = _supplyRepairRepository.GetAll().FirstOrDefault(ite => ite.SupplyId == u.SupplyId && ite.Status == (int)SupplyRepairStatus.等待领取);
            if (r == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到用品维修记录。");
            }
            var task = _workflowtaskRepository.GetAll().OrderByDescending(ite => ite.SenderTime).FirstOrDefault(ite => ite.InstanceID == r.Id.ToString());
            if (task == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前待办任务。");
            }

            // var _taskAppService=  Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            await _workFlowWorkTaskAppService.ExecuteTask(new ExecuteWorkFlowInput()
            {
                ActionType = "completed",
                FlowId = task.FlowID,
                GroupId = task.GroupID,
                StepId = task.StepID,
                TaskId = task.Id,
                Title = "报修用品已认领"
            });

        }

        /// <summary>
        /// 个人申请用品维修
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateSupplyRepairDto input)
        {
            var u = await _userSupplyRepository.GetAsync(input.UserSupplyId);
            var model = new SupplyRepair();
            model.UserSupplyId = input.UserSupplyId;
            model.Des = input.Des;
            model.IsImportant = input.IsImportant;
            model.RepairEndTime = input.RepairEndTime;
            model.SupplyId = u.SupplyId;
            model.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.申请用品维修附件,
                    Files = fileList
                });
            }
            _supplyRepairRepository.Insert(model);
            var s = _supplyBaseRepository.Get(u.SupplyId);
            s.Status = (int)SupplyStatus.维修中;
            _supplyBaseRepository.Update(s);

            u.Status = (int)UserSupplyStatus.维修中;
            _userSupplyRepository.Update(u);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }

        public async Task<InitWorkFlowOutput> CreateV2(CreateSupplyRepairDto input)
        {
            var u = await _userSupplyRepository.GetAsync(input.UserSupplyId);
            var model = new SupplyRepair();
            model.UserSupplyId = input.UserSupplyId;
            model.Des = input.Des;
            model.IsImportant = input.IsImportant;
            model.RepairEndTime = input.RepairEndTime;
            model.SupplyId = u.SupplyId;
            model.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.申请用品维修附件,
                    Files = fileList
                });
            }
            _supplyRepairRepository.Insert(model);
            var s = _supplyBaseRepository.Get(u.SupplyId);
            s.Status = (int)SupplyStatus.维修中;
            _supplyBaseRepository.Update(s);

            u.Status = (int)UserSupplyStatus.维修中;
            _userSupplyRepository.Update(u);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }



        /// <summary>
        /// 维修申请详情(台账列表中查看)
        /// </summary>
        /// <param name="input">用户台账id</param> 
        /// InstanceId 为userSupplyId
        /// <returns></returns>
        public async Task<SupplyRepairDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var us = _userSupplyRepository.Get(id);
            var model = await _supplyRepairRepository.FirstOrDefaultAsync(r => r.UserSupplyId == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到维修申请记录。");
            }
            var ret = new SupplyRepairDto();
            var s = _supplyBaseRepository.Get(model.SupplyId);

            ret.Code = s.Code;
            ret.Des = model.Des;
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申请用品维修附件 });
            ret.Id = model.Id;
            ret.IsImportant = model.IsImportant;
            ret.Name = s.Name;
            ret.RepairEndTime = model.RepairEndTime;
            ret.SupplyId = model.SupplyId;
            ret.UserId = model.CreatorUserId.Value;
            var u = _userAppService.Get(ret.UserId);
            ret.UserId_Name = u.Name;
            ret.Version = s.Version;
            ret.CheckRemark = model.CheckRemark;
            ret.ScrapReason = model.ScrapReason;
            ret.RepairResult = model.RepairResult;
            ret.SupplierId = model.SupplierId;
            ret.SupplierName = model.SupplierName;
            ret.CreationTime = model.CreationTime;
            return ret;
        }


        /// <summary>
        /// 流程处理I
        /// Id为维修Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<SupplyRepairOutDto> GetForFlow(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);

            var model = await _supplyRepairRepository.GetAsync(id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到维修申请记录。");
            }
            var ret = new SupplyRepairOutDto();
            var s = _supplyBaseRepository.Get(model.SupplyId);

            ret.Code = s.Code;
            ret.Des = model.Des;
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申请用品维修附件 });
            ret.Id = model.Id;
            ret.IsImportant = model.IsImportant;
            ret.Name = s.Name;
            ret.RepairEndTime = model.RepairEndTime;
            ret.SupplyId = model.SupplyId;
            ret.UserId = model.CreatorUserId.Value;
            var u = _userAppService.Get(ret.UserId);
            ret.UserId_Name = u.Name;
            ret.Version = s.Version;
            ret.CheckRemark = model.CheckRemark;
            ret.ScrapReason = model.ScrapReason;
            ret.RepairResult = model.RepairResult;
            ret.CreationTime = model.CreationTime;
            ret.SupplierId = model.SupplierId;
            ret.SupplierName = model.SupplierName;
            return ret;
        }


        /// <summary>
        /// 行政查看维修申请列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>


        public async Task<PagedResultDto<SupplyRepairDto>> GetAllV2(SupplyRepairListInputDtondSortedInputDto input)
        {
            var strflowid = input.FlowId.ToString();
            var query = from a in _supplyRepairRepository.GetAll()
                        join b in _userAppService.GetAll() on a.CreatorUserId equals b.Id
                        join c in _supplyBaseRepository.GetAll() on a.SupplyId equals c.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                              x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                              x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                         select c).Any()
                        select new SupplyRepairDto()
                        {
                            Code = c.Code,
                            Des = a.Des,
                            Id = a.Id,
                            IsImportant = a.IsImportant,
                            Name = c.Name,
                            Type = c.Type,
                            TypeName = ((SupplyType)c.Type).ToString(),
                            Money = c.Money,
                            RepairEndTime = a.RepairEndTime,
                            SupplyId = a.SupplyId,
                            UserId = a.CreatorUserId.Value,
                            UserId_Name = b.Name,
                            Version = c.Version,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            SortStatus = ((openModel ? 1 : 2) == 1 && a.IsImportant) ? 1
                                       : ((openModel ? 1 : 2) == 1 && !a.IsImportant) ? 2
                                       : 3,
                        };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey) || ite.UserId_Name.Contains(input.SearchKey));
            }

            if (!string.IsNullOrWhiteSpace(input.Status))
            {
                var statusArrty = input.Status.Split(",");
                query = query.Where(r => statusArrty.Contains(r.Status.ToString()));
            }
            var total = await query.CountAsync();
            //var model = await query.OrderBy(r => r.OpenModel).ThenByDescending(ite => ite.IsImportant).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();
            var model = await query.OrderBy(r => r.SortStatus).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<SupplyRepairDto>();
            foreach (var i in model)
            {
                i.InstanceId = i.Id.ToString();
                i.OldOpenModel = i.OpenModel;
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, i as BusinessWorkFlowListOutput);
                i.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = i.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申请用品维修附件 });
                ret.Add(i);
            }
            return new PagedResultDto<SupplyRepairDto>(total, ret);
        }


        public async Task<PagedResultDto<SupplyRepairDto>> GetAll(SupplyRepairListInputDtondSortedInputDto input)
        {
            var strflowid = input.FlowId.ToString();
            var query = from a in _supplyRepairRepository.GetAll()
                        join b in _userAppService.GetAll() on a.CreatorUserId equals b.Id
                        join c in _supplyBaseRepository.GetAll() on a.SupplyId equals c.Id
                        select new SupplyRepairDto()
                        {
                            Code = c.Code,
                            Des = a.Des,
                            Id = a.Id,
                            IsImportant = a.IsImportant,
                            Name = c.Name,
                            Type = c.Type,
                            TypeName = ((SupplyType)c.Type).ToString(),
                            Money = c.Money,
                            RepairEndTime = a.RepairEndTime,
                            SupplyId = a.SupplyId,
                            UserId = a.CreatorUserId.Value,
                            UserId_Name = b.Name,
                            Version = c.Version,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            SortStatus = (a.Status == 1 && a.IsImportant) ? 1
                                       : (a.Status == 1 && !a.IsImportant) ? 2
                                       : 3,
                            RepairResult = a.RepairResult,
                            RepairResult_Title = (int)a.RepairResult == 0 ? "" : a.RepairResult.ToString(),
                        };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey) || ite.UserId_Name.Contains(input.SearchKey));
            }

            if (!string.IsNullOrWhiteSpace(input.Status))
            {
                var statusArrty = input.Status.Split(",");
                query = query.Where(r => statusArrty.Contains(r.Status.ToString()));
            }
            var total = await query.CountAsync();
            //var model = await query.OrderBy(r => r.OpenModel).ThenByDescending(ite => ite.IsImportant).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();
            var model = await query.OrderBy(r => r.SortStatus).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();
            var ret = new List<SupplyRepairDto>();
            foreach (var i in model)
            {
                i.InstanceId = i.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, i as BusinessWorkFlowListOutput);
                i.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = i.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申请用品维修附件 });
                ret.Add(i);
            }
            return new PagedResultDto<SupplyRepairDto>(total, ret);
        }





        /// <summary>
        /// 维修失败（行政列表页面）
        /// </summary>
        /// <param name="input">维修申请id</param>
        public async Task<ExecuteWorkFlowOutput> RepairFailed(Guid input)
        {
            var r = _supplyRepairRepository.Get(input);
            r.Status = -1;
            var s = _supplyBaseRepository.Get(r.SupplyId);
            s.Status = (int)SupplyStatus.已报废;
            var u = _userSupplyRepository.GetAll().FirstOrDefault(ite => ite.SupplyId == r.SupplyId && ite.Status == (int)UserSupplyStatus.维修中);
            if (u != null)
            {
                u.Status = (int)UserSupplyStatus.已报废;
                _userSupplyRepository.Update(u);
            }
            _supplyRepairRepository.Update(r);
            _supplyBaseRepository.Update(s);

            /*
           判断当前申请是否都已处理完毕，是则调用流程结束方法
           */
            if (r == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到用品维修记录。");
            }
            var task = _workflowtaskRepository.GetAll().OrderByDescending(ite => ite.SenderTime).FirstOrDefault(ite => ite.InstanceID == r.Id.ToString() && ite.ReceiveID == AbpSession.UserId.Value);
            if (task == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前待办任务。");
            }

            // var _taskAppService=  Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            var ret = await _workFlowWorkTaskAppService.ExecuteTask(new ExecuteWorkFlowInput()
            {
                ActionType = "completed",
                FlowId = task.FlowID,
                GroupId = task.GroupID,
                StepId = task.StepID,
                TaskId = task.Id,
                Title = "报修用品已认领"
            });
            return ret;
        }
        /// <summary>
        /// 维修成功，通知领取
        /// </summary>
        /// <param name="input">维修申请id</param>
        public async Task<ExecuteWorkFlowOutput> RepairSuccess(Guid input)
        {
            var r = _supplyRepairRepository.Get(input);
            //r.Status = 1;
            var s = _supplyBaseRepository.Get(r.SupplyId);
            s.Status = (int)SupplyStatus.已修好;
            var u = _userSupplyRepository.GetAll().FirstOrDefault(ite => ite.SupplyId == r.SupplyId && ite.Status == (int)UserSupplyStatus.维修中);
            if (u != null)
            {
                u.Status = (int)UserSupplyStatus.已修好;
                _userSupplyRepository.Update(u);
            }
            //_supplyRepairRepository.Update(r);
            _supplyBaseRepository.Update(s);


            /*
            将流程往前推一步
            */

            var task = _workflowtaskRepository.GetAll().OrderByDescending(ite => ite.SenderTime).FirstOrDefault(ite => ite.InstanceID == input.ToString() && ite.ReceiveID == AbpSession.UserId.Value);
            if (task == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到当前待办任务。");
            }
            var nextstep = await _workFlowAppService.GetNextStepForRun(new GetNextStepForRunInput
            {
                //FlowId = task.FlowID,
                //GroupId = task.GroupID,
                //InstanceId = task.InstanceID,
                //StepId = task.StepID,
                TaskId = task.Id
            });
            if (nextstep == null || nextstep.Steps == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到下一步骤。");
            }
            if (string.IsNullOrWhiteSpace(nextstep.Steps[0].DefaultUserId))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到下一步骤默认处理者。");
            }
            // var _taskAppService=  Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            var ret = await _workFlowWorkTaskAppService.ExecuteTask(new ExecuteWorkFlowInput()
            {
                ActionType = "submit",
                FlowId = task.FlowID,
                GroupId = task.GroupID,
                StepId = task.StepID,
                TaskId = task.Id,
                InstanceId = input.ToString(),
                Steps = new List<ExecuteWorkChooseStep>() {
                          new ExecuteWorkChooseStep(){
                               id=nextstep.Steps[0].NextStepId.ToString(),
                                member=nextstep.Steps[0].DefaultUserId
                          }
                      },
                Title = "用品维修成功"
            });
            return ret;
        }

        public async Task Update(CreateSupplyRepairDto input)
        {
            var repair = _supplyRepairRepository.Get(input.Id.Value);
            repair.Des = input.Des;
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = repair.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.申请用品维修附件,
                    Files = fileList
                });
            }
            repair.RepairEndTime = input.RepairEndTime;
            repair.CheckRemark = input.CheckRemark;
            repair.ScrapReason = input.ScrapReason;
            repair.RepairResult = input.RepairResult;

            await _supplyRepairRepository.UpdateAsync(repair);
        }

        public async Task<PagedResultDto<SupplyRepairDto>> GetMy(GetListInput input)
        {
            var query = from a in _supplyRepairRepository.GetAll()
                        join b in _userAppService.GetAll() on a.CreatorUserId equals b.Id
                        join c in _supplyBaseRepository.GetAll() on a.SupplyId equals c.Id
                        where a.CreatorUserId == AbpSession.UserId.Value
                        select new SupplyRepairDto()
                        {
                            Code = c.Code,
                            Des = a.Des,
                            Type = c.Type,
                            TypeName = ((SupplyType)c.Type).ToString(),
                            Id = a.Id,
                            IsImportant = a.IsImportant,
                            Name = c.Name,
                            RepairEndTime = a.RepairEndTime,
                            SupplyId = a.SupplyId,
                            UserId = a.CreatorUserId.Value,
                            UserId_Name = b.Name,
                            Version = c.Version,
                            CreationTime = a.CreationTime,
                            Money = c.Money,
                            Status = a.Status,
                            StatusTitle = ((SupplyRepairStatus)a.Status).ToString()
                        };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(ite => ite.Name.Contains(input.SearchKey) || ite.UserId_Name.Contains(input.SearchKey));
            }

            var total = await query.CountAsync();
            var model = await query.OrderByDescending(ite => ite.IsImportant).ThenByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync();
            foreach (var i in model)
            {
                i.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = i.Id.ToString(), BusinessType = (int)AbpFileBusinessType.申请用品维修附件 });
                i.InstanceId = i.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, i as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<SupplyRepairDto>(total, model);
        }

        [RemoteService(IsEnabled = false)]
        public void AfterRepairAction(string instanceId, Guid flowId)
        {
            var id = instanceId.ToGuid();
            var model = _supplyRepairRepository.Get(id);
            var supplyModel = _supplyBaseRepository.Get(model.SupplyId);
            if (model.RepairResult == RepairResultEnum.维修成功)
            {
                var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICW_PersonalTodoAppService>();
                var first_User = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCode("XZRY").FirstOrDefault();
                if (first_User == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到行政人员");
                service.Create(new CreateCW_PersonalTodoInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = CW_PersonalType.用品维修,
                    CWType = CWGL.Enums.RefundResultType.财务应付款,
                    FlowId = flowId,
                    Status = CW_PersonalToStatus.未处理,
                    Title = $"用品：{supplyModel.Code}-维修",
                    UserId = first_User.Id,

                });

                var s = _supplyBaseRepository.Get(model.SupplyId);
                s.Status = (int)SupplyStatus.被领用;
                var u = _userSupplyRepository.FirstOrDefault(ite => ite.SupplyId == model.SupplyId && ite.Status == (int)UserSupplyStatus.维修中);
                if (u != null)
                    u.Status = (int)UserSupplyStatus.使用中;
            }
            else if (model.RepairResult == RepairResultEnum.维修失败)
            {
                var s = _supplyBaseRepository.Get(model.SupplyId);
                s.Status = (int)SupplyStatus.报废中;
                var u = _userSupplyRepository.FirstOrDefault(ite => ite.SupplyId == model.SupplyId && ite.Status == (int)UserSupplyStatus.维修中);
                if (u != null)
                    _userSupplyRepository.Delete(u);

                var serviceScranp = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISupplyScrapAppService>();
                var list = serviceScranp.Create(new CreateSupplyScrapMainInput()
                {
                    FlowId = "665548ae-a5fb-4eee-89de-a00b6875949e".ToGuid(),
                    FlowTitle = "维修失败报废",
                    SupplyScrapSub = new List<CreateSupplyScrapSubInput>()
                {
                     new  CreateSupplyScrapSubInput ()
                     {
                          SupplyId = model.SupplyId,
                          Reason = model.ScrapReason,
                          UserSupplyId =model.UserSupplyId,
                     }
                }
                });
                var info = list.FirstOrDefault();
                //备注：强制提交后有待办，建议前端直接调用以下方法
                CurrentUnitOfWork.SaveChanges();
                var flower = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowAppService>();
                var next = flower.GetNextStepForRunSync(new GetNextStepForRunInput()
                {
                    TaskId = info.TaskId
                });
                var run = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
                var steps = next.Steps.Select(ite => new ExecuteWorkChooseStep() { id = ite.NextStepId.ToString(), member = ite.DefaultUserId }).ToList();
                var ret = run.ExecuteTaskSync(new ExecuteWorkFlowInput()
                {
                    ActionType = "submit",
                    FlowId = info.FlowId,
                    GroupId = info.GroupId,
                    InstanceId = info.InStanceId,
                    IsHideNextTask = true,
                    StepId = info.StepId,
                    Steps = steps,
                    TaskId = info.TaskId,
                    Title = $"维修失败报废"
                });
            }
        }


        [RemoteService(IsEnabled = false)]
        public void RepairNoticeSupplierAction(string instanceId, string parameter)
        {
            var id = instanceId.ToGuid();
            var supplierId = parameter.ToGuid();
            var entity = _supplyRepairRepository.Get(id);
            var sModel = _supplySupplierRepository.Get(supplierId);
            entity.SupplierId = sModel.Id;
            entity.SupplierName = sModel.Name;
        }
    }
}
