using eLibrayProjectDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eLibrayProjectDemo.Services
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        //create the cnstuctor
        public AppDbContext(DbContextOptions options):base(options)
        {
                
        }

        public DbSet<Book> Books { get; set; }
    }
}
