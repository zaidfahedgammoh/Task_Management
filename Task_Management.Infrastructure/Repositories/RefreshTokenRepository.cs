using Task_Management.Application.Interfaces;
using Task_Management.Domain;

namespace Task_Management_Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly TaskDbContext _context;

    public RefreshTokenRepository(TaskDbContext context)
    {
        _context = context;
    }

    public void Add(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();
    }
}