using System;
using TemplateBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Registration;

public class RegistrationRequest : IRequest<RegistrationResponseDto>
{
    public RegistrationDto RegistrationDto { get; set; }
}

public class RegistrationCommand : IRequestHandler<RegistrationRequest, RegistrationResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public RegistrationCommand(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<RegistrationResponseDto> Handle(RegistrationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var errors = new List<string>();
            var existingUser = await _userManager.FindByEmailAsync(request.RegistrationDto.Email);
 
            if (existingUser != null)
            {
                errors.Add("User with this email already exists");
            }

            var newUser = _mapper.Map<ApplicationUser>(request.RegistrationDto);
            var result = await _userManager.CreateAsync(newUser, request.RegistrationDto.Password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(x => x.Description));
                return new RegistrationResponseDto{ Errors = errors, UserId = null };
            }
            else
            {
                return new RegistrationResponseDto { Errors = null, UserId = newUser.Id };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }   

    }
}
