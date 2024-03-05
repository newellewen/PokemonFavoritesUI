using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokemonFavoritesUI.Database.Models;

namespace PokemonFavoritesUI.Pages
{
    public class PokemonDetailModel : PageModel
    {
        [FromRoute(Name = "pokemonId")]
        public int PokemonId { get; set; }
        
        public Pokemon Pokemon { get; set; }
        
        public async Task OnGet()
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
            httpClient.BaseAddress = new Uri("http://localhost:5019/");
            
            var httpResponseMessage = await httpClient.GetAsync(
                $"pokemon/{PokemonId}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
            
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
    }
}
