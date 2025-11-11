using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class ItemRepository : GenericRepository<ItemDomain, Item>, IItemRepository
    {
        public ItemRepository(ePizzaHubDBContext dBContext, IMapper mapper) : base(dBContext,mapper)
        {
        }
    }
}
