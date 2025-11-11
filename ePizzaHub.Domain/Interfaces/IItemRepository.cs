using ePizzaHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Domain.Interfaces
{
    public interface IItemRepository: IGenericRepository<ItemDomain>
    {
        Task<ItemDomain> GetItemByIdAsync(int id);
    }
}
