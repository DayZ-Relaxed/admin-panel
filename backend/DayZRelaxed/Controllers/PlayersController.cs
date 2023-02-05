using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DayZRelaxed.Data;
using DayZRelaxed.Models;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly DayZRelaxedContext0 contextMap0;
        private readonly DayZRelaxedContext1 contextMap1;

        public PlayersController(DayZRelaxedContext0 context0, DayZRelaxedContext1 context1)
        {
            contextMap0 = context0;
            contextMap1 = context1;
        }

        // GET: api/players/{mapId}
        [HttpGet("{mapId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayer(int mapId)
        {
            if (mapId == 0)
            {
                if (contextMap0.Player == null) return NotFound();
                return await contextMap0.Player.ToListAsync(); ;
            }
          
            if (contextMap1.Player == null) return NotFound();
            return await contextMap1.Player.ToListAsync();
        }

        // GET: api/players/{mapId}/{id}
        [HttpGet("{mapId}/{id}")]
        public async Task<ActionResult<Player>> GetPlayerByPlayerId(int mapId, int id)
        {
            Player player;
            if (mapId == 0)
            {
                player = await contextMap0.Player.FindAsync(id);
            }
            else
            {
                player = await contextMap1.Player.FindAsync(id);
            }

            if (player == null) return NotFound();
            return player;
        }
    }
}
