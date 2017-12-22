using ElectroneumSpace.Models;
using ElectroneumSpace.Services;
using ElectroneumSpace.Utilities;
using Prism.Commands;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;

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

        bool _isSaving;

        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
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
            SaveCommand = new DelegateCommand(HandleSaveRequest, () => !IsSaving).ObservesProperty(() => IsSaving);
        }

        void HandleSaveRequest()
        {
            try
            {
                // Log
                IsSaving = true;
                LoggerFacade.Log("Attempting to add a new wallet.", Category.Info, Priority.Medium);

                // Validate (must not already exist)
                if (PoolService.Wallets.Any((wallet) => wallet.Address.Equals(WalletAddress)))
                {
                    LoggerFacade.Log("Wallet validation failed.", Category.Exception, Priority.Medium);
                    PageDialogService.DisplayAlertAsync("Error", "This address is already in use.", "Ok");
                    return;
                }

                // Validate (must start with etn and be 98 characters long)
                if (!WalletAddress.StartsWith("etn") || WalletAddress.Length != 98)
                {
                    LoggerFacade.Log("Wallet validation failed.", Category.Exception, Priority.Medium);
                    PageDialogService.DisplayAlertAsync("Error", "Your address is invalid, please ensure it is correct and try again.", "Ok");
                    return;
                }

                // Save
                var realm = RealmUtils.LocalRealm;
                realm.Write(() =>
                {
                    realm.Add(new Wallet()
                    {
                        Address = WalletAddress,
                        Nickname = WalletNickname,
                        Created = DateTime.Now
                    });
                });

                // Exit
                LoggerFacade.Log("New wallet saved successfully.", Category.Info, Priority.Medium);
                NavigationService.GoBackAsync();
            }
            finally
            {
                PoolService.NotifyWalletUpdate();
                IsSaving = false;
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            WalletAddress = string.Empty;
            WalletNickname = string.Empty;

            base.OnNavigatedTo(parameters);
        }
    }
}
