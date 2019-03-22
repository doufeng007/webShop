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
    public class EmployeeGoOutAppService : FRMSCoreAppServiceBase, IEmployeeGoOutAppService
    {
        private readonly IRepository<EmployeeGoOut, Guid> _repository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;



        public EmployeeGoOutAppService(IRepository<EmployeeGoOut, Guid> repository, IRepository<Employee, Guid> employeeRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , WorkFlowTaskManager workFlowTaskManager, IWorkFlowTaskRepository workFlowTaskRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<CreateEmployeeGoOutOutput> CreateAsync(CreateEmployeeGoOutInput input)
        {
            if (input.UserId.HasValue == false)
            {
                input.UserId = AbpSession.UserId;
            }
            var user = await UserManager.GetUserByIdAsync(input.UserId.Value);
            if (user == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "输入的用户不存在");
            var model = input.MapTo<EmployeeGoOut>();
            model.Id = Guid.NewGuid();
            model.TenantId = AbpSession.TenantId;
            await _repository.InsertAsync(model);
            var ret = new CreateEmployeeGoOutOutput();
            ret.InStanceId = model.Id.ToString();
            return ret;

        }

        public async Task UpdateAsync(EmployeeGoOutDto input)
        {
            var model = await _repository.GetAsync(input.Id);
            if (model != null)
            {
                var user = await UserManager.GetUserByIdAsync(input.UserId);
                if (user == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "输入的用户不存在");
                model.UserId = input.UserId;
                model.GoOutTime = input.GoOutTime;
                model.Reason = input.Reason;
                model.GoOutHour = input.GoOutHour;
                model.BackTime = input.BackTime;
                model.OutTele = input.OutTele;
                await _repository.UpdateAsync(model);
            }
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<GetEmployeeGoDtoOutput> GetAsync(GetEmployeeGoDtoInput input)
        {
            var ret = new GetEmployeeGoDtoOutput();
            var model = await _repository.GetAsync(input.Id);
            ret.Id = model.Id;
            ret.UserId = model.UserId;
            ret.UserName = (await UserManager.GetUserByIdAsync(model.UserId)).UserName;
            ret.GoOutTime = model.GoOutTime;
            ret.Reason = model.Reason;
            ret.GoOutHour = model.GoOutHour;
            ret.BackTime = model.BackTime;
            ret.OutTele = model.OutTele;
            ret.Status = model.Status;
            ret.Remark = model.Remark;
            ret.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, model.Status);
            return ret;
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<EmployeeGoOutDto>> GetListAsync(GetEmployeeGoOutListInput input)
        {

            //var query = from a in _repository.GetAll()
            //            join u in UserManager.Users on a.UserId equals u.Id
            //            select new { EmployeeGoOut = a, UserName = u.Name };
            var user = await base.GetCurrentUserAsync();
            var query = from a in _repository.GetAll()
                        join u in UserManager.Users on a.UserId equals u.Id
                        join b in _workFlowTaskRepository.GetAll() on a.Id.ToString() equals b.InstanceID into g
                        where (a.CreatorUserId == user.Id || (g.Count() > 0 && g.Any(r => r.ReceiveID == user.Id)))
                        select new { EmployeeGoOut = a, UserName = u.Name };

            query = query.WhereIf(input.StartTime.HasValue, r => r.EmployeeGoOut.GoOutTime >= input.StartTime.Value).WhereIf(input.EndTime.HasValue, r => r.EmployeeGoOut.GoOutTime <= input.EndTime.Value);

            //if (await UserManager.IsInRoleAsync(user, StaticRoleNames.Host.HR))
            //{
            //    if (input.OrgId.HasValue)
            //    {
            //        var users = _workFlowOrganizationUnitsManager.GetAllUsersById(input.OrgId.Value);
            //        if (users.Count > 0)
            //        {
            //            var userIds = users.Select(r => r.Id);
            //            query = query.Where(r => userIds.Contains(input.CreateUserId.Value));
            //        }
            //    }
            //    query = query.WhereIf(input.CreateUserId.HasValue, r => r.EmployeeGoOut.CreatorUserId == input.CreateUserId.Value);

            //}
            //else if (await UserManager.IsInRoleAsync(user, StaticRoleNames.Host.DepartmentLeader))
            //{
            //    var currentOrgId = _workFlowOrganizationUnitsManager.GetDeptByUserID(AbpSession.UserId.Value);
            //    var users = _workFlowOrganizationUnitsManager.GetAllUsersById(input.OrgId.Value);
            //    if (users.Count > 0)
            //    {
            //        var userIds = users.Select(r => r.Id);
            //        query = query.Where(r => r.EmployeeGoOut.CreatorUserId == AbpSession.UserId.Value || userIds.Contains(input.CreateUserId.Value));
            //    }
            //    else
            //    {
            //        query = query.Where(r => r.EmployeeGoOut.CreatorUserId == AbpSession.UserId.Value);
            //    }
            //}
            //else
            //{
            //    query = query.Where(r => r.EmployeeGoOut.CreatorUserId == AbpSession.UserId.Value);
            //}



            if (input.OrgId.HasValue)
            {
                var users = _workFlowOrganizationUnitsManager.GetAllUsersById(input.OrgId.Value);
                if (users.Count > 0)
                {
                    var userIds = users.Select(r => r.Id);
                    query = query.Where(r => r.EmployeeGoOut.CreatorUserId.HasValue && userIds.Contains(r.EmployeeGoOut.CreatorUserId.Value));
                }
            }
            query = query.WhereIf(input.CreateUserId.HasValue, r => r.EmployeeGoOut.CreatorUserId == input.CreateUserId.Value);



            if (input.Status.Count > 0)
            {
                query = query.Where(r => input.Status.Contains(r.EmployeeGoOut.Status));
            }



            query = query.Distinct();
            var totoalCount = await query.CountAsync();
            var ret = query.OrderByDescending(r => r.EmployeeGoOut.CreationTime).PageBy(input).ToList();
            var data = new List<EmployeeGoOutDto>();
            foreach (var item in ret)
            {
                var entity = new EmployeeGoOutDto()
                {
                    Id = item.EmployeeGoOut.Id,
                    BackTime = item.EmployeeGoOut.BackTime,
                    GoOutHour = item.EmployeeGoOut.GoOutHour,
                    GoOutTime = item.EmployeeGoOut.GoOutTime,
                    OutTele = item.EmployeeGoOut.OutTele,
                    Reason = item.EmployeeGoOut.Reason,
                    Status = item.EmployeeGoOut.Status,
                    StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, item.EmployeeGoOut.Status),
                    UserId = item.EmployeeGoOut.UserId,
                    UserName = item.UserName

                };
                data.Add(entity);
            }

            return new PagedResultDto<EmployeeGoOutDto>(totoalCount, data);

        }
    }
}
