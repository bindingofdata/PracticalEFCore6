using InventoryModels.Dtos;
using InventoryModels.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBusinessLayer
{
    public interface IItemsService
    {
        Task<List<ItemDto>> GetItems();
        Task<List<ItemDto>> GetItemsByDateRange(DateTime startDate, DateTime endDate);
        Task<List<GetItemsForListingDto>> GetItemsForListingFromProcedure();
        Task<List<GetItemsTotalValueDto>> GetItemsTotalValue(bool isActive);
        Task<string> GetAllItemsPipeDelimitedString();
        Task<List<FullItemDetailsDto>> GetItemsWithGenresAndCategories();
        Task<int> UpsertItem(CreateOrUpdateItemDto item);
        Task UpsertItems(List<CreateOrUpdateItemDto> items);
        Task DeleteItem(int id);
        Task DeleteItems(List<int> ItemIds);
    }
}
