namespace PokemonFavoritesUI.Helpers;

public static class HttpClientHelper
{
    public static HttpClient GetHttpClient()
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

        return httpClient;
    }
}