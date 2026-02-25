using Microsoft.EntityFrameworkCore; // import the library to handle db Operations

namespace ERP.Models
{
    public class AppDbContext: DbContext // represents database session
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { } // constructor that receives DB configuration (connection string, provider)
        public DbSet<Staff> Staff { get; set; } // represents Staff table in the database, each record is one staff object
    }
}
