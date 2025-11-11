using ePizzaHub.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Contracts
{
    public interface IItemService
    {
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
    }
}
