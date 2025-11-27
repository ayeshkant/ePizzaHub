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
            CreateMap<CartItemDomain, CartItem>().ReverseMap();
        }
    }

    public static class CartMappingExtensionMethods
    {
        public static CartDomain ToDomain(this Cart cart)
        {
            CartDomain cartDomain = new()
            {
                UserId = cart.UserId,
                CreatedDate = cart.CreatedDate,
                IsActive = cart.IsActive,
                Id = cart.Id,
                ItemDomains = cart.CartItems
                    .Select(x => new CartItemDomain()
                    {
                        CartId = x.CartId,
                        ImageUrl = x.Item.ImageUrl,
                        ItemName = x.Item.Name,
                        Quantity = x.Quantity,
                        ItemId = x.ItemId,
                        UnitPrice = x.UnitPrice,
                        Id = x.Id
                    })
                    .ToList()
            };

            return cartDomain;
        }
    }
}
