using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IConstructionOrganizationsAppService : IAsyncCrudAppService<ConstructionOrganizationsDto, int, PagedResultRequestDto, CreateOrUpdateConstructionOrganizationInput, ConstructionOrganizationsDto>
    {
     
    }
}
