using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.RealTime;
using Abp.Runtime.Session;

namespace Karion.BusinessSolution.Web.ThanhToanKhongChamSocket.SignalR
{
    public class ThanhToanKhongChamHub : OnlineClientHubBase
    {

        private IAbpSession TtkcAbpSession { get; }
        public ThanhToanKhongChamHub(
            IOnlineClientManager onlineClientManager, 
            IClientInfoProvider clientInfoProvider) : base(onlineClientManager, clientInfoProvider)
        {
            TtkcAbpSession = NullAbpSession.Instance;
        }

    }
}