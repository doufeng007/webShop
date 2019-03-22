using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Application;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class EmployeeWorkOvertimeAppService : FRMSCoreAppServiceBase, IEmployeeWorkOvertimeAppService
    {
        private readonly IRepository<EmployeeWorkOvertime, Guid> _repository;
        private readonly IRepository<EmployeeWorkOvertimeMember, Guid> _employeeWorkOvertimeMemberRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;




        public EmployeeWorkOvertimeAppService(IRepository<EmployeeWorkOvertime, Guid> repository, IRepository<Employee, Guid> employeeRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , WorkFlowTaskManager workFlowTaskManager, IWorkFlowTaskRepository workFlowTaskRepository, IRepository<EmployeeWorkOvertimeMember, Guid> employeeWorkOvertimeMemberRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _employeeWorkOvertimeMemberRepository = employeeWorkOvertimeMemberRepository;
        }

        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<CreateEmployeeWorkOvertimeOutput> CreateAsync(CreateEmployeeWorkOvertimeInput input)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var model = input.MapTo<EmployeeWorkOvertime>();
            model.Id = Guid.NewGuid();
            model.TenantId = AbpSession.TenantId;
            model.UserId = AbpSession.UserId.Value;
            await _repository.InsertAsync(model);
            foreach (var member in input.EmployeeWorkOvertimeMember)
            {
                var memberDto = new EmployeeWorkOvertimeMember() { Id = Guid.NewGuid(), WorkOvertimeId = model.Id, TenantId = AbpSession.TenantId, UserId = member.UserId };
                await _employeeWorkOvertimeMemberRepository.InsertAsync(memberDto);
            }
            var ret = new CreateEmployeeWorkOvertimeOutput();
            ret.InStanceId = model.Id.ToString();
            return ret;

        }

        public async Task UpdateAsync(UpdateEmployeeWorkOvertimeInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            if (model != null)
            {
                model.ApplyDate = input.ApplyDate;
                model.Reason = input.Reason;
                model.PreHours = input.PreHours;
                model.Hours = input.Hours;
                model.Remark = input.Remark;

                var exit_members = _employeeWorkOvertimeMemberRepository.GetAll().Where(r => r.WorkOvertimeId == model.Id);
                var add_members = input.EmployeeWorkOvertimeMember.Where(r => !r.Id.HasValue);
                foreach (var add_member in add_members)
                {
                    var addmemberModel = new EmployeeWorkOvertimeMember() { Id = Guid.NewGuid(), WorkOvertimeId = model.Id, TenantId = AbpSession.TenantId, UserId = add_member.UserId };
                    await _employeeWorkOvertimeMemberRepository.InsertAsync(addmemberModel);
                }

                var less_members = exit_members.Select(r => r.Id).ToList().Except(input.EmployeeWorkOvertimeMember.Where(r => r.Id.HasValue).Select(r => r.Id.Value).ToList());
                foreach (var less_member in less_members)
                {
                    await _employeeWorkOvertimeMemberRepository.DeleteAsync(exit_members.FirstOrDefault(r => r.Id == less_member));
                }
                var update_members = exit_members.Select(r => r.Id).ToList().Except(less_members);
                foreach (var update_member in update_members)
                {
                    var updatemodel = exit_members.FirstOrDefault(r => r.Id == update_member);
                    var newModel = input.EmployeeWorkOvertimeMember.FirstOrDefault(r => r.Id == updatemodel.Id);
                    updatemodel.UserId = newModel.UserId;
                    await _employeeWorkOvertimeMemberRepository.UpdateAsync(updatemodel);
                }


                await _repository.UpdateAsync(model);
            }
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<GetEmployeeWorkOvertimeOutput> GetAsync(GetEmployeeWorkOvertimeDtoInput input)
        {
            var ret = new GetEmployeeWorkOvertimeOutput();
            var query = from a in _repository.GetAll()
                        join b in _employeeWorkOvertimeMemberRepository.GetAll() on a.Id equals b.WorkOvertimeId into member
                        where a.Id == input.Id
                        select new
                        {
                            model = a,
                            members = from aa in member
                                      join u in UserManager.Users on aa.UserId equals u.Id
                                      select new { Id = aa.Id, UserId = u.Id, UserName = u.Name },
                        };
            if (query == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未获取到数据");
            }
            var data = await query.FirstOrDefaultAsync();
            ret.Id = data.model.Id;
            ret.UserId = data.model.UserId;
            ret.UserName = (await UserManager.GetUserByIdAsync(data.model.UserId)).UserName;
            ret.ApplyDate = data.model.ApplyDate;
            ret.Reason = data.model.Reason;
            ret.PreHours = data.model.PreHours;
            ret.Hours = data.model.Hours;
            ret.Remark = data.model.Remark;
            ret.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, data.model.Status);
            foreach (var member in data.members)
            {
                var entity = new EmployeeWorkOvertimeMemberpDto() { Id = member.Id, UserId = member.UserId, UserName = member.UserName };
                ret.EmployeeWorkOvertimeMember.Add(entity);
            }
            return ret;
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<EmployeeWorkOvertimeDto>> GetListAsync(GetEmployeeWorkOvertimeListInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var query = from a in _repository.GetAll()
                        join u in UserManager.Users on a.UserId equals u.Id
                        join b in _workFlowTaskRepository.GetAll() on a.Id.ToString() equals b.InstanceID into g
                        where (a.CreatorUserId == user.Id || (g.Count() > 0 && g.Any(r => r.ReceiveID == user.Id)))
                        select new { EmployeeWorkOvertime = a, UserName = u.Name };
            query = query.WhereIf(input.StartTime.HasValue, r => r.EmployeeWorkOvertime.ApplyDate >= input.StartTime.Value).WhereIf(input.EndTime.HasValue, r => r.EmployeeWorkOvertime.ApplyDate <= input.EndTime.Value);
            if (input.Status.Count > 0)
            {
                query = query.Where(r => input.Status.Contains(r.EmployeeWorkOvertime.Status));
            }
            query = query.Distinct();
            var totoalCount = await query.CountAsync();
            var ret = query.OrderByDescending(r => r.EmployeeWorkOvertime.CreationTime).PageBy(input).ToList();
            var data = new List<EmployeeWorkOvertimeDto>();
            foreach (var item in ret)
            {
                var entity = new EmployeeWorkOvertimeDto()
                {
                    Id = item.EmployeeWorkOvertime.Id,
                    Status = item.EmployeeWorkOvertime.Status,
                    StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, item.EmployeeWorkOvertime.Status),
                    UserId = item.EmployeeWorkOvertime.UserId,
                    ApplyDate = item.EmployeeWorkOvertime.ApplyDate,
                    Reason = item.EmployeeWorkOvertime.Reason,
                    PreHours = item.EmployeeWorkOvertime.PreHours,
                    Hours = item.EmployeeWorkOvertime.Hours,
                    UserName = item.UserName

                };
                data.Add(entity);
            }

            return new PagedResultDto<EmployeeWorkOvertimeDto>(totoalCount, data);

        }
    }
}
