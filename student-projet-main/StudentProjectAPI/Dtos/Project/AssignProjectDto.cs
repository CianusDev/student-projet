using System.ComponentModel.DataAnnotations;

public class AssignProjectDto : IValidatableObject
{
    [Required(ErrorMessage = "L'ID du projet est requis")]
    public int ProjectId { get; set; }

    public List<int> StudentIds { get; set; } = new();
    public List<int> GroupIds { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!StudentIds.Any() && !GroupIds.Any())
        {
            yield return new ValidationResult(
                "Au moins un étudiant ou un groupe doit être assigné.",
                new[] { nameof(StudentIds), nameof(GroupIds) });
        }
    }
}
