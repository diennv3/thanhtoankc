using Karion.BusinessSolution.MultiTenancy.Payments;

namespace Karion.BusinessSolution.Web.Models.Payment
{
    public class CancelPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}