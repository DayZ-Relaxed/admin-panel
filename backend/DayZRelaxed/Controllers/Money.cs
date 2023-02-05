using DayZRelaxed.Data;
using DayZRelaxed.Helpers;
using DayZRelaxed.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Money : ControllerBase
    {

        // GET: api/money/{mapId}
        [HttpGet("{mapId}")]
        public async Task<ActionResult<List<PlayerMoney>>> GetMoneyData(int mapId)
        {
            var playerMoney = new List<PlayerMoney>();

            var mapSettings = "settings-map0.json";
            if (mapId == 1) mapSettings = "settings-map1.json";

            var config = new ConfigurationBuilder().AddJsonFile(mapSettings).Build();

            var logFilePathMoney = config.GetValue<string>("DayZLogs:LogFilesPathMoney");
            List<String> files = Directory.GetFiles(logFilePathMoney, "", SearchOption.TopDirectoryOnly).ToList<String>();

            var BufferSize = 128;
            JsonSerializer serializer = new JsonSerializer();

            foreach (var filePath in files)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                using (var file = new JsonTextReader(sr))
                {
                    PlayerMoney money = (PlayerMoney)serializer.Deserialize(file, typeof(PlayerMoney));
                    playerMoney.Add(money);
                }
            } 

           return playerMoney;
        }
    }
}