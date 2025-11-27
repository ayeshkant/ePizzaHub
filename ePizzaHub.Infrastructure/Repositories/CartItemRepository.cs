using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class CartItemRepository : GenericRepository<CartItemDomain, CartItem>, ICartItemRepository
    {
        public CartItemRepository(
            ePizzaHubDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<CartItemDomain> GetCartItemsAsync(Guid cartId, int itemId)
        {
            var cartItemDomains = await _dBContext
                    .CartItems
                    .FirstOrDefaultAsync(
                            x => x.CartId == cartId && x.ItemId == itemId);

            return _mapper.Map<CartItemDomain>(cartItemDomains);    
        }
    }
}
