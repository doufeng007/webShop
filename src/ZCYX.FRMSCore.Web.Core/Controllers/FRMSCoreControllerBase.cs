using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ZCYX.FRMSCore.Controllers
{
    public abstract class FRMSCoreControllerBase: AbpController
    {
        protected FRMSCoreControllerBase()
        {
            LocalizationSourceName = FRMSCoreConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
