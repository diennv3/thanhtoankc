using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
