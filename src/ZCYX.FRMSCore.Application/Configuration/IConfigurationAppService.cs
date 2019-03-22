using System.Threading.Tasks;
using ZCYX.FRMSCore.Configuration.Dto;

namespace ZCYX.FRMSCore.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
