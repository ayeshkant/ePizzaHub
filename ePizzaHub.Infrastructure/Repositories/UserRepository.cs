using AutoMapper;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<UserDomain, User>, IUserRepository
    {
        public UserRepository(ePizzaHubDBContext dBContext, IMapper mapper) : base(dBContext, mapper)
        {
            
        }

        public async Task<int> AddUserAsync(UserDomain userDomain)
        {
            var roles = await _dBContext.Roles.FirstAsync(x => x.Name == userDomain.UserRoles.First());
            
            var user = _mapper.Map<User>(userDomain);
            user.EmailConfirmed = true;
            user.CreatedDate = DateTime.UtcNow;
            user.Roles.Add(roles);

            _dBContext.Users.Add(user);
            return await CommitAsync();
        }
    }
}
