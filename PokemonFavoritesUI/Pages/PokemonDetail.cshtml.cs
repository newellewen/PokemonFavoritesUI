using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokemonFavoritesUI.Database.Models;
using PokemonFavoritesUI.Helpers;

namespace PokemonFavoritesUI.Pages
{
    public class PokemonDetailModel : PageModel
    {
        [FromRoute(Name = "pokemonId")]
        public int PokemonId { get; set; }
        public int UserId { get; set; }
        
        public Pokemon Pokemon { get; set; }
        public bool IsFavorite { get; set; }
        
        public async Task OnGet()
        {
            await SetUserId();
            await SetPokemon();
            await SetIsFavorite();
        }


        public async Task<IActionResult> OnPostAddFavorite()
        {
            
            await SetPokemon();
            await SetUserId();
            var httpClient = GetHttpClient();

            var types = String.Join(", ", Pokemon.Types.Select(t => t.Type.Name));
            var thumbnail = Pokemon.Sprites?.FrontShiny;
            
            StringContent addFavoriteContent = new StringContent(
                $"{{ \"userId\": \"{UserId}\"," +
                $"\"pokemonId\": \"{PokemonId}\"," +
                $"\"name\": \"{Pokemon.Name}\"," +
                $"\"types\": \"{types}\"," +
                $" \"thumbnail\": \"{thumbnail}\"}}",
                UnicodeEncoding.UTF8,
                "application/json");
        
            var addFavoriteResponse = await httpClient.PostAsync(
                $"/Favorites", addFavoriteContent);

            if (addFavoriteResponse.IsSuccessStatusCode)
            {
                return Redirect($"/PokemonDetail/{PokemonId}");
            }

            return Redirect("/Home");
        }
        
        public async Task<IActionResult> OnPostRemoveFavorite()
        {
            await SetPokemon();
            await SetUserId();
            var httpClient = GetHttpClient();
            
            var deleteFavoriteResponse = await httpClient.DeleteAsync(
                $"/Favorites?userId={UserId}&pokemonId={PokemonId}");

            if (deleteFavoriteResponse.IsSuccessStatusCode)
            {
                return Redirect($"/PokemonDetail/{PokemonId}");
            }

            return Redirect("/Home");
        }

        private HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = 
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            
            var httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri("http://localhost:5019/");

            return httpClient;
        }

        private async Task SetPokemon()
        {
            var httpClient = GetHttpClient();
            var getPokemonResponse = await httpClient.GetAsync(
                $"pokemon/{PokemonId}");

            if (getPokemonResponse.IsSuccessStatusCode)
            {
                using var contentStream =
                    await getPokemonResponse.Content.ReadAsStreamAsync();

                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var json = reader.ReadToEnd();
                    Pokemon = JsonSerializer.Deserialize<Pokemon>(json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })!;
                }
            }
        }

        private async Task SetIsFavorite()
        {
            var httpClient = GetHttpClient();
            var getIsFavoriteResponse = await httpClient.GetAsync($"Favorites/IsFavorite?userId={UserId}&pokemonId={PokemonId}");
            if (getIsFavoriteResponse.IsSuccessStatusCode)
            {
                using var isFavoriteContent =
                    await getIsFavoriteResponse.Content.ReadAsStreamAsync();
            
                using (StreamReader reader = new StreamReader(isFavoriteContent))
                {
                    var json = reader.ReadToEnd();
                    IsFavorite = Boolean.Parse(json);
                }
            }
        }

        private async Task SetUserId()
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
