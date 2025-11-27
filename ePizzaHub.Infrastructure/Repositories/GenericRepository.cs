using AutoMapper;
using AutoMapper.QueryableExtensions;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain>
        where TDomain : class
        where TEntity : class
    {
        protected readonly ePizzaHubDBContext _dBContext;
        protected readonly IMapper _mapper;
        public GenericRepository(ePizzaHubDBContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDomain>> FindAsync(Expression<Func<TDomain, bool>> predicate)
        {
            var response = await _dBContext.Set<TEntity>()
                                     .ProjectTo<TDomain>(_mapper.ConfigurationProvider)
                                     .Where(predicate)
                                     .ToListAsync();
            return response;
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            var response= _dBContext.Set<TEntity>();
            return await response.ProjectTo<TDomain>(_mapper.ConfigurationProvider).ToListAsync();                
        }

        public async Task<TDomain> GetByIdAsync(object id)
        {
            var response = await _dBContext.Set<TEntity>().FindAsync(id);
            return response == null?null : _mapper.Map<TDomain>(response);
        }
        public async Task<int> CommitAsync()
        {
            return await _dBContext.SaveChangesAsync();
        }
        public async Task AddAsync(TDomain domainEntity)
        {
            var entity = _mapper.Map<TEntity>(domainEntity);

            await _dBContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TDomain domainEntity, object id)
        {
            var existingEntity = await _dBContext.Set<TEntity>().FindAsync(id);
            if (existingEntity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            _mapper.Map(domainEntity, existingEntity);
        }
    }
}
