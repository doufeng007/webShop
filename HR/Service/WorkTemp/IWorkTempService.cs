using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace HR
{
    public interface IWorkTempAppService : IApplicationService
    {

        Task<PagedResultDto<WorkTempListOutputDto>> GetList(GetWorkTempListInput input);
    }
}