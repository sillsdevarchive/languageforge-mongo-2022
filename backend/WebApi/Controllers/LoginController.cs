using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using LanguageForge.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LanguageForge.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly GoogleTokenValidator _googleTokenValidator;
    private readonly AuthenticationService _authService;
    private readonly JwtService _jwtService;

    public LoginController(
        GoogleTokenValidator googleTokenValidator,
        AuthenticationService authService,
        JwtService jwtService
    )
    {
        _googleTokenValidator = googleTokenValidator;
        _authService = authService;
        _jwtService = jwtService;
    }


    [HttpPost("login-by-password")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticatedResponse>> LoginByPassword(
        string emailOrUsername,
        [DataType(DataType.Password)] string password)
    {
        var user = await _authService.Authenticate(emailOrUsername, password);
        if (user == null)
        {
            return Unauthorized();
        }
        return new AuthenticatedResponse
        {
            User = user,
            Jwt = _jwtService.GenerateJwt(user),
            RefreshToken = _jwtService.GenerateRefreshToken(user)
        };
    }

    [HttpGet("validate-google-jwt")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticatedResponse>> GoogleResponse(string googleJwt)
    {
        var claimsPrincipal = await _googleTokenValidator.ValidateGoogleJwt(googleJwt);
        var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        ArgumentNullException.ThrowIfNull(email);
        var user = await _authService.Authenticate(email);
        if (user == null)
        {
            return Unauthorized();
        }
        return new AuthenticatedResponse
        {
            User = user,
            Jwt = _jwtService.GenerateJwt(user),
            RefreshToken = _jwtService.GenerateRefreshToken(user)
        };
    }

    [HttpGet("refresh-jwt")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RefreshJwt(string refreshToken)
    {
        try
        {
            return await _jwtService.ValidateRefreshToken(refreshToken);
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(new ApiError(e));
        }
    }
}
