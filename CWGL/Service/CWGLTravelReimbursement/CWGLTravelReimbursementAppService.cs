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
using Project;
using Abp.WorkFlowDictionary;
using Abp.Authorization;
using Abp;
using CWGL.Enums;
using ZCYX.FRMSCore.Extensions;
using Abp.Application.Services;
using ZCYX.FRMSCore.Users;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLTravelReimbursementAppService : FRMSCoreAppServiceBase, ICWGLTravelReimbursementAppService
    {
        private readonly IRepository<CWGLTravelReimbursement, Guid> _repository;
        private readonly IRepository<CWGLTravelReimbursementDetail, Guid> _detailRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<OAWorkout, Guid> _workOutRepository;
        private readonly IRepository<CWGLBorrowMoney, Guid> _cWGLBorrowMonetrepository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;

        public CWGLTravelReimbursementAppService(IRepository<CWGLTravelReimbursement, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager
            , IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<OAWorkout, Guid> workOutRepository
            , IRepository<CWGLBorrowMoney, Guid> cWGLBorrowMonetrepository, IRepository<AbpDictionary, Guid> abpDictionaryRepository
            , IRepository<CWGLTravelReimbursementDetail, Guid> detailRepository, WorkFlowCacheManager workFlowCacheManager
            , IWorkFlowTaskRepository workFlowTaskRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _organizationUnitRepository = organizationUnitRepository;
            _workOutRepository = workOutRepository;
            _cWGLBorrowMonetrepository = cWGLBorrowMonetrepository;
            _abpDictionaryRepository = abpDictionaryRepository;
            _detailRepository = detailRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLTravelReimbursementListOutputDto>> GetList(GetCWGLTravelReimbursementListInput input)
        {
            var strflowid = input.FlowId.ToString();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join user in UserManager.Users on a.UserId equals user.Id
                        join org in _organizationUnitRepository.GetAll() on a.OrgId equals org.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                             x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                             x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                         select c).Any()
                        select new CWGLTravelReimbursementListOutputDto()
                        {
                            Id = a.Id,
                            UserId_Name = user.Name,
                            OrgId_Name = org.DisplayName,
                            Money = a.Money,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            OpenModel = openModel ? 1 : 2,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<CWGLTravelReimbursementListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CWGLTravelReimbursementOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join user in UserManager.Users on a.UserId equals user.Id
                        join org in _organizationUnitRepository.GetAll() on a.OrgId equals org.Id
                        join b in _workOutRepository.GetAll() on a.WorkoutId equals b.Id
                        join dic in _abpDictionaryRepository.GetAll() on b.TranType equals dic.Id
                        join fee in _cWGLBorrowMonetrepository.GetAll() on a.BorrowMoneyId equals fee.Id into g
                        from fee in g.DefaultIfEmpty()
                        where a.Id == id
                        select new CWGLTravelReimbursementOutputDto()
                        {
                            Id = a.Id,
                            UserId_Name = user.Name,
                            UserId = user.Id,
                            OrgId_Name = org.DisplayName,
                            BorrowMoney = fee == null ? 0 : fee.Money,
                            BorrowMoneyId = fee == null ? Guid.Empty : fee.Id,
                            Destination = b.Destination,
                            WorkOutId = b.Id,
                            EndTime = b.EndTime,
                            FromPosition = b.FromPosition,
                            Hours = b.Hours,
                            Note = a.Note,
                            Nummber = a.Nummber,
                            Reason = b.Reason,
                            StartTime = b.StartTime,
                            TranTypeName = dic.Title,

                        };

            var model = await query.FirstOrDefaultAsync();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var details = await _detailRepository.GetAll().Where(r => r.TravelReimbursementId == model.Id).ToListAsync();
            model.Details = details.MapTo<List<CWGLTravelReimbursementDetailOutputDto>>();
            model.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.差旅费报销
            });

            var ret = model.MapTo<CWGLTravelReimbursementOutputDto>();
            return ret;
        }
        /// <summary>
        /// 添加一个CWGLTravelReimbursement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLTravelReimbursementInput input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var newmodel = new CWGLTravelReimbursement()
            {
                Id = Guid.NewGuid(),
                UserId = AbpSession.UserId.Value,
                OrgId = userOrgModel.OrgId,
                Money = input.Money,
                Note = input.Note,
                Nummber = input.Nummber,
                BorrowMoneyId = input.BorrowMoneyId,
                WorkoutId = input.WorkoutId,
            };
            newmodel.Status = 0;
            var totalFee = 0m;
            foreach (var item in input.DetailList)
            {
                var entity = item.MapTo<CWGLTravelReimbursementDetail>();
                entity.TravelReimbursementId = newmodel.Id;
                totalFee = totalFee + entity.Fare ?? 0 + entity.Accommodation ?? 0 + entity.Other ?? 0;
                await _detailRepository.InsertAsync(entity);
            }
            if (input.BorrowMoneyId.HasValue)
            {
                var entityBorro = await _cWGLBorrowMonetrepository.GetAsync(input.BorrowMoneyId.Value);
                if (entityBorro.Money == totalFee)
                    newmodel.ResultType = (int)RefundResultType.无退无报;
                else if (entityBorro.Money > totalFee)
                    newmodel.ResultType = (int)RefundResultType.财务应收款;
                else
                    newmodel.ResultType = (int)RefundResultType.财务应付款;
                newmodel.Money = Math.Abs(entityBorro.Money - totalFee);
            }
            else
            {
                newmodel.Money = totalFee;
                newmodel.ResultType = (int)RefundResultType.财务应付款;
            }

            await _repository.InsertAsync(newmodel);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = newmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.差旅费报销,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLTravelReimbursement
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLTravelReimbursementInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new CWGLTravelReimbursement();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone();
                }

                dbmodel.Money = input.Money;
                dbmodel.Note = input.Note;
                dbmodel.Nummber = input.Nummber;
                dbmodel.BorrowMoneyId = input.BorrowMoneyId;
                dbmodel.WorkoutId = input.WorkoutId;
                input.FACData.BusinessId = input.Id.ToString();
                var totalFee = 0m;
                foreach (var item in input.DetailList)
                {
                    totalFee = totalFee + item.Fare ?? 0 + item.Accommodation ?? 0 + item.Other ?? 0;
                }
                if (input.BorrowMoneyId.HasValue)
                {
                    var entityBorro = await _cWGLBorrowMonetrepository.GetAsync(input.BorrowMoneyId.Value);
                    if (entityBorro.Money == totalFee)
                        dbmodel.ResultType = (int)RefundResultType.无退无报;
                    else if (entityBorro.Money > totalFee)
                        dbmodel.ResultType = (int)RefundResultType.财务应收款;
                    else
                        dbmodel.ResultType = (int)RefundResultType.财务应付款;
                    dbmodel.Money = Math.Abs(entityBorro.Money - totalFee);
                }
                else
                {
                    dbmodel.Money = totalFee;
                    dbmodel.ResultType = (int)RefundResultType.财务应付款;
                }


                await _repository.UpdateAsync(dbmodel);
                var details = await _detailRepository.GetAll().Where(r => r.TravelReimbursementId == dbmodel.Id).ToListAsync();
                var old_Details = new List<CWGLTravelReimbursementDetailLogDto>();
                var old_Model = new CWGLTravelReimbursementLogDto() { Id = logModel.Id, Money = logModel.Money, Note = logModel.Note, Nummber = logModel.Nummber };
                foreach (var item in details)
                {
                    old_Details.Add(item.MapTo<CWGLTravelReimbursementDetailLogDto>());
                }
                old_Model.Detail = old_Details;
                var new_Detail = new List<CWGLTravelReimbursementDetailLogDto>();
                foreach (var item in input.DetailList)
                {
                    var entity = new CWGLTravelReimbursementDetailLogDto()
                    {
                        Accommodation = item.Accommodation,
                        Address = item.Address,
                        BeginTime = item.BeginTime,
                        Day = item.Day,
                        EndTime = item.EndTime,
                        Fare = item.Fare,
                        Id = item.Id ?? Guid.Empty,
                        Other = item.Other,
                        Vehicle = item.Vehicle
                    };
                    new_Detail.Add(entity);
                }
                var new_Model = new CWGLTravelReimbursementLogDto() { Id = dbmodel.Id, Money = dbmodel.Money, Note = dbmodel.Note, Nummber = dbmodel.Nummber, Detail = new_Detail };
                var add_details = input.DetailList.Where(r => !r.Id.HasValue);
                foreach (var item in add_details)
                {
                    var entity = item.MapTo<CWGLTravelReimbursementDetail>();
                    entity.Id = Guid.NewGuid();
                    entity.TravelReimbursementId = dbmodel.Id;
                    await _detailRepository.InsertAsync(entity);
                }
                var update_details = input.DetailList.Where(r => r.Id.HasValue);
                foreach (var item in update_details)
                {
                    var db_detail = await _detailRepository.GetAsync(item.Id.Value);
                    item.MapTo(db_detail);
                }
                var less_detailIds = details.Select(r => r.Id).Except(input.DetailList.Where(r => r.Id.HasValue).Select(r => r.Id.Value)).ToList();
                var less_details = details.Where(r => less_detailIds.Contains(r.Id));
                foreach (var item in less_details)
                {
                    await _detailRepository.DeleteAsync(item);
                }

                var fileList = new List<AbpFileListInput>();
                if (input.FileList != null)
                {
                    foreach (var item in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                    }
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = input.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.差旅费报销,
                    Files = fileList
                });

                var groupId = Guid.NewGuid();
                input.FACData.GroupId = groupId;
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var files = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = old_Model.Id.ToString(), BusinessType = (int)AbpFileBusinessType.差旅费报销 });
                    if (files.Count > 0)
                        old_Model.Files = files.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();

                    if (input.FileList.Count > 0)
                    {
                        new_Model.Files = input.FileList.Select(r => new AbpFileChangeDto { FileName = r.FileName, Id = r.Id }).ToList();
                    }

                    var logs = old_Model.GetColumnAllLogs(new_Model);                   
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table, groupId);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }


        [RemoteService(IsEnabled = false)]
        [AbpAuthorize]
        public bool IsNeedCWCLAuditCLF(Guid flowID, Guid groupID, string InstanceID)
        {
            var id = Guid.Parse(InstanceID);
            var model = _repository.Get(id);
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowTaskManager>();
            var firstSender = _service.GetFirstSnderID(flowID, groupID);
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
            var userRoles = userManager.GetRoles(model.CreatorUserId.Value);
            return userRoles.Any(r => r == "CW");
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


        public void ChangeCWGLBorrowMoneyReturn(string instanceId)
        {
            var id = instanceId.ToGuid();
            var model = _repository.Get(id);
            if (model.BorrowMoneyId.HasValue)
            {
                var bRmodel = _cWGLBorrowMonetrepository.Get(model.BorrowMoneyId.Value);
                bRmodel.IsPayBack = true;
            }

        }
    }
}