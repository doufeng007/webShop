using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;

namespace Project
{
    public interface IWorkLogAppService : IApplicationService
    {
        Task<WriteLogViewModelOut> GetForEdit(NullableIdDto<Guid> input);

        Task<PagedResultDto<WorkLogList>> GetWorkLogPage(GetWorkLogListInput input);
    }

}
