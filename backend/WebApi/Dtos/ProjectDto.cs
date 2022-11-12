using LanguageForge.Api.Entities;

namespace LanguageForge.WebApi.Dtos;

public class ProjectDto
{
    public required LfId<Project> Id { get; init; }
    public required string ProjectName { get; init; }
    public required string ProjectCode { get; init; }
    public required string[] InputSystems { get; init; }

    /// <summary>
    /// dictionary of user ids as the key
    /// </summary>
    public required ProjectUserDto[] Users { get; init; }
}

public record ProjectUserDto(LfId<User> UserId, ProjectRole Role)
{
}
