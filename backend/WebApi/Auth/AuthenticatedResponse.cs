namespace LanguageForge.WebApi.Auth;

public class AuthenticatedResponse
{
    public string Jwt { get; set; }
    public string RefreshToken { get; set; }
    public LfUser User { get; set; }
}