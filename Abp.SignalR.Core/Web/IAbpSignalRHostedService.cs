using Abp.RealTime;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.SignalR.Core
{
     public interface IAbpSignalRHostedService: IHostedService
    {
        void SendNoticeToClient(IReadOnlyList<IOnlineClient> clients, Guid? projectId, string title, string content, string link = "", bool confirm = false, string parm = null);

        void SendNoticeToClient(Guid roadFlowUserId, Guid? projectId, string title, string content, string link = "");
    }
}
