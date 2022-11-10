namespace LanguageForge.Api.Entities;

public class Project : EntityDocument
{
    public required string ProjectName { get; init; }
    public required string ProjectCode { get; init; }
    public required bool AllowSharing { get; init; }
    public required Dictionary<string, InputSystem> InputSystems { get; init; }
}
