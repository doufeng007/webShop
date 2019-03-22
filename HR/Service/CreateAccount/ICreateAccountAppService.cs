using Abp.Application.Services;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public interface ICreateAccountAppService: IApplicationService
    {
        InitWorkFlowOutput Create(CreateAccountInput input);

        void Update(CreateAccountDto input);

        Task<CreateAccountDto> Get(GetWorkFlowTaskCommentInput input);
    }
}
