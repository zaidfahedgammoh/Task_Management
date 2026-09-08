using System.Net.Http.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Task_Management_MVC.Models;

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
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "https://localhost:7011/api/Auth/login",
            request);

        if (!response.IsSuccessStatusCode)
        {
            return Unauthorized();
        }

        var result = await response.Content.ReadAsStringAsync();

        return Content(result, "application/json");
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