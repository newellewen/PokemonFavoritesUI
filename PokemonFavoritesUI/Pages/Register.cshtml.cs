using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokemonFavoritesUI.Models;

namespace PokemonFavoritesUI.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string Message { get; set; }
        
        public async Task<IActionResult> OnPostRegister()
        {
            // TODO - figure out named HttpClient with handler/ignore SSL
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = 
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            
            var httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri("https://localhost:7096/");

            StringContent registerContent = new StringContent(
                $"{{ \"firstName\": \"{FirstName}\"," +
                $"\"lastName\": \"{LastName}\"," +
                $"\"email\": \"{Email}\"," +
                $"\"username\": \"{Username}\"," +
                $" \"password\": \"{Password}\"}}",
                UnicodeEncoding.UTF8,
                "application/json");
        
            var registerResponse = await httpClient.PostAsync(
                $"/Users/Register", registerContent);

            if (registerResponse.IsSuccessStatusCode)
            {
                StringContent loginContent = new StringContent(
                    $"{{ \"username\": \"{Username}\"," +
                    $" \"password\": \"{Password}\"}}",
                    UnicodeEncoding.UTF8,
                    "application/json");
        
                var loginResponse = await httpClient.PostAsync(
                    $"/Users/Login", loginContent);

                using var contentStream =
                    await loginResponse.Content.ReadAsStreamAsync();
                
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var json = reader.ReadToEnd();
                    
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddHours(3),
                    };
                    Response.Cookies.Append("jwtCookie", json, cookieOptions);
                    return Redirect("/Login");
                }
            }
            
            using var registerResponseContent =
                await registerResponse.Content.ReadAsStreamAsync();
            
            using (StreamReader reader = new StreamReader(registerResponseContent))
            {
                Message = reader.ReadToEnd();
                return Page();
            }
        }
    }
}
