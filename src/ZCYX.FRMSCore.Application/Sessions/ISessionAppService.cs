using System.Threading.Tasks;
using Abp.Application.Services;
using ZCYX.FRMSCore.Sessions.Dto;

namespace ZCYX.FRMSCore.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
