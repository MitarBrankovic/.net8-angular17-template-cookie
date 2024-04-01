using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateBackend.Domain.Entities;
public class Subject
{
    public Guid Id { get; set; }
    public string Theme { get; set; }
    public string Description { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
