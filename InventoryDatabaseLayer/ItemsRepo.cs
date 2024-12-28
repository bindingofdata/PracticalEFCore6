using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System.Diagnostics;
using System.Threading.Tasks;
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

        public async Task DeleteItem(int id)
        {
            Item? item = await _context.Items.FirstOrDefaultAsync(item => item.Id == id);
            if (item == null)
            {
                return;
            }
            item.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItems(List<int> itemIds)
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
                        await DeleteItem(itemId);
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

        public async Task<List<GetItemsForListingDto>> GetItemListingFromProcedure()
        {
            return await _context.ItemsForListing.FromSqlRaw("EXECUTE dbo.GetItemsForListing")
                .ToListAsync();
        }

        public async Task<List<Item>> GetItems()
        {
            return await _context.Items
                .Include(item => item.Category)
                .Include(item => item.Category.CategoryDetail)
                .Include(item => item.Players)
                .Where(item => !item.IsDeleted)
                .OrderBy(item => item.Name)
                .ToListAsync();
        }

        public async Task<List<ItemDto>> GetItemsByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Items
                .Include(item => item.Category)
                .Where(item => item.CreatedDate >=  startDate && item.CreatedDate <= endDate)
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<GetItemsTotalValueDto>> GetItemsTotalValues(bool isActive)
        {
            SqlParameter isActiveParam = new SqlParameter("IsActive", 1);
            return await _context.ItemsTotalValues
                .FromSqlRaw("SELECT * from [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParam)
                .ToListAsync();
        }

        public async Task<List<FullItemDetailsDto>> GetItemsWithGenresAndCategories()
        {
            return await _context.FullItemDetails
                .FromSqlRaw("SELECT * from [dbo].[vwFullItemDetails]")
                .OrderBy(item => item.ItemName)
                .ThenBy(item => item.GenreName)
                .ThenBy(item => item.Category)
                .ToListAsync();
        }

        public async Task<int> UpsertItem(Item item)
        {
            if (item.Id > 0)
            {
                return await UpdateItem(item);
            }

            return await CreateItem(item);
        }

        public async Task UpsertItems(List<Item> items)
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
                        bool success = await UpsertItem(item) > 0;
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

        private async Task<int> CreateItem(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            Item? newItem = await _context.Items.FirstOrDefaultAsync(
                currentItem => currentItem.Name.ToLower().Equals(item.Name.ToLower()));

            if (newItem == null)
            {
                throw new Exception("Could not Create the item as expected.");
            }

            return newItem.Id;
        }

        private async Task<int> UpdateItem(Item item)
        {
            Item? dbItem = await _context.Items
                .Include(item => item.Category)
                .Include(item => item.ItemGenres)
                .Include(item => item.Players)
                .FirstOrDefaultAsync(currentItem => currentItem.Id == item.Id);

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
            await _context.SaveChangesAsync();
            return item.Id;
        }
    }
}
