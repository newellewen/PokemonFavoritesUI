using System.IdentityModel.Tokens.Jwt;

namespace PokemonFavoritesUI.Helpers;

public static class JwtHelper
{
    public static JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string? jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
    
        return token;
    }
    
    public static IEnumerable<Tuple<string, string>> GetJwtClaims(JwtSecurityToken token)
    {
        var claims = token.Claims
            .Select(claim => new Tuple<string, string>(claim.Type, claim.Value));
        
        return claims;
    }
    
}