using AutoMapper;
using AutoMapper.QueryableExtensions;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class ItemRepository : GenericRepository<ItemDomain, Item>, IItemRepository
    {
        
        public ItemRepository(ePizzaHubDBContext dBContext, IMapper mapper) : base(dBContext,mapper)
        {
            
        }

    }
}
