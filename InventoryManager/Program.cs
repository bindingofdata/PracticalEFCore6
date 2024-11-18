using InventoryHelpers;

using InventoryModels;
using InventoryModels.DTOs;

using libDB;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Net.NetworkInformation;

namespace InventoryManager
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

        public static void Main(string[] args)
        {
            BuildOptions();
        }

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
