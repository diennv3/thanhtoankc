using System.Threading.Tasks;

namespace Karion.BusinessSolution.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
