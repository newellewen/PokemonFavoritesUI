namespace PokemonFavoritesUI.Models;

public class PokemonSearchResponse
{
    public int TotalPages { get; set; }
    public int Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public IEnumerable<PokemonSearchResult> Results { get; set; }
}