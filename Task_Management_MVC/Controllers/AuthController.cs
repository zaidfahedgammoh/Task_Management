using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Task_Management_MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace Task_Management_MVC.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var client = _httpClientFactory.CreateClient("TaskManagementApi");

        var response = await client.PostAsJsonAsync(
            "api/Auth/login",
            request);

        if (!response.IsSuccessStatusCode)
        {
            return Unauthorized();
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result!.AccessToken);
        var claims = token.Claims.ToList();
        var identity = new ClaimsIdentity(
        claims,
        CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    principal);
        return Ok(result);
        
    }

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(culture)));

        return LocalRedirect(returnUrl);
    }
}