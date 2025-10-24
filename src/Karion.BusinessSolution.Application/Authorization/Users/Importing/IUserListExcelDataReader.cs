using System.Collections.Generic;
using Karion.BusinessSolution.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Karion.BusinessSolution.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
