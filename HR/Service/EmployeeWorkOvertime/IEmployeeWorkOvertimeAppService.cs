using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeWorkOvertimeAppService : IApplicationService
    {

        Task<CreateEmployeeWorkOvertimeOutput> CreateAsync(CreateEmployeeWorkOvertimeInput input);


        Task UpdateAsync(UpdateEmployeeWorkOvertimeInput input);


        Task<GetEmployeeWorkOvertimeOutput> GetAsync(GetEmployeeWorkOvertimeDtoInput input);


        Task<PagedResultDto<EmployeeWorkOvertimeDto>> GetListAsync(GetEmployeeWorkOvertimeListInput input);
    }
}
