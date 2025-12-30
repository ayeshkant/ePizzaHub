namespace ePizzaHub.UI.Utils.Contract
{
    public interface ITokenService
    {
        void SetAccessToken(string token);    
        void SetRefreshToken(string token);
        string GetToken();
        DateTime? GetTokenExpiryTime(string currentToken);
        Task<string> RefreshTokenAsync();
    }
}
