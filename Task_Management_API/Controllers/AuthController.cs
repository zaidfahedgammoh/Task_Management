using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Management.Application.Models;
using Task_Management.Application.Services;
namespace Task_Management_API.Controllers;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
    [Authorize(Roles = "Manager")]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            message = "You are authenticated.",
            user = User.FindFirstValue(ClaimTypes.Email),
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            role = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}
