using Newtonsoft.Json;

namespace ElectroneumSpace.Models
{
    public partial class Config
    {
        [JsonProperty("ports")]
        public Port[] Ports { get; set; }

        [JsonProperty("hashrateWindow")]
        public long HashrateWindow { get; set; }

        [JsonProperty("fee")]
        public double Fee { get; set; }

        [JsonProperty("coin")]
        public string Coin { get; set; }

        [JsonProperty("coinUnits")]
        public long CoinUnits { get; set; }

        [JsonProperty("coinDifficultyTarget")]
        public long CoinDifficultyTarget { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("donation")]
        public Donation Donation { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("minPaymentThreshold")]
        public long MinPaymentThreshold { get; set; }

        [JsonProperty("denominationUnit")]
        public long DenominationUnit { get; set; }
    }
}
