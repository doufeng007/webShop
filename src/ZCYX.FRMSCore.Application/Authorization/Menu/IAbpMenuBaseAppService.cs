using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Authorization.Permissions
{
    public interface IAbpMenuBaseAppService : IAsyncCrudAppService<AbpMenuBaseDto, long, PagedResultRequestDto, CreateAbpMenuBaseInput, AbpMenuBaseDto>
    {
        [RemoteService(false)]
        List<AbpMenuBaseDto> GetMenuFromCache();
    }
}
