using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.UI;
using ZCYX.FRMSCore.Application;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class CommendResumeAppService : ICommendResumeAppService
    {
        private readonly IRepository<CommendResume, Guid> _repository;
        private readonly IRepository<EmployeeRequire, Guid> _employeeRequire;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        public CommendResumeAppService(
            IRepository<CommendResume, Guid> repository,
            IRepository<EmployeeRequire, Guid> employeeRequire,
             WorkFlowTaskManager workFlowTaskManager
            )
        {
            _repository = repository;
            _employeeRequire = employeeRequire;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<CommendResumeOutput> CreateAsync(CommendResumeInput input)
        {
            var model = input.MapTo<CommendResume>();
            var id = await _repository.InsertAndGetIdAsync(model);
            var ret = new CommendResumeOutput();
            ret.InStanceId = model.Id.ToString();
            return ret;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CommendResumeDetailDto> GetAsync(GetWorkFlowTaskCommentInput input)
        {
            var model = await _repository.GetAsync(input.InstanceId.ToGuid());
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<CommendResumeDetailDto>();
        }
        [AbpAuthorize]
        public async Task<PagedResultDto<CommendResumeListOutput>> GetListAsync(CommendResumeSearchInput input)
        {
            var query = from a in _repository.GetAll()
                        join b in _employeeRequire.GetAll() on a.Job equals b.Id
                        select new CommendResumeListOutput()
                        {
                            Age = a.Age,
                            CreationTime = a.CreationTime,
                            Id = a.Id,
                            Job = a.Job,
                            JobName = b.Title,
                            Name = a.Name,
                            Phone = a.Phone,
                            Status = a.Status
                        };
            if (input.StartTime.HasValue)
            {
                query = query.Where(r => r.CreationTime > input.StartTime.Value);
            }
            if (input.EndTime.HasValue)
            {
                query = query.Where(r => r.CreationTime < input.EndTime.Value);
            }
            if (input.Status != null && input.Status.Count > 0)
            {
                query = query.Where(r => input.Status.Contains(r.Status));
            }
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                query = query.Where(r => r.Name.Contains(input.SearchKey));
            }
            var totoalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var r in ret) {
                r.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, r.Status);
            }
            return new PagedResultDto<CommendResumeListOutput>(totoalCount, ret);
        }

        public async Task UpdateAsync(CommendResumeInput input)
        {
            var model = input.MapTo<CommendResume>();
            await _repository.UpdateAsync(model);

        }
    }
}
