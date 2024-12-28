using AutoMapper;

using InventoryBusinessLayer;

using InventoryDatabaseLayer;

using InventoryModels;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using System.Net.NetworkInformation;
using InventoryModels.Dtos;
using Shouldly;

namespace InventoryManagerUnitTests
{
    [TestClass]
    public sealed class InventoryManagerUnitTests
    {
        private IItemsService _itemsService;
        private Mock<IItemsRepo> _itemsRepo;
        private ICategoriesService _categoriesService;
        private Mock<ICategoriesRepo> _categoriesRepo;
        private static List<Category> _categories;
        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;
        public TestContext TestContext { get; set; }

        #region Constants/Readonly
        private const string SEED_USER_ID = "873fb5cd-ad6b-458d-ab59-3c5eca45a368";
        private const string TITLE_NEWHOPE = "Star Wars IV: A New Hope";
        private const string TITLE_EMPIRE = "Star Wars V: The Empire Strikes Back";
        private const string TITLE_JEDI = "Star Wars VI: Return of the Jedi";
        private const string DESC_NEWHOPE = "Luke's Friends";
        private const string DESC_EMPIRE = "Luke's Dad";
        private const string DESC_JEDI = "Luke's Sister";
        private static readonly DateTime RELEASE_NEWHOPE = new DateTime(1977, 5, 25);
        private static readonly DateTime RELEASE_EMPIRE = new DateTime(1980, 5, 21);
        private static readonly DateTime RELEASE_JEDI = new DateTime(1983, 5, 25);

        private static readonly DateTime START_DATE = new DateTime(1979, 1, 1);
        private static readonly DateTime END_DATE = new DateTime(1981, 1, 1);

        private const string CAT_NAME_MOVIES = "Movies";
        private const string CAT_NAME_GAMES = "Games";
        private const string CAT_NAME_BOOKS = "Books";
        private static readonly DateTime CAT_ADD_DATE = DateTime.UtcNow;
        #endregion

        [ClassInitialize]
        public static void InitializeTestEnvironment(TestContext testContext)
        {
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

        [TestInitialize]
        public void InitializeTests()
        {
            InstantiateCategoriesRepoMock();
            _categoriesService = new CategoriesService(_categoriesRepo.Object, _mapper);
            InstantiateItemsRepoMock();
            _itemsService = new ItemsService(_itemsRepo.Object, _mapper);
        }

        private void InstantiateCategoriesRepoMock()
        {
            _categoriesRepo = new Mock<ICategoriesRepo>();
            _categories = GetCategoriesTestData();
        }

        private void InstantiateItemsRepoMock()
        {
            _itemsRepo = new Mock<IItemsRepo>();
            List<Item> items = GetItemsTestData();
            _itemsRepo.Setup(mock => mock.GetItems()).Returns(Task.FromResult(items));
            _itemsRepo.Setup(mock => mock.GetItemsByDateRange(
                START_DATE, END_DATE))
                .Returns(Task.FromResult(_mapper.Map<List<ItemDto>>(items.Where(item => item.Name.Equals(TITLE_EMPIRE)))));
        }

        private static List<Item> GetItemsTestData()
        {
            return new List<Item>()
            {
                new Item()
                {
                    Id = 1, Name = TITLE_NEWHOPE,
                    Description = DESC_NEWHOPE, CategoryId = 1,
                    Category = _categories[0],
                    CreatedDate = RELEASE_NEWHOPE,
                },
                new Item()
                {
                    Id = 2, Name = TITLE_EMPIRE,
                    Description = DESC_EMPIRE, CategoryId = 1,
                    Category = _categories[0],
                    CreatedDate = RELEASE_EMPIRE,
                },
                new Item()
                {
                    Id = 3, Name = TITLE_JEDI,
                    Description = DESC_JEDI, CategoryId = 1,
                    Category = _categories[0],
                    CreatedDate = RELEASE_JEDI,
                },
            };
        }

        private static List<Category> GetCategoriesTestData()
        {
            return new List<Category>()
            {
                new Category()
                {
                    CreatedDate = CAT_ADD_DATE,
                    CreatedByUserId = SEED_USER_ID,
                    IsActive = true,
                    IsDeleted = false,
                    Name = CAT_NAME_MOVIES,
                    CategoryDetail = new CategoryDetail() { ColorValue = "#FF0000", ColorName = "Red" }
                },
                new Category()
                {
                    CreatedDate = CAT_ADD_DATE,
                    CreatedByUserId = SEED_USER_ID,
                    IsActive = true,
                    IsDeleted = false,
                    Name = CAT_NAME_GAMES,
                    CategoryDetail = new CategoryDetail() { ColorValue = "#00FF00", ColorName = "Green" }
                },
                new Category()
                {
                    CreatedDate = CAT_ADD_DATE,
                    CreatedByUserId = SEED_USER_ID,
                    IsActive = true,
                    IsDeleted = false,
                    Name = CAT_NAME_BOOKS,
                    CategoryDetail = new CategoryDetail() { ColorValue = "#0000FF", ColorName = "Blue" }
                }
            };
        }

        [TestMethod]
        public async Task TestGetItems()
        {
            List<ItemDto> result = await _itemsService.GetItems();
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);
            List<Item> expected = GetItemsTestData();

            result[0].Name.ShouldBe(expected[0].Name);
            result[0].Description.ShouldBe(expected[0].Description);
            result[0].CreatedDate.ShouldBe(expected[0].CreatedDate);

            result[1].Name.ShouldBe(expected[1].Name);
            result[1].Description.ShouldBe(expected[1].Description);
            result[1].CreatedDate.ShouldBe(expected[1].CreatedDate);

            result[2].Name.ShouldBe(expected[2].Name);
            result[2].Description.ShouldBe(expected[2].Description);
            result[2].CreatedDate.ShouldBe(expected[2].CreatedDate);
        }

        [TestMethod]
        public async Task TestGetItemsByDateRange()
        {
            List<ItemDto> result = await _itemsService.GetItemsByDateRange(START_DATE, END_DATE);
            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);
            ItemDto expected = _mapper.Map<ItemDto>(GetItemsTestData().FirstOrDefault(item => item.Name.Equals(TITLE_EMPIRE)));

            result[0].Name.ShouldBe(expected.Name);
        }
    }
}
