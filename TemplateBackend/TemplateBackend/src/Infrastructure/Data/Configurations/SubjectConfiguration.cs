using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Infrastructure.Data.Configurations;
public class SubjectConfiguration
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasOne(c => c.User)
            .WithMany(p => p.Subjects)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade); // If User is deleted, all subjects are deleted
    }
}
