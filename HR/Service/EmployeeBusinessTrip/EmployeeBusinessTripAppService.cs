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
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class EmployeeBusinessTripAppService : FRMSCoreAppServiceBase, IEmployeeBusinessTripAppService
    {
        private readonly IRepository<EmployeeBusinessTrip, Guid> _repository;
        private readonly IRepository<EmployeeBusinessTripMember, Guid> _employeeBusinessTripMemberRepository;
        private readonly IRepository<EmployeeBusinessTripTask, Guid> _employeeBusinessTripTaskRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;




        public EmployeeBusinessTripAppService(IRepository<EmployeeBusinessTrip, Guid> repository, IRepository<Employee, Guid> employeeRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , WorkFlowTaskManager workFlowTaskManager, IWorkFlowTaskRepository workFlowTaskRepository, IRepository<EmployeeBusinessTripMember, Guid> employeeBusinessTripMemberRepository
            , IRepository<EmployeeBusinessTripTask, Guid> employeeBusinessTripTaskRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _employeeBusinessTripMemberRepository = employeeBusinessTripMemberRepository;
            _employeeBusinessTripTaskRepository = employeeBusinessTripTaskRepository;
        }

        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<CreateEmployeeBusinessTripOutput> CreateAsync(CreateEmployeeBusinessTripInput input)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var model = input.MapTo<EmployeeBusinessTrip>();
            model.Id = Guid.NewGuid();
            model.TenantId = AbpSession.TenantId;
            model.UserId = AbpSession.UserId.Value;
            await _repository.InsertAsync(model);
            foreach (var member in input.EmployeeBusinessTripMember)
            {
                var memberDto = new EmployeeBusinessTripMember() { Id = Guid.NewGuid(), BusinessTripId = model.Id, TenantId = AbpSession.TenantId, UserId = member.UserId, Remark = member.Remark };
                await _employeeBusinessTripMemberRepository.InsertAsync(memberDto);
            }
            foreach (var task in input.EmployeeBusinessTripTask)
            {
                var taskDto = new EmployeeBusinessTripTask() { Id = Guid.NewGuid(), NotInPlan = false, BusinessTripId = model.Id, TaskName = task.TaskName, Remark = task.Remark, CompleteStatus = (int)EmployeeBusinessTripTaskCompleteStatus.未报告 };
                await _employeeBusinessTripTaskRepository.InsertAsync(taskDto);
            }
            var ret = new CreateEmployeeBusinessTripOutput();
            ret.InStanceId = model.Id.ToString();
            return ret;

        }

        public async Task UpdateAsync(UpdateEmployeeBusinessTripInput input)
        {
            var model = await _repository.GetAsync(input.Id);
            if (model != null)
            {
                model.Destination = input.Destination;
                model.PreBeginDate = input.PreBeginDate;
                model.PreEndDate = input.PreEndDate;
                model.PreSchedule = input.PreSchedule;
                model.BeginDate = input.BeginDate;
                model.EndDate = input.EndDate;
                model.Schedule = input.Schedule;
                model.FeePlan = input.FeePlan;
                model.PreFeeTotal = input.PreFeeTotal;
                model.FeeTotal = input.FeeTotal;
                model.FeeAccommodation = input.FeeAccommodation;
                model.FeeOther = input.FeeOther;
                model.Remark = input.Remark;
                var exit_tasks = await _employeeBusinessTripTaskRepository.GetAll().Where(r => r.BusinessTripId == model.Id).ToListAsync();
                var add_Tasks = input.EmployeeBusinessTripTask.Where(r => !r.Id.HasValue);
                foreach (var add_task in add_Tasks)
                {
                    var taskDto = new EmployeeBusinessTripTask() { Id = Guid.NewGuid(), BusinessTripId = model.Id, TenantId = AbpSession.TenantId, TaskName = add_task.TaskName, Remark = add_task.Remark };
                    await _employeeBusinessTripTaskRepository.InsertAsync(taskDto);
                }
                var less_tasks = exit_tasks.Select(r => r.Id).ToList().Except(input.EmployeeBusinessTripTask.Where(r => r.Id.HasValue).Select(r => r.Id.Value).ToList());
                foreach (var less_task in less_tasks)
                {
                    await _employeeBusinessTripTaskRepository.DeleteAsync(exit_tasks.FirstOrDefault(r => r.Id == less_task));
                }
                var update_tasks = exit_tasks.Select(r => r.Id).ToList().Except(less_tasks);
                foreach (var update_task in update_tasks)
                {
                    var updatemodel = exit_tasks.FirstOrDefault(r => r.Id == update_task);
                    var newModel = input.EmployeeBusinessTripTask.FirstOrDefault(r => r.Id == updatemodel.Id);
                    updatemodel.TaskName = newModel.TaskName;
                    updatemodel.Remark = newModel.Remark;
                    await _employeeBusinessTripTaskRepository.UpdateAsync(updatemodel);
                }

                var exit_members = await _employeeBusinessTripMemberRepository.GetAll().Where(r => r.BusinessTripId == model.Id).ToListAsync();
                var add_members = input.EmployeeBusinessTripMember.Where(r => !r.Id.HasValue);
                foreach (var add_member in add_members)
                {
                    var addmemberModel = new EmployeeBusinessTripMember() { Id = Guid.NewGuid(), BusinessTripId = model.Id, TenantId = AbpSession.TenantId, UserId = add_member.UserId };
                    await _employeeBusinessTripMemberRepository.InsertAsync(addmemberModel);
                }

                var less_members = exit_members.Select(r => r.Id).ToList().Except(input.EmployeeBusinessTripMember.Where(r => r.Id.HasValue).Select(r => r.Id.Value).ToList());
                foreach (var less_member in less_members)
                {
                    await _employeeBusinessTripMemberRepository.DeleteAsync(exit_members.FirstOrDefault(r => r.Id == less_member));
                }
                var update_members = exit_members.Select(r => r.Id).ToList().Except(less_members);
                foreach (var update_member in update_members)
                {
                    var updatemodel = exit_members.FirstOrDefault(r => r.Id == update_member);
                    var newModel = input.EmployeeBusinessTripMember.FirstOrDefault(r => r.Id == updatemodel.Id);
                    updatemodel.UserId = newModel.UserId;
                    await _employeeBusinessTripMemberRepository.UpdateAsync(updatemodel);
                }


                await _repository.UpdateAsync(model);
            }
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<GetEmployeeBusinessTripOutput> GetAsync(GetEmployeeBusinessTripDtoInput input)
        {
            var ret = new GetEmployeeBusinessTripOutput();
            var query = from a in _repository.GetAll()
                        join b in _employeeBusinessTripMemberRepository.GetAll() on a.Id equals b.BusinessTripId into member
                        join c in _employeeBusinessTripTaskRepository.GetAll() on a.Id equals c.BusinessTripId into task
                        where a.Id == input.Id
                        select new
                        {
                            model = a,
                            members = member != null ? from aa in member
                                                       join u in UserManager.Users on aa.UserId equals u.Id
                                                       select new { Id = aa.Id, UserId = u.Id, UserName = u.Name, ReMark = aa.Remark } : null,
                            tasks = task ?? null
                        };

            var data = await query.FirstOrDefaultAsync();
            if (data == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未获取到数据");
            ret.Id = data.model.Id;
            ret.UserId = data.model.UserId;
            ret.UserName = (await UserManager.GetUserByIdAsync(data.model.UserId)).UserName;
            ret.Destination = data.model.Destination;
            ret.PreBeginDate = data.model.PreBeginDate;
            ret.PreEndDate = data.model.PreEndDate;
            ret.FeePlan = data.model.FeePlan;
            ret.PreSchedule = data.model.PreSchedule;
            ret.BeginDate = data.model.BeginDate;
            ret.EndDate = data.model.EndDate;
            ret.Schedule = data.model.Schedule;
            ret.PreFeeTotal = data.model.PreFeeTotal;
            ret.FeeTotal = data.model.FeeTotal;
            ret.FeeAccommodation = data.model.FeeAccommodation;
            ret.FeeOther = data.model.FeeOther;
            ret.Remark = data.model.Remark;
            ret.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, data.model.Status);
            if (data.members != null)
            {
                foreach (var member in data.members)
                {
                    var entity = new EmployeeBusinessTripMemberpDto() { Id = member.Id, UserId = member.UserId, UserName = member.UserName, Remark = member.ReMark };
                    ret.EmployeeBusinessTripMember.Add(entity);
                }
            }
            if (data.tasks != null)
            {
                foreach (var task in data.tasks)
                {
                    var entity = new EmployeeBusinessTripTaskDto()
                    {
                        Id = task.Id,
                        CompleteStatus = (EmployeeBusinessTripTaskCompleteStatus)task.CompleteStatus,
                        NotInPlan = task.NotInPlan,
                        Remark = task.Remark,
                        TaskName = task.TaskName
                    };
                    ret.EmployeeBusinessTripTask.Add(entity);
                }
            }
            return ret;
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<EmployeeBusinessTripDto>> GetListAsync(GetEmployeeBusinessTripListInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var query = from a in _repository.GetAll()
                        join u in UserManager.Users on a.UserId equals u.Id
                        join b in _workFlowTaskRepository.GetAll() on a.Id.ToString() equals b.InstanceID into g
                        where (a.CreatorUserId == user.Id || (g.Count() > 0 && g.Any(r => r.ReceiveID == user.Id)))
                        select new { EmployeeBusinessTrip = a, UserName = u.Name };
            query = query.WhereIf(input.StartTime.HasValue, r => r.EmployeeBusinessTrip.PreBeginDate >= input.StartTime.Value).WhereIf(input.EndTime.HasValue, r => r.EmployeeBusinessTrip.PreEndDate <= input.EndTime.Value);
            if (input.Status.Count > 0)
            {
                query = query.Where(r => input.Status.Contains(r.EmployeeBusinessTrip.Status));
            }

            query = query.Distinct();
            var totoalCount = await query.CountAsync();
            var ret = query.OrderByDescending(r => r.EmployeeBusinessTrip.CreationTime).PageBy(input).ToList();
            var data = new List<EmployeeBusinessTripDto>();
            foreach (var item in ret)
            {
                var entity = new EmployeeBusinessTripDto()
                {
                    Id = item.EmployeeBusinessTrip.Id,
                    Status = item.EmployeeBusinessTrip.Status,
                    StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, item.EmployeeBusinessTrip.Status),
                    UserId = item.EmployeeBusinessTrip.UserId,
                    BeginDate = item.EmployeeBusinessTrip.BeginDate,
                    EndDate = item.EmployeeBusinessTrip.EndDate,
                    PreBeginDate = item.EmployeeBusinessTrip.PreBeginDate,
                    PreEndDate = item.EmployeeBusinessTrip.PreEndDate,
                    UserName = item.UserName

                };
                data.Add(entity);
            }

            return new PagedResultDto<EmployeeBusinessTripDto>(totoalCount, data);

        }
    }
}
