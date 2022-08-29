using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DayZRelaxed.Data;
using DayZRelaxed.Models;

namespace DayZRelaxed.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GetTerritoryMembers : ControllerBase
    {
        private readonly DayZRelaxedContext _context;

        public GetTerritoryMembers(DayZRelaxedContext context)
        {
            _context = context;
        }

        // GET: api/getterritorymembers/:territoryId
        [HttpGet("{territoryId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayer(int territoryId)
        {
            if (_context.TerritoryMember == null) return NotFound();
                
            var territory_members_raw = await _context.TerritoryMember
                .Where(member => member.TerritoryId == territoryId)
                .ToListAsync();


            var territory_members = new List<Player>();
            foreach(var member in territory_members_raw)
            {
                var player = await _context.Player.FindAsync(member.MemberPlayerId);
                if (player == null) continue;

                territory_members.Add(player);
            }

            return territory_members;

        }
    }
}
