using LanguageForge.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LanguageForge.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly SystemDbContext _systemDbContext;

    public UserController(SystemDbContext systemDbContext)
    {
        _systemDbContext = systemDbContext;
    }

    // GET: api/User
    [HttpGet]
    public async Task<List<User>> Get()
    {
        return await _systemDbContext.Users.Find(user => true).ToListAsync();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST: api/User
    [HttpPost]
    public void Post([FromBody] string value)
    {
        // _systemDbContext.Users.
    }

    // PUT: api/User/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
