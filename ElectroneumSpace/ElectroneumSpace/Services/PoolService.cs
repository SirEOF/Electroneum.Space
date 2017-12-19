using ElectroneumSpace.Models;
using ElectroneumSpace.Utilities;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Services;
using Refit;

using System;
using System.Collections.Generic;
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

        string _poolHashRate = "0.00";

        public string PoolHashRate
        {
            get => _poolHashRate;
            set => SetProperty(ref _poolHashRate, value);
        }

        string _poolLastBlockFound = "0";

        public string PoolLastBlockFound
        {
            get => _poolLastBlockFound;
            set => SetProperty(ref _poolLastBlockFound, value);
        }

        string _poolConnectedMiners = "0";

        public string PoolConnectedMiners
        {
            get => _poolConnectedMiners;
            set => SetProperty(ref _poolConnectedMiners, value);
        }

        string _poolTotalFee = "0.0%";

        public string PoolTotalFee
        {
            get => _poolTotalFee;
            set => SetProperty(ref _poolTotalFee, value);
        }

        string _poolBlockEstimate = "0";

        public string PoolBlockEstimate
        {
            get => _poolBlockEstimate;
            set => SetProperty(ref _poolBlockEstimate, value);
        }

        string _poolAverageBlockInterval = "0";

        public string PoolAverageBlockInterval
        {
            get => _poolAverageBlockInterval;
            set => SetProperty(ref _poolAverageBlockInterval, value);
        }

        #endregion

        #region Network Metrics

        string _networkHashRate = "0.00";

        public string NetworkHashRate
        {
            get => _networkHashRate;
            set => SetProperty(ref _networkHashRate, value);
        }

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
            UpdatePoolStatisticsAsync(true).ConfigureAwait(false);
        }

        async Task<bool> UpdatePoolStatisticsAsync(bool displayErrors = false)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (displayErrors)
                    await PageDialogService.DisplayAlertAsync("Error", "Could not get response from server, please check you have an active connection.", "Ok");

                return false;
            }

            IsRefreshing = true;

            try
            {
                LoggerFacade.Log("Attempting to update pool statistics.", Category.Debug, Priority.Low);

                var stats = await PoolApi.GetPoolStatisticsAsync().ConfigureAwait(true);

                // Some stats are wrong or not included, for this we parse the webpage
                // http://www.electroneum.space/

                if (stats == null)
                    return false;

                // Update tracked results

                // Set pool settings
                SetPoolHashRate(stats.Pool.Hashrate);
                SetLastBlockFound(stats.Pool.LastBlockFound);
                SetConnectedMiners(stats.Pool.Miners);
                SetPoolTotalFee(stats.Config.Fee);
                SetPoolBlockEstimate(stats.Pool.Blocks);

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

        void SetNetworkHashRate(string hash)
        {
            // Log
            LoggerFacade.Log("Setting network hashrate.", Category.Debug, Priority.Low);

            // Calculate
            //var toMh = hash / 1000000;
            //var toStr = toMh.ToString("F");

            // Set
            //PoolHashRate = toStr;
        }

        void SetPoolBlockEstimate(string[] blocks)
        {
            // Log
            LoggerFacade.Log("Calculating block estimate.", Category.Debug, Priority.Low);

            // Prepare
            var timestamps = new List<DateTime>();

            // Parse
            foreach (var block in blocks)
            {
                // Check if is a valid block
                if (!block.Contains(":"))
                    continue;

                // Get Timestamp
                var elements = block.Split(':');
                if (!long.TryParse(elements[1], out long unixTimestamp))
                    continue;

                var epoch = new DateTime(1970, 1, 1);
                var actualTimestamp = epoch.AddSeconds(unixTimestamp);

                // Append
                timestamps.Add(actualTimestamp);
            }

            // Append now
            timestamps.Add(DateTime.UtcNow);

            // Calculate
            var average = DateUtils.GetAverageIntervalFromCollectionOfTimestamps(timestamps);
            var averageMinutes = average.Minutes;
            var averageStr = averageMinutes.ToString();

            // Set
            PoolAverageBlockInterval = averageStr;
        }

        void SetPoolTotalFee(double fee)
        {
            // Log
            LoggerFacade.Log("Setting pool fees.", Category.Debug, Priority.Low);

            // Configure
            var feeStr = $"{fee}%";

            // Set
            PoolTotalFee = feeStr;
        }

        void SetConnectedMiners(long miners)
        {
            // Log
            LoggerFacade.Log("Setting connected miners.", Category.Debug, Priority.Low);

            // Convert
            var minerStr = miners.ToString();

            // Set
            PoolConnectedMiners = minerStr;
        }

        void SetLastBlockFound(string stringEpoch)
        {
            // Log
            LoggerFacade.Log("Setting pool last block found.", Category.Debug, Priority.Low);

            // Get Current Time
            var blockEpoch = long.Parse(stringEpoch);
            var blockDateTime = DateUtils.GetDateTimeFromUnixTimestamp(blockEpoch);

            // Calculate Minutes
            var minuteDifference = (DateTime.UtcNow - blockDateTime).TotalMinutes;
            var minuteFloor = (int) Math.Floor(minuteDifference);

            // Set
            PoolLastBlockFound = minuteFloor.ToString();
        }

        void SetPoolHashRate(double hashrate)
        {
            // Log
            LoggerFacade.Log("Setting pool hashrate.", Category.Debug, Priority.Low);

            // Calculate
            var toMh = hashrate / 1000000;
            var toStr = toMh.ToString("F");

            // Set
            PoolHashRate = toStr;
        }
    }
}
