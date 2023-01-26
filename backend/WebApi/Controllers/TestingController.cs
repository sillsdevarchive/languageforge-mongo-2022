using System.ComponentModel.DataAnnotations;
using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace LanguageForge.WebApi.Controllers;
#if DEBUG
[Route("api/[controller]")]
[ApiController]
public class TestingController : Controller
{
    private readonly JwtService _jwtService;
    private readonly AuthenticationService _authService;

    public TestingController(JwtService jwtService, AuthenticationService authService)
    {
        _jwtService = jwtService;
        _authService = authService;
    }

    [HttpGet("make-jwt-role/{role}")]
    [AllowAnonymous]
    public string MakeJwtForRole(UserRole role)
    {
        return _jwtService.GenerateJwt(
            new LfUser("test@test.com",
                LfId.FromDb<User>(ObjectId.GenerateNewId()),
                role,
                new[] { new UserProjectRole("test-3", ProjectRole.Manager) })
        );
    }

    [HttpGet("make-jwt-email/{email}")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> MakeJwtForUser([EmailAddress] string email)
    {
        var lfUser = await _authService.Authenticate(email);
        return lfUser == null ? NotFound() : _jwtService.GenerateJwt(lfUser);
    }

    [HttpGet("requires-auth")]
    public OkObjectResult RequiresAuth()
    {
        return Ok("success");
    }

    [RoleRequired(UserRole.SystemAdmin)]
    [HttpGet("requires-admin")]
    public OkResult RequiresAdmin()
    {
        return Ok();
    }
}
#endif
