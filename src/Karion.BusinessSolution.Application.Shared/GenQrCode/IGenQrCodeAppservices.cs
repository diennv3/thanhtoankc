using Abp.Application.Services;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.GenQrCode
{
    public interface IGenQrCodeAppservices : IApplicationService
    {

        string PayQrGenerator(QRInputDto input);
        GeneratorOutput Generator(GeneratorInput input);
    }
}