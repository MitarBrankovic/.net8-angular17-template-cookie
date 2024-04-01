using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Domain.Entities;
using TemplateBackend.Infrastructure.Data;

namespace TemplateBackend.Infrastructure.Repository;
public class SubjectsRepository: ISubjectsRepository
{
    private readonly ApplicationDbContext _context;
    public SubjectsRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Subject>> GetSubjectsByUserId(string userId)
    {
        return await _context.Subjects.Where(c => c.UserId == userId).ToListAsync();
    }
}
