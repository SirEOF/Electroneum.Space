using Newtonsoft.Json;

namespace ElectroneumSpace.Models
{

    public partial class Network
    {
        [JsonProperty("difficulty")]
        public long Difficulty { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("reward")]
        public long Reward { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }

}
