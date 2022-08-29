using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DayZRelaxed.Models;

namespace DayZRelaxed.Data
{
    public class DayZRelaxedContext : DbContext
    {
        public DayZRelaxedContext (DbContextOptions<DayZRelaxedContext> options)
            : base(options)
        {
        }

        public DbSet<DayZRelaxed.Models.Player> Player { get; set; } = default!;
        public DbSet<DayZRelaxed.Models.Territory>? Territory { get; set; }
        public DbSet<DayZRelaxed.Models.TerritoryMember>? TerritoryMember { get; set; }

    }
}
