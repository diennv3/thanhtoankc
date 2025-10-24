using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
