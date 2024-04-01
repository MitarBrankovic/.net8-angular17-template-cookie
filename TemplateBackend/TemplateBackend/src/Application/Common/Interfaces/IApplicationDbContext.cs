using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    //DbSet<ApplicationUser> Users { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
