using System.Collections.Generic;
using Karion.BusinessSolution.Editions.Dto;
using Karion.BusinessSolution.MultiTenancy.Payments;

namespace Karion.BusinessSolution.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}