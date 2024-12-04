using InventoryModels.Dtos;
using InventoryDatabaseLayer;
using Microsoft.EntityFrameworkCore.Query.Internal;
using libDB;
using AutoMapper;

namespace InventoryBusinessLayer
{
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepo _dbRepo;

        public ItemsService(InventoryDbContext context, IMapper mapper)
        {
            _dbRepo = new ItemsRepo(context, mapper);
        }

        public string GetAllItemsPipeDelimitedString()
        {
            return string.Join('|', GetItems());
        }

        public List<ItemDto> GetItems()
        {
            return _dbRepo.GetItems();
        }

        public List<ItemDto> GetItemsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _dbRepo.GetItemsByDateRange(startDate, endDate);
        }

        public List<GetItemsForListingDto> GetItemsForListingFromProcedure()
        {
            return _dbRepo.GetItemListingFromProcedure();
        }

        public List<GetItemsTotalValueDto> GetItemsTotalValue(bool isActive)
        {
            return _dbRepo.GetItemsTotalValues(isActive);
        }

        public List<FullItemDetailsDto> GetItemsWithGenresAndCategories()
        {
            return _dbRepo.GetItemsWithGenresAndCategories();
        }
    }
}
