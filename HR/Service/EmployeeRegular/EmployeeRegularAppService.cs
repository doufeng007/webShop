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

namespace HR
{
    public class EmployeeRegularAppService : FRMSCoreAppServiceBase, IEmployeeRegularAppService
    {
        private readonly IRepository<EmployeeRegular, Guid> _repository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        public EmployeeRegularAppService(IRepository<EmployeeRegular, Guid> repository, IRepository<Employee, Guid> employeeRepository
            , IAbpFileRelationAppService abpFileRelationAppService, IWorkFlowTaskRepository workFlowTaskRepository, WorkFlowTaskManager workFlowTaskManager)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }

        [AbpAuthorize]
        public async Task<CreateEmployeeRegularOutput> CreateAsync(CreateEmployeeRegularInput input)
        {
            var employee = await _employeeRepository.GetAsync(input.EmployeeId);
            // if(employee.Status) 员工状态判断
            var model = input.MapTo<EmployeeRegular>();
            model.Id = Guid.NewGuid();
            model.UserId = employee.UserId.Value;
            var ret = new CreateEmployeeRegularOutput();
            var retid = await _repository.InsertAndGetIdAsync(model);
            ret.InStanceId = retid.ToString();
            return ret;
        }

        [AbpAuthorize]
        public async Task UpdateAsync(UpdateEmployeeRegularInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            model.ApplyDate = input.ApplyDate;
            model.StrialBeginTime = input.StrialBeginTime;
            model.StrialEndTime = input.StrialEndTime;
            model.Remark = input.Remark;
            model.WorkSummary = input.WorkSummary;
            await _repository.UpdateAsync(model);
        }

        [AbpAuthorize]
        public async Task UpdateWorkSummaryAsync(UpdateWorkSummaryInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            var fileList = new List<AbpFileListInput>();
            foreach (var item in input.Files)
            {
                var fileone = new AbpFileListInput() { Id = item };
                fileList.Add(fileone);
            }

            _abpFileRelationAppService.Create(new CreateFileRelationsInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.转正业绩资料,
                Files = fileList
            });

        }

        public async Task<EmployeeRegularDto> GetAsync(GetEmployeeRegularDtoInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            var ret = model.MapTo<EmployeeRegularDto>();
            ret.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.Id.ToString(), BusinessType = (int)AbpFileBusinessType.转正业绩资料 });
            return ret;

        }

        public async Task<PagedResultDto<EmployeeRegularListOut>> GetListAsync(EmployeeRegularListInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var query = from a in _repository.GetAll()
                        join u in UserManager.Users on a.UserId equals u.Id
                        join b in _workFlowTaskRepository.GetAll() on a.Id.ToString() equals b.InstanceID into g
                        where (a.CreatorUserId == user.Id || (g.Count() > 0 && g.Any(r => r.ReceiveID == user.Id)))
                        select new { EmployeeRegular = a, UserName = u.Name };
            query = query.WhereIf(input.StartTime.HasValue, r => r.EmployeeRegular.ApplyDate >= input.StartTime.Value).WhereIf(input.EndTime.HasValue, r => r.EmployeeRegular.ApplyDate <= input.EndTime.Value);
            if (input.Status.Count > 0)
            {
                query = query.Where(r => input.Status.Contains(r.EmployeeRegular.Status));
            }
            query = query.Distinct();
            var totoalCount = await query.CountAsync();
            var ret = query.OrderByDescending(r => r.EmployeeRegular.CreationTime).PageBy(input).ToList();
            var data = new List<EmployeeRegularListOut>();
            foreach (var item in ret)
            {
                var entity = new EmployeeRegularListOut()
                {
                    Id = item.EmployeeRegular.Id,
                    Status = item.EmployeeRegular.Status,
                    StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, item.EmployeeRegular.Status),
                    UserName = item.UserName

                };
                data.Add(entity);
            }
            return new PagedResultDto<EmployeeRegularListOut>(totoalCount, data);


        }



    }
}
