using System.Collections.Immutable;
using LanguageForge.Api;
using LanguageForge.Api.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LanguageForge.WebApi.Auth;

public class AuthenticationService
{
    private readonly SystemDbContext _systemDbContext;
    private readonly BCryptOptions _bCryptOptions;

    public AuthenticationService(SystemDbContext systemDbContext, IOptions<BCryptOptions> bCryptOptions)
    {
        _systemDbContext = systemDbContext;
        _bCryptOptions = bCryptOptions.Value;
    }

    public async Task<LfUser?> Authenticate(string emailOrUsername, string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, _bCryptOptions.Cost, _bCryptOptions.EnhancedEntropy);
        var user = await _systemDbContext.Users
            .Find(u => u.Email == emailOrUsername || u.Username == emailOrUsername)
            .SingleOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        return await BuildLfUser(user);
    }

    public async Task<LfUser?> Authenticate(string email)
    {
        var user = await _systemDbContext.Users
            .Find(user => user.Email == email)
            .SingleOrDefaultAsync();
        return await BuildLfUser(user);
    }

    private async Task<LfUser?> BuildLfUser(User? user)
    {
        if (user == null)
        {
            return null;
        }

        IReadOnlyList<UserProjectRole> projectRoles = user.Projects == null
            ? ImmutableList.Create<UserProjectRole>() : await _systemDbContext.Projects
            .Find(proj => user.Projects.Contains(proj.Id) && proj.Users.ContainsKey(user.Id))
            .Project(proj => new UserProjectRole(proj.ProjectCode, proj.Users.GetValueOrDefault(user.Id)!.Role))
            .ToListAsync();
        return new LfUser(user.Email, user.Id, user.Role, projectRoles);
    }
}
