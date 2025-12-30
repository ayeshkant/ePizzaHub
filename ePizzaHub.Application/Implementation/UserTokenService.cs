using AutoMapper;
using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Application.DTOs.Response;
using ePizzaHub.Domain.Entities;
using ePizzaHub.Domain.Interfaces;
using ePizzaHub.Infrastructure.Entities;
using ePizzaHub.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Implementation
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IMapper _mapper;
        private readonly IUserTokenRepository _userTokenRepository;

        public UserTokenService(IUserService userService, IMapper mapper, IUserTokenRepository userTokenRepository)
        {
            _mapper = mapper;
            _userTokenRepository = userTokenRepository;
        }
        public async Task<bool> AddUserTokenAsync(UserTokenRequestDto userToken)
        {
            var userTokenDomain = _mapper.Map<UserTokenDomain>(userToken);
            int rowsInserted = await _userTokenRepository.AddUserTokenAsync(userTokenDomain);
            return rowsInserted > 0; 
        }

        public async Task<UserTokenResponseDto> GetUserTokenAsync(int userId)
        {
            var userTokenDomain=await _userTokenRepository.GetUserTokenAsync(userId);
            return _mapper.Map<UserTokenResponseDto>(userTokenDomain);
        }
    }
}
