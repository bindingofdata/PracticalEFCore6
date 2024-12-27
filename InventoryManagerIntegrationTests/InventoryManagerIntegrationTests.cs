using AutoMapper;

using InventoryDatabaseLayer;

using InventoryDataMigrator;

using libDB;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using Xunit;
using InventoryModels;
using InventoryModels.Dtos;

namespace InventoryManagerIntegrationTests
{
    public class InventoryManagerIntegrationTests
    {
        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;

        DbContextOptions<InventoryDbContext> _options;
        IItemsRepo _dbRepo;

        public InventoryManagerIntegrationTests()
        {
            SetupOptions();
            BuildDefaults();
        }

        private void SetupOptions()
        {
            _options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName:"InventoryManagerTest")
                .Options;

            ServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(typeof(InventoryMapper));
            _serviceProvider = services.BuildServiceProvider();

            _mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<InventoryMapper>();
            });
            _mapperConfig.AssertConfigurationIsValid();
            _mapper = _mapperConfig.CreateMapper();
        }

        private void BuildDefaults()
        {
            using (InventoryDbContext context = new InventoryDbContext(_options))
            {
                if (context.Items.SingleOrDefault(item => item.Name.Equals("Airplane!")) != null)
                {
                    return;
                }

                CategoryBuilder categoryBuilder = new CategoryBuilder(context);
                categoryBuilder.GenerateSeedData();
                ItemBuilder itemBuilder = new ItemBuilder(context);
                itemBuilder.GenerateSeedData();
            }
        }

        [Fact]
        public void TestGetItems()
        {
            // arrange
            using (InventoryDbContext context = new InventoryDbContext(_options))
            {
                // act
                _dbRepo = new ItemsRepo(context, _mapper);
                List<Item> items = _dbRepo.GetItems();

                // assert
                items.ShouldNotBeNull();
                items.Count.ShouldBe(15);
                Item? firstItem = items.FirstOrDefault(item => item.Id == 1);
                firstItem.ShouldNotBeNull();
                firstItem.Name.ShouldBe("Airplane!");
                firstItem.CurrentOrFinalPrice.ShouldBe(9.99m);
                firstItem.Category.ShouldNotBeNull();
                firstItem.Category.Name.ShouldBe(MOVIE_CATEGORY_NAME_STRING);
                firstItem.Category.CategoryDetail.ShouldNotBeNull();
                firstItem.Category.CategoryDetail.ColorName.ShouldBe(MOVIE_CATEGORY_COLOR_NAME);
                firstItem.Category.CategoryDetail.ColorValue.ShouldBe(MOVIE_CATEGORY_COLOR_VALUE);
                firstItem.Description.ShouldBe("Don't call me Shirley.");
                firstItem.IsOnSale.ShouldBeFalse();
                firstItem.Notes.ShouldBe("https://www.imdb.com/title/tt0080339/");
                firstItem.PurchasePrice.ShouldBe(23.99m);
                firstItem.PurchasedDate.ShouldBeNull();
                firstItem.Quantity.ShouldBe(1_000);
                firstItem.SoldDate.ShouldBeNull();
                firstItem.CreatedByUserId.ShouldBe(SEED_USER_ID);
                firstItem.IsDeleted.ShouldBeFalse();
                firstItem.IsActive.ShouldBeTrue();
                firstItem.Players.ShouldNotBeNull();
                firstItem.Players.Count.ShouldBe(2);
                Player firstPlayer = firstItem.Players.First();
                firstPlayer.Name.ShouldBe("Robert Hays");
                firstPlayer.Description.ShouldBe("https://www.imdb.com/name/nm0001332/");
                firstPlayer.CreatedByUserId.ShouldBe(SEED_USER_ID);
                firstPlayer.IsDeleted.ShouldBeFalse();
                firstPlayer.IsActive.ShouldBeTrue();
            }
        }

        [Theory]
        [InlineData(MOVIE_CATEGORY_NAME_STRING, MOVIE_CATEGORY_COLOR_NAME, MOVIE_CATEGORY_COLOR_VALUE)]
        [InlineData(GAME_CATEGORY_NAME_STRING, GAME_CATEGORY_COLOR_NAME, GAME_CATEGORY_COLOR_VALUE)]
        [InlineData(BOOK_CATEGORY_NAME_STRING, BOOK_CATEGORY_COLOR_NAME, BOOK_CATEGORY_COLOR_VALUE)]
        public void TestCategoryColors(string name, string color, string colorValue)
        {
            // arrange
            using (InventoryDbContext context = new InventoryDbContext(_options))
            {
                // act
                ICategoriesRepo categoriesRepo = new CategoriesRepo(context, _mapper);
                List<CategoryDto> categories = categoriesRepo.ListCategoriesAndDetails();

                categories.ShouldNotBeNull();
                categories.Count.ShouldBe(3);

                CategoryDto? category = categories.FirstOrDefault(cat => cat.Category.Equals(name));
                category.ShouldNotBeNull();
                category.CategoryDetail.Color.ShouldBe(color);
                category.CategoryDetail.Value.ShouldBe(colorValue);
            }
        }

        private const string SEED_USER_ID = "873fb5cd-ad6b-458d-ab59-3c5eca45a368";
        private const string MOVIE_CATEGORY_NAME_STRING = "Movies";
        private const string MOVIE_CATEGORY_COLOR_NAME = "Red";
        private const string MOVIE_CATEGORY_COLOR_VALUE = "#FF0000";
        private const string GAME_CATEGORY_NAME_STRING = "Games";
        private const string GAME_CATEGORY_COLOR_NAME = "Green";
        private const string GAME_CATEGORY_COLOR_VALUE = "#00FF00";
        private const string BOOK_CATEGORY_NAME_STRING = "Books";
        private const string BOOK_CATEGORY_COLOR_NAME = "Blue";
        private const string BOOK_CATEGORY_COLOR_VALUE = "#0000FF";
    }
}
