using System.Collections.Generic;

namespace Karion.BusinessSolution.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
