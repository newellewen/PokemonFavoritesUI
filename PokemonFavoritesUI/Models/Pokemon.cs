using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PokemonFavoritesUI.Database.Models;

// TODO - make common package?
public class Pokemon
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<PokemonAbility>? Abilities { get; set; }
    [JsonPropertyName("base_experience")]
    public int BaseExperience { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public PokemonSprite? Sprites { get; set; }
    public IEnumerable<PokemonStat> Stats { get; set; }
    public IEnumerable<PokemonType> Types { get; set; }
}