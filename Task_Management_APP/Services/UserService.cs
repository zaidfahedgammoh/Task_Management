using Task_Management.Application.Interfaces;
using Task_Management.Application.Models;


namespace Task_Management.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
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

        return new LoginResponse();
        
    }
}