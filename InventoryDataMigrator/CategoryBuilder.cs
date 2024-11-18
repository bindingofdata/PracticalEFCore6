using libDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryModels;

namespace InventoryDataMigrator
{
    public sealed class CategoryBuilder : BaseBuilder
    {
        public CategoryBuilder(InventoryDbContext context) : base(context) { }

        public override void GenerateSeedData()
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
