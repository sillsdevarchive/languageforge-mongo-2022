using LanguageForge.WebApi.Dtos;
using LanguageForge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanguageForge.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/User
    [HttpGet]
    public async Task<List<UserDto>> Get()
    {
        return await _userService.ListUsers();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(string id)
    {
        var user = await _userService.FindUser(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/User
    [HttpPost]
    public async Task Post([FromBody] UserDto user)
    {
        // _systemDbContext.Users.
    }

    // PUT: api/User/5
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> PutAsync(string id, [FromBody] UserDto user)
    {
        if (id != user.Id)
        {
            throw new ArgumentException("User Id in URL and body do not match.");
        }

        var updatedUser = await _userService.UpdateUser(user);
        if (updatedUser == null)
        {
            return NotFound();
        }

        return updatedUser;
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<StatusCodeResult> Delete(string id)
    {
        var deleted = await _userService.DeleteUser(id);
        return deleted ? Ok() : NotFound();
    }
}
