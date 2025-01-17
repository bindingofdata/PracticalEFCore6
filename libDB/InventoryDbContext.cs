﻿using InventoryModels;
using InventoryModels.Dtos;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

using System.Text;

namespace libDB
{
    public class InventoryDbContext : DbContext
    {
        private static IConfigurationRoot _configRoot;
        private static string _systemId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Environment.UserName));
        private const string SEED_USER_ID = "873fb5cd-ad6b-458d-ab59-3c5eca45a368";

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GetItemsForListingDto> ItemsForListing { get; set; }
        public DbSet<AllItemNamesPipeDelimitedDto> AllItemNamesPipeDelimited { get; set; }
        public DbSet<GetItemsTotalValueDto> ItemsTotalValues { get; set; }
        public DbSet<FullItemDetailsDto> FullItemDetails { get; set; }

        // Default constructor to support scaffolding
        public InventoryDbContext() { }

        public InventoryDbContext(DbContextOptions contextOptions) :
            base(contextOptions) { }

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

            modelBuilder.Entity<GetItemsForListingDto>(itemsForListing =>
            {
                itemsForListing.HasNoKey();
                itemsForListing.ToView("ItemsForListing");
            });

            modelBuilder.Entity<AllItemNamesPipeDelimitedDto>(itemNamesPipeDelimited =>
            {
                itemNamesPipeDelimited.HasNoKey();
                itemNamesPipeDelimited.ToView("AllItemNamesPipeDelimited");
            });

            DateTime genreCreateDate = new DateTime(2001, 01, 01);
            modelBuilder.Entity<Genre>(table =>
            {
                table.HasData(
                    new Genre() { Id = 1, CreatedDate = genreCreateDate, CreatedByUserId = SEED_USER_ID, IsActive = true, IsDeleted = false, Name = "Fantasy" },
                    new Genre() { Id = 2, CreatedDate = genreCreateDate, CreatedByUserId = SEED_USER_ID, IsActive = true, IsDeleted = false, Name = "Sci/Fi" },
                    new Genre() { Id = 3, CreatedDate = genreCreateDate, CreatedByUserId = SEED_USER_ID, IsActive = true, IsDeleted = false, Name = "Horror" },
                    new Genre() { Id = 4, CreatedDate = genreCreateDate, CreatedByUserId = SEED_USER_ID, IsActive = true, IsDeleted = false, Name = "Comedy" },
                    new Genre() { Id = 5, CreatedDate = genreCreateDate, CreatedByUserId = SEED_USER_ID, IsActive = true, IsDeleted = false, Name = "Drama" },
                    new Genre() { Id = 6, CreatedDate = genreCreateDate, CreatedByUserId = SEED_USER_ID, IsActive = true, IsDeleted = false, Name = "Act/Adv" });
            });

            modelBuilder.Entity<FullItemDetailsDto>(x =>
            {
                x.HasNoKey();
                x.ToView("FullItemDetailsDto");
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
