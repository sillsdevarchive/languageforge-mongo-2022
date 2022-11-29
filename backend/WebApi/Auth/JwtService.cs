using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LanguageForge.WebApi.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LanguageForge.WebApi.Auth;

public class JwtService
{
    public const string RoleClaimType = "role";
    public const string EmailClaimType = JwtRegisteredClaimNames.Email;
    private readonly IOptions<JwtOptions> _userOptions;
    private readonly UserService _userService;

    public JwtService(IOptions<JwtOptions> userOptions, UserService userService)
    {
        _userOptions = userOptions;
        _userService = userService;
    }

    public string GenerateJwt(LfUser user)
    {
        var options = _userOptions.Value;
        return GenerateToken(user, options.Audience, options.Lifetime);
    }

    public string GenerateRefreshToken(LfUser user)
    {
        return GenerateToken(user, _userOptions.Value.RefreshAudience, _userOptions.Value.RefreshLifetime);
    }

    public async Task<string> ValidateRefreshToken(string refreshToken)
    {
        var options = _userOptions.Value;
        var validationParameters = TokenValidationParameters(options, true);
        var tokenHandler = new JwtSecurityTokenHandler
        {
            MapInboundClaims = false
        };
        var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out var token);
        var email = principal.FindFirstValue(EmailClaimType);
        ArgumentException.ThrowIfNullOrEmpty(email);
        var user = await _userService.GetUserByEmail(email);
        ArgumentNullException.ThrowIfNull(user);
        return GenerateJwt(user);
    }

    private string GenerateToken(LfUser user, string audience, TimeSpan tokenLifetime)
    {
        var jwtDate = DateTime.UtcNow;
        var options = _userOptions.Value;
        var jwt = new JwtSecurityToken(
            audience: audience,
            issuer: options.Issuer,
            claims: GetClaims(user),
            notBefore: jwtDate,
            expires: jwtDate + tokenLifetime,
            signingCredentials: new SigningCredentials(
                GetSigningKey(options),
                SecurityAlgorithms.HmacSha256
            )
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return token;
    }

    private static SymmetricSecurityKey GetSigningKey(JwtOptions jwtOptions)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
    }

    private List<Claim> GetClaims(LfUser user)
    {
        return new()
        {
            new Claim(EmailClaimType, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            //todo add project specific roles
            new Claim(RoleClaimType, user.Role.ToString()),
        };
    }

    public static TokenValidationParameters TokenValidationParameters(JwtOptions jwtOptions, bool forRefresh = false)
    {
        return new TokenValidationParameters
        {
            RoleClaimType = RoleClaimType,
            NameClaimType = EmailClaimType,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSigningKey(jwtOptions),
            ValidAudience = forRefresh ? jwtOptions.RefreshAudience : jwtOptions.Audience,
            ValidIssuer = jwtOptions.Issuer,

            RequireSignedTokens = true,
            RequireExpirationTime = true,

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };
    }
}
