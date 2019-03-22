using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeRegularAppService : IApplicationService
    {

        Task<CreateEmployeeRegularOutput> CreateAsync(CreateEmployeeRegularInput input);


        Task UpdateAsync(UpdateEmployeeRegularInput input);

        Task UpdateWorkSummaryAsync(UpdateWorkSummaryInput input);


        Task<EmployeeRegularDto> GetAsync(GetEmployeeRegularDtoInput input);

        Task<PagedResultDto<EmployeeRegularListOut>> GetListAsync(EmployeeRegularListInput input);

    }
}
