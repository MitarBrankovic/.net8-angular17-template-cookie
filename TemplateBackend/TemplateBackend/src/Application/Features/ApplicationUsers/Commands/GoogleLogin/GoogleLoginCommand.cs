using TemplateBackend.Domain.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using TemplateBackend.Application.Common.Models;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.GoogleLogin;

public class GoogleLoginRequest : IRequest<Result>
{
    public string Email { get; set; }
    public string Provider { get; set; }
    public string IdToken { get; set; }
}

public class GoogleLoginCommand: IRequestHandler<GoogleLoginRequest, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _googleSettings;

    public GoogleLoginCommand(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _googleSettings = _configuration.GetSection("GoogleAuthSettings");
    }

    public async Task<Result> Handle(GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = await VerifyGoogleToken(request.IdToken);

            if (payload == null)
            {
                throw new RestException("Failed to authenticate Google token.");
            }

            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    return Result.Failure(["User with this email is not registered in the system."]);
                }
                else
                {
                    var externalLogin = await _userManager.AddLoginAsync(user, info);
                    if (externalLogin.Succeeded)
                    {
                        await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                        return Result.Success();
                    }
                    else
                    {
                        return Result.Failure(["Failed to login with Google."]);
                    }
                }
            }
            else
            {
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                return Result.Success();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() 
                { 
                    _googleSettings.GetSection("androidClientId").Value,
                    _googleSettings.GetSection("iosClientId").Value,
                }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
