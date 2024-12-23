using InventoryModels.Dtos;
using InventoryDatabaseLayer;
using Microsoft.EntityFrameworkCore.Query.Internal;
using libDB;
using AutoMapper;
using InventoryModels.Dtos;
using InventoryModels;
using System.Diagnostics;

namespace InventoryBusinessLayer
{
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepo _dbRepo;
        private readonly IMapper _mapper;

        public ItemsService(IItemsRepo dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        public void DeleteItem(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid id before deleting");
            }
            _dbRepo.DeleteItem(id);
        }

        public void DeleteItems(List<int> ItemIds)
        {
            try
            {
                _dbRepo.DeleteItems(ItemIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public string GetAllItemsPipeDelimitedString()
        {
            return string.Join('|', GetItems());
        }

        public List<ItemDto> GetItems()
        {
            return _mapper.Map<List<ItemDto>>(_dbRepo.GetItems());
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

        public int UpsertItem(CreateOrUpdateItemDto item)
        {
            if (item.CategoryId <= 0)
            {
                throw new ArgumentException("Please set the category ID before insert or update");
            }
            return _dbRepo.UpsertItem(_mapper.Map<Item>(item));
        }

        public void UpsertItems(List<CreateOrUpdateItemDto> items)
        {
            try
            {
                _dbRepo.UpsertItems(_mapper.Map<List<Item>>(items));
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }
    }
}
