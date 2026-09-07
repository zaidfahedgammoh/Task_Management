using Task_Management.Application.Interfaces;
using Task_Management.Application.Models;


namespace Task_Management.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public UserService(
      
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
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
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

    }
}