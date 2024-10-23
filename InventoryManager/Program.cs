using InventoryHelpers;

using InventoryModels;
using InventoryModels.DTOs;

using libDB;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryManager
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

        public static void Main(string[] args)
        {
            BuildOptions();
#if DEBUG
            EnsureItems();
            GetItemsForListing();
#endif

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
            EnsureItem("Airplane!",
                "Don't call me Shirley.",
                "Robert Hays & Julie Hagerty");
            EnsureItem("The Rocky Horror Picture Show",
                "Turn yourself over, to absolute pleasure.",
                "Tim Curry, Susan Sarandon, Barry Bostwick");
            EnsureItem("Elf",
                "Son of a nutcracker!",
                "Will Ferrell & James Caan");
            EnsureItem("Mars Attacks!",
                "Ack ack. ACK!",
                "Everyone and their mom is in this movie.");
            EnsureItem("Batman The Movie",
                "Sometimes you just can't get rid of a bomb.",
                "Burt Ward & Adam West");
        }

        public static void EnsureItem(string name, string description, string notes)
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
                        Description = description,
                        IsActive = true,
                        Quantity = random.Next(1, 1_000),
                        LastModifiedUserId = string.Empty,
                        Notes = notes,
                    };

                    db.Items.Add(item);
                    db.SaveChanges();
                }
            }
        }

        private static void UpdateItems()
        {
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                List<Item> items = db.Items.ToList();
                foreach (Item item in items)
                {
                    item.CurrentOrFinalPrice = 9.99M;
                }
                
                db.Items.UpdateRange(items);
                db.SaveChanges();
            }
        }

        private static void GetItemsForListing()
        {
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                foreach (GetItemsForListingDTO item in db.ItemsForListing
                    .FromSqlRaw("EXECUTE dbo.GetItemsForListing")
                    .ToList())
                {
                    string output = $"ITEM {item.Name} - \"{item.Description}\"";
                    if (!string.IsNullOrWhiteSpace(item.CategoryName))
                    {
                        output += $" [{item.CategoryName}]";
                    }
                    Console.WriteLine(output);
                }
            }
        }
#endif
        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
    }
}
