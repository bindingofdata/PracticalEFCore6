using libDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryModels;

namespace InventoryDataMigrator
{
    public class CategoryBuilder
    {
        private readonly InventoryDbContext _context;
        private const string SEED_USER_ID = "873fb5cd-ad6b-458d-ab59-3c5eca45a368";

        public CategoryBuilder(InventoryDbContext context)
        {
            _context = context;
        }

        public void GenerateSeedData()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedByUserId = SEED_USER_ID,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Movies",
                        CategoryDetail = new CategoryDetail() { ColorValue = "#FF0000", ColorName = "Red" }
                    },
                    new Category()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedByUserId = SEED_USER_ID,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Games",
                        CategoryDetail = new CategoryDetail() { ColorValue = "#00FF00", ColorName = "Green" }
                    },
                    new Category()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedByUserId = SEED_USER_ID,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Books",
                        CategoryDetail = new CategoryDetail() { ColorValue = "#0000FF", ColorName = "Blue" }
                    });

                _context.SaveChanges();
            }
        }
    }
}
