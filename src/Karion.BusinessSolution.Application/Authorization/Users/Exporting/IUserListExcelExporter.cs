using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Users.Dto;
using Karion.BusinessSolution.Dto;

namespace Karion.BusinessSolution.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}