using Microsoft.EntityFrameworkCore;

namespace APBD_projekt.Models
{
    public class s17878DBContext : DbContext
    {
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Client> Client { get; set; }

        public s17878DBContext()
        {

        }
        public s17878DBContext(DbContextOptions options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Banner>(entity =>
            { 
                entity.Property(b => b.Area).HasColumnType("decimal(6, 2)");
                entity.Property(b => b.Price).HasColumnType("decimal(6, 2)");
            });
            modelBuilder.Entity<Building>(entity =>
            {
                entity.Property(b => b.Height).HasColumnType("decimal(6, 2)");
            });
            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.Property(e => e.PricePerSquareMeter).HasColumnType("decimal(6, 2)");
                entity.HasOne(d => d.BuildingFromId)
                        .WithMany(p => p.CampaignsFromId)
                        .HasForeignKey(d => d.FromIdBuilding)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Campaing_BuildingFromId");

                entity.HasOne(d => d.BuildingToId)
                        .WithMany(p => p.CampaignsToId)
                        .HasForeignKey(d => d.ToIdBuilding)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Campaign_BuildingToId");
            });
        }
    }
}
