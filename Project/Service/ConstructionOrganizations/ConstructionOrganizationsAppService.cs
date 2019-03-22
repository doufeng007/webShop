using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.WorkFlow.Entity;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Project
{
    public class ConstructionOrganizationsAppService : AsyncCrudAppService<ConstructionOrganizations, ConstructionOrganizationsDto, int, PagedResultRequestDto, CreateOrUpdateConstructionOrganizationInput, ConstructionOrganizationsDto>, IConstructionOrganizationsAppService
    {
        private readonly IRepository<ConstructionOrganizations> _repository;
        public ConstructionOrganizationsAppService(IRepository<ConstructionOrganizations> repository)
            : base(repository)
        {
            _repository = repository;
        }




    }
}
