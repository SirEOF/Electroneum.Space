using ElectroneumSpace.Models;
using Prism.Mvvm;
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

    }

    public class PoolService : BindableBase, IPoolService
    {

        IPoolApi PoolApi { get; set; }

        public PoolService()
        {
            PoolApi = RestService.For<IPoolApi>("http://api.electroneum.space/v1");

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
            var t = await PoolApi.GetPoolStatisticsAsync();
            return true;
        }
    }
}
