using InventoryHelpers;

using libDB;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryDataMigrator
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

        static void Main(string[] args)
        {
            BuildOptions();
            ApplyMigrations();
            GenerateCustomSeedData();
        }

        private static void BuildOptions()
        {
            _configuration = ConfigBuilder.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        }

        private static void ApplyMigrations()
        {
            using InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options);
                db.Database.Migrate();
        }

        private static void GenerateCustomSeedData()
        {
            using (InventoryDbContext context = new InventoryDbContext(_optionsBuilder.Options))
            {
                CategoryBuilder categoryBuilder = new CategoryBuilder(context);
                categoryBuilder.GenerateSeedData();
                ItemBuilder itemBuilder = new ItemBuilder(context);
                itemBuilder.GenerateSeedData();
            }
        }
    }
}
