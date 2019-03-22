using System.Threading.Tasks;
using Abp.Configuration;

namespace ZCYX.FRMSCore.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}
