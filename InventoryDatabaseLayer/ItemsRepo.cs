using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryModels.Dtos;

using libDB;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

        public List<GetItemsForListingDto> GetItemListingFromProcedure()
        {
            return _context.ItemsForListing.FromSqlRaw("EXECUTE dbo.GetItemsForListing")
                .ToList();
        }

        public List<ItemDto> GetItems()
        {
            return _context.Items
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
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
                .FromSqlRaw("SELECT * from [dbo].[GetItemsTotalValues] (@IsActive)", isActiveParam)
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
    }
}
