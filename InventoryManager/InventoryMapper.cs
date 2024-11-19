using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using InventoryModels;
using InventoryModels.DTOs;

namespace InventoryManager
{
    public class InventoryMapper : Profile
    {
        public InventoryMapper()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
