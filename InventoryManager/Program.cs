using AutoMapper;
using AutoMapper.QueryableExtensions;

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
            PrintSectionHeader(nameof(ListInventory));
            ListInventory();

            PrintSectionHeader(nameof(ListInventoryLinq));
            ListInventoryLinq();

            PrintSectionHeader(nameof(ListInventoryWithProjetion));
            ListInventoryWithProjetion();

            PrintSectionHeader(nameof(GetFullitemDetails));
            GetFullitemDetails();

            PrintSectionHeader(nameof(ListCategoriesAndColors));
            ListCategoriesAndColors();
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
        private static void PrintSectionHeader(string sectionName)
        {
            Console.WriteLine();
            Console.WriteLine($"--> {sectionName} <--");
            Console.WriteLine(_sectionSeparator);
        }

        private static void ListInventory()
        {
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                List<Item> items = db.Items.OrderBy(x => x.Name).ToList();
                List<ItemDto> results = _mapper.Map<List<Item>, List<ItemDto>>(items);
                results.ForEach(item => Console.WriteLine($"Item: {item.Name}"));
            }
        }

        private static void ListInventoryLinq()
        {
            DateTime minDate = new DateTime(2021, 1, 1);
            DateTime maxDate = new DateTime(2025, 1, 1);
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                List<ItemDto> results =
                    db.Items.Select(item => new ItemDto
                    {
                        CreatedDate = item.CreatedDate,
                        CategoryName = item.Category.Name,
                        Description = item.Description,
                        IsActive = item.IsActive,
                        IsDeleted = item.IsDeleted,
                        Name = item.Name,
                        Notes = item.Notes,
                        CategoryId = item.Category.Id,
                        Id = item.Id,
                    })
                .Where(item => item.CreatedDate >= minDate && item.CreatedDate <= maxDate)
                .OrderBy(item => item.CategoryName)
                .ThenBy(item => item.Name)
                .ToList();

                foreach (var item in results)
                {
                    Console.WriteLine($"ITEM: {item.CategoryName} | {item.Name} - {item.Description}");
                }
            }
        }

        private static void ListInventoryWithProjetion()
        {
            using (InventoryDbContext db = new InventoryDbContext( _optionsBuilder.Options))
            {
                List<ItemDto> items = db.Items
                    .OrderBy(item => item.Name)
                    .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                    .ToList();

                items.ForEach(item => Console.WriteLine($"New item: {item}"));
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

        private static void ListCategoriesAndColors()
        {
            using (InventoryDbContext db = new InventoryDbContext(_optionsBuilder.Options))
            {
                List<CategoryDto> results = db.Categories
                    .Include(item => item.CategoryDetail)
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                    .ToList();

                foreach (CategoryDto category in results)
                {
                    Console.WriteLine($"Category [{category.Category}] is {category.CategoryDetail.Color}");
                }
            }
        }
#endif

        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
        private static string _sectionSeparator = "--------------------------------------------------------------";
    }
}
