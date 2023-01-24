using LanguageForge.WebApi.Auth;

namespace LanguageForge.WebApi;

public interface ILfWebContext
{
    /// <summary>
    /// Authenticated user details
    /// </summary>
    LfUser User { get; }
}

public class LfWebContext : ILfWebContext
{
    public LfUser User { get; }

    public LfWebContext(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User == null)
        {
            throw new InvalidOperationException($"{nameof(ILfWebContext)} should only be accessed in contexts where the user is authenticated.");
        }
        User = JwtService.ExtractLfUser(httpContext.User);
    }

}
