using Newtonsoft.Json;

namespace ElectroneumSpace.Models
{
    public partial class Charts
    {
        [JsonProperty("hashrate")]
        public long[][] Hashrate { get; set; }

        [JsonProperty("workers")]
        public long[][] Workers { get; set; }

        [JsonProperty("difficulty")]
        public long[][] Difficulty { get; set; }
    }
}
