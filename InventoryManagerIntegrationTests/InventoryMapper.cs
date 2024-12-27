using AutoMapper;

using InventoryModels.Dtos;
using InventoryModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagerIntegrationTests
{
    public class InventoryMapper : Profile
    {
        public InventoryMapper()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<Item, ItemDto>()
                .ReverseMap();

            CreateMap<Category, CategoryDto>()
                .ForMember(catDto => catDto.Category, opt => opt.MapFrom(cat => cat.Name))
                .ReverseMap()
                .ForMember(cat => cat.Name, opt => opt.MapFrom(catDto => catDto.Category));

            CreateMap<CategoryDetail, CategoryDetailDto>()
                .ForMember(catDetailDto => catDetailDto.Color, opt => opt.MapFrom(catDetail => catDetail.ColorName))
                .ForMember(catDetailDto => catDetailDto.Value, opt => opt.MapFrom(catDetail => catDetail.ColorValue))
                .ReverseMap()
                .ForMember(catDetail => catDetail.ColorName, opt => opt.MapFrom(catDetailDto => catDetailDto.Color))
                .ForMember(catDetail => catDetail.ColorValue, opt => opt.MapFrom(catDetailDto => catDetailDto.Value));

            CreateMap<Item, CreateOrUpdateItemDto>()
                .ReverseMap()
                .ForMember(item => item.Category, memberOptions => memberOptions.Ignore());

            CreateMap<Player, PlayerDto>()
                .ReverseMap();
        }
    }
}
