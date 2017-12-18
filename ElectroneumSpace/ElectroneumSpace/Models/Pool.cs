using Newtonsoft.Json;

namespace ElectroneumSpace.Models
{
    public partial class Pool
    {
        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("blocks")]
        public string[] Blocks { get; set; }

        [JsonProperty("totalBlocks")]
        public long TotalBlocks { get; set; }

        [JsonProperty("payments")]
        public string[] Payments { get; set; }

        [JsonProperty("totalPayments")]
        public long TotalPayments { get; set; }

        [JsonProperty("totalMinersPaid")]
        public long TotalMinersPaid { get; set; }

        [JsonProperty("miners")]
        public long Miners { get; set; }

        [JsonProperty("hashrate")]
        public double Hashrate { get; set; }

        [JsonProperty("roundHashes")]
        public long RoundHashes { get; set; }

        [JsonProperty("lastBlockFound")]
        public string LastBlockFound { get; set; }
    }
}
