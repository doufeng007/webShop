using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.Application.Services.Dto;
using Abp.UI;
using Abp.Organizations;
using System.Linq;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Application;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using HR.Service.EmployeeRequire.Dto;
using Abp.Authorization;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class EmployeeRequireAppService : IEmployeeRequireAppService
    {
        private readonly IRepository<EmployeeRequire, Guid> _repository;
        private readonly IRepository<OrganizationUnit, long> _organization;
        private readonly IRepository<User, long> _user;
        private readonly IRepository<PostInfo, Guid> _postinfo;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        public EmployeeRequireAppService(
            IRepository<EmployeeRequire, Guid> repository,
            IRepository<OrganizationUnit, long> organization,
            IRepository<User, long> user,
            IRepository<PostInfo, Guid> postinfo,
            WorkFlowTaskManager workFlowTaskManager
            )
        {
            _organization = organization;
            _repository = repository;
            _user = user;
            _postinfo = postinfo;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<EmployeeRequireOutput> CreateAsync(EmployeeRequireInput input)
        {
            var model = input.MapTo<EmployeeRequire>();
            var job = _postinfo.Get(input.Job);
            var org = _organization.Get(input.Department);
            model.Title = org.DisplayName+"-"+job.Name;
            var id = await _repository.InsertAndGetIdAsync(model);
            var ret = new EmployeeRequireOutput();
            ret.InStanceId = model.Id.ToString();
            return ret;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<EmployeeRequireDetailDto> GetAsync(GetWorkFlowTaskCommentInput input)
        {
            var model = await _repository.GetAsync(input.InstanceId.ToGuid());
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<EmployeeRequireDetailDto>();
        }
        [AbpAuthorize]
        public async Task<PagedResultDto<EmployeeRequireListOutput>> GetListAsync(EmployeeRequireSearchInput input)
        {
            var query = from a in _repository.GetAll()
                        join b in _organization.GetAll() on a.Department equals b.Id
                        join c in _user.GetAll() on a.CreatorUserId equals c.Id
                        join d in _postinfo.GetAll() on a.Job equals d.Id
                        select new EmployeeRequireListOutput()
                        {
                            ApplyTime = a.ApplyTime,
                            CreatorUserId = a.CreatorUserId.Value,
                            CreatorUserName = c.Surname,
                            Department = a.Department,
                            DepartmentName = b.DisplayName,
                            Id = a.Id,
                            Job = a.Job,
                            JobName = d.Name,
                            Status=a.Status
                        };
            if (input.StartTime.HasValue) {
                query = query.Where(r => r.ApplyTime>input.StartTime.Value);
            }
            if (input.EndTime.HasValue)
            {
                query = query.Where(r => r.ApplyTime < input.EndTime.Value);
            }
            if (input.Status!=null&& input.Status.Count > 0)
            {
                query = query.Where(r => input.Status.Contains(r.Status));
            }
            if (input.OrgId.HasValue) {
                query = query.Where(r => input.OrgId.Value==r.Department);
            }
            if (input.CreateUserId.HasValue) {
                query = query.Where(ite => ite.CreatorUserId == input.CreateUserId.Value);
            }
            
            var totoalCount = await query.CountAsync();
            var ret =await query.OrderByDescending(r => r.ApplyTime).PageBy(input).ToListAsync();
            foreach (var r in ret)
            {
                r.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, r.Status);
            }
            return new PagedResultDto<EmployeeRequireListOutput>(totoalCount, ret); 

        }

        public async Task UpdateAsync(EmployeeRequireInput input)
        {
            var model = input.MapTo<EmployeeRequire>();
            var job = _postinfo.Get(input.Job);
            var org = _organization.Get(input.Department);
            model.Title = org.DisplayName + "-" + job.Name;
            await _repository.UpdateAsync(model);
            
        }
    }
}
