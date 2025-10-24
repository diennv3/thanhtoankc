using Abp;
using Abp.RealTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.ThanhToanKhongChamSocket
{
   public interface IThanhToanKhongChamCommunicator
    {
        Task TestNotificationTTKC(IUserIdentifier receiver);
        Task TestBroadCast();
        Task ThongBaoThanhToanNganHangSignalR(IUserIdentifier receiver, ResponseKiemTraThanhToanMBBank responseKiemTraThanhToanMBBank);
    }
}
