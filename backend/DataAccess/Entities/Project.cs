namespace LanguageForge.Api.Entities;

public class Project : EntityBase
{
    public required string ProjectName { get; init; }
    public required string ProjectCode { get; init; }
    public required bool AllowSharing { get; init; }
}
