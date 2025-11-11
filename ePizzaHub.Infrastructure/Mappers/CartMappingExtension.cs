using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Infrastructure.Entities;

namespace ePizzaHub.Infrastructure.Mappers
{
    public class CartMappingExtension : Profile
    {
        public CartMappingExtension()
        {
            CreateMap<CartDomain, Cart>().ReverseMap();
        }
    }
}
