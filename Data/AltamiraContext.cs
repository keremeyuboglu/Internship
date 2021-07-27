using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.Entities
{
    public class AltamiraContext : DbContext
    {
        public AltamiraContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }


    }


}
