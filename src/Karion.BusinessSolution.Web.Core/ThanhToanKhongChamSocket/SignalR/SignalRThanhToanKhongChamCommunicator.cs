using Abp;
using Abp.Dependency;
using Abp.RealTime;
using Karion.BusinessSolution.ThanhToanKhongChamSocket;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.Web.ThanhToanKhongChamSocket.SignalR
{
    public class SignalRThanhToanKhongChamCommunicator : IThanhToanKhongChamCommunicator , ITransientDependency
    {
        private readonly IHubContext<ThanhToanKhongChamHub> _thanhToanKhongChamHub;
        private readonly IOnlineClientManager _onlineClientManager;

        public SignalRThanhToanKhongChamCommunicator( 
            IHubContext<ThanhToanKhongChamHub> thanhToanKhongChamHub,
            IOnlineClientManager onlineClientManager)
        {
            _thanhToanKhongChamHub = thanhToanKhongChamHub;
            _onlineClientManager = onlineClientManager;
        }

        public async Task TestBroadCast()
        {
            await _thanhToanKhongChamHub.Clients.All.SendAsync("test", "hahahahaha");
        }

        public async Task TestNotificationTTKC(IUserIdentifier receiver)
        {
            var clients = _onlineClientManager.GetAllByUserId(receiver);

            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }

                await signalRClient.SendAsync("test", "Thanh cong");
            }
        }
        private IClientProxy GetSignalRClientOrNull(IOnlineClient client)
        {
            var signalRClient = _thanhToanKhongChamHub.Clients.Client(client.ConnectionId);
            if (signalRClient == null)
            {
                return null;
            }

            return signalRClient;
        }

        public async Task ThongBaoThanhToanNganHangSignalR(IUserIdentifier receiver, ResponseKiemTraThanhToanMBBank responseKiemTraThanhToanMBBank)
        {
            var clients = _onlineClientManager.GetAllByUserId(receiver);

            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }
                await signalRClient.SendAsync("thongbao", responseKiemTraThanhToanMBBank);
            }
        }
    }
}