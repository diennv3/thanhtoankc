using System.Threading.Tasks;
using Karion.BusinessSolution.Views;
using Xamarin.Forms;

namespace Karion.BusinessSolution.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
