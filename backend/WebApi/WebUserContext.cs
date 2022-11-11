using LanguageForge.Api.Entities;

namespace LanguageForge.WebApi;

/// <summary>
/// Class to contain authenticated user details the source of that info being JWT
/// </summary>
public class WebUserContext
{
    public required string UserId { get; init; }
    public required UserProjectRole[] Projects { get; init; }
    public required UserRole Role { get; init; }
}


public record UserProjectRole(string ProjectId, ProjectRole Role);
