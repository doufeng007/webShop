using Abp.Application.Services;
using Abp.Dependency;
using Abp.RealTime;
using Abp.SignalR.Core.Hubs;
using Castle.Core.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.SignalR.Core
{
    [RemoteService(IsEnabled = false)]
    public class SignalrNoticeAppService : ApplicationService, ISignalrNoticeAppService
    {

        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private IHubContext<NoticeHub> _hubContext;
       // private IHubClients Clients { get; }

        private readonly IOnlineClientManager _onlineClientManager;


        public SignalrNoticeAppService(IServiceProvider service, IOnlineClientManager onlineClientManager)
        {
            _hubContext = service.GetService((typeof(IHubContext<NoticeHub>))) as IHubContext<NoticeHub>;
         //   Clients = _hubContext.Clients;
            _onlineClientManager = onlineClientManager;
        }

        int counter = 0;
        //protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    while (true)
        //    {
        //        // await Clients.All.InvokeAsync("increment", counter);

        //        var task = Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        //        try
        //        {
        //            await task;
        //        }
        //        catch (TaskCanceledException)
        //        {
        //            break;
        //        }

        //        counter++;
        //    }

        //}

        public void SendNoticeToClient(IReadOnlyList<IOnlineClient> clients, string instanceId, string title, string content, string link = "", bool confirm = false, string parm = null)
        {
            foreach (var client in clients)
            {
                var signalRClient=  _hubContext.Clients.Client(client.ConnectionId);
                if (signalRClient == null)
                {
                    Logger.Debug("Can not get notice user " + client.UserId + " from SignalR hub!");
                    return;
                }

                //signalRClient.getNoticeMessage(projectId, title, content, link, confirm, parm);
                var messageObj = new MessageInfo() { BusinessId = instanceId, Content = content, Link = link, Title = title };
                signalRClient.SendAsync("increment", messageObj);

            }
        }

        public void SendNoticeToClientV2(long userId, string instanceId, string title, string content, string link = "")
        {
            var onlineClient = _onlineClientManager.GetAllClients().Where(p => p.UserId == userId).ToList();
            SendNoticeToClient(onlineClient, instanceId, title, content, link);
        }

    }
}
