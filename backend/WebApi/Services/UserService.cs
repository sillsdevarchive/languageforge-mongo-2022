using LanguageForge.Api;
using LanguageForge.Api.Entities;
using LanguageForge.Api.Extensions;
using LanguageForge.WebApi.Auth;
using LanguageForge.WebApi.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LanguageForge.WebApi.Services;

public class UserService
{
    private readonly SystemDbContext _systemDbContext;

    private static readonly ProjectionDefinition<User, UserDto> UserToDtoProjection =
        Builders<User>.Projection.Expression(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            DateCreated = user.DateCreated,
            Role = user.Role,
            Active = user.Active,
        });

    public UserService(SystemDbContext systemDbContext)
    {
        _systemDbContext = systemDbContext;
    }


    public async Task<List<UserDto>> ListUsers()
    {
        return await _systemDbContext.Users
            .Find(new BsonDocument())
            .Project(UserToDtoProjection)
            .ToListAsync();
    }

    public async Task<UserDto?> FindUser(LfId<User> userId)
    {
        return await _systemDbContext.Users
            .Find(u => u.Id == userId)
            .Project(UserToDtoProjection)
            .SingleOrDefaultAsync();
    }

    public async Task<UserDto?> UpdateUser(UserDto user)
    {
        var update = Builders<User>.Update
            .Set((u) => u.Name, user.Name)
            .Set((u) => u.Email, user.Email);
        return await _systemDbContext.Users.Update(
            user.Id,
            update,
            UserToDtoProjection
        );
    }

    public async Task<bool> DeleteUser(LfId<User> id)
    {
        var result = await _systemDbContext.Users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<LfUser?> FindLfUser(string email)
    {
        var user = await _systemDbContext.Users
            .Find(user => user.Email == email)
            .SingleOrDefaultAsync();

        if (user == null)
        {
            return null;
        }

        var projectRoles = await _systemDbContext.Projects
            .Find(proj => user.Projects.Contains(proj.Id) && proj.Users.ContainsKey(user.Id))
            .Project(proj => new UserProjectRole(proj.ProjectCode, proj.Users.GetValueOrDefault(user.Id)!.Role))
            .ToListAsync();

        return new LfUser(user.Email, user.Id, user.Role, projectRoles);
    }

    public async Task<bool> IsPasswordValid(string email, string password)
    {
        throw new NotImplementedException();
    }
}
