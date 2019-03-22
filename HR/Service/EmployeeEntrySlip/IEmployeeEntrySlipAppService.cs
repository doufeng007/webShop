using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeEntrySlipAppService : IApplicationService
    {

        Task<CreateEmployeeEntrySlipOutput> CreateAsync(CreateEmployeeEntrySlipInput input);


        Task UpdateAsync(UpdateEmployeeEntrySlipInput input);


        Task<EmployeeEntrySlipDto> GetAsync(GetEmployeeEntrySlipDtoInput input);


        Task<PagedResultDto<EmployeeEntrySlipListDto>> GetListAsync(GetEmployeeEntrySlipListInput input);

    }
}
