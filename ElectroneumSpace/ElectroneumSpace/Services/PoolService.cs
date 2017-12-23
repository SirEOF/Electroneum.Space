using ElectroneumSpace.Constants;
using ElectroneumSpace.Models;
using ElectroneumSpace.Utilities;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Services;
using Realms;
using Refit;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ElectroneumSpace.Services
{

    public interface IPoolApi
    {
        [Get("/poolStats")]
        Task<PoolStats> GetPoolStatisticsAsync();
    }

    public interface IPoolService
    {
        bool IsRefreshing { get; }

        string NetworkAddress { get; }

        IRealmCollection<Wallet> Wallets { get; }

        string GetReadableHashRateString(double hashRate);

        void NotifyWalletUpdate();
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

        string _networkHashRate = string.Empty;

        public string NetworkHashRate
        {
            get => _networkHashRate;
            set => SetProperty(ref _networkHashRate, value);
        }

        string _networkLastBlockFound = string.Empty;

        public string NetworkLastBlockFound
        {
            get => _networkLastBlockFound;
            set => SetProperty(ref _networkLastBlockFound, value);
        }

        string _networkDifficulty = string.Empty;

        public string NetworkDifficulty
        {
            get => _networkDifficulty;
            set => SetProperty(ref _networkDifficulty, value);
        }

        string _blockchainHeight = string.Empty;

        public string BlockchainHeight
        {
            get => _blockchainHeight;
            set => SetProperty(ref _blockchainHeight, value);
        }

        string _lastReward = string.Empty;

        public string LastReward
        {
            get => _lastReward;
            set => SetProperty(ref _lastReward, value);
        }

        #endregion

        #region Wallet Metrics

        IRealmCollection<Wallet> _wallets;

        public IRealmCollection<Wallet> Wallets
        {
            get => _wallets;
            set => SetProperty(ref _wallets, value);
        }

        #endregion

        public PoolService(ILoggerFacade loggerFacade, IPageDialogService pageDialogService)
        {
            LoggerFacade = loggerFacade;
            PageDialogService = pageDialogService;
            PoolApi = RestService.For<IPoolApi>(PoolConstants.PoolBaseUri);

            // Setup
            LoggerFacade.Log($"Starting Service: {nameof(PoolService)}.", Category.Debug, Priority.Low);

            // Get Database Objects
            var realm = RealmUtils.LocalRealm;
            Wallets = realm.All<Wallet>().AsRealmCollection();

            // Commands
            RefreshPoolDataCommand = new DelegateCommand(() => UpdatePoolStatisticsAsync(true).ConfigureAwait(false), () => !IsRefreshing).ObservesProperty(() => IsRefreshing);

            // Start background tasks
            Device.StartTimer(TimeSpan.FromSeconds(30), () => 
            {
                UpdatePoolStatisticsAsync().ConfigureAwait(false);
                return true;
            });

            UpdatePoolStatisticsAsync().ConfigureAwait(false);
        }

        public void NotifyWalletUpdate()
        {
            RaisePropertyChanged(nameof(Wallets));
        }

        public string GetReadableHashRateString(double hashRate)
        {
            var i = 0;
            var byteUnits = new string[] { "H", "KH", "MH", "GH", "TH", "PH" };
            
            while (hashRate > 1000)
            {
                hashRate = hashRate / 1000;
                i++;
            }

            return $"{hashRate.ToString("F")} {byteUnits[i]}/sec";
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
                SetNetworkHashRate(stats.Network.Difficulty);
                SetNetworkLastBlockFound(stats.Network.Timestamp);
                SetNetworkDifficulty(stats.Network.Difficulty);
                SetBlockchainHeight(stats.Network.Height);
                SetLastReward(stats.Network.Reward);

                // Set
                PoolStatistics = stats;

                return true;
            }

            catch (Exception e)
            {
                LoggerFacade.Log("An error occured getting stats from the server.", Category.Exception, Priority.Medium);
                return false;
            }

            finally
            {
                IsRefreshing = false;
            }
        }

        void SetLastReward(double reward)
        {
            // Log
            LoggerFacade.Log("Setting last reward.", Category.Debug, Priority.Low);

            // Calculate
            reward = reward / 100;

            // Set
            LastReward = reward.ToString();
        }

        void SetBlockchainHeight(long height)
        {
            // Log
            LoggerFacade.Log("Setting blockchain height.", Category.Debug, Priority.Low);

            // Set
            BlockchainHeight = height.ToString();
        }

        void SetNetworkDifficulty(double difficulty)
        {
            // Log
            LoggerFacade.Log("Setting network difficulty.", Category.Debug, Priority.Low);

            // Set
            NetworkDifficulty = difficulty.ToString();
        }

        void SetNetworkLastBlockFound(long timestamp)
        {
            // Log
            LoggerFacade.Log("Setting network last block found.", Category.Debug, Priority.Low);

            // Get Current Time
            var blockDateTime = DateUtils.GetDateTimeFromUnixTimestamp(timestamp);

            // Calculate Minutes
            var minuteDifference = (DateTime.UtcNow - blockDateTime).TotalMinutes;
            var minuteFloor = (int)Math.Floor(minuteDifference);

            // Set
            NetworkLastBlockFound = minuteFloor.ToString();
        }

        void SetNetworkHashRate(double difficulty)
        {
            // Log
            LoggerFacade.Log("Setting network hashrate.", Category.Debug, Priority.Low);

            // Set
            NetworkHashRate = GetReadableHashRateString(difficulty / 60);
        }

        void SetPoolHashRate(double hashrate)
        {
            // Log
            LoggerFacade.Log("Setting pool hashrate.", Category.Debug, Priority.Low);

            // Set
            PoolHashRate = GetReadableHashRateString(hashrate);
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
    }
}
