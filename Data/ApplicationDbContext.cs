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
        public DbSet<Emploi> Emplois { get; set; }
        public DbSet<Experience> Experiences { get; set; }







        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuration pour Event (si vous en avez)

            // Configuration pour Emploi
            builder.Entity<Emploi>(entity =>
            {

                entity.Property(e => e.Titre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Entreprise).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Localisation).HasMaxLength(100);

                // Index pour améliorer les performances
                entity.HasIndex(e => e.DateFin);
                entity.HasIndex(e => e.Localisation);
            });

            // Configuration pour Experience
            builder.Entity<Experience>(entity =>
            {
                entity.Property(e => e.Poste).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Entreprise).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Localisation).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.TypeContrat).HasMaxLength(50);
                entity.Property(e => e.Secteur).HasMaxLength(100);

                // Relation avec AlumniUser
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Index pour améliorer les performances
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.DateDebut);
                entity.HasIndex(e => e.Entreprise);
                entity.HasIndex(e => e.EnCours);
            });
        }
    }
}
