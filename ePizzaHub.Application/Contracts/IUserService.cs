using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Contracts
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto user);
        Task<UserDomain> GetUserAsync(string userName);
    }
}
