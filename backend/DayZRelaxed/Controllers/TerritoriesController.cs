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
        private readonly DayZRelaxedContext _context;

        public TerritoriesController(DayZRelaxedContext context)
        {
            _context = context;
        }

        // GET: api/territories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TerritoryList>>> GetTerritory()
        {
            if (_context.Territory == null) return NotFound();

            var territory_list = await _context.Territory.ToListAsync();
            var territories = new List<TerritoryList>();

            
            foreach (var territory in territory_list)
            {
                var owner = _context.Player.Where(player => player.PlayerId == territory.OwnerPlayerId).Single();
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
