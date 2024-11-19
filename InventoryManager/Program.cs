using InventoryHelpers;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Net.NetworkInformation;
using System.Text;

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
            GetFullitemDetails();
#endif
        }

#if DEBUG
        private static void GetFullitemDetails()
        {
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                IQueryable<FullItemDetailsDto> result = db.FullItemDetails.FromSqlRaw(
                    "SELECT * FROM [dbo].[vwFullItemDetails] ORDER BY ItemName, GenreName, Category, PlayerName");

                StringBuilder resultView = new StringBuilder();
                foreach (FullItemDetailsDto item in result)
                {
                    resultView.Clear();
                    resultView.Append($"Item] {item.Id, -10}");
                    resultView.Append($"|{item.ItemName,-50}");
                    resultView.Append($"|{item.ItemDescription,-4}");
                    resultView.Append($"|{item.PlayerName,-5}");
                    resultView.Append($"|{item.Category,-5}");
                    resultView.Append($"|{item.GenreName,-5}");
                    Console.WriteLine(resultView.ToString());
                }
            }
        }
#endif

        public static void BuildOptions()
        {
            _configuration = ConfigBuilder.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        }

        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
    }
}
