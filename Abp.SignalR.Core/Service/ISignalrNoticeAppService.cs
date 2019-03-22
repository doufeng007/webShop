using System.Collections.Generic;
using Abp;
using System;
using Abp.RealTime;
using Abp.Dependency;
using Abp.Application.Services;

namespace Abp.SignalR.Core
{
    public interface ISignalrNoticeAppService : IApplicationService
    {
        void SendNoticeToClient(IReadOnlyList<IOnlineClient> clients, string instanceId, string title, string content, string link = "", bool confirm = false, string parm = null);

        void SendNoticeToClientV2(long userId, string instanceId, string title, string content, string link = "");
    }
}