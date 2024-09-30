using InventoryHelpers;

using libDB;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InventoryModels;

namespace InventoryManager
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

        public static void Main(string[] args)
        {
            BuildOptions();
            EnsureItems();
        }

        public static void BuildOptions()
        {
            _configuration = ConfigBuilder.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        }

#if DEBUG
        public static void EnsureItems()
        {
            EnsureItem("Airplane!");
            EnsureItem("The Rocky Horror Picture Show");
            EnsureItem("Elf");
            EnsureItem("Mars Attacks!");
            EnsureItem("Batman The Movie");
        }

        public static void EnsureItem(string name)
        {
            Random random = new Random();

            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                Item? currentItem = db.Items.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

                if (currentItem == null)
                {
                    Item item = new()
                    {
                        Name = name,
                        Description = string.Empty,
                        CreatedByUserId = _loggedInUserId,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        Quantity = random.Next(),
                        LastModifiedUserId = string.Empty,
                        Notes = string.Empty,
                    };

                    db.Items.Add(item);
                    db.SaveChanges();
                }
            }
        }
#endif
        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
    }
}
