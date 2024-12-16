using InventoryModels;
using InventoryModels.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBusinessLayer
{
    public interface ICategoriesService
    {
        List<CategoryDto> ListCategoriesAndDetails();
        int UpsertCategory(Category category);
        void UpsertCategories(List<Category> categories);
        void DeleteCategory(int id);
        void DeleteCategories(List<int> categoryIds);
    }
}
