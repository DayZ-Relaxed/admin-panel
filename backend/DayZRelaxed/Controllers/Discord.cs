using DayZRelaxed.Data;
using DayZRelaxed.Helpers;
using DayZRelaxed.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using RestSharp;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Discord : ControllerBase
    {
      
        // GET: api/discord/user
        [HttpGet("user")]
        public async Task<ActionResult<OAuthDiscordUser>> GetDiscordUser()
        {
            var token = "";
            var cookies = Request.Cookies;
            foreach (var cookie in cookies)
            {
                if (cookie.Key == "token") token = cookie.Value;
            }    
 
            var jwtHelper = new JWTHelper() { Token = token };
            if (!jwtHelper.ValidateJWT()) return Unauthorized();

            var jwtData = jwtHelper.DecodeJWT();

            var discordHelper = new DiscordHelper()
            {
                AccessToken = (string) jwtData["access_token"]
            };

            return discordHelper.FetchUser();

        }
    }
}
