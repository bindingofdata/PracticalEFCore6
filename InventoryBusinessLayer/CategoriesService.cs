using AutoMapper;

using InventoryDatabaseLayer;

using InventoryModels.Dtos;

using libDB;

using System;
using System.Collections.Generic;
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
        public List<CategoryDto> ListCategoriesAndDetails()
        {
            return _dbRepo.ListCategoriesAndDetails();
        }
    }
}
