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
using Abp.WorkFlow;
using Abp.Extensions;
using ZCYX.FRMSCore.Application;
using Abp.Application.Services;
using Abp.File;
using ZCYX.FRMSCore.Model;

namespace Supply
{
    public class CuringProcurementAppService : FRMSCoreAppServiceBase, ICuringProcurementAppService
    {
        private readonly IRepository<CuringProcurement, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<CuringProcurementPlan, Guid> _curingProcurementPlanRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ISupplyBaseRepository _supplyBaseRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;

        public CuringProcurementAppService(IRepository<CuringProcurement, Guid> repository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , IRepository<CuringProcurementPlan, Guid> curingProcurementPlanRepository, IAbpFileRelationAppService abpFileRelationAppService
            , ISupplyBaseRepository supplyBaseRepository, IWorkFlowTaskRepository workFlowTaskRepository)
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _curingProcurementPlanRepository = curingProcurementPlanRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _supplyBaseRepository = supplyBaseRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<PagedResultDto<CuringProcurementListOutputDto>> GetList(GetCuringProcurementListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                    x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                    x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new CuringProcurementListOutputDto()
                        {
                            Id = a.Id,
                            Code = a.Code,
                            NeedMember = a.NeedMember,
                            Type = a.Type,
                            ExecuteSummary = a.ExecuteSummary,
                            Remark = a.Remark,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                            OpenModel = openModel.Count(y => y.Type !=6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            if (!input.Status.IsNullOrWhiteSpace())
            {
                var statusArry = input.Status.Split(',');
                query = query.Where(r => statusArry.Contains(r.Status.ToString()));
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                //item.TypeName = ((SupplyType)item.Type.ToInt()).ToString();
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<CuringProcurementListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CuringProcurementOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = input.InstanceId.ToGuid();
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<CuringProcurementOutputDto>();
            var plans = await _curingProcurementPlanRepository.GetAll().Where(r => r.MainId == ret.Id && r.BusinessType == (int)CuringProcurementType.固化采购).ToListAsync();
            foreach (var item in plans)
            {
                var planModel = item.MapTo<CuringProcurementPlanOutputDto>();
                planModel.TypeName = ((SupplyType)planModel.Type.ToInt()).ToString();
                planModel.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.固化采购计划附件 });
                planModel.StatusTitle = ((CuringProcurementPlanStatus)planModel.Status).ToString();

                ret.Plans.Add(planModel);

            }
            return ret;
        }
        /// <summary>
        /// 添加一个CuringProcurement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCuringProcurementInput input)
        {
            var ret = new InitWorkFlowOutput();
            var newmodel = new CuringProcurement()
            {
                Id = Guid.NewGuid(),
                Code = input.Code,
                NeedMember = input.NeedMember,
                Type = input.Type,
                ExecuteSummary = input.ExecuteSummary,
                Remark = input.Remark,

            };
            await _repository.InsertAsync(newmodel);
            foreach (var item in input.Plans)
            {
                var entity = item.MapTo<CuringProcurementPlan>();
                entity.Id = Guid.NewGuid();
                entity.MainId = newmodel.Id;
                entity.BusinessType = (int)CuringProcurementType.固化采购;
                _curingProcurementPlanRepository.Insert(entity);
                if (item.FileList != null)
                {
                    var fileList = new List<AbpFileListInput>();
                    foreach (var ite in item.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                    }
                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                    {
                        BusinessId = entity.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.固化采购计划附件,
                        Files = fileList
                    });
                }
            }


            ret.InStanceId = newmodel.Id.ToString();
            return ret;

        }

        /// <summary>
        /// 修改一个CuringProcurement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateCuringProcurementInput input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            if (dbmodel.Status == -1)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "不能编辑审核过的固化采购，请发起编辑流程");
            dbmodel.Code = input.Code;
            dbmodel.NeedMember = input.NeedMember;
            dbmodel.Type = input.Type;
            dbmodel.ExecuteSummary = input.ExecuteSummary;
            dbmodel.Remark = input.Remark;
            await _repository.UpdateAsync(dbmodel);
            var add_plan = input.Plans.Where(r => !r.Id.HasValue);
            foreach (var item in input.Plans.Where(r => !r.Id.HasValue))
            {
                var newModel = item.MapTo<CuringProcurementPlan>();
                newModel.Id = Guid.NewGuid();
                newModel.BusinessType = (int)CuringProcurementType.固化采购;
                await _curingProcurementPlanRepository.InsertAsync(newModel);
                if (item.FileList != null)
                {
                    var fileList = new List<AbpFileListInput>();
                    foreach (var ite in item.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = ite.Id, Sort = ite.Sort });
                    }
                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                    {
                        BusinessId = newModel.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.固化采购计划附件,
                        Files = fileList
                    });
                }


            }

            var exit_Models = _curingProcurementPlanRepository.GetAll().Where(r => r.MainId == input.Id);
            var less_Ids = exit_Models.Select(r => r.Id).Except(input.Plans.Where(r => r.Id.HasValue).Select(r => r.Id.Value));
            less_Ids.ToList().ForEach(r =>
            {
                _curingProcurementPlanRepository.Delete(m => m.Id == r);
            });

            var update_Ids = input.Plans.Where(r => r.Id.HasValue).Select(r => r.Id.Value).Except(less_Ids);
            update_Ids.ToList().ForEach(r =>
            {
                var exit_model = exit_Models.SingleOrDefault(m => m.Id == r);
                var inputUpdateModel = input.Plans.SingleOrDefault(p => p.Id == r);
                inputUpdateModel.MapTo(exit_model);
                var fileList = new List<AbpFileListInput>();
                if (inputUpdateModel.FileList != null)
                {
                    foreach (var item in inputUpdateModel.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                    }
                }
                _abpFileRelationAppService.Update(new CreateFileRelationsInput()
                {
                    BusinessId = input.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.固化采购计划附件,
                    Files = fileList
                });


            });
        }



        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }

        [RemoteService(IsEnabled = false)]
        public void UpdateToChange(Guid id)
        {
            var model = _repository.Get(id);
            if (model.Status != -1)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "暂时不能整改该固化采购");
            else
            {
                model.Status = -3;
            }
        }


        /// <summary>
        /// 固化采购入库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PutIn(RegisterSupplyPurchasePlanInput input)
        {
            var model = await _curingProcurementPlanRepository.GetAsync(input.PlanId);
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
                    Type = model.Type.ToInt(),
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

                supplyModel.Status = (int)SupplyStatus.在库;
                await _supplyBaseRepository.InsertAsync(supplyModel);
            }
        }
    }
}