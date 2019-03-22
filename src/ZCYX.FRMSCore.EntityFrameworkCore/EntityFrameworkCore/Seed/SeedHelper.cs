using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using ZCYX.FRMSCore.EntityFrameworkCore.Seed.Host;
using ZCYX.FRMSCore.EntityFrameworkCore.Seed.Tenants;
using ZCYX.FRMSCore.Authorization;

namespace ZCYX.FRMSCore.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        private static IIocResolver _iocResolver;
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
            WithDbContext<FRMSCoreDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(FRMSCoreDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            new InitialHostDbBuilder(context, _iocResolver.Resolve<IAbpPermissionBaseAppService>()).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
