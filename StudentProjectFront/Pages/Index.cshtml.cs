using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudentProjectFront.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string? UserEmail { get; set; }
        public bool IsLoggedIn { get; set; }

        public void OnGet()
        {
            // Vérifier si l'utilisateur est connecté via la session
            UserEmail = HttpContext.Session.GetString("UserEmail");
            IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
        }

        public IActionResult OnPostLogout()
        {
            // Déconnexion
            HttpContext.Session.Clear();
            return RedirectToPage("/Account/Login");
        }
    }
}