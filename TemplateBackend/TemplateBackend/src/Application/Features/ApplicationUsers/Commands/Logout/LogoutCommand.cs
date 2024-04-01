using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using TemplateBackend.Application.Common.Models;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Logout;

public class LogoutRequest : IRequest<Result>
{
    public string Email { get; set; }
}


public class LogoutCommand : IRequestHandler<LogoutRequest, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;

    public LogoutCommand(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<Result> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null)
            {
                return Result.Failure(["User not found"]);
            }

            await _signInManager.SignOutAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
