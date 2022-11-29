using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace LanguageForge.WebApi.Auth;

public class GoogleTokenValidator
{
    private readonly IOptions<JwtOptions> _options;
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _openIdConfigManager;

    public GoogleTokenValidator(IOptions<JwtOptions> options, IHttpClientFactory clientFactory)
    {
        _options = options;
        _openIdConfigManager = new(
            "https://accounts.google.com/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetrieverFactory(clientFactory));
    }

    public async Task<ClaimsPrincipal> ValidateGoogleJwt(string jwt)
    {
        var discoveryDocument = await _openIdConfigManager.GetConfigurationAsync();
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKeys = discoveryDocument.SigningKeys,
            ValidateIssuerSigningKey = true,
            ValidIssuer = discoveryDocument.Issuer,
            ValidAudience = _options.Value.GoogleClientId,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(jwt, validationParameters, out var token);
        return principal;
    }

    private class HttpDocumentRetrieverFactory : IDocumentRetriever
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpDocumentRetriever internalRetriever => new(_httpClientFactory.CreateClient());

        public HttpDocumentRetrieverFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public Task<string> GetDocumentAsync(string address, CancellationToken cancel)
        {
            return internalRetriever.GetDocumentAsync(address, cancel);
        }
    }
}
