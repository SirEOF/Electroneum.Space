using Newtonsoft.Json;

namespace ElectroneumSpace.Models
{
    public partial class PoolStats
    {
        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonProperty("network")]
        public Network Network { get; set; }

        [JsonProperty("pool")]
        public Pool Pool { get; set; }

        [JsonProperty("charts")]
        public Charts Charts { get; set; }
    }
}
