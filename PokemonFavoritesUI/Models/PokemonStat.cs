using System.Text.Json.Serialization;

namespace PokemonFavoritesUI.Database.Models;

public class PokemonStat
{
    [JsonPropertyName("base_stat")]
    public int BaseStat { get; set; }
    public int Effort { get; set; }
    public PokemonResultItem Stat { get; set; }
}