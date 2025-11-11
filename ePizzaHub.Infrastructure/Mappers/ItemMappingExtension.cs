using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Infrastructure.Entities;

namespace ePizzaHub.Infrastructure.Mappers
{
    public class ItemMappingExtension : Profile
    {
        public ItemMappingExtension()
        {
            CreateMap<ItemDomain, Item>().ReverseMap();
        }
    }
}
