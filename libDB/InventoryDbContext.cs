using InventoryModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace libDB
{
    public class InventoryDbContext : DbContext
    {
        private static IConfigurationRoot _configRoot;

        public DbSet<Item> Items { get; set; }

        // Default constructor to support scaffolding
        public InventoryDbContext() { }

        public InventoryDbContext(DbContextOptions contextOptions) :
            base(contextOptions)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configRoot = builder.Build();
            }
        }
    }
}
