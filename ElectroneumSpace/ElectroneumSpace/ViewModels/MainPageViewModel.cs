using ElectroneumSpace.Constants;
using Prism.Navigation;

using System;
using System.Linq;
using System.Collections.Generic;
using Prism.Logging;
using ElectroneumSpace.Services;

namespace ElectroneumSpace.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        readonly IPoolService PoolService;

        IList<HomeSection> _homeSections;

        public IList<HomeSection> HomeSections
        {
            get => _homeSections;
            set => SetProperty(ref _homeSections, value);
        }

        public MainPageViewModel(INavigationService navigationService, ILoggerFacade loggerFacade,
            IPoolService poolService) : base(navigationService, loggerFacade)
        {
            PoolService = poolService;
            SetupViewModel();
        }

        void SetupViewModel()
        {
            LoggerFacade.Log($"Setting up ViewModel - {nameof(MainPageViewModel)}", Category.Info, Priority.Low);

            Title = "Electroneum.space";
            HomeSections = new List<HomeSection>(Enum.GetValues(typeof(HomeSection)).Cast<HomeSection>());
        }
    }
}
