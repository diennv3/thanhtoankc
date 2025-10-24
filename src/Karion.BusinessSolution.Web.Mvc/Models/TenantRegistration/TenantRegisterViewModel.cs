using Karion.BusinessSolution.Editions;
using Karion.BusinessSolution.Editions.Dto;
using Karion.BusinessSolution.MultiTenancy.Payments;
using Karion.BusinessSolution.Security;
using Karion.BusinessSolution.MultiTenancy.Payments.Dto;

namespace Karion.BusinessSolution.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
