using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlumniConnect.API.Models;

namespace AlumniConnect.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<AlumniUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Event> Events { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Temoignage> Temoignages { get; set; }

    }
}
