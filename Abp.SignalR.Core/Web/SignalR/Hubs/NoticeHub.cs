using Abp.Dependency;
using Abp.RealTime;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.SignalR.Core.Hubs
{
    public class NoticeHub : AbpCommonHub, ITransientDependency
    {
        private readonly IOnlineClientManager _onlineClientManager;



        public NoticeHub(IOnlineClientManager onlineClientManager, IHttpContextAccessor accessor) :
            base(onlineClientManager, accessor)
        {
            //_onlineClientManager = onlineClientManager;

            //Logger = NullLogger.Instance;
            //AbpSession = NullAbpSession.Instance;
        }

    }


    public class NoticeHostedService : HostedService
    {
        public NoticeHostedService(IHubContext<NoticeHub> context)
        {
            Clients = context.Clients;
        }

        private IHubClients Clients { get; }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //await Clients.All.InvokeAsync("increment");

            //int counter = 0;
            //while (true)
            //{
            //    await Clients.All.InvokeAsync("increment", counter);

            //    var task = Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            //    try
            //    {
            //        await task;
            //    }
            //    catch (TaskCanceledException)
            //    {
            //        break;
            //    }

            //    counter++;
            //}
        }
    }
}
