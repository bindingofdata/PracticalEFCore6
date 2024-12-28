﻿using InventoryModels;
using InventoryModels.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDatabaseLayer
{
    public interface ICategoriesRepo
    {
        Task<List<CategoryDto>> ListCategoriesAndDetails();
        Task<int> UpsertCategory(Category category);
        Task UpsertCategories(List<Category> categories);
        Task DeleteCategory(int id);
        Task DeleteCategories(List<int> categoryIds);
    }
}
