using AutoMapper;

using InventoryBusinessLayer;

using InventoryDatabaseLayer;

using InventoryModels;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using System.Net.NetworkInformation;

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
            //_itemsService = new ItemsService();
            _itemsRepo = new Mock<IItemsRepo>();
            List<Item> items = new List<Item>()
            {
                new Item()
                {
                    Id = 1, Name = "Star Wars IV: A New Hope",
                    Description = "Luke's Friends", CategoryId = 2,
                },
                new Item()
                {
                    Id = 2, Name = "Star Wars V: The Empire Strikes Back",
                    Description = "Luke's Dad", CategoryId = 2,
                },
                new Item()
                {
                    Id = 3, Name = "Star Wars VI: Return of the Jedi",
                    Description = "Luke's Sister", CategoryId = 2,
                },
            };
            _itemsRepo.Setup(mock => mock.GetItems()).Returns(items);
        }

        [TestMethod]
        public void TestGetItems()
        {
            var result = _itemsService.GetItems();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
    }
}
