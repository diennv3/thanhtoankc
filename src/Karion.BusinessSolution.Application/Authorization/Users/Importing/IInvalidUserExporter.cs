using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Users.Importing.Dto;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
