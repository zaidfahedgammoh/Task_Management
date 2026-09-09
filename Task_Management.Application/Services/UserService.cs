using Microsoft.Extensions.Configuration;
using Task_Management.Application.Interfaces;
using Task_Management.Application.Models;
using Task_Management.Domain;


namespace Task_Management.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    

    public UserService(
      
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IRefreshTokenRepository refreshTokenRepository,
    IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
       
    }
   
    
    public LoginResponse? Login(LoginRequest request)
    {
        var user = _userRepository.GetByEmail(request.Email);

        if (user is null)
        {
            return null;
        }

        var validPassword = _passwordHasher.Verify(
            request.Password,
            user.password);

        if (!validPassword)
        {
            return null;
        }
        var accessToken = _tokenService.GenerateAccessToken(
    user.Id,
    user.email,
    user.role.ToString());
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken.Token,
            ExpiresAt = refreshToken.ExpiresAt
        };
        _refreshTokenRepository.Add(refreshTokenEntity);
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };

    }
    public RefreshTokenResponse? Refresh(RefreshTokenRequest request)
    {
        var refreshToken = _refreshTokenRepository.GetByToken(request.RefreshToken);

        if (refreshToken is null)
        {
            return null;
        }

        if (refreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        if (refreshToken.RevokedAt is not null)
        {
            return null;
        }

        refreshToken.RevokedAt = DateTime.UtcNow;
        _refreshTokenRepository.Update(refreshToken);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = refreshToken.UserId,
            Token = newRefreshToken.Token,
            ExpiresAt = newRefreshToken.ExpiresAt
        };
        _refreshTokenRepository.Add(newRefreshTokenEntity);

        var accessToken = _tokenService.GenerateAccessToken(
            refreshToken.UserId,
            refreshToken.User.email,
            refreshToken.User.role.ToString());

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}
