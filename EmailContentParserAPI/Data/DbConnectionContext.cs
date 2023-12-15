using EmailContentParserAPI.Model;
using EmailContentParserAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace EmailContentParserAPI.Data
{
    public class DbConnectionContext: DbContext
    {
         public DbConnectionContext (DbContextOptions<DbConnectionContext> options):base(options)
         {

         }
        public DbSet<ExpenseClaim> ExpenseClaims { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
