using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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

    public partial class Stats
    {
        [JsonProperty("lastBlockFound")]
        public string LastBlockFound { get; set; }
    }

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

    public partial class Port
    {
        [JsonProperty("port")]
        public long PurplePort { get; set; }

        [JsonProperty("difficulty")]
        public long Difficulty { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }
    }

    public partial class Donation
    {
    }

    public partial class Charts
    {
        [JsonProperty("hashrate")]
        public long[][] Hashrate { get; set; }

        [JsonProperty("workers")]
        public long[][] Workers { get; set; }

        [JsonProperty("difficulty")]
        public long[][] Difficulty { get; set; }
    }

    public partial class PoolStats
    {
        public static PoolStats FromJson(string json) => JsonConvert.DeserializeObject<PoolStats>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PoolStats self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
