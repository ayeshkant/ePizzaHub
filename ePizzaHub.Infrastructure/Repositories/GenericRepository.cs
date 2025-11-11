using AutoMapper;
using AutoMapper.QueryableExtensions;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain>
        where TDomain : class
        where TEntity : class
    {
        private readonly ePizzaHubDBContext _dBContext;
        private readonly IMapper _mapper;
        public GenericRepository(ePizzaHubDBContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            var response= _dBContext.Set<TEntity>();
            return await response.ProjectTo<TDomain>(_mapper.ConfigurationProvider).ToListAsync();                
        }
    }
}
