using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryBusinessLayer;

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
using System.Runtime.CompilerServices;
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
        private static List<CategoryDto> _categories;

        public static void Main(string[] args)
        {
            BuildOptions();
            BuildMapper();
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                _itemsService = new ItemsService(db, _mapper);
                _categoriesService = new CategoriesService(db, _mapper);
                _categories = GetCategories();
                //PrintSectionHeader(nameof(ListInventory));
                //ListInventory();

                //PrintSectionHeader(nameof(GetItemsForListing));
                //GetItemsForListing();

                //PrintSectionHeader(nameof(GetAllItemsAsPipeDelimitedString));
                //GetAllItemsAsPipeDelimitedString();

                //PrintSectionHeader(nameof(GetItemsTotalValues));
                //GetItemsTotalValues();

                //PrintSectionHeader(nameof(GetFullitemDetails));
                //GetFullitemDetails();

                //PrintSectionHeader(nameof(ListInventoryLinq));
                //ListInventoryLinq();

                //PrintSectionHeader(nameof(ListCategoriesAndColors));
                //ListCategoriesAndColors();
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("1. [C]reate items");
                    Console.WriteLine("2. [R]etrieve items");
                    Console.WriteLine("3. [U]pdate items");
                    Console.WriteLine("4. [D]elete items");
                    switch (Console.ReadLine().ToLower())
                    {
                        case "1":
                        case "c":
                            Console.Clear();
                            Console.WriteLine("Adding new Item(s)");
                            CreateMultipleItems();
                            Console.WriteLine("Items added");
                            List<ItemDto> inventory = _itemsService.GetItems();
                            inventory.ForEach(item => Console.WriteLine($"Item: {item}"));
                            break;
                        default:
                            exit = true;
                            break;
                    }
                }
            }
        }

        private static void CreateMultipleItems()
        {
            Console.WriteLine("Would you like to create items as a batch?");
            bool batchCreate = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);
            List<CreateOrUpdateItemDTO> allItems = new List<CreateOrUpdateItemDTO>();

            bool createAnother = true;
            while (createAnother)
            {
                CreateOrUpdateItemDTO newItem = new CreateOrUpdateItemDTO();
                Console.WriteLine("Creating new item.");
                Console.WriteLine("Enter item name.");
                newItem.Name = Console.ReadLine();
                Console.WriteLine("Enter item description.");
                newItem.Description = Console.ReadLine();
                Console.WriteLine("Enter any notes.");
                newItem.Notes = Console.ReadLine();
                Console.WriteLine("Enter the Category: [B]ooks, [M]ovies, [G]ames");
                newItem.CategoryId = GetCategoryId(Console.ReadLine().Substring(0, 1).ToUpper());

                if (!batchCreate)
                {
                    _itemsService.UpsertItem(newItem);
                }
                else
                {
                    allItems.Add(newItem);
                }

                Console.WriteLine("Would you like to create another item?");
                createAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

                if (batchCreate && !createAnother)
                {
                    _itemsService.UpsertItems(allItems);
                }
            }
        }

        private static int GetCategoryId(string input)
        {
            switch (input)
            {
                case "B":
                    return _categories.FirstOrDefault(catDto => catDto.Category.ToLower().Equals("books"))?.Id ?? -1;
                case "M":
                    return _categories.FirstOrDefault(catDto => catDto.Category.ToLower().Equals("movies"))?.Id ?? -1;
                case "G":
                    return _categories.FirstOrDefault(catDto => catDto.Category.ToLower().Equals("games"))?.Id ?? -1;
                default:
                    return -1;
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
            foreach (CategoryDto category in _categories)
            {
                Console.WriteLine($"Category [{category.Category}] is {category.CategoryDetail.Color}");
            }
        }

        private static List<CategoryDto> GetCategories()
        {
            return _categoriesService.ListCategoriesAndDetails();
        }

        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
        private static string _sectionSeparator = "--------------------------------------------------------------";
    }
}
