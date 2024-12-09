using InventoryModels.Dtos;
using InventoryModels.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBusinessLayer
{
    public interface IItemsService
    {
        List<ItemDto> GetItems();
        List<ItemDto> GetItemsByDateRange(DateTime startDate, DateTime endDate);
        List<GetItemsForListingDto> GetItemsForListingFromProcedure();
        List<GetItemsTotalValueDto> GetItemsTotalValue(bool isActive);
        string GetAllItemsPipeDelimitedString();
        List<FullItemDetailsDto> GetItemsWithGenresAndCategories();
        int UpsertItem(CreateOrUpdateItemDTO item);
        void UpsertItems(List<CreateOrUpdateItemDTO> items);
        void DeleteItem(int id);
        void DeleteItems(List<int> ItemIds);
    }
}
