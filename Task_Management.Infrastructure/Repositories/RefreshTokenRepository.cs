using Microsoft.EntityFrameworkCore;
using Task_Management.Application.Interfaces;
using Task_Management.Domain;

namespace Task_Management.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly TaskDbContext _context;

    public RefreshTokenRepository(TaskDbContext context)
    {
        _context = context;
    }
    public RefreshToken? GetByToken(string token)
    
    {
        return _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefault(x => x.Token == token);
    
    }

    public void Add(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();
    }
}