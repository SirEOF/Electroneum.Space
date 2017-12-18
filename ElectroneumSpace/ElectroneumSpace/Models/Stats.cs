using Newtonsoft.Json;

namespace ElectroneumSpace.Models
{
    public partial class Stats
    {
        [JsonProperty("lastBlockFound")]
        public string LastBlockFound { get; set; }
    }
}
