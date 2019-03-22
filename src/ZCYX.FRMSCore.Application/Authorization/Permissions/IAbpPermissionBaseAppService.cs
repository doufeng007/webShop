using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Authorization
{
    public interface IAbpPermissionBaseAppService : IAsyncCrudAppService<AbpPermissionBaseDto, long, PagedResultRequestDto, CreateAbpPermissionBaseInput, AbpPermissionBaseDto>
    {
        List<AbpPermissionBaseDto> GetByMoudleName(string moudleName);
    }
}
