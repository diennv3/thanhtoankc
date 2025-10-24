using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.MultiTenancy.Payments.PayPal.Dto;

namespace Karion.BusinessSolution.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
