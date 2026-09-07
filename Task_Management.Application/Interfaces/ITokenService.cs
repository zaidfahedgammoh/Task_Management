

using Task_Management.Application.Models;

namespace Task_Management.Application.Interfaces;
public interface ITokenService
{
   
    string GenerateAccessToken(int userId, string email, string role);
    RefreshTokenResult GenerateRefreshToken();
}