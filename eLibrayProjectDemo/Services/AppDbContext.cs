using eLibrayProjectDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace eLibrayProjectDemo.Services
{
    public class AppDbContext : DbContext
    {
        //create the cnstuctor
        public AppDbContext(DbContextOptions options):base(options)
        {
                
        }

        public DbSet<Book> Books { get; set; }
    }
}
