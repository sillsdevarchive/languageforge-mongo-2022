namespace LanguageForge.WebApi.Auth;

public class BCryptOptions
{
    public const string BCrypt = nameof(BCrypt);

    public required int Cost { get; init; }
    public required bool EnhancedEntropy { get; init; }
}
