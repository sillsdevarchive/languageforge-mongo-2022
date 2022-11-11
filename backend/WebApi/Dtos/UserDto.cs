using LanguageForge.Api.Entities;

namespace LanguageForge.WebApi.Dtos;

public record UserDto
{
    public required string Id { get; init; }
    public required string Username { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required DateTimeOffset DateCreated { get; init; }
    public required UserRole Role { get; init; }
    public required bool Active { get; init; }
}
