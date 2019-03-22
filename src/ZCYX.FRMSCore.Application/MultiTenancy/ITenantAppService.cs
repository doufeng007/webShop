using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore.MultiTenancy.Dto;

namespace ZCYX.FRMSCore.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
