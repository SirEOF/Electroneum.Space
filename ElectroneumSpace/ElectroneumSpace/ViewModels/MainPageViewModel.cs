using Prism.Navigation;

namespace ElectroneumSpace.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "Electroneum.space";
        }
    }
}
