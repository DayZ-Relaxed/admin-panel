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
    public class GetPlayerPosition : ControllerBase
    {
        private readonly DayZRelaxedContext0 contextMap0;
        private readonly DayZRelaxedContext1 contextMap1;

        public GetPlayerPosition(DayZRelaxedContext0 context0, DayZRelaxedContext1 context1)
        {
            contextMap0 = context0;
            contextMap1 = context1;
        }

        // GET: api/getplayerposition/{mapId}/{territoryId}
        [HttpGet("{mapId}/{territoryId}")]
        public async Task<ActionResult<IEnumerable<PlayerLog>>> GetPlayer(int mapId, int territoryId)
        {
            if (contextMap0.Territory == null || contextMap1.Territory == null) return NotFound();

            var playerLog = new List<PlayerLog>();
            Territory territory;
            if(mapId == 0)
            {
                territory = await contextMap0.Territory.FindAsync(territoryId);
            }
            else
            {
                territory = await contextMap1.Territory.FindAsync(territoryId);
            }

            if (territory == null) return NotFound();

            var logparser = new LogParser(mapId);
            List<String> dirs = logparser.getDirs();

            Regex playerPosRg = new Regex(@"^([\d]{2}:[\d]{2}:[\d]{2}) \| Player ""([ a-zA-z0-9.]+)"" \(id=([_a-zA-Z0-9-=]+) pos=<([\d]+.[\d]), ([\d]+.[\d]), ([\d]+.[\d])>\)$", RegexOptions.Compiled);
            Regex adminLogRg = new Regex(@"AdminLog started on ([\d]+-[\d]+-[\d]+) at [\d]+:[\d]+:[\d]+", RegexOptions.Compiled);
            Regex playerPosJsonRg = new Regex(@"^([\d]{2}:[\d]{2}:[\d]{2}) \| JSON(.*)$", RegexOptions.Compiled);

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
                    
                    while ((line = streamReader.ReadLine()) != null) {
                        if (date == "")
                        {
                            Match adminLogRgMatch = adminLogRg.Match(line);
                            if (adminLogRgMatch.Success) date = adminLogRgMatch.Groups[1].Value;
                        }

                        Match playerPosMatch = playerPosRg.Match(line);
                        if (playerPosMatch.Success)
                        {
                            var player = new PlayerLog()
                            {
                                Date = date,
                                Time = playerPosMatch.Groups[1].Value,
                                PlayerName = playerPosMatch.Groups[2].Value,
                                DayzId = playerPosMatch.Groups[3].Value,
                                PosX = Convert.ToDouble(playerPosMatch.Groups[4].Value.Replace(".", ",")),
                                PosY = Convert.ToDouble(playerPosMatch.Groups[5].Value.Replace(".", ",")),
                                PosZ = Convert.ToDouble(playerPosMatch.Groups[6].Value.Replace(".", ",")),
                            };

                            if (!player.IsInsideCircle(territory)) continue;

                            playerLog.Add(player);
                            continue;
                        }
                        
                        Match playerposJson = playerPosJsonRg.Match(line);
                        if(playerposJson.Success)
                        {
                            var log = playerposJson.Groups[2].Value.Replace("\"\"", "\"\\u201c");

                            var playerEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<LogEvent>(log);         
                            if (playerEvent.EventType != "PLAYER_POSITION") continue;
                         
                            var player = new PlayerLog()
                            {
                                Date = date,
                                Time = playerposJson.Groups[1].Value,
                                PlayerName = playerEvent.Player.Name,
                                DayzId = playerEvent.Player.DayZId,
                                PosX = Convert.ToDouble(playerEvent.Player.Position.PosX.Replace(".", ",")),
                                PosY = Convert.ToDouble(playerEvent.Player.Position.PosZ.Replace(".", ",")),
                                PosZ = Convert.ToDouble(playerEvent.Player.Position.PosY.Replace(".", ",")),
                            };

                            if (!player.IsInsideCircle(territory)) continue;

                            playerLog.Add(player);
                        }
                    }
                }    
            }

            playerLog.Reverse();

            return playerLog;
        }
    }
}

