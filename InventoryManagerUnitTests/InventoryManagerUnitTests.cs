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
        private static MapperConfiguration _mapperConfig;
        private static IMapper _mapper;
        private static IServiceProvider _serviceProvider;
        public TestContext TestContext { get; set; }

        #region Constants
        private const string TITLE_NEWHOPE = "Star Wars IV: A New Hope";
        private const string TITLE_EMPIRE = "Star Wars V: The Empire Strikes Back";
        private const string TITLE_JEDI = "Star Wars VI: Return of the Jedi";
        private const string DESC_NEWHOPE = "Luke's Friends";
        private const string DESC_EMPIRE = "Luke's Dad";
        private const string DESC_JEDI = "Luke's Sister";
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
            InstantiateItemsRepoMock();
            _itemsService = new ItemsService(_itemsRepo.Object, _mapper);
        }

        private void InstantiateItemsRepoMock()
        {
            _itemsRepo = new Mock<IItemsRepo>();
            List<Item> items = GetItemsTestData();
            _itemsRepo.Setup(mock => mock.GetItems()).Returns(items);
        }

        private static List<Item> GetItemsTestData()
        {
            return new List<Item>()
            {
                new Item()
                {
                    Id = 1, Name = TITLE_NEWHOPE,
                    Description = DESC_NEWHOPE, CategoryId = 2,
                },
                new Item()
                {
                    Id = 2, Name = TITLE_EMPIRE,
                    Description = DESC_EMPIRE, CategoryId = 2,
                },
                new Item()
                {
                    Id = 3, Name = TITLE_JEDI,
                    Description = DESC_JEDI, CategoryId = 2,
                },
            };
        }

        [TestMethod]
        public void TestGetItems()
        {
            List<ItemDto> result = _itemsService.GetItems();
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);
            List<Item> expected = GetItemsTestData();

            result[0].Name.ShouldBe(expected[0].Name);
            result[0].Description.ShouldBe(expected[0].Description);

            result[1].Name.ShouldBe(expected[1].Name);
            result[1].Description.ShouldBe(expected[1].Description);

            result[2].Name.ShouldBe(expected[2].Name);
            result[2].Description.ShouldBe(expected[2].Description);
        }
    }
}
