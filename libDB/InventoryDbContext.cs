using InventoryModels;
using InventoryModels.DTOs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace libDB
{
    public class InventoryDbContext : DbContext
    {
        private static IConfigurationRoot _configRoot;
        private static string _systemId = Environment.MachineName;

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GetItemsForListingDTO> ItemsForListing { get; set; }
        public DbSet<AllItemNamesPipeDelimitedDTO> AllItemNamesPipeDelimited { get; set; }
        public DbSet<GetItemsTotalValueDTO> ItemsTotalValues { get; set; }

        // Default constructor to support scaffolding
        public InventoryDbContext() { }

        public InventoryDbContext(DbContextOptions contextOptions) :
            base(contextOptions)
        {
            
        }

        public override int SaveChanges()
        {
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry.Entity is FullAuditModel referenceEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            referenceEntity.CreatedDate = DateTime.UtcNow;
                            if (string.IsNullOrWhiteSpace(referenceEntity.CreatedByUserId))
                            {
                                referenceEntity.CreatedByUserId = _systemId;
                            }
                            break;
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            referenceEntity.LastModifiedDate = DateTime.UtcNow;
                            if (string.IsNullOrWhiteSpace(referenceEntity.LastModifiedUserId))
                            {
                                referenceEntity.LastModifiedUserId = _systemId;
                            }
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(item => item.Players)
                .WithMany(player => player.Items)
                .UsingEntity<Dictionary<string, object>>(
                "ItemPlayers",
                itemPlayer => itemPlayer.HasOne<Player>()
                    .WithMany()
                    .HasForeignKey("PlayerId")
                    .HasConstraintName("FK_ItemPlayer_Players_PlayerId")
                    .OnDelete(DeleteBehavior.Cascade),
                playerItem => playerItem.HasOne<Item>()
                    .WithMany()
                    .HasForeignKey("ItemID")
                    .HasConstraintName("FK_PlayerItem_Items_ItemId")
                    .OnDelete(DeleteBehavior.Cascade)
                );

            modelBuilder.Entity<GetItemsForListingDTO>(itemsForListing =>
            {
                itemsForListing.HasNoKey();
                itemsForListing.ToView("ItemsForListing");
            });

            modelBuilder.Entity<AllItemNamesPipeDelimitedDTO>(itemNamesPipeDelimited =>
            {
                itemNamesPipeDelimited.HasNoKey();
                itemNamesPipeDelimited.ToView("AllItemNamesPipeDelimited");
            });

            DateTime genreCreateDate = new DateTime(2001, 01, 01);
            modelBuilder.Entity<Genre>(table =>
            {
                table.HasData(
                    new Genre() { Id = 1, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Fantasy" },
                    new Genre() { Id = 2, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Sci/Fi" },
                    new Genre() { Id = 3, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Horror" },
                    new Genre() { Id = 4, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Comedy" },
                    new Genre() { Id = 5, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Drama" },
                    new Genre() { Id = 6, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Act/Adv" });
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configRoot = builder.Build();
                optionsBuilder.UseSqlServer(_configRoot.GetConnectionString("InventoryManager"));
            }
        }
    }
}
