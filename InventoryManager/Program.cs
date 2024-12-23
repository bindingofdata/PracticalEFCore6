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
using System.Runtime.CompilerServices;
using System.Security.Authentication;
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
        private static IPlayersService _playersService;
        private static List<CategoryDto> _categories;
        private static List<PlayerDto> _players;

        public static void Main(string[] args)
        {
            BuildOptions();
            BuildMapper();
            using (var db = new InventoryDbContext(_optionsBuilder.Options))
            {
                _itemsService = new ItemsService(db, _mapper);
                _categoriesService = new CategoriesService(db, _mapper);
                _playersService = new PlayersService(db, _mapper);
                _categories = GetCategories();
                _players = GetPlayers();
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
                    Console.WriteLine("5. [E]xit");
                    switch (Console.ReadLine().ToLower())
                    {
                        case "1":
                        case "c":
                            Console.Clear();
                            Console.WriteLine("Adding new Item(s)");
                            CreateMultipleItems();
                            Console.WriteLine("Items added");
                            PrintAllItems(true, false);
                            break;
                        case "3":
                        case "u":
                            Console.Clear();
                            Console.WriteLine("Updating Item(s)");
                            UpdateMultipleItems();
                            Console.WriteLine("Items updated");
                            PrintAllItems(true, false);
                            break;
                        case "4":
                        case "d":
                            Console.Clear();
                            Console.WriteLine("Deleting Item(s)");
                            DeleteMultipleItems();
                            Console.WriteLine("Items deleted");
                            PrintAllItems(false, false);
                            break;
                        default:
                            exit = true;
                            break;
                    }
                    Console.Clear();
                }
            }
        }

        private static void CreateMultipleItems()
        {
            Console.WriteLine("Would you like to create items as a batch? y/n");
            bool batchCreate = GetBoolFromUser();
            List<CreateOrUpdateItemDto> allItems = new List<CreateOrUpdateItemDto>();

            bool createAnother = true;
            while (createAnother)
            {
                CreateOrUpdateItemDto newItem = new CreateOrUpdateItemDto();
                Console.WriteLine("Creating new item.");
                Console.WriteLine("Enter item name.");
                newItem.Name = Console.ReadLine();
                Console.WriteLine("Enter item description.");
                newItem.Description = Console.ReadLine();
                newItem.CategoryId = GetCategoryId();
                Console.WriteLine("Enter purchase price.");
                newItem.PurchasePrice = GetDecimalFromUser();
                Console.WriteLine("Enter current or final price.");
                newItem.CurrentOrFinalPrice = GetDecimalFromUser();
                Console.WriteLine("Enter purchase quantity.");
                newItem.Quantity = GetIntFromUser();
                Console.WriteLine("Enter any notes.");
                newItem.Notes = Console.ReadLine();
                newItem.Players = AddPlayers(batchCreate);

                newItem.IsActive = true;
                newItem.IsDeleted = false;
                newItem.IsOnSale = false;

                if (!batchCreate)
                {
                    _itemsService.UpsertItem(newItem);
                }
                else
                {
                    allItems.Add(newItem);
                }

                Console.WriteLine("Would you like to create another item? y/n");
                createAnother = GetBoolFromUser();

                if (!createAnother && batchCreate)
                {
                    _itemsService.UpsertItems(allItems);
                }
            }
        }

        private static void UpdateMultipleItems()
        {
            Console.WriteLine("Would you like to update items as a batch? y/n");
            bool batchUpdate = GetBoolFromUser();
            List<CreateOrUpdateItemDto> allItems = new List<CreateOrUpdateItemDto>();

            bool updateAnother = true;
            while (updateAnother)
            {
                Console.WriteLine("Enter the ID number to update.");
                Console.WriteLine(_sectionSeparator);
                List<ItemDto> items = PrintAllItems(true, true);
                Console.WriteLine(_sectionSeparator);

                Console.Write("ID to update: ");
                int id = GetIntFromUser();

                ItemDto? itemMatch = items.FirstOrDefault(item => item.Id == id);
                if (itemMatch != null)
                {
                    CreateOrUpdateItemDto updateItem =
                        _mapper.Map<CreateOrUpdateItemDto>(_mapper.Map<Item>(itemMatch));
                    Console.WriteLine("Leave fields blank to keep current values.");
                    // string fields
                    Console.WriteLine("Enter new item name.");
                    string userStringInput = Console.ReadLine();
                    updateItem.Name = !string.IsNullOrWhiteSpace(userStringInput) ? userStringInput : updateItem.Name;
                    Console.WriteLine("Enter new item description.");
                    userStringInput = Console.ReadLine();
                    updateItem.Description = !string.IsNullOrWhiteSpace(userStringInput) ? userStringInput : updateItem.Description;
                    Console.WriteLine("Enter new item notes.");
                    userStringInput = Console.ReadLine();
                    updateItem.Notes = !string.IsNullOrWhiteSpace(userStringInput) ? userStringInput : updateItem.Notes;

                    // numeric fields
                    int userIntInput = GetCategoryId();
                    updateItem.CategoryId = userIntInput >= 0 ? userIntInput : updateItem.CategoryId;
                    Console.WriteLine("Enter new item quantity.");
                    userIntInput = GetIntFromUser();
                    updateItem.Quantity = userIntInput >= 0 ? userIntInput : updateItem.Quantity;
                    Console.WriteLine("Enter new item purchase price.");
                    decimal userDecInput = GetDecimalFromUser();
                    updateItem.PurchasePrice = userDecInput >= 0.0M ? userDecInput : updateItem.PurchasePrice;
                    Console.WriteLine("Enter new item current or final price.");
                    userDecInput = GetDecimalFromUser();
                    updateItem.CurrentOrFinalPrice = userDecInput >= 0.0M ? userDecInput : updateItem.CurrentOrFinalPrice;

                    // boolean fields
                    Console.WriteLine("Is item active? y/n");
                    updateItem.IsActive = GetBoolFromUser();
                    Console.WriteLine("Is item on sale? y/n");
                    updateItem.IsOnSale = GetBoolFromUser();

                    if (!batchUpdate)
                    {
                        _itemsService.UpsertItem(updateItem);
                    }
                    else
                    {
                        allItems.Add(updateItem);
                    }
                }

                Console.WriteLine("Would you like to update another item? y/n");
                updateAnother = GetBoolFromUser();

                if (!updateAnother && batchUpdate)
                {
                    _itemsService.UpsertItems(allItems);
                }
            }
        }

        private static void DeleteMultipleItems()
        {
            Console.WriteLine("Would you like to delete items as a batch? y/n");
            bool batchDelete = GetBoolFromUser();
            List<ItemDto> allItems = new List<ItemDto>();

            bool deleteAnother = true;
            while (deleteAnother)
            {
                Console.WriteLine("Enter the ID number to delete.");
                Console.WriteLine(_sectionSeparator);
                List<ItemDto> items = PrintAllItems(true, true);
                Console.WriteLine(_sectionSeparator);

                Console.Write("ID to delete: ");
                int id = GetIntFromUser();

                ItemDto itemMatch = items.FirstOrDefault(item => item.Id == id);
                if (itemMatch != null)
                {
                    if (batchDelete)
                    {
                        if (!allItems.Contains(itemMatch))
                        {
                            allItems.Add(itemMatch);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Are you sure you want to delete the item {itemMatch.Name} [{itemMatch.Id}]? y/n");
                        if (GetBoolFromUser())
                        {
                            _itemsService.DeleteItem(id);
                            Console.WriteLine("Item deleted");
                        }
                    }
                }

                Console.WriteLine("Would you like to delete another item? y/n");
                deleteAnother = GetBoolFromUser();

                if (!deleteAnother && batchDelete)
                {
                    Console.WriteLine("Are you sure you want to delete the following items? y/n");
                    allItems.ForEach(item => Console.WriteLine($"Item: {itemMatch.Name} [{itemMatch.Id}]"));
                    if (GetBoolFromUser())
                    {
                        _itemsService.DeleteItems(allItems.Select(item => item.Id).ToList());
                        Console.WriteLine("Items deleted.");
                    }
                }
            }
        }

        private static List<Player> AddPlayers(bool batchCreate)
        {
            List<Player> players = new List<Player>();
            List<Player> newPlayers = new List<Player>();
            Console.WriteLine("Add a player? y/n");
            bool createAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);
            while (createAnother)
            {
                Console.WriteLine("Enter player name.");
                string playerName = Console.ReadLine();
                Player newPlayer = _mapper.Map<Player>(_players.FirstOrDefault(player => player.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase)));
                if (newPlayer == null)
                {
                    newPlayer = new Player();
                    newPlayer.Name = playerName;
                    Console.WriteLine("Enter a description.");
                    newPlayer.Description = Console.ReadLine();
                    newPlayer.IsDeleted = false;
                    newPlayer.IsActive = true;
                    newPlayer.CreatedDate = DateTime.Now;

                    if (!batchCreate)
                    {
                        _playersService.UpsertPlayer(newPlayer);
                    }
                    else
                    {
                        newPlayers.Add(newPlayer);
                    }
                }
                players.Add(newPlayer);

                Console.WriteLine("Would you like to add another player? y/n");
                createAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

                if (!createAnother && batchCreate)
                {
                    _playersService.UpsertPlayers(newPlayers);
                }
            }

            return players;
        }

        private static List<ItemDto> PrintAllItems(bool activeOnly, bool sortByName)
        {
            IEnumerable<ItemDto> inventory = _itemsService.GetItems();
            if (activeOnly)
            {
                inventory = inventory.Where(item => !item.IsDeleted);
            }
            if (sortByName)
            {
                inventory = inventory.OrderBy(item => item.Name);
            }

            foreach (ItemDto item in inventory)
            {
                Console.WriteLine($"ID: {item.Id} | {item.Name}");
            }
            Console.WriteLine("Press any key to continue...");
            _ = Console.ReadKey();
            return inventory.ToList();
        }

        private static int GetCategoryId()
        {
            bool validInput = false;
            int categoryId = -1;

            while (!validInput)
            {
                Console.WriteLine("Enter the Category: [B]ooks, [M]ovies, [G]ames");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return -1;
                }

                switch (input.Substring(0, 1).ToUpper())
                {
                    case "B":
                        categoryId = _categories.FirstOrDefault(catDto => catDto.Category.ToLower().Equals("books"))?.Id ?? -1;
                        validInput = true;
                        break;
                    case "M":
                        categoryId = _categories.FirstOrDefault(catDto => catDto.Category.ToLower().Equals("movies"))?.Id ?? -1;
                        validInput = true;
                        break;
                    case "G":
                        categoryId = _categories.FirstOrDefault(catDto => catDto.Category.ToLower().Equals("games"))?.Id ?? -1;
                        validInput = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Enter a valid category");
                        validInput = false;
                        break;
                }
            }

            return categoryId;
        }

        private static decimal GetDecimalFromUser()
        {
            decimal result;
            string userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return -1M;
            }

            while (!decimal.TryParse(userInput, out result))
            {
                Console.WriteLine("Enter a valid price. eg. 3.99");
            }

            return result;
        }

        private static int GetIntFromUser()
        {
            int result;
            string userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return -1;
            }

            while (!int.TryParse(userInput, out result))
            {
                Console.WriteLine("Enter a valid whole number.");
            }

            return result;
        }

        private static bool GetBoolFromUser()
        {
            string userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput) ||
                !userInput.StartsWith("y", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
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

        private static List<CategoryDto> GetCategories()
        {
            return _categoriesService.ListCategoriesAndDetails();
        }

        private static List<PlayerDto> GetPlayers()
        {
            return _playersService.GetPlayers();
        }

        #region Tutorial Methods
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
        #endregion

        private static string _systemId = Environment.MachineName;
        private static string _loggedInUserId = Environment.UserName;
        private static string _sectionSeparator = "--------------------------------------------------------------";
    }
}
