using ElectroneumSpace.Constants;
using ElectroneumSpace.Services;

using Prism.Navigation;
using Prism.Commands;
using Prism.Logging;

using System;
using System.Linq;
using System.Collections.Generic;

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

                case nameof(HomeSection.Donate):
                    CurrentSectionPosition = 3;
                    break;

                case nameof(HomeSection.Settings):
                    CurrentSectionPosition = 4;
                    break;
            }
        }
    }
}
