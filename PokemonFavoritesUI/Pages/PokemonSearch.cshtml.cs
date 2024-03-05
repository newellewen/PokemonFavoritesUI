using System.Text.Json;
using JW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokemonFavoritesUI.Models;

namespace PokemonFavoritesUI.Pages
{
    public class PokemonSearchModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        [FromQuery(Name = "limit")]
        public int Limit { get; set; }
        [FromQuery(Name = "offset")]
        public int Offset { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public PokemonSearchResponse Response { get; set; }
        public SelectList LimitList = new SelectList(new []{ 1, 5, 10, 20, 50});

        public PokemonSearchModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task OnGet()
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
            
            var httpResponseMessage = await httpClient.GetAsync(
                $"pokemon?limit={Limit}&offset={Offset}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                using (StreamReader reader = new StreamReader(contentStream))
                {
                    var json = reader.ReadToEnd();
                    Response = JsonSerializer.Deserialize<PokemonSearchResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    TotalPages = Response.Count / Limit;
                    CurrentPage = Offset / Limit;
                    Console.WriteLine(Response.Next);
                }
            }
        }
    }
}
