using InventoryModels.Dtos;
using InventoryDatabaseLayer;
using Microsoft.EntityFrameworkCore.Query.Internal;
using libDB;
using AutoMapper;
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

        public async Task DeleteItem(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid id before deleting");
            }
            await _dbRepo.DeleteItem(id);
        }

        public async Task DeleteItems(List<int> ItemIds)
        {
            try
            {
                await _dbRepo.DeleteItems(ItemIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetAllItemsPipeDelimitedString()
        {
            List<ItemDto> items = await GetItems();
            return string.Join('|', items);
        }

        public async Task<List<ItemDto>> GetItems()
        {
            return _mapper.Map<List<ItemDto>>(await _dbRepo.GetItems());
        }

        public async Task<List<ItemDto>> GetItemsByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _dbRepo.GetItemsByDateRange(startDate, endDate);
        }

        public async Task<List<GetItemsForListingDto>> GetItemsForListingFromProcedure()
        {
            return await _dbRepo.GetItemListingFromProcedure();
        }

        public async Task<List<GetItemsTotalValueDto>> GetItemsTotalValue(bool isActive)
        {
            return await _dbRepo.GetItemsTotalValues(isActive);
        }

        public async Task<List<FullItemDetailsDto>> GetItemsWithGenresAndCategories()
        {
            return await _dbRepo.GetItemsWithGenresAndCategories();
        }

        public async Task<int> UpsertItem(CreateOrUpdateItemDto item)
        {
            if (item.CategoryId <= 0)
            {
                throw new ArgumentException("Please set the category ID before insert or update");
            }
            return await _dbRepo.UpsertItem(_mapper.Map<Item>(item));
        }

        public async Task UpsertItems(List<CreateOrUpdateItemDto> items)
        {
            try
            {
                await _dbRepo.UpsertItems(_mapper.Map<List<Item>>(items));
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
