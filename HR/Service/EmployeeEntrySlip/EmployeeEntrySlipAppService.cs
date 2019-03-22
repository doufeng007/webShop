using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.File;
using ZCYX.FRMSCore;
using System.Linq;
using Abp.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Application;

namespace HR
{
    public class EmployeeEntrySlipAppService : FRMSCoreAppServiceBase, IEmployeeEntrySlipAppService
    {
        private readonly IRepository<EmployeeEntrySlip, Guid> _repository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<EmployeeInterview, Guid> _employeeInterviewRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workFlowOrganizationUnitsRepository;
        public EmployeeEntrySlipAppService(IRepository<EmployeeEntrySlip, Guid> repository, IRepository<Employee, Guid> employeeRepository
            , IAbpFileRelationAppService abpFileRelationAppService, IWorkFlowTaskRepository workFlowTaskRepository, WorkFlowTaskManager workFlowTaskManager
            , IRepository<EmployeeInterview, Guid> employeeInterviewRepository, IRepository<WorkFlowOrganizationUnits, long> workFlowOrganizationUnitsRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _employeeInterviewRepository = employeeInterviewRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowOrganizationUnitsRepository = workFlowOrganizationUnitsRepository;
        }

        [AbpAuthorize]
        public async Task<CreateEmployeeEntrySlipOutput> CreateAsync(CreateEmployeeEntrySlipInput input)
        {
            var employee = await _employeeRepository.GetAsync(input.EmployeeInterviewId);
            // if(employee.Status) 员工状态判断
            var model = input.MapTo<EmployeeEntrySlip>();
            model.Id = Guid.NewGuid();
            model.EmployeeInterviewId = employee.Id;
            var ret = new CreateEmployeeEntrySlipOutput();
            var retid = await _repository.InsertAndGetIdAsync(model);
            ret.InStanceId = retid.ToString();
            return ret;
        }

        [AbpAuthorize]
        public async Task UpdateAsync(UpdateEmployeeEntrySlipInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            input.MapTo(model);
            await _repository.UpdateAsync(model);
        }

        public async Task<EmployeeEntrySlipDto> GetAsync(GetEmployeeEntrySlipDtoInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            var ret = model.MapTo<EmployeeEntrySlipDto>();
            return ret;
        }

        public async Task<PagedResultDto<EmployeeEntrySlipListDto>> GetListAsync(GetEmployeeEntrySlipListInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var query = from a in _repository.GetAll()
                        join c in _employeeInterviewRepository.GetAll() on a.EmployeeInterviewId equals c.Id
                        join o in _workFlowOrganizationUnitsRepository.GetAll() on c.Department equals o.Id
                        //join u in UserManager.Users on a.UserId equals u.Id
                        join b in _workFlowTaskRepository.GetAll() on a.Id.ToString() equals b.InstanceID into g
                        where (a.CreatorUserId == user.Id || (g.Count() > 0 && g.Any(r => r.ReceiveID == user.Id)))
                        select new
                        {
                            EmployeeEntrySlip = a,
                            EmployeeInterviewName = c.Name,
                            OrgId = o.Id,
                            OrgName = o.DisplayName,
                            PostId = c.Job,
                            PostName = c.JobName,
                        };
            query = query.WhereIf(input.StartTime.HasValue, r => r.EmployeeEntrySlip.EntryDate >= input.StartTime.Value).WhereIf(input.EndTime.HasValue, r => r.EmployeeEntrySlip.EntryDate <= input.EndTime.Value);
            if (input.Status.Count > 0)
            {
                query = query.Where(r => input.Status.Contains(r.EmployeeEntrySlip.Status));
            }
            query = query.Distinct();
            var totoalCount = await query.CountAsync();
            var ret = query.OrderByDescending(r => r.EmployeeEntrySlip.CreationTime).PageBy(input).ToList();
            var data = new List<EmployeeEntrySlipListDto>();
            foreach (var item in ret)
            {
                var entity = new EmployeeEntrySlipListDto()
                {
                    Id = item.EmployeeEntrySlip.Id,
                    EmployeeInterviewId = item.EmployeeEntrySlip.EmployeeInterviewId,
                    EmployeeInterviewName = item.EmployeeInterviewName,
                    EmployeeNumber = item.EmployeeEntrySlip.EmployeeNumber,
                    EntryDate =item.EmployeeEntrySlip.EntryDate,
                    Status = item.EmployeeEntrySlip.Status,
                    StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, item.EmployeeEntrySlip.Status),
                    //UserName = item.UserName

                };
                data.Add(entity);
            }
            return new PagedResultDto<EmployeeEntrySlipListDto>(totoalCount, data);


        }



    }
}
