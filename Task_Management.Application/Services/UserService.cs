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
    private readonly IConfiguration _configuration;

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
        _configuration = configuration;
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
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        _refreshTokenRepository.Add(refreshTokenEntity);
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

    }
}