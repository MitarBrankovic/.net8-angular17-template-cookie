using System.ComponentModel.DataAnnotations;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;
public class RegistrationDto
{
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public List<SubjectDto>? Subjects{ get; set; }
}

