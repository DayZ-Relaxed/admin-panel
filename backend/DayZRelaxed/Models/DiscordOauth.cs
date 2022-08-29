using System.Text.Json.Serialization;

namespace DayZRelaxed.Models
{
    public class DiscordOauth
    {
    }

    public class OauthTokenAuth
    {
        [JsonIgnore]
        public string access_token { get; set; }

        [JsonIgnore]
        public string token_type { get; set; }

        [JsonIgnore]
        public int expires_in { get; set; }

        [JsonIgnore]
        public string refresh_token { get; set; }

        [JsonIgnore]
        public string scope { get; set; }

        public string token { get; set; }
        public string? error { get; set; }
        public string? error_description { get; set; }
    }

    public class OAuthDiscordUser {
        public string id { get; set; }
        public string username { get; set; }
        public string discriminator { get; set; }
        public string avatar { get; set; }
        public bool? mfa_enabled { get; set; }
        public string? banner { get; set; }
        public string? accent_color { get; set; }
        public string? locale { get; set; }
        public int? flags { get; set; }
        public int? premium_type { get; set; }
        public int? public_flags { get; set; }
    }
}
