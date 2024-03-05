var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("PokemonFavoritesAPI", httpClient =>
{
    var pokeApiBaseUrl = builder.Configuration.GetValue<string>("BaseUrls:PokemonFavoritesAPI");
    var handler = new HttpClientHandler();
    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
    handler.ServerCertificateCustomValidationCallback = 
        (httpRequestMessage, cert, cetChain, policyErrors) =>
        {
            return true;
        };
    
    httpClient = new HttpClient(handler);
    httpClient.BaseAddress = new Uri(pokeApiBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();