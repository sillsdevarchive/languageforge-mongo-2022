namespace LanguageForge.WebApi.Dtos;

public class SenseDto
{
    public InputSystemValueDto[]? Gloss { get; init; }
    public InputSystemValueDto[]? Definition { get; init; }
    public PartOfSpeechDto? PartOfSpeech { get; init; }
    public string[]? SemanticDomain { get; init; }
}
