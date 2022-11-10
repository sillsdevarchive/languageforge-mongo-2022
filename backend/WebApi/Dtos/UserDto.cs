namespace LanguageForge.Api.Dtos;

public class UserDto
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public required bool Active { get; init; }
}
