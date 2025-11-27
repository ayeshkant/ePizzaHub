using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using ePizzaHub.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<CartDomain, Cart>, ICartRepository
    {
        public CartRepository(
            ePizzaHubDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<bool> DeleteItemFromCartAsync(Guid cartId, int itemId)
        {
            var cartItems = await _dBContext.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.ItemId == itemId);

            if (cartItems != null)
            {
                _dBContext.CartItems.Remove(cartItems);

                return await CommitAsync() > 0;
            }

            return false;
        }

        public async Task<CartDomain> GetCartDetailAsync(Guid cartId)
        {
            var result = await _dBContext.Carts
                .Where(x => x.Id == cartId && x.IsActive)
                .Include(x => x.CartItems)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            return result.ToDomain();
        }

        public async Task<int> GetCartItemsCountAsync(Guid cartId)
        {
            var itemCount = await _dBContext.CartItems.Where(x => x.CartId == cartId).CountAsync();
            return itemCount;
        }

        public async Task<int> UpdateCartUserAsync(Guid cartId, int userId)
        {
            var cart =
                await _dBContext.Carts.FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart is not null)
                cart.UserId = userId;

            return await _dBContext.SaveChangesAsync();
        }

        public async Task<int> UpdateItemQuantity(Guid cartId, int itemId, int quantity)
        {
            var currentItems = await _dBContext
                                            .CartItems
                                                .Where(x => x.CartId == cartId
                                                       && x.ItemId == itemId)
                                                .FirstOrDefaultAsync();

            currentItems.Quantity = quantity;
            _dBContext.Entry(currentItems).State = EntityState.Modified;
            return await CommitAsync();
        }
    }
}
