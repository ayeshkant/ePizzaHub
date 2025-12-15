using AutoMapper;
using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDomain> GetUserAsync(string userName)
        {
            var user= await _userRepository.FindAsync(x => x.Email.Equals(userName));
            return user.FirstOrDefault();
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto user)
        {
            var userDomain = _mapper.Map<UserDomain>(user);
            userDomain.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            userDomain.UserRoles = new List<string> { RolesEnum.User.ToString() };

            int rowsInserted= await _userRepository.AddUserAsync(userDomain);
            return rowsInserted > 0;
        }
    }
}
