using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
   public  interface ICommendResumeAppService : IApplicationService
    {
        Task<CommendResumeOutput> CreateAsync(CommendResumeInput input);
        Task UpdateAsync(CommendResumeInput input);
        Task<CommendResumeDetailDto> GetAsync(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<CommendResumeListOutput>> GetListAsync(CommendResumeSearchInput input);
    }
}
