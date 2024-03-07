using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokemonFavoritesUI.Helpers;
using PokemonFavoritesUI.Models;

namespace PokemonFavoritesUI.Pages;

public class Login : PageModel
{
    private readonly IConfiguration configuration;
    public Login(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [BindProperty]
    public string Username { get; set; }
    [BindProperty, DataType(DataType.Password)]
    public string Password { get; set; }
    public string Message { get; set; }
    public bool ShowRegisterModal { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var jwt = Request.Cookies["jwtCookie"];
        if (!String.IsNullOrEmpty(jwt))
        {
            return Redirect("/Home");
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPostLogin()
    {
        var httpClient = HttpClientHelper.GetHttpClient();

        StringContent content = new StringContent(
            $"{{ \"username\": \"{Username}\"," +
            $" \"password\": \"{Password}\"}}",
        UnicodeEncoding.UTF8,
        "application/json");
        
        var httpResponseMessage = await httpClient.PostAsync(
            $"/Users/Login", content);

        using var contentStream =
            await httpResponseMessage.Content.ReadAsStreamAsync();

        using (StreamReader reader = new StreamReader(contentStream))
        {
            var json = reader.ReadToEnd();
            
            if (json == "User not found")
            {
                Message = json;
                return Page();
            }
            
            if (json == "Incorrect password")
            {
                Message = json;
                return Page();
            }
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(3),
            };
            Response.Cookies.Append("jwtCookie", json, cookieOptions);
            
            return Redirect("/Home");
        }
    }
    
    public async Task<IActionResult> OnPostRegister()
    {
        return Redirect("/Register");
    }
    
}

