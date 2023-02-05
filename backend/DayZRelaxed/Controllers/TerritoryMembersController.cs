using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DayZRelaxed.Data;
using DayZRelaxed.Models;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoryMembersController : ControllerBase
    {
        private readonly DayZRelaxedContext0 contextMap0;
        private readonly DayZRelaxedContext1 contextMap1;

        public TerritoryMembersController(DayZRelaxedContext0 context0, DayZRelaxedContext1 context1)
        {
            contextMap0 = context0;
            contextMap1 = context1;
        }

        // GET: api/territorymembers/{mapId}
        [HttpGet("{mapId}")]
        public async Task<ActionResult<IEnumerable<TerritoryMember>>> GetTerritoryMember(int mapId)
        {
          if (contextMap0.TerritoryMember == null || contextMap1.TerritoryMember == null) return NotFound();

          if(mapId == 0)
            {
                return await contextMap0.TerritoryMember.ToListAsync();
            }
          else
            {
                return await contextMap1.TerritoryMember.ToListAsync();
            }
        }

       
    }
}
