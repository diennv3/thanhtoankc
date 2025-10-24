using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.MultiTenancy.Accounting.Dto;

namespace Karion.BusinessSolution.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
