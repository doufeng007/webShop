using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IOATaskAppService : IApplicationService
    {

        
        Task<OATaskDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OATaskListDto>> GetAll(GetOATaskListInput input);


        Task<InitWorkFlowOutput> Create(OATaskCreateInput input);

        Task Update(OATaskUpdateInput input);

        Task BeginOATask(Guid oATaskId, Guid taskId);
    }

}
