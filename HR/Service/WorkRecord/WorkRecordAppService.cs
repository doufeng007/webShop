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
using Abp.Extensions;
using Abp.WorkFlow;
using Abp.Authorization;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using CWGL;

namespace HR
{
    public class WorkRecordAppService : FRMSCoreAppServiceBase, IWorkRecordAppService
    {
        private readonly IRepository<WorkRecord, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<ExpertDataBase, Guid> _expertDataBaseRepository;
        private readonly IRepository<CollaborativeInstitutions, Guid> _collaborativeInstitutionsRepository;
        private readonly IRepository<LegalAdviser, Guid> _legalAdviserRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<Lecturer, Guid> _lecturerRepository;


        public WorkRecordAppService(IRepository<WorkRecord, Guid> repository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<ExpertDataBase, Guid> expertDataBaseRepository,
            IRepository<CollaborativeInstitutions, Guid> collaborativeInstitutionsRepository, IRepository<LegalAdviser, Guid> legalAdviserRepository
            , IWorkFlowTaskRepository workFlowTaskRepository, IRepository<Lecturer, Guid> lecturerRepository)
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _collaborativeInstitutionsRepository = collaborativeInstitutionsRepository;
            _expertDataBaseRepository = expertDataBaseRepository;
            _legalAdviserRepository = legalAdviserRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _lecturerRepository = lecturerRepository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<WorkRecordListOutputDto>> GetList(GetWorkRecordListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _collaborativeInstitutionsRepository.GetAll() on a.BusinessId equals b.Id into g1
                        from b in g1.DefaultIfEmpty()
                        join c in _expertDataBaseRepository.GetAll() on a.BusinessId equals c.Id into g2
                        from c in g2.DefaultIfEmpty()
                        join d in _legalAdviserRepository.GetAll() on a.BusinessId equals d.Id into g3
                        from d in g3.DefaultIfEmpty()
                        join e in _lecturerRepository.GetAll() on a.BusinessId equals e.Id into g4
                        from e in g4.DefaultIfEmpty()
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                             x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                             x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new WorkRecordListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.BusinessType == (int)CollaborativePersonnel.专家 ? c.Name :
                            a.BusinessType == (int)CollaborativePersonnel.协作单位 ? b.Name :
                            a.BusinessType == (int)CollaborativePersonnel.培训讲师 ? e.Name :
                            a.BusinessType == (int)CollaborativePersonnel.法律顾问 ? d.Name : "",
                            Head = a.BusinessType == (int)CollaborativePersonnel.协作单位 ? b.Head : "-",
                            Function = a.BusinessType == (int)CollaborativePersonnel.专家 ? c.Function :
                            a.BusinessType == (int)CollaborativePersonnel.协作单位 ? b.Function :
                            a.BusinessType == (int)CollaborativePersonnel.培训讲师 ? "-" :
                            a.BusinessType == (int)CollaborativePersonnel.法律顾问 ? d.Function : "",
                            //UserId = a.UserId,
                            BusinessId = a.BusinessId,
                            BusinessType = a.BusinessType,
                            Content = a.Content,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            Remuneration = a.Remuneration,
                            DataPerformance = a.DataPerformance,
                            NoDataPerformance = a.NoDataPerformance,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2,
                        };

            if (input.BusinessId.HasValue)
            {
                query = query.Where(ite => ite.BusinessId == input.BusinessId.Value && ite.BusinessType == (int)input.BusinessType);
            }

            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();


            foreach (var item in ret)
            {

                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<WorkRecordListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<WorkRecordOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = new WorkRecordOutputDto();
            var db_model = await _repository.GetAsync(id);
            if (db_model.BusinessType == (int)CollaborativePersonnel.协作单位)
            {
                model = (from a in _repository.GetAll()
                         join b in _collaborativeInstitutionsRepository.GetAll() on a.BusinessId equals b.Id
                         where a.Id == id
                         select new WorkRecordOutputDto()
                         {
                             Address = b.Address,
                             BankDeposit = b.BankDeposit,
                             BankName = b.BankName,
                             BankNum = b.BankNum,
                             BusinessId = a.BusinessId,
                             BusinessType = a.BusinessType,
                             Content = a.Content,
                             CreationTime = a.CreationTime,
                             EndTime = a.EndTime,
                             Function = b.Function,
                             Head = b.Head,
                             Id = a.Id,
                             Name = b.Name,
                             Remuneration = a.Remuneration,
                             ScaleNum = b.ScaleNum,
                             StartTime = a.StartTime,
                             DataPerformance = a.DataPerformance,
                             NoDataPerformance = a.NoDataPerformance,
                             Status = a.Status,
                             Tel = b.Tel,
                             UserId = a.CreatorUserId.Value,
                         }).FirstOrDefault();
            }
            else if (db_model.BusinessType == (int)CollaborativePersonnel.专家)
            {
                model = (from a in _repository.GetAll()
                         join b in _expertDataBaseRepository.GetAll() on a.BusinessId equals b.Id
                         where a.Id == id
                         select new WorkRecordOutputDto()
                         {
                             Address = "-",
                             BankDeposit = b.BankDeposit,
                             BankName = b.BankName,
                             BankNum = b.BankNum,
                             BusinessId = a.BusinessId,
                             BusinessType = a.BusinessType,
                             Content = a.Content,
                             CreationTime = a.CreationTime,
                             EndTime = a.EndTime,
                             Function = b.Function,
                             Head = "-",
                             Id = a.Id,
                             Name = b.Name,
                             Remuneration = a.Remuneration,
                             ScaleNum = 0,
                             StartTime = a.StartTime,
                             Status = a.Status,
                             DataPerformance = a.DataPerformance,
                             NoDataPerformance = a.NoDataPerformance,
                             Tel = b.Tel,
                             UserId = a.CreatorUserId.Value,
                         }).FirstOrDefault();
            }
            else if (db_model.BusinessType == (int)CollaborativePersonnel.法律顾问)
            {
                model = (from a in _repository.GetAll()
                         join b in _legalAdviserRepository.GetAll() on a.BusinessId equals b.Id
                         where a.Id == id
                         select new WorkRecordOutputDto()
                         {
                             Address = "-",
                             BankDeposit = b.BankDeposit,
                             BankName = b.BankName,
                             BankNum = b.BankNum,
                             BusinessId = a.BusinessId,
                             BusinessType = a.BusinessType,
                             Content = a.Content,
                             CreationTime = a.CreationTime,
                             EndTime = a.EndTime,
                             Function = b.Function,
                             Head = "-",
                             Id = a.Id,
                             Name = b.Name,
                             Remuneration = a.Remuneration,
                             ScaleNum = 0,
                             StartTime = a.StartTime,
                             Status = a.Status,
                             DataPerformance = a.DataPerformance,
                             NoDataPerformance = a.NoDataPerformance,
                             Tel = b.Tel,
                             UserId = a.CreatorUserId.Value,
                         }).FirstOrDefault();
            }
            else if (db_model.BusinessType == (int)CollaborativePersonnel.培训讲师)
            {
                model = (from a in _repository.GetAll()
                         join b in _lecturerRepository.GetAll() on a.BusinessId equals b.Id
                         where a.Id == id
                         select new WorkRecordOutputDto()
                         {
                             Address = "-",
                             BankDeposit = b.OpenBank,
                             BankName = b.Bank,
                             BankNum = b.BankId,
                             BusinessId = a.BusinessId,
                             BusinessType = a.BusinessType,
                             Content = a.Content,
                             CreationTime = a.CreationTime,
                             EndTime = a.EndTime,
                             Function = "-",
                             Head = "-",
                             Id = a.Id,
                             Name = b.Name,
                             Remuneration = a.Remuneration,
                             ScaleNum = 0,
                             StartTime = a.StartTime,
                             DataPerformance = a.DataPerformance,
                             NoDataPerformance = a.NoDataPerformance,
                             Status = a.Status,
                             Tel = b.Tel,
                             UserId = a.CreatorUserId.Value,
                         }).FirstOrDefault();
            }


            if (model.Id == Guid.NewGuid())
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            if (model.UserId != 0)
            {
                var userModel = await UserManager.GetUserByIdAsync(model.UserId);
                model.UserId_Name = userModel.Name;
            }
            else
            {
                if (model.BusinessType == (int)CollaborativePersonnel.专家)
                {
                    model.UserId_Name = (await _expertDataBaseRepository.FirstOrDefaultAsync(r => r.Id == model.BusinessId)).Name;
                }
                else if (model.BusinessType == (int)CollaborativePersonnel.协作单位)
                {
                    model.UserId_Name = (await _collaborativeInstitutionsRepository.FirstOrDefaultAsync(r => r.Id == model.BusinessId)).Name;
                }
                else if (model.BusinessType == (int)CollaborativePersonnel.法律顾问)
                {
                    model.UserId_Name = (await _legalAdviserRepository.FirstOrDefaultAsync(r => r.Id == model.BusinessId)).Name;
                }
                else if (model.BusinessType == (int)CollaborativePersonnel.培训讲师)
                {
                    model.UserId_Name = (await _lecturerRepository.FirstOrDefaultAsync(r => r.Id == model.BusinessId)).Name;
                }
                else
                {
                }
            }
            return model;
        }
        /// <summary>
        /// 添加一个WorkRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateWorkRecordInput input)
        {
            var ret = new InitWorkFlowOutput();
            var newmodel = new WorkRecord()
            {
                UserId = input.UserId,
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType,
                Content = input.Content,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                DataPerformance = input.DataPerformance,
                NoDataPerformance = input.NoDataPerformance,
                Remuneration = input.Remuneration,
            };
            await _repository.InsertAsync(newmodel);
            ret.InStanceId = newmodel.Id.ToString();
            return ret;
        }

        /// <summary>
        /// 修改一个WorkRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateWorkRecordInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var old_Model = dbmodel.DeepClone();
                dbmodel.UserId = input.UserId;
                dbmodel.BusinessId = input.BusinessId;
                dbmodel.BusinessType = input.BusinessType;
                dbmodel.Content = input.Content;
                dbmodel.StartTime = input.StartTime;
                dbmodel.EndTime = input.EndTime;
                dbmodel.DataPerformance = input.DataPerformance;
                dbmodel.NoDataPerformance = input.NoDataPerformance;
                dbmodel.Remuneration = input.Remuneration;
                //dbmodel.Status = input.Status;
                input.FACData.BusinessId = input.Id.ToString();
                await _repository.UpdateAsync(dbmodel);


                var groupId = Guid.NewGuid();
                input.FACData.GroupId = groupId;
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table, groupId);
                }
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

        private WorkRecordChangeDto GetChangeModel(WorkRecord model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<WorkRecordChangeDto>();
            return ret;
        }
    }
}