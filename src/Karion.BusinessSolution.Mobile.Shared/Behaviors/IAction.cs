using Xamarin.Forms.Internals;

namespace Karion.BusinessSolution.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}