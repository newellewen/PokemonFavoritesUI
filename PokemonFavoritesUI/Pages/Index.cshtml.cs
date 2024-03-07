using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PokemonFavoritesUI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        var jwt = Request.Cookies["jwtCookie"];
        if (String.IsNullOrEmpty(jwt))
        {
            return Redirect("/Login");
        }

        return null;
    }
}