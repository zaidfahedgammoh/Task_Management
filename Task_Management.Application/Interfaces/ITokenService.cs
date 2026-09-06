

namespace Task_Management.Application.Interfaces;
public interface ITokenService
{
   
    string GenerateAccessToken(int userId, string email, string role);
    string GenerateRefreshToken();
}