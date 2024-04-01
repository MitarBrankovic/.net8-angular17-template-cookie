using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Common.Interfaces;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Application.Features.ApplicationUsers.Commands;
using TemplateBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace TemplateBackend.Application.Features.ApplicationUsers.Queries.GetByEmail;

public class GetByEmailRequest : IRequest<ApplicationUserDto>
{
    public string Email { get; set; }
}

public class GetByEmailCommand : IRequestHandler<GetByEmailRequest, ApplicationUserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ISubjectsRepository _subjectsRepository;
    private readonly IMapper _mapper;

    public GetByEmailCommand(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ISubjectsRepository subjectsRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _subjectsRepository = subjectsRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationUserDto> Handle(GetByEmailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }

            var subjects = await _subjectsRepository.GetSubjectsByUserId(user.Id);

            var applicationUserDto = _mapper.Map<ApplicationUserDto>(user);
            return applicationUserDto;
        }
        catch (Exception)
        {
            throw;
        }

    }
}
