using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class UserTokenRepository : GenericRepository<UserTokenDomain, UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(ePizzaHubDBContext dBContext, IMapper mapper) : base(dBContext, mapper)
        {
        }
        public async Task<UserTokenDomain> GetUserTokenAsync(int userId)
        {
            var userToken = await _dBContext.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId);
            return _mapper.Map<UserTokenDomain>(userToken);
        }
        public async Task<int> AddUserTokenAsync(UserTokenDomain userTokenDomain)
        {
            var tokenDetails = await _dBContext.UserTokens.Where(x => x.UserId == userTokenDomain.UserId).ToListAsync();
            if (tokenDetails.Any())
            {
                _dBContext.UserTokens.RemoveRange(tokenDetails);
            }

            await _dBContext.AddAsync(_mapper.Map<UserToken>(tokenDetails));

            return await _dBContext.SaveChangesAsync();
        }
    }
}
