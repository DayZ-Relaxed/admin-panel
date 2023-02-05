using Newtonsoft.Json;

namespace DayZRelaxed.Models
{
    public class PlayerMoney
    {
        [JsonProperty("Version")]
        public string Version { get; set; }
        
        [JsonProperty("SteamID64")]
        public string SteamId { get; set; }
        
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("MoneyAmount")]
        public int MoneyAmount { get; set; }

        [JsonProperty("MaxAmount")]
        public int MaxAmount { get; set; }

        [JsonProperty("Licences")]
        public Licence[] Licences { get; set; }

        [JsonProperty("Insurances")]
        public Insurance Insurances { get; set; }
    }

    public class Licence
    {
    }

    public class Insurance
    {
    }
    }
