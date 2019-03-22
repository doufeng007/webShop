using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ZCYX.FRMSCore.EntityFrameworkCore
{
    public static class FRMSCoreDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<FRMSCoreDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<FRMSCoreDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
