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
            CreateMap<Category, CategoryDto>()
                .ForMember(catDto => catDto.Category,opt => opt.MapFrom(cat => cat.Name))
                .ReverseMap()
                .ForMember(cat => cat.Name, opt => opt.MapFrom(catDto => catDto.Category));

            CreateMap<CategoryDetail, CategoryDetailDto>()
                .ForMember(catDetailDto => catDetailDto.Color, opt => opt.MapFrom(catDetail => catDetail.ColorName))
                .ForMember(catDetailDto => catDetailDto.Value, opt => opt.MapFrom(catDetail => catDetail.ColorValue))
                .ReverseMap()
                .ForMember(catDetail => catDetail.ColorName, opt => opt.MapFrom(catDetailDto => catDetailDto.Color))
                .ForMember(catDetail => catDetail.ColorValue, opt => opt.MapFrom(catDetailDto => catDetailDto.Value));
        }
    }
}
