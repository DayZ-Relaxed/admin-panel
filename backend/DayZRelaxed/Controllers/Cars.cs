using DayZRelaxed.Data;
using DayZRelaxed.Helpers;
using DayZRelaxed.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cars : ControllerBase
    {

        // GET: api/cars/{mapId}/covers?playerName=admin&posX=123&posY&posZ?daysAgo=5
        [HttpGet("{mapId}/covers")] 
        public async Task<ActionResult<List<CarCover>>> GetCarData(int mapId, string? playerName, double? posX, double? posY, double? posZ, int? daysAgo)
        {
            var carcovers = new List<CarCover>();

            Regex carcoverRg = new Regex(@"^([\d]{2}.[\d]{2}.[\d]{4} [\d]{2}:[\d]{2}:[\d]{2}) \[CarCover] Player ([ a-zA-z0-9.]+) \(([\d]+)\) (covered|uncovered) ([a-zA-Z0-9_]+) at <([\d]+.[\d]+), ([\d]+.[\d]+), ([\d]+.[\d]+)>$", RegexOptions.Compiled);

            var mapSettings = "settings-map0.json";
            if (mapId == 1) mapSettings = "settings-map1.json";

            var config = new ConfigurationBuilder().AddJsonFile(mapSettings).Build();
          
            var logFilePathCover = config.GetValue<string>("DayZLogs:LogFilesPathCarCover");
            var logFileNameCover = config.GetValue<string>("DayZLogs:LogFileNameCarCover");

            var BufferSize = 128;

            string path = @$"{logFilePathCover}\{logFileNameCover}";
            if (!System.IO.File.Exists(path))
            {
                Console.WriteLine($"Was unable to find the file of Car covers inside of the path {logFilePathCover}\\{logFileNameCover}");
                // TODO kill request
            }


            var logparser = new LogParser(mapId);
            List<String> dirs = logparser.getDirs();

            Regex adminLogRg = new Regex(@"AdminLog started on ([\d]+-[\d]+-[\d]+) at [\d]+:[\d]+:[\d]+", RegexOptions.Compiled);
            Regex carEventRg = new Regex(@"^([\d]{2}:[\d]{2}:[\d]{2}) \| JSON(.*)$", RegexOptions.Compiled);

            foreach (string dir in dirs)
            {
                if (!System.IO.File.Exists(logparser.getFileName(dir))) continue;

                using (var fileStream = new FileStream(logparser.getFileName(dir), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    var line = "";
                    var date = "";
                        
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (date == "")
                        {
                            Match adminLogRgMatch = adminLogRg.Match(line);
                            if (adminLogRgMatch.Success) date = adminLogRgMatch.Groups[1].Value;
                        }

                     
                        Match carJson = carEventRg.Match(line);
                        if (!carJson.Success) continue;

                        var log = carJson.Groups[2].Value.Replace("\"\"", "\"\\u201c");
                        var carEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<LogEvent>(log);
                        if (carEvent.EventType != "PLAYER_VEHICLE") continue;
                       
                        var carcover = new CarCover()
                        {
                            Date = DateTime.ParseExact($"{date} {carJson.Groups[1].Value}", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                            PlayerName = carEvent.Player.Name,
                            SteamId = carEvent.Player.SteamId,
                            Action = carEvent.Data.Tag[1..],
                            Car = "Unknown",
                            PosX = Convert.ToDouble(carEvent.Player.Position.PosX.Replace(".", ",")),
                            PosY = Convert.ToDouble(carEvent.Player.Position.PosY.Replace(".", ",")),
                            PosZ = Convert.ToDouble(carEvent.Player.Position.PosZ.Replace(".", ",")),
                        };

                        if (daysAgo == null) daysAgo = 1;

                        if (carcover.Date < DateTime.Now.AddDays((double)-daysAgo)) continue;
                        if (posX != null && !carcover.IsInsidePosX((double)posX)) continue;
                        if (posZ != null && !carcover.IsInsidePosZ((double)posZ)) continue;
                        if (posY != null && !carcover.IsInsidePosY((double)posY)) continue;
                        if (playerName != null && carcover.PlayerName != playerName) continue;

                        carcovers.Add(carcover);
                    }
                }
            }

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                var line = "";
                
                while ((line = streamReader.ReadLine()) != null)
                {
                    Match carcoverMatch = carcoverRg.Match(line);
                    if (!carcoverMatch.Success) continue;

                    var carcover = new CarCover()
                    {
                        Date = DateTime.ParseExact(carcoverMatch.Groups[1].Value, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                        PlayerName = carcoverMatch.Groups[2].Value,
                        SteamId = carcoverMatch.Groups[3].Value,
                        Action = carcoverMatch.Groups[4].Value,
                        Car = carcoverMatch.Groups[5].Value,
                        PosX = Convert.ToDouble(carcoverMatch.Groups[6].Value.Replace(".", ",")),
                        PosY = Convert.ToDouble(carcoverMatch.Groups[7].Value.Replace(".", ",")),
                        PosZ = Convert.ToDouble(carcoverMatch.Groups[8].Value.Replace(".", ",")),
                    };

                    if (daysAgo == null) daysAgo = 1;

                    if (carcover.Date < DateTime.Now.AddDays((double) -daysAgo)) continue;
                    if (posX != null &&  !carcover.IsInsidePosX((double) posX)) continue;
                    if (posZ != null &&  !carcover.IsInsidePosZ((double) posZ)) continue;
                    if (posY != null && !carcover.IsInsidePosY((double) posY)) continue;
                    if (playerName != null && carcover.PlayerName != playerName) continue;

                    carcovers.Add(carcover);
                }
            }

            carcovers = carcovers.OrderBy(o => o.Date).ToList();

            carcovers.Reverse();

            return carcovers;
        }
    }
}