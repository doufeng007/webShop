using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using HR.Service.EmployeeRequire.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public  interface IEmployeeRequireAppService: IApplicationService
    {
        Task<EmployeeRequireOutput> CreateAsync(EmployeeRequireInput input);
        Task UpdateAsync(EmployeeRequireInput input);
        Task<EmployeeRequireDetailDto> GetAsync(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<EmployeeRequireListOutput>> GetListAsync(EmployeeRequireSearchInput input);
    }
}
