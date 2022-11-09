namespace LanguageForge.Api.Entities;

public class InputSystem: EntityBase
{
    public string Abbreviation { get; set; }
    public string Tag { get; set; }
    public string LanguageName { get; set; }
    public bool IsRightToLeft { get; set; }
}
