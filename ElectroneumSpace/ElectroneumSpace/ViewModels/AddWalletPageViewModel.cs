using System;
using ElectroneumSpace.Services;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;

namespace ElectroneumSpace.ViewModels
{
	public class AddWalletPageViewModel : ViewModelBase
	{

        IPoolService _poolService;

        public IPoolService PoolService
        {
            get => _poolService;
            set => SetProperty(ref _poolService, value);
        }

        DelegateCommand _goBackCommand;

        public DelegateCommand GoBackCommand
        {
            get => _goBackCommand;
            set => SetProperty(ref _goBackCommand, value);
        }

        public AddWalletPageViewModel(INavigationService navigationService, ILoggerFacade loggerFacade,
            IPoolService poolService) : base(navigationService, loggerFacade)
        {
            PoolService = poolService;

            ConfigureViewModel();
        }

        void ConfigureViewModel()
        {
            LoggerFacade.Log($"Setting up ViewModel - {nameof(MainPageViewModel)}", Category.Info, Priority.Low);

            Title = "Add wallet";
            GoBackCommand = new DelegateCommand(() => NavigationService.GoBackAsync());
        }
    }
}
