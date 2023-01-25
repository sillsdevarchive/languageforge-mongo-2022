using System.Security.Claims;
using LanguageForge.WebApi.Auth;
using LanguageForge.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LanguageForge.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly GoogleTokenValidator _googleTokenValidator;
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    public LoginController(
        GoogleTokenValidator googleTokenValidator,
        UserService userService,
        JwtService jwtService
    )
    {
        _googleTokenValidator = googleTokenValidator;
        _userService = userService;
        _jwtService = jwtService;
    }


    [HttpPost("login-by-password")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticatedResponse>> LoginByPassword(string email, string password)
    {
        if (!await _userService.IsPasswordValid(email, password))
        {
            return Unauthorized();
        }

        var user = await _userService.FindLfUser(email);
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
        var user = await _userService.FindLfUser(email);
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
