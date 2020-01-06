using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class BiuroContext:IdentityDbContext
{
        public BiuroContext(DbContextOptions<BiuroContext> options) :base(options)
        { }

        public DbSet<Jedzenie> Jedzenie { get; set; }
        public DbSet<Zakwaterowanie> Zakwaterowanie { get; set; }
        public DbSet<Wycieczka> Wycieczka { get; set; }
        public DbSet<Wycieczka_Klient> Wycieczka_Klient { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
    }
}
