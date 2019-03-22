using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.WorkFlow.Entity;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class WorkFlowButtonAppService : AsyncCrudAppService<WorkFlowButtons, WorkFlowButtonDto, Guid, PagedResultRequestDto, CreateWorkFlowButtonDto, WorkFlowButtonDto>, IWorkFlowButtonAppService
    {
        private readonly IRepository<WorkFlowButtons, Guid> _repository;
        public WorkFlowButtonAppService(IRepository<WorkFlowButtons, Guid> repository)
            : base(repository)
        {
            _repository = repository;
        }

    }
}
