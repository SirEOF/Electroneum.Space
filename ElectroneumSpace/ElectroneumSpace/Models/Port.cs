using Newtonsoft.Json;

namespace ElectroneumSpace.Models
{
    public partial class Port
    {
        [JsonProperty("port")]
        public long PurplePort { get; set; }

        [JsonProperty("difficulty")]
        public long Difficulty { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }
    }
}
