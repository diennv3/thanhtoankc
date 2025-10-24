namespace Karion.BusinessSolution.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}