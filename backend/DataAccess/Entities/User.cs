using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LanguageForge.Api.Entities;

public class User : EntityDocument
{
    [BsonConstructor(nameof(_roleString))]
    public User(string role)
    {
        _roleString = role;
    }

    public required string Name { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }

    [BsonElement("role")]
    private readonly string _roleString;

    [BsonIgnore]
    public required UserRole Role
    {
        get => UserRoleFromString(_roleString);
        init => _roleString = UserRoleToString(value);
    }

    private UserRole UserRoleFromString(string role)
    {
        return role switch
        {
            "system_admin" => UserRole.SystemAdmin,
            "user" => UserRole.User,
            _ => throw new NotSupportedException($"""user role "{role}" is not supported""")
        };
    }

    private string UserRoleToString(UserRole userRole)
    {
        return userRole switch
        {
            UserRole.SystemAdmin => "system_admin",
            UserRole.User => "user",
            _ => throw new NotSupportedException($"""user role "{userRole}" is not supported""")
        };
    }

    public required bool Active { get; init; }
}

public enum UserRole
{
    SystemAdmin,
    User
}
