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
using ZCYX.FRMSCore.Model;
using HR.Service;

namespace HR
{
    public class EmployeeResumeAppService : FRMSCoreAppServiceBase, IEmployeeResumeAppService
    { 
        private readonly IRepository<EmployeeResume, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<EducationExperience, Guid> _educationExperienceRepository;
        private readonly IRepository<WorkExperience, Guid> _workExperienceRepository;
        private readonly IRepository<EmployeeProjecExperience, Guid> _projecExperienceRepository;
        private readonly IEmployeePlanAppService _planAppService;
        private readonly IRepository<EmployeePlan, Guid> _employeePlanRepository;
        private readonly IRepository<EmployeeResult, Guid> _employeeResultRepository;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;

        public EmployeeResumeAppService(IRepository<EmployeeResume, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<EmployeeResult, Guid> employeeResultRepository
           ,WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , IRepository<EducationExperience, Guid> educationExperienceRepository, IRepository<WorkExperience, Guid> workExperienceRepository, IRepository<EmployeeProjecExperience, Guid> projecExperienceRepository, IEmployeePlanAppService planAppService, IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, IRepository<EmployeePlan, Guid> employeePlanRepository
        )
        {
            _abpFileRelationAppService = abpFileRelationAppService;
            this._repository = repository;
            _educationExperienceRepository = educationExperienceRepository;
            _workExperienceRepository = workExperienceRepository;
            _projecExperienceRepository = projecExperienceRepository;
            _planAppService = planAppService;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _employeePlanRepository = employeePlanRepository;
            _employeeResultRepository = employeeResultRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeResumeListOutputDto>> GetList(GetEmployeeResumeListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        let isExtract = _employeePlanRepository.GetAll().Any(x => x.Phone == a.Phone && x.Status >= 0)
                        select new EmployeeResumeListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Sex = a.Sex,
                            Position = a.Position,
                            Phone = a.Phone,
                            Remark = a.Remark,
                            Number = a.Number,
                            Experience = a.Experience,
                            IsExtract = isExtract,
                            CreationTime = a.CreationTime,
                            Address = a.Address,
                            Status = a.Status,
                            StatusTitle = a.Status.ToString()
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
            {
                query = query.Where(x => x.Name.Contains(input.SearchKey) || x.Position.Contains(input.SearchKey)||x.Phone.Contains(input.SearchKey));
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<EmployeeResumeListOutputDto>(toalCount, ret);
        }

        /// <summary>
        ///获取当前人员面试记录 
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        public async Task<List<EmployeePlanDto>> GetLog(Guid resumeId)
        {
            var resume = _repository.Get(resumeId);
            var plan = _employeePlanRepository.GetAll().OrderByDescending(ite => ite.ApplyCount).FirstOrDefault(ite => ite.Phone == resume.Phone);
            if (plan != null) {
                var log = _employeeResultRepository.GetAll().OrderByDescending(ite=>ite.CreationTime).Where(ite => ite.EmployeePlanId == plan.Id).ToList().MapTo<List<EmployeePlanDto>>();
                foreach (var ret in log) {
                    if (string.IsNullOrWhiteSpace(ret.AdminUserId) == false)
                    {
                        ret.AdminUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.AdminUserId);
                    }
                    if (string.IsNullOrWhiteSpace(ret.EmployeeUserIds) == false)
                    {
                        ret.EmployeeUserIds_Name = _workFlowOrganizationUnitsManager.GetNames(ret.EmployeeUserIds);
                    }
                    if (string.IsNullOrWhiteSpace(ret.MergeUserId) == false)
                    {
                        ret.MergeUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.MergeUserId);
                    }
                    if (string.IsNullOrWhiteSpace(ret.RecordUserId) == false)
                    {
                        ret.RecordUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.RecordUserId);
                    }
                    if (string.IsNullOrWhiteSpace(ret.VerifyUserId) == false)
                    {
                        ret.VerifyUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.VerifyUserId);
                    }
                }
                log[0].AdminVerifyDiscuss = plan.AdminVerifyDiscuss;
                return log;
            }
            return null;
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<EmployeeResumeOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }

            var entity = new EmployeeResumeOutputDto();
            model.MapTo(entity);
            var edu = await _educationExperienceRepository.GetAll().Where(ite => ite.EmployeeId == model.Id).ToListAsync();
            if (edu != null && edu.Count > 0)
            {
                entity.EducationExperience = edu.MapTo<List<EducationExperienceDto>>();
            }
            var work = await _workExperienceRepository.GetAll().Where(ite => ite.EmployeeId == model.Id).ToListAsync();
            if (work != null && work.Count > 0)
            {
                entity.WorkExperience = work.MapTo<List<WorkExperienceDto>>();
            }
            var exp = await _projecExperienceRepository.GetAll().Where(ite => ite.EmployeeId == model.Id).ToListAsync();
            if (exp != null && exp.Count > 0)
            {
                entity.ProjecExperience = exp.MapTo<List<EmployeeProjecExperienceDto>>();
            }
            entity.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.人力资源招聘人才库
            });
            return entity;
        }
        public async Task CreatePlan(CreateResumePlanInput input) {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.ResumeId);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            CreatePlanInput planInput = new CreatePlanInput();
            planInput.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.人力资源招聘人才库
            });
            planInput.ApplyUser = model.Name;
            planInput.Phone = model.Phone;
            planInput.ApplyJob = model.Position;
            planInput.FlowId = Guid.Parse("df798037-dc22-4b12-bbb8-71f671659613");
            planInput.FlowTitle = "招聘流程";
            planInput.ApplyPostId = input.PostId;
            model.Status = ResumeStatus.未面试;
            _repository.Update(model);
            var plan = await _planAppService.Create(planInput);
            _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput() { FlowId = planInput.FlowId, FlowTitle = planInput.FlowTitle, InStanceId = plan.InStanceId });
        }

        /// <summary>
        /// 添加一个EmployeeResume
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateEmployeeResumeInput input)
        {
            var id = Guid.NewGuid();
            var model = new EmployeeResume()
            {
                Id = id,
                Name = input.Name,
                Sex = input.Sex,
                Email = input.Email,
                Age = input.Age,
                Position = input.Position,
                Phone = input.Phone,
                Address = input.Address,
                Salary = input.Salary,
                StartingSalary = input.StartingSalary,
                IsFace = input.IsFace,
                Experience = input.Experience,
                Remark = input.Remark
            };
            if (input.WorkExperience != null && input.WorkExperience.Count > 0)
            {
                foreach (var w in input.WorkExperience)
                {
                    var wmodel = w.MapTo<WorkExperience>();
                    wmodel.EmployeeId = model.Id;
                    _workExperienceRepository.Insert(wmodel);
                }
            }
            if (input.EducationExperience != null && input.EducationExperience.Count > 0)
            {
                foreach (var e in input.EducationExperience)
                {
                    var edumodel = e.MapTo<EducationExperience>();
                    edumodel.EmployeeId = model.Id;
                    _educationExperienceRepository.Insert(edumodel);
                }
            }
            if (input.ProjecExperience != null && input.ProjecExperience.Count > 0)
            {
                foreach (var e in input.ProjecExperience)
                {

                    var expmodel = e.MapTo<EmployeeProjecExperience>();
                    expmodel.EmployeeId = model.Id;
                    _projecExperienceRepository.Insert(expmodel);
                }
            }
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.人力资源招聘人才库,
                    Files = fileList
                });
            }
            await _repository.InsertAsync(model);
        }

		/// <summary>
        /// 修改一个EmployeeResume
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateEmployeeResumeInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (model == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
               }

                model.Name = input.Name;
                model.Sex = input.Sex;
                model.Email = input.Email;
                model.Age = input.Age;
                model.Position = input.Position;
                model.Phone = input.Phone;
                model.Address = input.Address;
                model.Salary = input.Salary;
                model.StartingSalary = input.StartingSalary;
                model.IsFace = input.IsFace;
                model.Remark = input.Remark;
                model.Experience = input.Experience;

                if (input.WorkExperience != null && input.WorkExperience.Count > 0)
                {
                    var ids = input.WorkExperience.Select(ite => ite.Id).ToList();
                    _workExperienceRepository.Delete(ite => ite.EmployeeId == model.Id && !ids.Contains(ite.Id));
                    foreach (var w in input.WorkExperience)
                    {
                        if (w.Id.HasValue)
                        {
                            var wmodel = _workExperienceRepository.Get(w.Id.Value);

                            ObjectMapper.Map(w, wmodel);
                            _workExperienceRepository.Update(wmodel);
                        }
                        else
                        {
                            var wmodel = w.MapTo<WorkExperience>();
                            wmodel.EmployeeId = model.Id;
                            _workExperienceRepository.Insert(wmodel);
                        }
                    }
                }
                if (input.EducationExperience != null && input.EducationExperience.Count > 0)
                {
                    var ids = input.EducationExperience.Select(ite => ite.Id).ToList();
                    _educationExperienceRepository.Delete(ite => ite.EmployeeId == model.Id && !ids.Contains(ite.Id));
                    foreach (var e in input.EducationExperience)
                    {
                        if (e.Id.HasValue)
                        {
                            var edumodel = _educationExperienceRepository.Get(e.Id.Value);
                            ObjectMapper.Map(e, edumodel);
                            _educationExperienceRepository.Update(edumodel);
                        }
                        else
                        {
                            var edumodel = e.MapTo<EducationExperience>();
                            edumodel.EmployeeId = model.Id;
                            _educationExperienceRepository.Insert(edumodel);
                        }
                    }
                }
                if (input.ProjecExperience != null && input.ProjecExperience.Count > 0)
                {
                    var ids = input.ProjecExperience.Select(ite => ite.Id).ToList();
                    _projecExperienceRepository.Delete(ite => ite.EmployeeId == model.Id && !ids.Contains(ite.Id));
                    foreach (var e in input.ProjecExperience)
                    {
                        if (e.Id.HasValue)
                        {
                            var expmodel = _projecExperienceRepository.Get(e.Id.Value);
                            ObjectMapper.Map(e, expmodel);
                            _projecExperienceRepository.Update(expmodel);
                        }
                        else
                        {
                            var expmodel = e.MapTo<EmployeeProjecExperience>();
                            expmodel.EmployeeId = model.Id;
                            _projecExperienceRepository.Insert(expmodel);
                        }
                    }
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
                    BusinessType = (int)AbpFileBusinessType.人力资源招聘人才库,
                    Files = fileList
                });

                await _repository.UpdateAsync(model);
			   
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
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}