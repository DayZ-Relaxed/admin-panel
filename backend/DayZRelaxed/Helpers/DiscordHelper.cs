using DayZRelaxed.Models;
using Newtonsoft.Json;
using RestSharp;

namespace DayZRelaxed.Helpers
{
    public class DiscordHelper
    {
        const string API_ENDPOINT = "https://discord.com/api/v10";

        public string AccessToken { get; set; }

        public OAuthDiscordUser FetchUser()
        {
            var client = new RestClient(API_ENDPOINT);
            var req = new RestRequest("users/@me", Method.Get);
            req.AddHeader("Authorization", $"Bearer {this.AccessToken}");

            var res = client.Execute(req);

            return JsonConvert.DeserializeObject<OAuthDiscordUser>(res.Content);
        }
    }
}
