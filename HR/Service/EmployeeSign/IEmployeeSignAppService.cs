using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface IEmployeeSignAppService : IApplicationService
    {

        Task CreateAsync(CreateEmployeeSignInput input);


        Task<GetEmployeeSignOutput> GetAsync();

    }
}
