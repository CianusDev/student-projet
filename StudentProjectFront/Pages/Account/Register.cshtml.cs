using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectFront.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Le r�le est requis")]
            [Display(Name = "R�le utilisateur")]
            public string Role { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le nom complet est requis")]
            [Display(Name = "Nom complet")]
            [StringLength(100, ErrorMessage = "Le nom ne peut pas d�passer 100 caract�res")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "L'adresse e-mail est requise")]
            [EmailAddress(ErrorMessage = "Format d'e-mail invalide")]
            [Display(Name = "Adresse e-mail institutionnelle")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le mot de passe est requis")]
            [StringLength(100, ErrorMessage = "Le mot de passe doit contenir au moins {2} caract�res", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmer le mot de passe")]
            [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
            public string ConfirmPassword { get; set; } = string.Empty;

            // Champs pour �tudiant
            [Display(Name = "Num�ro d'�tudiant")]
            public string? StudentNumber { get; set; }

            [Display(Name = "Fili�re ou sp�cialit�")]
            public string? Major { get; set; }

            [Display(Name = "Niveau d'�tudes")]
            public string? StudyLevel { get; set; }

            [Display(Name = "Groupe de projet (optionnel)")]
            public string? ProjectGroup { get; set; }

            // Champs pour Enseignant
            [Display(Name = "Matricule enseignant")]
            public string? TeacherCode { get; set; }

            [Display(Name = "D�partement ou UFR")]
            public string? Department { get; set; }
        }

        public void OnGet()
        {
            // Logique pour l'affichage de la page
        }

        // Correction 1: Suppression d'async car pas d'await utilis�
        public IActionResult OnPost()
        {
            // Validation conditionnelle selon le r�le
            if (Input.Role == "Etudiant")
            {
                if (string.IsNullOrWhiteSpace(Input.StudentNumber))
                {
                    ModelState.AddModelError("Input.StudentNumber", "Le num�ro d'�tudiant est requis");
                }
                if (string.IsNullOrWhiteSpace(Input.Major))
                {
                    ModelState.AddModelError("Input.Major", "La fili�re est requise");
                }
                if (string.IsNullOrWhiteSpace(Input.StudyLevel))
                {
                    ModelState.AddModelError("Input.StudyLevel", "Le niveau d'�tudes est requis");
                }
            }
            else if (Input.Role == "Enseignant")
            {
                if (string.IsNullOrWhiteSpace(Input.TeacherCode))
                {
                    ModelState.AddModelError("Input.TeacherCode", "Le matricule enseignant est requis");
                }
                if (string.IsNullOrWhiteSpace(Input.Department))
                {
                    ModelState.AddModelError("Input.Department", "Le d�partement est requis");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Logique d'inscription - sauvegarder en base de donn�es
            // Exemple simple pour d�monstration
            try
            {
                // Simulation de l'inscription r�ussie
                TempData["SuccessMessage"] = "Inscription r�ussie ! Vous pouvez maintenant vous connecter.";
                return RedirectToPage("/Account/Login");
            }
            catch // Correction 2: Suppression de la variable ex non utilis�e
            {
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'inscription. Veuillez r�essayer.");
                return Page();
            }
        }
    }
}