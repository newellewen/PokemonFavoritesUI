using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PokemonFavoritesUI.Pages
{
    public class PokemonSearchModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        [FromQuery(Name = "limit")]
        public int Limit { get; set; }
        [FromQuery(Name = "offset")]
        public int Offset { get; set; }
        
        public PokemonSearchModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            Limit = 1;
            Offset = 1;
        }
        
        public async Task OnGet()
        {
            //var httpClient = _httpClientFactory.CreateClient("PokemonFavoritesAPI");
            
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
                    ViewData["Results"] = reader.ReadToEnd();
                }
            }
        }
    }
}
