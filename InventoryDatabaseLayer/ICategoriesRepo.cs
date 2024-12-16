using InventoryModels;
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
        List<CategoryDto> ListCategoriesAndDetails();
        int UpsertCategory(Category category);
        void UpsertCategories(List<Category> categories);
        void DeleteCategory(int id);
        void DeleteCategories(List<int> categoryIds);
    }
}
