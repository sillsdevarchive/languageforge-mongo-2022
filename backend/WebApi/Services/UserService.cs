using LanguageForge.Api;
using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LanguageForge.WebApi.Services;

public class UserService
{
    private readonly SystemDbContext _systemDbContext;

    public UserService(SystemDbContext systemDbContext)
    {
        _systemDbContext = systemDbContext;
    }

    public async Task<List<UserDto>> ListUsers()
    {
        return await _systemDbContext.Users
            .Find(new BsonDocument())
            .Project(u => toDto(u))
            .ToListAsync();
    }

    public async Task<UserDto?> FindUser(string userId)
    {
        return await _systemDbContext.Users
            .Find(u => u.Id == userId)
            .Project(u => toDto(u))
            .SingleOrDefaultAsync();
    }

    public Task<UserDto> UpdateUser(UserDto user)
    {
        var update = Builders<User>.Update
            .Set((u) => u.Name, user.Name)
            .Set((u) => u.Email, user.Email);
        return _systemDbContext.Users.FindOneAndUpdateAsync(
            (u) => u.Id == user.Id,
            update,
            new FindOneAndUpdateOptions<User, UserDto>
            {
                ReturnDocument = ReturnDocument.After
            }
        );
    }

    public async Task<bool> DeleteUser(string id)
    {
        var result = await _systemDbContext.Users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    private UserDto toDto(User u)
    {
        return new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            Active = u.Active,
        };
    }
}
