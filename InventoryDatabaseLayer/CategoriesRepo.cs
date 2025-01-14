using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;
using libDB.Migrations;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Transactions;

namespace InventoryDatabaseLayer
{
    public class CategoriesRepo : ICategoriesRepo
    {
        private readonly IMapper _mapper;
        private readonly InventoryDbContext _context;

        public CategoriesRepo(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteCategory(int id)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Id == id);
            if (category == null)
            {
                return;
            }
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategories(List<int> categoryIds)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (int catId in categoryIds)
                    {
                        await DeleteCategory(catId);
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

        public async Task<List<CategoryDto>> ListCategoriesAndDetails()
        {
            return await _context.Categories.Include(category => category.CategoryDetail)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<int> UpsertCategory(Category category)
        {
            if (category.Id > 0)
            {
                return await UpdateCategory(category);
            }

            return await CreateCategory(category);
        }

        private async Task<int> CreateCategory(Category category)
        {
            category.CreatedDate = DateTime.UtcNow;
            category.CreatedByUserId = Environment.UserName;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            Category? newCategory = await _context.Categories
                .FirstOrDefaultAsync(currentCat => currentCat.Name.ToLower().Equals(category.Name.ToLower()));

            if (newCategory == null)
            {
                throw new Exception("Could not Create the category as expected.");
            }

            return newCategory.Id;
        }

        private async Task<int> UpdateCategory(Category category)
        {
            Category? dbCategory = await _context.Categories
                .Include(category => category.CategoryDetail)
                .FirstOrDefaultAsync(currentCat => currentCat.Id == category.Id);

            if (dbCategory == null)
            {
                throw new Exception("Category not found");
            }

            dbCategory.Name = category.Name;
            if (category.Items != null)
            {
                dbCategory.Items = category.Items;
            }
            if (category.CategoryDetail != null)
            {
                dbCategory.CategoryDetail = category.CategoryDetail;
            }
            dbCategory.IsActive = category.IsActive;
            dbCategory.IsDeleted = category.IsDeleted;
            dbCategory.LastModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return dbCategory.Id;
        }

        public async Task UpsertCategories(List<Category> categories)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (Category category in categories)
                    {
                        bool success = await UpsertCategory(category) > 0;
                        if (!success)
                        {
                            throw new Exception($"ERROR saving the category {category.Name}");
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
    }
}
