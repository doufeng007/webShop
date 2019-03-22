using Abp.Reflection.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using ZCYX.FRMSCore.Configuration;

namespace ZCYX.FRMSCore.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var coreAssemblyDirectoryPath = typeof(Program).GetAssembly().GetDirectoryPathOrNull();
            var _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            var serverRootAddress = _appConfiguration["App:ServerRootAddress"];
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls(serverRootAddress)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
