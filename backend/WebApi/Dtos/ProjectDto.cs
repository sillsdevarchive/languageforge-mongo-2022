namespace LanguageForge.Api.Dtos;

public class ProjectDto
{
    public required string Id { get; init; }
    public required string ProjectName { get; init; }
    public required string ProjectCode { get; init; }
    public required string[] InputSystems { get; init; }
}
