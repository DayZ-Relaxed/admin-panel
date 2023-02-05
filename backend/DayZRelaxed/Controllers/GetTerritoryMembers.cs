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
        private readonly DayZRelaxedContext0 contextMap0;
        private readonly DayZRelaxedContext1 contextMap1;

        public GetTerritoryMembers(DayZRelaxedContext0 context0, DayZRelaxedContext1 context1)
        {
            contextMap0 = context0;
            contextMap1 = context1;
        }

        // GET: api/getterritorymembers/{mapId}/{territoryId}
        [HttpGet("{mapId}/{territoryId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayer(int mapId, int territoryId)
        {
            if (contextMap0.TerritoryMember == null || contextMap1.TerritoryMember == null) return NotFound();

            List<TerritoryMember> territory_members_raw;
            if(mapId == 0)
            {
                territory_members_raw = await contextMap0.TerritoryMember
                .Where(member => member.TerritoryId == territoryId)
                .ToListAsync();
            }
            else
            {
                territory_members_raw = await contextMap1.TerritoryMember
                .Where(member => member.TerritoryId == territoryId)
                .ToListAsync();
            }


            var territory_members = new List<Player>();
            foreach(var member in territory_members_raw)
            {
                Player player;
                if(mapId == 0)
                {
                    player = await contextMap0.Player.FindAsync(member.MemberPlayerId);
                }
                else
                {
                    player = await contextMap1.Player.FindAsync(member.MemberPlayerId);
                }

                if (player == null) continue;

                territory_members.Add(player);
            }

            return territory_members;

        }
    }
}
