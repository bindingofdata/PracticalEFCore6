using InventoryModels;

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
