using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class BiuroContext:DbContext
{
        public BiuroContext(DbContextOptions<BiuroContext> options) 
            :base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Jedzenie> Jedzenie { get; set; }
        public DbSet<Zakwaterowanie> Zakwaterowanie { get; set; }
        public DbSet<Wycieczka> Wycieczka { get; set; }
        public DbSet<Wycieczka_Klient> Wycieczka_Klient { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }

    }
}
