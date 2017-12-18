using ElectroneumSpace.Models;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Services;
using Refit;

using System;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ElectroneumSpace.Services
{

    public interface IPoolApi
    {
        [Get("/stats/nonBrowser")]
        Task<PoolStats> GetPoolStatisticsAsync();
    }

    public interface IPoolService
    {
        bool IsRefreshing { get; }

        string NetworkAddress { get; }
    }

    public class PoolService : BindableBase, IPoolService
    {

        IPoolApi PoolApi { get; set; }

        ILoggerFacade LoggerFacade { get; set; }

        IPageDialogService PageDialogService { get; set; }

        DelegateCommand _refreshPoolDataCommand;

        public DelegateCommand RefreshPoolDataCommand
        {
            get => _refreshPoolDataCommand;
            set => SetProperty(ref _refreshPoolDataCommand, value);
        }

        PoolStats _poolStatistics;

        public PoolStats PoolStatistics
        {
            get => _poolStatistics;
            set => SetProperty(ref _poolStatistics, value);
        }

        bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        string _networkAddress;

        public string NetworkAddress
        {
            get => _networkAddress;
            set => SetProperty(ref _networkAddress, value);
        }

        #region Pool Metrics

        double _poolHashRate = 0.0d;

        public double PoolHashRate
        {
            get => _poolHashRate;
            set => SetProperty(ref _poolHashRate, value);
        }

        #endregion

        #region Network Metrics
        #endregion
        
        public PoolService(ILoggerFacade loggerFacade, IPageDialogService pageDialogService)
        {
            LoggerFacade = loggerFacade;
            PageDialogService = pageDialogService;
            PoolApi = RestService.For<IPoolApi>("http://api.electroneum.space/v1");

            // Setup
            LoggerFacade.Log($"Starting Service: {nameof(PoolService)}.", Category.Debug, Priority.Low);

            // Commands
            RefreshPoolDataCommand = new DelegateCommand(HandlePoolDataRefreshRequest, () => !IsRefreshing).ObservesProperty(() => IsRefreshing);

            // Start background tasks
            Device.StartTimer(TimeSpan.FromHours(1), () => 
            {
                UpdatePoolStatisticsAsync().ConfigureAwait(false);
                return true;
            });

            UpdatePoolStatisticsAsync().ConfigureAwait(false);
        }

        void HandlePoolDataRefreshRequest()
        {
            UpdatePoolStatisticsAsync().ConfigureAwait(false);
        }

        async Task<bool> UpdatePoolStatisticsAsync()
        {
            IsRefreshing = true;

            try
            {
                LoggerFacade.Log("Attempting to update pool statistics.", Category.Debug, Priority.Low);

                var stats = await PoolApi.GetPoolStatisticsAsync().ConfigureAwait(true);

                if (stats == null)
                    return false;

                // Update tracked results

                // Set pool settings

                // Set network settings

                // Set
                PoolStatistics = stats;

                return true;
            }

            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
