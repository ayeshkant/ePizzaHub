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
        private readonly ePizzaHubDBContext _dBContext;
        private readonly IMapper _mapper;
        public ItemRepository(ePizzaHubDBContext dBContext, IMapper mapper) : base(dBContext,mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public async Task<ItemDomain> GetItemByIdAsync(int id)
        {
            var response = _dBContext.Items.Where(x => x.Id == id);
            return await response.ProjectTo<ItemDomain>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }
    }
}
