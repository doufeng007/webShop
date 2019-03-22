using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.RealTime;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace Abp.SignalR.Core.Hubs
{
    /// <summary>
    /// Common Hub of ABP.
    /// </summary>
    public class AbpCommonHub : AbpHubBase, ITransientDependency
    {
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IHttpContextAccessor _accessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="AbpCommonHub"/> class.
        /// </summary>
        public AbpCommonHub(IOnlineClientManager onlineClientManager, IHttpContextAccessor accessor)
        {
            _onlineClientManager = onlineClientManager;

            Logger = NullLogger.Instance;
            _accessor = accessor;
            //AbpSession = NullAbpSession.Instance;
        }

        public void Register()
        {
            Logger.Debug("A client is registered: " + Context.ConnectionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task InitUser(long userId)
        {
            var oneClient = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            if (oneClient != null)
            {
                _onlineClientManager.Remove(Context.ConnectionId);
                var newclient = CreateClientForCurrentConnection(userId);
                _onlineClientManager.Add(newclient);
            }
            else
            {
                 
            }
            //await Clients.All.InvokeAsync("NoticeOnline", $"用户组数据更新完成,新增id为：{Context.ConnectionId}userId:{userId}");
        }


        public async Task UnitUser(long userId)
        {
            await OnDisconnectedAsync(null);
        }


        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
            //var userInfo = _accessor.HttpContext.User.Identities.First(u => u.IsAuthenticated);

            var client = CreateClientForCurrentConnection();
            Logger.Debug("A client is connected: " + client);

            _onlineClientManager.Add(client);
        }


        //public override async Task OnReconnected()
        //{
        //    await base.OnReconnected();

        //    var client = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
        //    if (client == null)
        //    {
        //        client = CreateClientForCurrentConnection();
        //        _onlineClientManager.Add(client);
        //        Logger.Debug("A client is connected (on reconnected event): " + client);
        //    }
        //    else
        //    {
        //        Logger.Debug("A client is reconnected: " + client);
        //    }
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            Logger.Debug("A client is disconnected: " + Context.ConnectionId);

            try
            {
                _onlineClientManager.Remove(Context.ConnectionId);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
            }
        }

        private IOnlineClient CreateClientForCurrentConnection()
        {
            return new OnlineClient(
                Context.ConnectionId,
                GetIpAddressOfClient(),
                AbpSession.TenantId,
                AbpSession.UserId
            );
        }

        private IOnlineClient CreateClientForCurrentConnection(long userId)
        {
            return new OnlineClient(
                Context.ConnectionId,
                GetIpAddressOfClient(),
                AbpSession.TenantId,
                userId
            );
        }

        private string GetIpAddressOfClient()
        {
            try
            {
                return _accessor.HttpContext.Connection.LocalIpAddress.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error("Can not find IP address of the client! connectionId: " + Context.ConnectionId);
                Logger.Error(ex.Message, ex);
                return "";
            }
        }
    }
}
