using DayZRelaxed.Data;
using DayZRelaxed.Models;
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using JWT.Builder;
using DayZRelaxed.Helpers;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Oauth : ControllerBase
    {

        // GET: api/oauth/:code
        // Discord OAuth
        [HttpGet("{code}")]
        public async Task<ActionResult<OauthTokenAuth>> OauthUser(string code)
        {
            string endpoint = "https://discord.com/api/v10";
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var clientId = config.GetValue<string>("Discord:OAuthClientId");
            var clientSecret = config.GetValue<string>("Discord:OAuthClientSecret");
            var redirectUri = config.GetValue<string>("Discord:OauthRedirctUri");

            var client = new RestClient(endpoint);
            var req = new RestRequest("oauth2/token", Method.Post);

            req.AddParameter("client_id", clientId, ParameterType.GetOrPost);
            req.AddParameter("grant_type", "authorization_code", ParameterType.GetOrPost);
            req.AddParameter("client_secret", clientSecret, ParameterType.GetOrPost);
            req.AddParameter("redirect_uri", redirectUri, ParameterType.GetOrPost);
            req.AddParameter("code", code, ParameterType.GetOrPost);

            var res = client.Execute(req);
            if (res.Content == null) return BadRequest();

            var resObject = JsonConvert.DeserializeObject<OauthTokenAuth>(res.Content);
            if (resObject == null) return BadRequest();
            if (resObject.error != null) if(resObject.error == "invalid_request") return BadRequest();

            var discordHelper = new DiscordHelper()
            {
                AccessToken = resObject.access_token
            };

            var user = discordHelper.FetchUser();

            List<string> allowedUsers = config.GetSection("Discord:AllowedUsers").Get<List<string>>();
            if (allowedUsers.IndexOf(user.id) == -1) return Unauthorized();

            var payload = new Dictionary<string, object>
            {
                { "access_token", resObject.access_token },
                { "expires_in", resObject.expires_in },
                { "refresh_token", resObject.refresh_token }
            };

            var jwtToken = config.GetValue<string>("JWTSecret");

            var token = JwtBuilder.Create()
                .AddClaims(payload) 
                .WithSecret(new[] { jwtToken })
                .IssuedAt(DateTime.Now)
                .ExpirationTime(DateTime.Now.AddHours(1))
                .WithAlgorithm(new HMACSHA256Algorithm())
                .Encode();  

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTimeOffset.Now.AddDays(1);
           
            Response.Cookies.Append("token", token, cookieOptions);

            resObject.token = token;

            return resObject;
        }
    }
}
