using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class BiuroContext:DbContext
{
        public DbSet<User> User { get; set; }

}
}
