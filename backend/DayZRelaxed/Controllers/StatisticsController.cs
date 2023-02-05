using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DayZRelaxed.Data;
using DayZRelaxed.Models;

namespace DayZRelaxed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly DayZRelaxedContext0 contextMap0;
        private readonly DayZRelaxedContext1 contextMap1;

        public StatisticsController(DayZRelaxedContext0 context0, DayZRelaxedContext1 context1)
        {
            contextMap0 = context0;
            contextMap1 = context1;
        }

        // GET: api/statistics/{mapId}
        [HttpGet("{mapId}")]
        public async Task<ActionResult<IEnumerable<Statistics>>> GetPlayer(int mapId)
        {
            if (mapId == 0)
            {
                if (contextMap0.Statistics == null) return NotFound();
                return await contextMap0.Statistics.ToListAsync(); ;
            }
          
            if (contextMap1.Statistics == null) return NotFound();
            return await contextMap1.Statistics.ToListAsync();
        }
    }
}
