using ePizzaHub.Application.Contracts;
using ePizzaHub.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Implementation
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public Task<TokenResponseDto> GenerateToken(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
