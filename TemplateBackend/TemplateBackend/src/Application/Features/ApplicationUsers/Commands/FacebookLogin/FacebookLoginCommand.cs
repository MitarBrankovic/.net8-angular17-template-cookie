using TemplateBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Newtonsoft.Json;
using TemplateBackend.Application.Common.Models;

namespace TemplateBackend.Application.Features.ApplicationUsers.Commands.FacebookLogin;

public class FacebookLoginRequest : IRequest<Result>
{
    public string AccessToken { get; set; }
    public string Provider { get; set; }
}

public class FacebookLoginCommand: IRequestHandler<FacebookLoginRequest, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _facebookSettings;
    private readonly HttpClient _httpClient;

    public FacebookLoginCommand(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _facebookSettings = _configuration.GetSection("FacebookAuthSettings");
        _httpClient = httpClientFactory.CreateClient("Facebook");
    }

    public async Task<Result> Handle(FacebookLoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userInfoUrl = _facebookSettings.GetSection("UserInfoUrl").Value;
            var appId = _facebookSettings.GetSection("AppId").Value;
            var appSecret = _facebookSettings.GetSection("AppSecret").Value;
            var url = string.Format(userInfoUrl, request.AccessToken);
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync();
                var payload = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(responseAsString);
                
                var info = new UserLoginInfo(request.Provider, payload.Id, "Facebook");
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
                            return Result.Failure(["Failed to login with Facebook."]);
                        }
                    }
                }
                else
                {
                    await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                    return Result.Success();
                }
            }
            else
            {
                return Result.Failure(["Failed to authenticate Facebook token."]);
            }
        }
        catch (Exception ex)
        {
            throw new RestException(ex.Message);
        }
    }
}
