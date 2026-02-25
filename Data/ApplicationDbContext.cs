using ERP.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data
{   //this is basically the bridge between the code and the database, lets EF talk to azure
    public class ApplicationDbContext: DbContext // inherits from Db Context in EF
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { } // constructor
        public DbSet<Staff> Staff { get; set; } //maps to dbo.staff table
    }
}
