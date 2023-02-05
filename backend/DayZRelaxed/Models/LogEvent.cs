using Newtonsoft.Json;

namespace DayZRelaxed.Models
{
    public class LogEvent
    {
        [JsonProperty("event")]
        public string EventType { get; set; }
        [JsonProperty("data")]
        public EventData? Data { get; set; }
        [JsonProperty("player")]
        public EventPlayer Player { get; set; }
    }

    public class EventData
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }

    public class EventPlayer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dzid")]
        public string DayZId { get; set; }
        [JsonProperty("steamId")]
        public string SteamId { get; set; }
        [JsonProperty("position")]
        public Coordinates Position { get; set; }
        [JsonProperty("direction")]
        public Coordinates Direction { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("x")]
        public string PosX { get; set; }
        [JsonProperty("y")]
        public string PosY { get; set; }
        [JsonProperty("z")]
        public string PosZ { get; set; }
    }
}
