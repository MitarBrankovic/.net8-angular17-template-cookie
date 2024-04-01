using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Application.Contracts;
using TemplateBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Rest;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.DeleteUser;

public class DeleteUserRequest : IRequest<bool>
{
    public string Email { get; set; }
}

public class DeleteUserCommand: IRequestHandler<DeleteUserRequest, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public DeleteUserCommand(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new RestException("User does not exist.");
            }

            await _signInManager.SignOutAsync();
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            throw new Exception("Problem deleting user");
        }
        catch (Exception)
        {
            throw new Exception("Problem deleting user");
        }
    }
}
