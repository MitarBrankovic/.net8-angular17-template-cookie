using Microsoft.AspNetCore.Identity;

namespace TemplateBackend.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public List<Subject> Subjects { get; set; }

}
