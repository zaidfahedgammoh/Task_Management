
using Task_Management.Domain;

namespace Task_Management.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void Add(RefreshToken refreshToken);
        RefreshToken? GetByToken(string token);
    }
}
