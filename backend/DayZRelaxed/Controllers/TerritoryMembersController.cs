using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly DayZRelaxedContext _context;

        public TerritoryMembersController(DayZRelaxedContext context)
        {
            _context = context;
        }

        // GET: api/territorymembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TerritoryMember>>> GetTerritoryMember()
        {
          if (_context.TerritoryMember == null) return NotFound();

            Console.WriteLine(_context.TerritoryMember.First().MemberDayzId);

          return await _context.TerritoryMember.ToListAsync();
        }

       
    }
}
