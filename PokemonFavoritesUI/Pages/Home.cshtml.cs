using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokemonFavoritesUI.Database.Models;
using PokemonFavoritesUI.Helpers;
using PokemonFavoritesUI.Models;

namespace PokemonFavoritesUI.Pages
{
    public class HomeModel : PageModel
    {
        public IEnumerable<FavoritePokemon> FavoritePokemon { get; set; }
        public int UserId { get; set; }
        
        public async Task OnGet()
        {
            SetUserId();
            
            var httpClient = HttpClientHelper.GetHttpClient();
            var getPokemonResponse = await httpClient.GetAsync(
                $"Favorites?userId={UserId}");

            if (getPokemonResponse.IsSuccessStatusCode)
            {
                using var contentStream =
                    await getPokemonResponse.Content.ReadAsStreamAsync();

                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var json = reader.ReadToEnd();
                    FavoritePokemon = JsonSerializer.Deserialize<IEnumerable<FavoritePokemon>>(json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })!;
                }
            }
        }
        
        private void SetUserId()
        {
            var jwt = Request.Cookies["jwtCookie"];
            var token = JwtHelper.ConvertJwtStringToJwtSecurityToken(jwt);
            var claims = JwtHelper.GetJwtClaims(token);
            UserId = int.Parse(claims
                .First(c => c.Item1 == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                .Item2);
        }
        
    }
}
