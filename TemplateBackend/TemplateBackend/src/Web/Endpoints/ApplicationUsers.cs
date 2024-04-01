using TemplateBackend.Application.Features.ApplicationUsers.Commands.DeleteUser;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.FacebookLogin;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.GoogleLogin;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Login;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Logout;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Register;
using TemplateBackend.Application.Features.ApplicationUsers.Commands.Registration;
using TemplateBackend.Application.Features.ApplicationUsers.Queries.GetByEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TemplateBackend.Web.Endpoints;

public class ApplicationUsers : EndpointGroupBase
{

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(LoginUser, "LoginUser")
            .MapPost(GoogleLogin, "GoogleLogin")
            .MapPost(FacebookLogin, "FacebookLogin/{accessToken}")
            .MapPost(LogoutUser, "LogoutUser/{email}")
            .MapPost(RegisterUser, "RegisterUser")
            .MapDelete(DeleteUser, "DeleteUser/{email}")
            .MapGet(GetByEmail, "GetByEmail/{email}");

    }

    [AllowAnonymous]
    public async Task<IResult> LoginUser([FromBody] LoginDto dto, ISender sender)
    {
        var loginRequest = new LoginRequest
        {
            Username = dto.Username,
            Password = dto.Password,
            RememberMe = dto.RememberMe
        };
        var response = await sender.Send(loginRequest);
        return response.Succeeded ? Results.Ok(response.Data) : Results.BadRequest(response.Errors);
    }

    [AllowAnonymous]
    public async Task<IResult> GoogleLogin([FromBody] GoogleLoginDto dto, ISender sender)
    {
        var loginRequest = new GoogleLoginRequest
        {
            Email = dto.Email,
            Provider = dto.Provider,
            IdToken = dto.IdToken
        };
        var response = await sender.Send(loginRequest);
        return response.Succeeded ? Results.Ok() : Results.BadRequest(response.Errors);
    }

    [AllowAnonymous]
    public async Task<IResult> FacebookLogin(string accessToken, ISender sender)
    {
        var loginRequest = new FacebookLoginRequest
        {
            AccessToken = accessToken
        };
        var response = await sender.Send(loginRequest);
        return response.Succeeded ? Results.Ok() : Results.BadRequest(response.Errors);
    }

    public async Task<IResult> LogoutUser(string email, ISender sender)
    {
        var logoutRequest = new LogoutRequest
        {
            Email = email
        };
        var response = await sender.Send(logoutRequest);
        return response.Succeeded ? Results.Ok() : Results.BadRequest(response.Errors);
    }

    [AllowAnonymous]
    public async Task<IResult> RegisterUser([FromBody] RegistrationDto registrationDto, ISender sender)
    {
        var request = new RegistrationRequest
        {
            RegistrationDto = registrationDto
        };

        var response = await sender.Send(request);
        return response.UserId != null ? Results.Ok(response) : Results.BadRequest(response);
    }

    public async Task<IResult> DeleteUser(string email, ISender sender)
    {
        var request = new DeleteUserRequest
        {
            Email = email
        };
        var response = await sender.Send(request);
        return Results.Ok(response);
    }

    public async Task<IResult> GetByEmail(string email, ISender sender)
    {
        var request = new GetByEmailRequest
        {
            Email = email
        };
        var response = await sender.Send(request);
        return response != null ? Results.Ok(response) : Results.BadRequest("User not found.");
    }
}
