using System.ComponentModel.DataAnnotations;
using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Auth;
using LanguageForge.WebApi.Services;
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
    private readonly UserService _userService;

    public TestingController(JwtService jwtService, UserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }

    [HttpGet("make-jwt-role/{role}")]
    [AllowAnonymous]
    public string MakeJwtForTole(UserRole role)
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
        var lfUser = await _userService.FindLfUser(email);
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
