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
        private readonly ICategoriesRepo _dbRepo;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoriesRepo dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        public async Task DeleteCategories(List<int> categoryIds)
        {
            try
            {
                await _dbRepo.DeleteCategories(categoryIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid ID");
            }

            await _dbRepo.DeleteCategory(id);
        }

        public async Task<List<CategoryDto>> ListCategoriesAndDetails()
        {
            return await _dbRepo.ListCategoriesAndDetails();
        }

        public async Task UpsertCategories(List<Category> categories)
        {
            try
            {
                await _dbRepo.UpsertCategories(categories);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public async Task<int> UpsertCategory(Category category)
        {
            return await _dbRepo.UpsertCategory(category);
        }
    }
}
