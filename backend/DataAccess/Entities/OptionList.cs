namespace LanguageForge.Api.Entities;

public class OptionList
{
    public required string Code { get; init; }
    public required List<Option> Items { get; init; }
}

public class Option
{
    public required Guid Guid { get; init; }
    public required string Key { get; init; }
    public required string Value { get; init; }
    public required string Abbreviation { get; init; }
}
