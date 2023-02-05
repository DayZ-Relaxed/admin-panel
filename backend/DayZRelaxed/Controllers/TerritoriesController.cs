using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DayZRelaxed.Data;
using DayZRelaxed.Models;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoriesController : ControllerBase
    {
        private readonly DayZRelaxedContext0 contextMap0;
        private readonly DayZRelaxedContext1 contextMap1;

        public TerritoriesController(DayZRelaxedContext0 context0, DayZRelaxedContext1 context1)
        {
            contextMap0 = context0;
            contextMap1 = context1;
        }

        // GET: api/territories/{mapId}
        [HttpGet("{mapId}")]
        public async Task<ActionResult<IEnumerable<TerritoryList>>> GetTerritory(int mapId)
        {
            if (contextMap0.Territory == null || contextMap1.Territory == null) return NotFound();

            List<Territory> territory_list;
            if (mapId == 0)
            {
                territory_list = await contextMap0.Territory.ToListAsync();
            }
            else
            {
                territory_list = await contextMap1.Territory.ToListAsync();
            }
           
            var territories = new List<TerritoryList>();

            
            foreach (var territory in territory_list)
            {
                Player owner;
                if(mapId == 0)
                {
                    owner = contextMap0.Player.Where(player => player.PlayerId == territory.OwnerPlayerId).Single();
                }
                else
                {
                    owner = contextMap1.Player.Where(player => player.PlayerId == territory.OwnerPlayerId).Single();
                }

                var territory_obj = new TerritoryList()
                {
                    LastFound = territory.LastFound,
                    OwnerBattleEyeId = owner.BattleEyeId,
                    OwnerCountry = owner.Country,
                    OwnerDayzId = owner.DayzId,
                    OwnerLastLogin = owner.LastLogin,
                    OwnerPlayerId = owner.PlayerId,
                    OwnerPlayerName = owner.PlayerName,
                    OwnerSteamId = owner.SteamId,
                    PosX = territory.PosX,
                    PosY = territory.PosY,
                    PosZ = territory.PosZ,
                    TerritoryId = territory.TerritoryId,
                };

                territories.Add(territory_obj);
            }
            
            return territories;
        }

    }
}
