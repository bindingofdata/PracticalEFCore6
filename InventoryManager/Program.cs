using libDB;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryManager
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

        static void Main(string[] args)
        {
            
        }

        static void BuildOptions()
        {
            _configuration = ConfigBuilder.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        }
    }
}
