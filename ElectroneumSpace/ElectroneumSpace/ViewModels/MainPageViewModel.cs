using ElectroneumSpace.Constants;
using ElectroneumSpace.Services;

using Prism.Navigation;
using Prism.Commands;
using Prism.Logging;

using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ElectroneumSpace.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        IPoolService _poolService;

        public IPoolService PoolService
        {
            get => _poolService;
            set => SetProperty(ref _poolService, value);
        }

        IList<HomeSection> _homeSections;

        public IList<HomeSection> HomeSections
        {
            get => _homeSections;
            set => SetProperty(ref _homeSections, value);
        }

        int _currentSectionPosition = 0;

        public int CurrentSectionPosition
        {
            get => _currentSectionPosition;
            set => SetProperty(ref _currentSectionPosition, value);
        }

        DelegateCommand<string> _swapHomeSectionCommand;

        public DelegateCommand<string> SwapHomeSectionCommand
        {
            get => _swapHomeSectionCommand;
            set => SetProperty(ref _swapHomeSectionCommand, value);
        }

        DelegateCommand _displaySettingsCommand;

        public DelegateCommand DisplaySettingsCommand
        {
            get => _displaySettingsCommand;
            set => SetProperty(ref _displaySettingsCommand, value);
        }

        #region Support Properties

        string _donationAddress = PoolConstants.DonationAddress;

        public string DonationAddress
        {
            get => _donationAddress;
            set => SetProperty(ref _donationAddress, value);
        }

        DelegateCommand<Entry> _donationAddressChangeCommand;

        public DelegateCommand<Entry> DonationAddressChangeCommand
        {
            get => _donationAddressChangeCommand;
            set => SetProperty(ref _donationAddressChangeCommand, value);
        }

        #endregion

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

            SwapHomeSectionCommand = new DelegateCommand<string>(HandleHomeSectionSwapRequest);
            DisplaySettingsCommand = new DelegateCommand(HandleDisplaySettingsRequest);
            DonationAddressChangeCommand = new DelegateCommand<Entry>(HandleDonationAddressChange);
        }

        /// <summary>
        /// Block modification
        /// </summary>
        /// <param name="sender"></param>
        void HandleDonationAddressChange(Entry sender)
        {
            if (sender.Text.Equals(PoolConstants.DonationAddress))
                return;

            sender.Text = PoolConstants.DonationAddress;
        }

        void HandleDisplaySettingsRequest()
        {
            // 4 is the index for settings
            CurrentSectionPosition = 4;
        }

        void HandleHomeSectionSwapRequest(string obj)
        {
            switch (obj)
            {
                default:
                case nameof(HomeSection.Home):
                    CurrentSectionPosition = 0;
                    break;

                case nameof(HomeSection.Stats):
                    CurrentSectionPosition = 1;
                    break;

                case nameof(HomeSection.Blocks):
                    CurrentSectionPosition = 2;
                    break;

                case nameof(HomeSection.Support):
                    CurrentSectionPosition = 3;
                    break;

                case nameof(HomeSection.Settings):
                    CurrentSectionPosition = 4;
                    break;
            }
        }
    }
}
