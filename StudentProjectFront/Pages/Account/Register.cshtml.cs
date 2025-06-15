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
            [Required(ErrorMessage = "Le rôle est requis")]
            [Display(Name = "Rôle utilisateur")]
            public string Role { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le nom complet est requis")]
            [Display(Name = "Nom complet")]
            [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "L'adresse e-mail est requise")]
            [EmailAddress(ErrorMessage = "Format d'e-mail invalide")]
            [Display(Name = "Adresse e-mail institutionnelle")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le mot de passe est requis")]
            [StringLength(100, ErrorMessage = "Le mot de passe doit contenir au moins {2} caractères", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmer le mot de passe")]
            [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
            public string ConfirmPassword { get; set; } = string.Empty;

            // Champs pour Étudiant
            [Display(Name = "Numéro d'étudiant")]
            public string? StudentNumber { get; set; }

            [Display(Name = "Filière ou spécialité")]
            public string? Major { get; set; }

            [Display(Name = "Niveau d'études")]
            public string? StudyLevel { get; set; }

            [Display(Name = "Groupe de projet (optionnel)")]
            public string? ProjectGroup { get; set; }

            // Champs pour Enseignant
            [Display(Name = "Matricule enseignant")]
            public string? TeacherCode { get; set; }

            [Display(Name = "Département ou UFR")]
            public string? Department { get; set; }
        }

        public void OnGet()
        {
            // Logique pour l'affichage de la page
        }

        // Correction 1: Suppression d'async car pas d'await utilisé
        public IActionResult OnPost()
        {
            // Validation conditionnelle selon le rôle
            if (Input.Role == "Etudiant")
            {
                if (string.IsNullOrWhiteSpace(Input.StudentNumber))
                {
                    ModelState.AddModelError("Input.StudentNumber", "Le numéro d'étudiant est requis");
                }
                if (string.IsNullOrWhiteSpace(Input.Major))
                {
                    ModelState.AddModelError("Input.Major", "La filière est requise");
                }
                if (string.IsNullOrWhiteSpace(Input.StudyLevel))
                {
                    ModelState.AddModelError("Input.StudyLevel", "Le niveau d'études est requis");
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
                    ModelState.AddModelError("Input.Department", "Le département est requis");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Logique d'inscription - sauvegarder en base de données
            // Exemple simple pour démonstration
            try
            {
                // Simulation de l'inscription réussie
                TempData["SuccessMessage"] = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
                return RedirectToPage("/Account/Login");
            }
            catch // Correction 2: Suppression de la variable ex non utilisée
            {
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'inscription. Veuillez réessayer.");
                return Page();
            }
        }
    }
}