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
        List<ItemDto> GetItems();
        List<ItemDto> GetItemsByDateRange(DateTime startDate, DateTime endDate);
        List<GetItemsForListingDto> GetItemListingFromProcedure();
        List<GetItemsTotalValueDto> GetItemsTotalValues(bool isActive);
        List<FullItemDetailsDto> GetItemsWithGenresAndCategories();
    }
}
