namespace LanguageForge.WebApi.Dtos;

public class EntryDto
{
    public InputSystemValueDto[]? Lexeme { get; init; }
    public InputSystemValueDto[]? CitationForm { get; init; }
    public InputSystemValueDto[]? CVPattern { get; init; }
    public SenseDto[]? Sense { get; init; }
}
