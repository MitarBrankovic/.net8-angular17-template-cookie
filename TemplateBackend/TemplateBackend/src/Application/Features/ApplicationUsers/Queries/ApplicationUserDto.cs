using TemplateBackend.Application.Features.ApplicationUsers.Commands;

namespace TemplateBackend.Application.Features.ApplicationUsers.Queries;
public class ApplicationUserDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public List<SubjectDto> Subjects { get; set; }
}
