using AutoMapper;

using InventoryDatabaseLayer;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBusinessLayer
{
    public class CategoriesService : ICategoriesService
    {
        private readonly CategoriesRepo _dbRepo;

        public CategoriesService(InventoryDbContext context, IMapper mapper)
        {
            _dbRepo = new CategoriesRepo(context, mapper);
        }

        public void DeleteCategories(List<int> categoryIds)
        {
            try
            {
                _dbRepo.DeleteCategories(categoryIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public void DeleteCategory(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid ID");
            }

            _dbRepo.DeleteCategory(id);
        }

        public List<CategoryDto> ListCategoriesAndDetails()
        {
            return _dbRepo.ListCategoriesAndDetails();
        }

        public void UpsertCategories(List<Category> categories)
        {
            try
            {
                _dbRepo.UpsertCategories(categories);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public int UpsertCategory(Category category)
        {
            return _dbRepo.UpsertCategory(category);
        }
    }
}
