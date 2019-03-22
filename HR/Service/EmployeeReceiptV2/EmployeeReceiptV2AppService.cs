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

namespace HR
{
    public class EmployeeReceiptV2AppService : FRMSCoreAppServiceBase, IEmployeeReceiptV2AppService
    {
        private readonly IRepository<EmployeeReceiptV2, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        public EmployeeReceiptV2AppService(IRepository<EmployeeReceiptV2, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeReceiptV2ListOutputDto>> GetList(GetEmployeeReceiptV2ListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted).WhereIf(!input.PostName.IsNullOrEmpty(), x => x.PostName.Contains(input.PostName)).WhereIf(!input.SearchKey.IsNullOrEmpty(), x => x.PostName.Contains(input.SearchKey) || x.Number.Contains(input.SearchKey) || x.Remark.Contains(input.SearchKey))
                        let openModel = (from b in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value) select b)
                        select new EmployeeReceiptV2ListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            DepartmentId = a.DepartmentId,
                            PostName = a.PostName,
                            Number = a.Number,
                            Address = a.Address,
                            PostDemand = a.PostDemand,
                            DemandGrade = a.DemandGrade,
                            PostDemandName = a.PostDemandName,
                            DemandGradeName = a.DemandGradeName,
                            Sex = a.Sex,
                            Age = a.Age,
                            Status = a.Status,
                            Education = a.Education,
                            EducationName = a.EducationName,
                            ProfessionalRequirements = a.ProfessionalRequirements,
                            SkillRequirement = a.SkillRequirement,
                            CertificateRequirements = a.CertificateRequirements,
                            OtherRequirements = a.OtherRequirements,
                            OperatingDuty = a.OperatingDuty,
                            SalaryProposal = a.SalaryProposal,
                            Remark = a.Remark,
                            DepartmentName = a.DepartmentName ,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                        };
            var toalCount = query.Count();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<EmployeeReceiptV2ListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public EmployeeReceiptV2OutputDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = _repository.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<EmployeeReceiptV2OutputDto>();
        }
        /// <summary>
        /// 添加一个EmployeeReceiptV2
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(CreateEmployeeReceiptV2Input input)
        {
            var newmodel = new EmployeeReceiptV2()
            {
                DepartmentId = input.DepartmentId,
                PostName = input.PostName,
                Number = input.Number,
                Address = input.Address,
                PostDemand = input.PostDemand,
                DemandGrade = input.DemandGrade,
                PostDemandName = input.PostDemandName,
                DemandGradeName = input.DemandGradeName,
                Sex = input.Sex,
                Age = input.Age,
                Education = input.Education,
                EducationName = input.EducationName,
                ProfessionalRequirements = input.ProfessionalRequirements,
                SkillRequirement = input.SkillRequirement,
                CertificateRequirements = input.CertificateRequirements,
                OtherRequirements = input.OtherRequirements,
                OperatingDuty = input.OperatingDuty,
                SalaryProposal = input.SalaryProposal,
                Remark = input.Remark,
                DepartmentName = input.DepartmentName
            };
            newmodel.Status = 0;
            _repository.Insert(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个EmployeeReceiptV2
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateEmployeeReceiptV2Input input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = _repository.FirstOrDefault(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                var logModel = new EmployeeReceiptV2();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<EmployeeReceiptV2>();
                }
                dbmodel.DepartmentId = input.DepartmentId;
                dbmodel.PostName = input.PostName;
                dbmodel.Number = input.Number;
                dbmodel.Address = input.Address;
                dbmodel.PostDemand = input.PostDemand;
                dbmodel.DemandGrade = input.DemandGrade;
                dbmodel.PostDemandName = input.PostDemandName;
                dbmodel.DemandGradeName = input.DemandGradeName;
                dbmodel.Sex = input.Sex;
                dbmodel.Age = input.Age;
                dbmodel.Education = input.Education;
                dbmodel.EducationName = input.EducationName;
                dbmodel.ProfessionalRequirements = input.ProfessionalRequirements;
                dbmodel.SkillRequirement = input.SkillRequirement;
                dbmodel.CertificateRequirements = input.CertificateRequirements;
                dbmodel.OtherRequirements = input.OtherRequirements;
                dbmodel.OperatingDuty = input.OperatingDuty;
                dbmodel.SalaryProposal = input.SalaryProposal;
                dbmodel.Remark = input.Remark;
                dbmodel.DepartmentName = input.DepartmentName;
                _repository.Update(dbmodel);
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.CodeValErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.InStanceId.ToString(), flowModel.TitleField.Table);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
        private EmployeeReceiptV2LogDto GetChangeModel(EmployeeReceiptV2 model)
        {
            var ret = model.MapTo<EmployeeReceiptV2LogDto>();
            return ret;
        }
    }
}