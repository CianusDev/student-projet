using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudentProjectFront.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Supprimer les données de session
            HttpContext.Session.Clear();

            // Rediriger vers la page d'accueil
            return RedirectToPage("/Index");
        }
    }
}