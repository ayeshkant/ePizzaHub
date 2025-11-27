using ePizzaHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Domain.Interfaces
{
    public interface ICartItemRepository : IGenericRepository<CartItemDomain>
    {
        Task<CartItemDomain> GetCartItemsAsync(Guid cartId, int itemId);
    }
}
