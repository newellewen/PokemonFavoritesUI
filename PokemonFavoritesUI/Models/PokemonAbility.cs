using System.Text.Json.Serialization;

namespace PokemonFavoritesUI.Database.Models;

public class PokemonAbility
{
    public PokemonResultItem Ability { get; set; }
    [JsonPropertyName("is_hidden")]
    public bool IsHidden { get; set; }
    public int Slot { get; set; }
}