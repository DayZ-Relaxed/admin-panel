using DayZRelaxed.Data;
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
        private readonly DayZRelaxedContext _context;

        public GetPlayerPosition(DayZRelaxedContext context)
        {
            _context = context;
        }

        // GET: api/getplayerposition/:territoryId
        [HttpGet("{territoryId}")]
        public async Task<ActionResult<IEnumerable<PlayerLog>>> GetPlayer(int territoryId)
        {
            if (_context.Territory == null) return NotFound();

            var playerLog = new List<PlayerLog>();
            var territory = await _context.Territory.FindAsync(territoryId);

            if (territory == null) return NotFound();

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var logFilePath = config.GetValue<string>("LogFilesPath");
            var logFileName = config.GetValue<string>("LogFileName");
            
            string[] dirs = Directory.GetDirectories(@logFilePath, "", SearchOption.TopDirectoryOnly);

            Regex playerPosRg = new Regex(@"^([\d]{2}:[\d]{2}:[\d]{2}) \| Player ""([ a-zA-z0-9]+)"" \(id=([_a-zA-Z0-9-=]+) pos=<([\d]+.[\d]), ([\d]+.[\d]), ([\d]+.[\d])>\)$", RegexOptions.Compiled);
            Regex adminLogRg = new Regex(@"AdminLog started on ([\d]+-[\d]+-[\d]+) at [\d]+:[\d]+:[\d]+", RegexOptions.Compiled);

            foreach (string dir in dirs)
            {
                string path = @$"{dir}\{logFileName}";
              
                var BufferSize = 128;
                using (var fileStream = System.IO.File.OpenRead(path))
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
                        if (!playerPosMatch.Success) continue;

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
                    }
                }
               
            }

            playerLog.Reverse();

            return playerLog;

        }

     
    }

   
}

