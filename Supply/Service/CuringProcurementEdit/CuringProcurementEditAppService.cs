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
    public class CuringProcurementEditAppService : FRMSCoreAppServiceBase, ICuringProcurementEditAppService
    {
        private readonly IRepository<CuringProcurementEdit, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<CuringProcurementPlan, Guid> _curingProcurementPlanRepository;
        private readonly IRepository<CuringProcurement, Guid> _curingProcurementRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public CuringProcurementEditAppService(IRepository<CuringProcurementEdit, Guid> repository
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IRepository<CuringProcurementPlan, Guid> curingProcurementPlanRepository
            , IRepository<CuringProcurement, Guid> curingProcurementRepository, IAbpFileRelationAppService abpFileRelationAppService)
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _curingProcurementPlanRepository = curingProcurementPlanRepository;
            _curingProcurementRepository = curingProcurementRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CuringProcurementEditListOutputDto>> GetList(GetCuringProcurementEditListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        select new CuringProcurementEditListOutputDto()
                        {
                            Id = a.Id,
                            MainId = a.MainId,
                            Code = a.Code,
                            NeedMember = a.NeedMember,
                            Type = a.Type,
                            ExecuteSummary = a.ExecuteSummary,
                            Remark = a.Remark,
                            Status = a.Status,
                            CreationTime = a.CreationTime
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
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<CuringProcurementEditListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CuringProcurementEditOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = input.InstanceId.ToGuid();
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<CuringProcurementEditOutputDto>();
            var plans = await _curingProcurementPlanRepository.GetAll().Where(r => r.MainId == ret.Id && r.BusinessType == (int)CuringProcurementType.固化采购调整).ToListAsync();
            foreach (var item in plans)
            {
                var planModel = item.MapTo<CuringProcurementPlanOutputDto>();
                planModel.TypeName = ((SupplyType)planModel.Type.ToInt()).ToString();
                planModel.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.固化采购计划整改附件 });
                planModel.StatusTitle = ((CuringProcurementPlanStatus)planModel.Status).ToString();

                ret.Plans.Add(planModel);

            }
            return ret;
        }
        /// <summary>
        /// 添加一个CuringProcurementEdit
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCuringProcurementEditInput input)
        {

            var exit_Flag = _repository.GetAll().Any(r => r.MainId == input.MainId && r.Status != -1);
            if (exit_Flag)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "存在未完成的固化采购整改");
            var ret = new InitWorkFlowOutput();
            var newmodel = new CuringProcurementEdit()
            {
                MainId = input.MainId,
                Code = input.Code,
                NeedMember = input.NeedMember,
                Type = input.Type,
                ExecuteSummary = input.ExecuteSummary,
                Remark = input.Remark,
                Status = input.Status
            };
            await _repository.InsertAsync(newmodel);
            foreach (var item in input.Plans)
            {
                var entity = item.MapTo<CuringProcurementPlan>();
                entity.Id = Guid.NewGuid();
                entity.MainId = newmodel.Id;
                entity.BusinessType = (int)CuringProcurementType.固化采购调整;
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
                        BusinessType = (int)AbpFileBusinessType.固化采购计划整改附件,
                        Files = fileList
                    });
                }
            }
            ret.InStanceId = newmodel.Id.ToString();
            return ret;
        }

        /// <summary>
        /// 修改一个CuringProcurementEdit
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateCuringProcurementEditInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                //dbmodel.Code = input.Code;
                dbmodel.NeedMember = input.NeedMember;
                dbmodel.Type = input.Type;
                dbmodel.ExecuteSummary = input.ExecuteSummary;
                dbmodel.Remark = input.Remark;
                //dbmodel.Status = input.Status;

                await _repository.UpdateAsync(dbmodel);

                var add_plan = input.Plans.Where(r => !r.Id.HasValue);
                foreach (var item in input.Plans.Where(r => !r.Id.HasValue))
                {
                    var newModel = item.MapTo<CuringProcurementPlan>();
                    newModel.BusinessType = (int)CuringProcurementType.固化采购调整;
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
                            BusinessType = (int)AbpFileBusinessType.固化采购计划整改附件,
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
                        BusinessType = (int)AbpFileBusinessType.固化采购计划整改附件,
                        Files = fileList
                    });
                });

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
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
        public void CompleteEdit(Guid instanceId)
        {
            var editModel = _repository.Get(instanceId);
            editModel.Status = -1;
            var old_plans = _curingProcurementPlanRepository.GetAll().Where(r => r.MainId == editModel.MainId);
            foreach (var item in old_plans)
            {
                _curingProcurementPlanRepository.Delete(item);
            }
            var editPlans = _curingProcurementPlanRepository.GetAll().Where(r => r.MainId == editModel.Id);
            foreach (var item in editPlans)
            {
                var entity = new CuringProcurementPlan()
                {
                    Id = Guid.NewGuid(),
                    MainId = item.MainId,
                    Name = item.Name,
                    Version = item.Version,
                    Number = item.Number,
                    Unit = item.Unit,
                    Money = item.Money,
                    Des = item.Des,
                    Type = item.Type,
                    Remark = item.Remark,
                    Status = item.Status,
                    BusinessType = (int)CuringProcurementType.固化采购,
                };
                _curingProcurementPlanRepository.Insert(entity);

            }
        }



    }
}