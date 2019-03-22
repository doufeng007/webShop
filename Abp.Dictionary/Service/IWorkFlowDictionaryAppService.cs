using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abp.WorkFlowDictionary
{
    public interface IWorkFlowDictionaryAppService : IAsyncCrudAppService<WorkFlowDictionaryDto, Guid, PagedResultRequestDto, CreateWorkFlowDictionaryDto, WorkFlowDictionaryDto>
    {
        Task<PagedResultDto<WorkFlowDictionaryDto>> GetWorkFlowDictionaries(GetWorkFlowDictionariesInput input);
    }
}
