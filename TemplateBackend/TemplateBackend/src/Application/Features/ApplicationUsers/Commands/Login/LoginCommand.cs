using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TemplateBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Rest;
using TemplateBackend.Application.Common.Models;
using TemplateBackend.Application.Features.ApplicationUsers.Queries;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.Login;

public class LoginRequest : IRequest<Result>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class LoginCommand : IRequestHandler<LoginRequest, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;

    public LoginCommand(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<Result> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return Result.Failure(["User not found."]);
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);

            if (result.Succeeded)
            {
                var applicationUserDto = _mapper.Map<ApplicationUserDto>(user);
                return Result.Success(applicationUserDto);
            }

            throw new RestException(HttpStatusCode.Unauthorized.ToString());
        }
        catch (Exception ex)
        {
            return Result.Failure(["Invalid username/password."]);
        }

    }

}
