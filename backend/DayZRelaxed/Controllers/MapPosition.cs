using DayZRelaxed.Data;
using DayZRelaxed.Helpers;
using DayZRelaxed.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapPositions : ControllerBase
    {

        // GET: api/mappositions/{mapId}?posX=123&posZ=123?daysAgo=5
        [HttpGet("{mapId}")]
        public async Task<ActionResult<IEnumerable<MapPosition>>> GetMapPosition(int mapId, double posX, double posZ, int? daysAgo, double radius)
        {
            var mapPositions = new List<MapPosition>();
          
            var logparser = new LogParser(mapId);
            List<String> dirs = logparser.getDirs();

            Regex playerPosRg = new Regex(@"^([\d]{2}:[\d]{2}:[\d]{2}) \| Player ""([ a-zA-z0-9.]+)"" \(id=([_a-zA-Z0-9-=]+) pos=<([\d]+.[\d]), ([\d]+.[\d]), ([\d]+.[\d])>\)$", RegexOptions.Compiled);
            Regex adminLogRg = new Regex(@"AdminLog started on ([\d]+-[\d]+-[\d]+) at [\d]+:[\d]+:[\d]+", RegexOptions.Compiled);
            Regex playerPosJsonRg = new Regex(@"^([\d]{2}:[\d]{2}:[\d]{2}) \| JSON({[a-zA-Z0-9=. \-#"":_,{}]*)$", RegexOptions.Compiled);

            var BufferSize = 128;

            foreach (string dir in dirs)
            {
                string path = logparser.getFileName(dir);

                if (!System.IO.File.Exists(path)) continue;

                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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

                        Match playerPosMatch = playerPosRg.Match(line);
                        if (playerPosMatch.Success)
                        {
                           
                            var mapPos = new MapPosition(radius)
                            {
                                Date = DateTime.ParseExact($"{date} {playerPosMatch.Groups[1].Value}", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                                PlayerName = playerPosMatch.Groups[2].Value,
                                PosX = Convert.ToDouble(playerPosMatch.Groups[4].Value.Replace(".", ",")),
                                PosY = Convert.ToDouble(playerPosMatch.Groups[5].Value.Replace(".", ",")),
                                PosZ = Convert.ToDouble(playerPosMatch.Groups[6].Value.Replace(".", ",")),
                            };

                            if (daysAgo == null) daysAgo = 10;
                            if (mapPos.Date < DateTime.Now.AddDays((double)-daysAgo)) continue;
                            if (!mapPos.IsInsideCircle((double)posX, (double)posZ)) continue;

                            mapPositions.Add(mapPos);
                            continue;
                        }

                        Match playerposJson = playerPosJsonRg.Match(line);
                        if (playerposJson.Success)
                        {

                            var playerEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<LogEvent>(playerposJson.Groups[2].Value);
                            if (playerEvent.EventType == null) continue;

                            var mapPos = new MapPosition(radius)
                            {
                                Date = DateTime.ParseExact($"{date} {playerposJson.Groups[1].Value}", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), 
                                PlayerName = playerEvent.Player.Name,
                                PosX = Convert.ToDouble(playerEvent.Player.Position.PosX.Replace(".", ",")),
                                PosY = Convert.ToDouble(playerEvent.Player.Position.PosZ.Replace(".", ",")),
                                PosZ = Convert.ToDouble(playerEvent.Player.Position.PosY.Replace(".", ",")),
                            };

                            if (daysAgo == null) daysAgo = 1;
                            if (mapPos.Date < DateTime.Now.AddDays((double)-daysAgo)) continue;
                            if (!mapPos.IsInsideCircle((double)posX, (double)posZ)) continue;

                            mapPositions.Add(mapPos);
                        }


                    }
                }
            }


            mapPositions.Reverse();
            
            return mapPositions;

        }
        
    }
}
