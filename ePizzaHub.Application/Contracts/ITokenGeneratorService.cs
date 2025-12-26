using ePizzaHub.Application.DTOs.Request;
using ePizzaHub.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.Contracts
{
    public interface ITokenGeneratorService
    {
        Task<TokenResponseDto> GenerateToken(string userName, string password);
        Task<TokenResponseDto> GenerateRefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
