namespace PokemonFavoritesUI.Models;

public class FavoritePokemon
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PokemonId { get; set; }
    public string? Name { get; set; }
    public string? Types { get; set; }
    public string? Thumbnail { get; set; }
}