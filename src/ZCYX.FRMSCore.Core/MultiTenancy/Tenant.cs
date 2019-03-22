using Abp.MultiTenancy;
using ZCYX.FRMSCore.Authorization.Users;

namespace ZCYX.FRMSCore.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
