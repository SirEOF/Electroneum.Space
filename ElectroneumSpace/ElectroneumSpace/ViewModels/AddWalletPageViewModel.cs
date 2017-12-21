using ElectroneumSpace.Services;
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

        public AddWalletPageViewModel(INavigationService navigationService, ILoggerFacade loggerFacade,
            IPoolService poolService) : base(navigationService, loggerFacade)
        {
            PoolService = poolService;
        }
	}
}
