using Microsoft.AspNetCore.Mvc;
using Task_Management.Application.Models;
using Task_Management.Application.Services;
namespace Task_Management_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var response = _userService.Login(request);

        if (response is null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }

    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshTokenRequest request)
    {
        var response = _userService.Refresh(request);

        if (response is null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }
}