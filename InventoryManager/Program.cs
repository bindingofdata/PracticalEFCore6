using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryBusinessLayer;

using InventoryHelpers;

using InventoryModels;
using InventoryModels.Dtos;

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
        private static IItemsService _itemsService;
        private static ICategoriesService _categoriesService;

        public static void Main(string[] args)
        {
            BuildOptions();
            BuildMapper();
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                _itemsService = new ItemsService(db, _mapper);
                _categoriesService = new CategoriesService(db, _mapper);
                PrintSectionHeader(nameof(ListInventory));
                ListInventory();

                PrintSectionHeader(nameof(GetItemsForListing));
                GetItemsForListing();

                PrintSectionHeader(nameof(GetAllItemsAsPipeDelimitedString));
                GetAllItemsAsPipeDelimitedString();

                PrintSectionHeader(nameof(GetItemsTotalValues));
                GetItemsTotalValues();

                PrintSectionHeader(nameof(GetFullitemDetails));
                GetFullitemDetails();

                PrintSectionHeader(nameof(ListInventoryLinq));
                ListInventoryLinq();

                PrintSectionHeader(nameof(ListCategoriesAndColors));
                ListCategoriesAndColors();
            }
        }

        private static void BuildOptions()
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

        private static void PrintSectionHeader(string sectionName)
        {
            Console.WriteLine();
            Console.WriteLine($"--> {sectionName} <--");
            Console.WriteLine(_sectionSeparator);
        }

        private static void ListInventory()
        {
            List<ItemDto> results = _itemsService.GetItems();

            results.OrderBy(x => x.Name)
                .ToList()
                .ForEach(item => Console.WriteLine($"Item: {item.Name}"));
        }

        private static void GetItemsForListing()
        {
            List<GetItemsForListingDto> results = _itemsService.GetItemsForListingFromProcedure();
            StringBuilder output = new StringBuilder();
            foreach (GetItemsForListingDto item in results)
            {
                output.Append($"ITEM {item.Name}");
                if (!string.IsNullOrWhiteSpace(item.CategoryName))
                {
                    output.Append($" ({item.CategoryName})");
                }
                output.Append($" | {item.Description}");
                Console.WriteLine(output.ToString());
                output.Clear();
            }
        }

        private static void GetAllItemsAsPipeDelimitedString()
        {
            Console.WriteLine($"All items: {_itemsService.GetAllItemsPipeDelimitedString()}");
        }

        private static void GetItemsTotalValues()
        {
            foreach (GetItemsTotalValueDto item in _itemsService.GetItemsTotalValue(true))
            {
                Console.WriteLine($"Item -{item.Id,-10}|{item.Name,-50}|{item.Quantity,-4}|{item.TotalValue,-5}");
            }
        }

        private static void GetFullitemDetails()
        {
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                List<FullItemDetailsDto> result = _itemsService.GetItemsWithGenresAndCategories();

                StringBuilder resultView = new StringBuilder();
                foreach (FullItemDetailsDto item in result)
                {
                    resultView.Append($"Item] {item.Id,-10}");
                    resultView.Append($"|{item.ItemName,-50}");
                    resultView.Append($"|{item.ItemDescription,-4}");
                    resultView.Append($"|{item.PlayerName,-5}");
                    resultView.Append($"|{item.Category,-5}");
                    resultView.Append($"|{item.GenreName,-5}");
                    Console.WriteLine(resultView.ToString());
                    resultView.Clear();
                }
            }
        }

        private static void ListInventoryLinq()
        {
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime maxDate = new DateTime(2025, 1, 1);

            List<ItemDto> results = _itemsService.GetItemsByDateRange(minDate,maxDate)
                .OrderBy(item => item.CategoryName)
                .ThenBy(item => item.Name)
                .ToList();

            foreach (ItemDto item in results)
            {
                Console.WriteLine($"ITEM: {item.CategoryName} | {item.Name} - {item.Description}");
            }
        }

        private static void ListCategoriesAndColors()
        {
            foreach (CategoryDto category in _categoriesService.ListCategoriesAndDetails())
            {
                Console.WriteLine($"Category [{category.Category}] is {category.CategoryDetail.Color}");
            }
        }

        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
        private static string _sectionSeparator = "--------------------------------------------------------------";
    }
}
