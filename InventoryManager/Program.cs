using AutoMapper;

using InventoryHelpers;

using InventoryModels;
using InventoryModels.Dtos;
using InventoryModels.DTOs;

using libDB;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Net.NetworkInformation;
using System.Text;

namespace InventoryManager
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            BuildOptions();
            BuildMapper();
#if DEBUG
            ListInventory();
            GetFullitemDetails();
#endif
        }

        public static void BuildOptions()
        {
            _configuration = ConfigBuilder.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
        }

        private static void BuildMapper()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(InventoryMapper));
            _serviceProvider = services.BuildServiceProvider();

            _mapperConfiguration = new MapperConfiguration(config =>
                config.AddProfile<InventoryMapper>());
            _mapperConfiguration.AssertConfigurationIsValid();
            _mapper = _mapperConfiguration.CreateMapper();
        }

#if DEBUG
        private static void ListInventory()
        {
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                List<Item> items = db.Items.OrderBy(x => x.Name).ToList();
                List<ItemDto> results = _mapper.Map<List<Item>, List<ItemDto>>(items);
                results.ForEach(item => Console.WriteLine($"Item: {item.Name}"));
            }
        }

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

        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
    }
}
