using ElectroneumSpace.Services;

using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

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

        IPageDialogService _pageDialogService;

        public IPageDialogService PageDialogService
        {
            get => _pageDialogService;
            set => SetProperty(ref _pageDialogService, value);
        }

        string _walletAddress = string.Empty;

        public string WalletAddress
        {
            get => _walletAddress;
            set => SetProperty(ref _walletAddress, value);
        }

        string _walletNickname = string.Empty;

        public string WalletNickname
        {
            get => _walletNickname;
            set => SetProperty(ref _walletNickname, value);
        }

        DelegateCommand _goBackCommand;

        public DelegateCommand GoBackCommand
        {
            get => _goBackCommand;
            set => SetProperty(ref _goBackCommand, value);
        }

        DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand
        {
            get => _saveCommand;
            set => SetProperty(ref _saveCommand, value);
        }

        public AddWalletPageViewModel(INavigationService navigationService, ILoggerFacade loggerFacade, IPageDialogService pageDialogService,
            IPoolService poolService) : base(navigationService, loggerFacade)
        {
            PageDialogService = pageDialogService;
            PoolService = poolService;

            ConfigureViewModel();
        }

        void ConfigureViewModel()
        {
            LoggerFacade.Log($"Setting up ViewModel - {nameof(AddWalletPageViewModel)}", Category.Info, Priority.Low);

            Title = "Add wallet";
            GoBackCommand = new DelegateCommand(() => NavigationService.GoBackAsync());
            SaveCommand = new DelegateCommand(HandleSaveRequest);
        }

        void HandleSaveRequest()
        {
            // Log
            LoggerFacade.Log("Attempting to add a new wallet.", Category.Info, Priority.Medium);

            // Validate (must start with etn and be 98 characters long)
            if (!WalletAddress.StartsWith("etn") || WalletAddress.Length != 98)
            {
                LoggerFacade.Log("Wallet validation failed.", Category.Exception, Priority.Medium);
                PageDialogService.DisplayAlertAsync("Error", "Your address is invalid, please ensure it is correct and try again.", "Ok");
                return;
            }

            // Save
            // TODO

            // Exit
            LoggerFacade.Log("New wallet saved successfully.", Category.Info, Priority.Medium);
            NavigationService.GoBackAsync();
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            WalletAddress = string.Empty;
            WalletNickname = string.Empty;

            base.OnNavigatedTo(parameters);
        }
    }
}
