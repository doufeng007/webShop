using Xunit;

namespace ZCYX.FRMSCore.Tests
{
    public sealed class MultiTenantFactAttribute : FactAttribute
    {
        public MultiTenantFactAttribute()
        {
            //if (!FRMSCoreConsts.MultiTenancyEnabled)
            //{
            //    Skip = "MultiTenancy is disabled.";
            //}
        }
    }
}
