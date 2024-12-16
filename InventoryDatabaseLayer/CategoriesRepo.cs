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

        public void DeleteCategory(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(cat => cat.Id == id);
            if (category == null)
            {
                return;
            }
            category.IsDeleted = true;
            _context.SaveChanges();
        }

        public void DeleteCategories(List<int> categoryIds)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (int catId in categoryIds)
                    {
                        DeleteCategory(catId);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // log int:
                    Debug.WriteLine(ex.ToString());
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<CategoryDto> ListCategoriesAndDetails()
        {
            return _context.Categories.Include(category => category.CategoryDetail)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToList();
        }

        public int UpsertCategory(Category category)
        {
            if (category.Id > 0)
            {
                return UpdateCategory(category);
            }

            return CreateCategory(category);
        }

        private int CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            Category? newCategory = _context.Categories.ToList()
                .FirstOrDefault(currentCat => currentCat.Name.ToLower().Equals(category.Name.ToLower()));

            if (newCategory != null)
            {
                throw new Exception("Could not Create the category as expected.");
            }

            return newCategory.Id;
        }

        private int UpdateCategory(Category category)
        {
            Category? dbCategory = _context.Categories
                .Include(category => category.CategoryDetail)
                .FirstOrDefault(currentCat => currentCat.Id == category.Id);

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
            _context.SaveChanges();
            return dbCategory.Id;
        }

        public void UpsertCategories(List<Category> categories)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (Category category in categories)
                    {
                        bool success = UpsertCategory(category) > 0;
                        if (!success)
                        {
                            throw new Exception($"ERROR saving the category {category.Name}");
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
