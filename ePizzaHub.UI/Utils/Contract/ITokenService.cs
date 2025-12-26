namespace ePizzaHub.UI.Utils.Contract
{
    public interface ITokenService
    {
        void SetToken(string token);
        void SetRefreshToken(string refreshToken);
        string GetToken();
    }
}
