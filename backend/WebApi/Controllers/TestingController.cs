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

    public TestingController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpGet("make-jwt")]
    [AllowAnonymous]
    public string MakeJwt(UserRole role)
    {
        return _jwtService.GenerateJwt(
            new LfUser("test@test.com",
                LfId.FromDb<User>(ObjectId.GenerateNewId()),
                role)
        );
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
