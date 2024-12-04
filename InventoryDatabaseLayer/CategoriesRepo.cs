using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryModels.DTOs;

using libDB;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<CategoryDto> ListCategoriesAndDetails()
        {
            return _context.Categories.Include(category => category.CategoryDetail)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
