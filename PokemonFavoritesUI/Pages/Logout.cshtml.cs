using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PokemonFavoritesUI.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            //Response.Cookies.Append("jwtCookie", json, cookieOptions);
            Response.Cookies.Delete("jwtCookie");
            return Redirect("/Login");
        }
    }
}
