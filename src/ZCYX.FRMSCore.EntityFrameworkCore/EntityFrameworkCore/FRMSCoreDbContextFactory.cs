using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Web;

namespace ZCYX.FRMSCore.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class FRMSCoreDbContextFactory : IDesignTimeDbContextFactory<FRMSCoreDbContext>
    {
        public FRMSCoreDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FRMSCoreDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            FRMSCoreDbContextConfigurer.Configure(builder, configuration.GetConnectionString(FRMSCoreConsts.ConnectionStringName));

            return new FRMSCoreDbContext(builder.Options);
        }
    }
}
