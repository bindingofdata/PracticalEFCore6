using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System.Diagnostics;
using System.Transactions;

namespace InventoryDatabaseLayer
{
    public class ItemsRepo : IItemsRepo
    {
        private readonly IMapper _mapper;
        private readonly InventoryDbContext _context;

        public ItemsRepo(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void DeleteItem(int id)
        {
            Item? item = _context.Items.FirstOrDefault(item => item.Id == id);
            if (item == null)
            {
                return;
            }
            item.IsDeleted = true;
            _context.SaveChanges();
        }

        public void DeleteItems(List<int> itemIds)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                }))
            {
                try
                {
                    foreach (int itemId in itemIds)
                    {
                        DeleteItem(itemId);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        public List<GetItemsForListingDto> GetItemListingFromProcedure()
        {
            return _context.ItemsForListing.FromSqlRaw("EXECUTE dbo.GetItemsForListing")
                .ToList();
        }

        public List<Item> GetItems()
        {
            return _context.Items
                .Include(item => item.Category)
                .AsEnumerable()
                .Where(item => !item.IsDeleted)
                .OrderBy(item => item.Name)
                .ToList();
        }

        public List<ItemDto> GetItemsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Items
                .Include(item => item.Category)
                .Where(item => item.CreatedDate >=  startDate && item.CreatedDate <= endDate)
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .ToList();
        }

        public List<GetItemsTotalValueDto> GetItemsTotalValues(bool isActive)
        {
            SqlParameter isActiveParam = new SqlParameter("IsActive", 1);
            return _context.ItemsTotalValues
                .FromSqlRaw("SELECT * from [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParam)
                .ToList();
        }

        public List<FullItemDetailsDto> GetItemsWithGenresAndCategories()
        {
            return _context.FullItemDetails
                .FromSqlRaw("SELECT * from [dbo].[vwFullItemDetails]")
                .AsEnumerable()
                .OrderBy(item => item.ItemName)
                .ThenBy(item => item.GenreName)
                .ThenBy(item => item.Category)
                .ToList();
        }

        public int UpsertItem(Item item)
        {
            if (item.Id > 0)
            {
                return UpdateItem(item);
            }

            return CreateItem(item);
        }

        public void UpsertItems(List<Item> items)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                }))
            {
                try
                {
                    foreach (Item item in items)
                    {
                        bool success = UpsertItem(item) > 0;
                        if (!success)
                        {
                            throw new Exception($"ERROR saving the item {item.Name}");
                        }
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        private int CreateItem(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            Item? newItem = _context.Items.ToList()
                .FirstOrDefault(currentItem => currentItem.Name.ToLower().Equals(item.Name.ToLower()));
            
            if (newItem == null)
            {
                throw new Exception("Could not Create the item as expected.");
            }

            return newItem.Id;
        }

        private int UpdateItem(Item item)
        {
            Item? dbItem = _context.Items
                .Include(item => item.Category)
                .Include(item => item.ItemGenres)
                .Include(item => item.Players)
                .FirstOrDefault(currentItem => currentItem.Id == item.Id);

            if (dbItem == null)
            {
                throw new Exception("Item not found");
            }

            dbItem.CategoryId = item.CategoryId;
            dbItem.CurrentOrFinalPrice = item.CurrentOrFinalPrice;
            dbItem.Description = item.Description;
            dbItem.IsActive = item.IsActive;
            dbItem.IsDeleted = item.IsDeleted;
            dbItem.IsOnSale = item.IsOnSale;
            if (item.ItemGenres != null)
            {
                dbItem.ItemGenres = item.ItemGenres;
            }
            dbItem.Name = item.Name;
            dbItem.Notes = item.Notes;
            if (item.Players != null)
            {
                dbItem.Players = item.Players;
            }
            dbItem.PurchasedDate = item.PurchasedDate;
            dbItem.PurchasePrice = item.PurchasePrice;
            dbItem.Quantity = item.Quantity;
            dbItem.SoldDate = item.SoldDate;
            _context.SaveChanges();
            return item.Id;
        }
    }
}
