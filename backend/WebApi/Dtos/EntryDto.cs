using LanguageForge.Api.Entities;

namespace LanguageForge.WebApi.Dtos;

public class EntryDto
{
    public required Dictionary<string, ValueWrapper>? Lexeme { get; set; }
    public required Dictionary<string, ValueWrapper>? CitationForm { get; set; }
    public required Dictionary<string, ValueWrapper>? Pronunciation { get; set; }
    public required Dictionary<string, ValueWrapper>? CVPattern { get; set; }
    public required Dictionary<string, ValueWrapper>? Tone { get; set; }
    public required string? Location { get; set; }
    public required Dictionary<string, ValueWrapper>? Etymology { get; set; }
    public required Dictionary<string, ValueWrapper>? EtymologyGloss { get; set; }
    public required Dictionary<string, ValueWrapper>? EtymologyComment { get; set; }
    public required Dictionary<string, ValueWrapper>? EtymologySource { get; set; }
    public required Dictionary<string, ValueWrapper>? Note { get; set; }
    public required Dictionary<string, ValueWrapper>? LiteralMeaning { get; set; }
    public required Dictionary<string, ValueWrapper>? Bibliography { get; set; }
    public required Dictionary<string, ValueWrapper>? Restrictions { get; set; }
    public required Dictionary<string, ValueWrapper>? SummaryDefinition { get; set; }
    public required List<SenseDto>? Senses { get; set; }
}

public class SenseDto
{
    public required Guid Guid { get; set; }
    public required Dictionary<string, ValueWrapper>? Gloss { get; set; }
    public required Dictionary<string, ValueWrapper>? Definition { get; set; }
    public required List<FileDto>? Pictures { get; set; }
    public required string? PartOfSpeech { get; set; }
    public required List<string>? SemanticDomain { get; set; }
    public required Dictionary<string, ValueWrapper>? ScientificName { get; set; }
    public required Dictionary<string, ValueWrapper>? AnthropologyNote { get; set; }
    public required Dictionary<string, ValueWrapper>? Bibliography { get; set; }
    public required Dictionary<string, ValueWrapper>? DiscourseNote { get; set; }
    public required Dictionary<string, ValueWrapper>? EncyclopedicNote { get; set; }
    public required Dictionary<string, ValueWrapper>? GeneralNote { get; set; }
    public required Dictionary<string, ValueWrapper>? GrammarNote { get; set; }
    public required Dictionary<string, ValueWrapper>? PhonologyNote { get; set; }
    public required Dictionary<string, ValueWrapper>? Restrictions { get; set; }
    public required Dictionary<string, ValueWrapper>? SemanticsNote { get; set; }
    public required Dictionary<string, ValueWrapper>? SociolinguisticsNote { get; set; }
    public required Dictionary<string, ValueWrapper>? Source { get; set; }
    public required string? SenseType { get; set; }
    public required string? Status { get; set; }
    public required List<ExampleDto>? Examples { get; set; }
}

public class ExampleDto
{
    public required Guid Guid { get; set; }
    public required Dictionary<string, ValueWrapper>? Sentence { get; set; }
    public required Dictionary<string, ValueWrapper>? Translation { get; set; }
    public required Dictionary<string, ValueWrapper>? Reference { get; set; }
}

public class FileDto
{
    public required Guid Guid { get; set; }
    public required string FileName { get; set; }
}

