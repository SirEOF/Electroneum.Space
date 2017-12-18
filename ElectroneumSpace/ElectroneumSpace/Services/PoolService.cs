using ElectroneumSpace.Models;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Services;
using Refit;

using System;
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
        string NetworkAddress { get; }
    }

    public class PoolService : BindableBase, IPoolService
    {

        IPoolApi PoolApi { get; set; }

        ILoggerFacade LoggerFacade { get; set; }

        IPageDialogService PageDialogService { get; set; }

        PoolStats _poolStatistics;

        public PoolStats PoolStatistics
        {
            get => _poolStatistics;
            set => SetProperty(ref _poolStatistics, value);
        }

        string _networkAddress;

        public string NetworkAddress
        {
            get => _networkAddress;
            set => SetProperty(ref _networkAddress, value);
        }

        public PoolService(ILoggerFacade loggerFacade, IPageDialogService pageDialogService)
        {
            LoggerFacade = loggerFacade;
            PageDialogService = pageDialogService;
            PoolApi = RestService.For<IPoolApi>("http://api.electroneum.space/v1");

            // Setup
            LoggerFacade.Log($"Starting Service: {nameof(PoolService)}.", Category.Debug, Priority.Low);

            // Start background tasks
            Device.StartTimer(TimeSpan.FromHours(1), () => 
            {
                UpdatePoolStatisticsAsync().ConfigureAwait(false);
                return true;
            });

            UpdatePoolStatisticsAsync().ConfigureAwait(false);
        }

        async Task<bool> UpdatePoolStatisticsAsync()
        {
            PoolStatistics = await PoolApi.GetPoolStatisticsAsync();
            return true;
        }
    }
}
