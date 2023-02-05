using DayZRelaxed.Data;
using DayZRelaxed.Helpers;
using DayZRelaxed.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleDamage : ControllerBase
    {

        // GET: api/vehicledamage/{mapId}?posX=123&posY&posZ?daysAgo=5&radius=50
        [HttpGet("{mapId}")]
        public async Task<ActionResult<List<Models.VehicleDamage>>> GetVehicleDamage(int mapId, double? posX, double? posZ, int daysAgo, int? radius)
        {
            var vehicleDmg = new List<Models.VehicleDamage>();
            Regex vehicleDmgRg = new Regex(@"^([0-9]+-[0-9]+-[0-9]+ [0-9]+:[0-9]+:[0-9]+)\|([ a-zA-z0-9.]+)\|Vehicle\|([ a-zA-z0-9.]+)\|Weapon\|([ a-zA-z0-9.-]+)\|Ammo\|([ a-zA-z0-9.]+)\|Position\|([0-9.]+), ([0-9.]+), ([0-9.]+)\|Zone\|(.*)$", RegexOptions.Compiled);

            var mapSettings = "settings-map0.json";
            if (mapId == 1) mapSettings = "settings-map1.json";

            var config = new ConfigurationBuilder().AddJsonFile(mapSettings).Build();

            var logFilePath = config.GetValue<string>("DayZLogs:LogFilesPathKillNotifications");
            var activeLogFilePath = config.GetValue<string>("DayZLogs:LogFilesPathKillNotificationsActive");
            var logFileName = config.GetValue<string>("DayZLogs:LogFileNameKillNotifications");

            List<String> files = Directory.GetFiles(logFilePath, "", SearchOption.TopDirectoryOnly).ToList<String>();
            files.Add(@$"{activeLogFilePath}\{logFileName}");

            var BufferSize = 128;

            foreach (var filePath in files)
            {
                if (!System.IO.File.Exists(filePath)) continue;

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    var line = "";
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Match vehicleDamageMatch = vehicleDmgRg.Match(line);
                        if (vehicleDamageMatch.Success)
                        {
                            var vehicleDamage = new Models.VehicleDamage()
                            {
                                Date = DateTime.ParseExact($"{vehicleDamageMatch.Groups[1].Value}", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                                PlayerName = vehicleDamageMatch.Groups[2].Value,
                                VehicleName = vehicleDamageMatch.Groups[3].Value,
                                Weapon = vehicleDamageMatch.Groups[4].Value,
                                Ammo = vehicleDamageMatch.Groups[5].Value,
                                PosX = Convert.ToDouble(vehicleDamageMatch.Groups[6].Value.Replace(".", ",")),
                                PosY = Convert.ToDouble(vehicleDamageMatch.Groups[7].Value.Replace(".", ",")),
                                PosZ = Convert.ToDouble(vehicleDamageMatch.Groups[8].Value.Replace(".", ",")),
                                Zone = vehicleDamageMatch.Groups[9].Value,
                            };

                            if (vehicleDamage.Date < DateTime.Now.AddDays((double)-daysAgo)) continue;
                            if(posX != null && posZ != null && radius != null)
                            {
                                if (!vehicleDamage.IsInsideCircle((double)posX, (double)posZ, (double)radius)) continue;
                            }

                            vehicleDmg.Add(vehicleDamage);
                        }
                    }
                }
            }

            vehicleDmg.Reverse();
            return vehicleDmg;
        }
    }
}