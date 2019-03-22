using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeGoOutAppService : IApplicationService
    {

        Task<CreateEmployeeGoOutOutput> CreateAsync(CreateEmployeeGoOutInput input);


        Task UpdateAsync(EmployeeGoOutDto input);


        Task<GetEmployeeGoDtoOutput> GetAsync(GetEmployeeGoDtoInput input);


        Task<PagedResultDto<EmployeeGoOutDto>> GetListAsync(GetEmployeeGoOutListInput input);
    }
}
