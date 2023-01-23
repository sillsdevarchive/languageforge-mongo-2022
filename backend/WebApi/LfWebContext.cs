using LanguageForge.WebApi.Auth;

namespace LanguageForge.WebApi;

public interface ILfWebContext
{
    /// <summary>
    /// Authenticated user details from JWT token
    /// </summary>
    LfUser User { get; }
}

public class LfWebContext : ILfWebContext
{
    public required LfUser User { get; init; }
}
