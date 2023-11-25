using Domain.Modals;

namespace Web_API.Middleware.Auth
{
    public interface IJWTAuthManager
    {
        string GenerateJWT(User user);
    }
}
