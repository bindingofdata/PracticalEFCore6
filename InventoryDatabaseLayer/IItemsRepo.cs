using InventoryModels;
using InventoryModels.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDatabaseLayer
{
    public interface IItemsRepo
    {
        Task<List<Item>> GetItems();
        Task<List<ItemDto>> GetItemsByDateRange(DateTime startDate, DateTime endDate);
        Task<List<GetItemsForListingDto>> GetItemListingFromProcedure();
        Task<List<GetItemsTotalValueDto>> GetItemsTotalValues(bool isActive);
        Task<List<FullItemDetailsDto>> GetItemsWithGenresAndCategories();
        Task<int> UpsertItem(Item item);
        Task UpsertItems(List<Item> items);
        Task DeleteItem(int id);
        Task DeleteItems(List<int> itemIds);
    }
}
