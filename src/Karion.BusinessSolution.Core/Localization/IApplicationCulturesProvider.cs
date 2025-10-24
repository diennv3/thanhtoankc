using System.Globalization;

namespace Karion.BusinessSolution.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}