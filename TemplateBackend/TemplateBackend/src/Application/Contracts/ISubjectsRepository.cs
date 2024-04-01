using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Domain.Entities;

namespace TemplateBackend.Application.Contracts;
public interface ISubjectsRepository
{
    Task<List<Subject>> GetSubjectsByUserId(string userId);
}
