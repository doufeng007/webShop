using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeBusinessTripAppService : IApplicationService
    {

        Task<CreateEmployeeBusinessTripOutput> CreateAsync(CreateEmployeeBusinessTripInput input);


        Task UpdateAsync(UpdateEmployeeBusinessTripInput input);


        Task<GetEmployeeBusinessTripOutput> GetAsync(GetEmployeeBusinessTripDtoInput input);


        Task<PagedResultDto<EmployeeBusinessTripDto>> GetListAsync(GetEmployeeBusinessTripListInput input);
    }
}
