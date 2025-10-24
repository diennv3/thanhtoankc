using System.Collections.Generic;
using Karion.BusinessSolution.Editions;
using Karion.BusinessSolution.Editions.Dto;
using Karion.BusinessSolution.MultiTenancy.Payments;
using Karion.BusinessSolution.MultiTenancy.Payments.Dto;

namespace Karion.BusinessSolution.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
