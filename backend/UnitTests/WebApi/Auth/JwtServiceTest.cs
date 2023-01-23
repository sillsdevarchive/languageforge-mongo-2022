using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LanguageForge.Api.Entities;
using LanguageForge.UnitTests.Fixtures;
using LanguageForge.WebApi.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageForge.UnitTests.WebApi.Auth;

public class JwtServiceTest : IClassFixture<IntegrationTestFixture>
{
    private readonly JwtService _jwtService;

    public JwtServiceTest(IntegrationTestFixture iocFixture)
    {
        _jwtService = iocFixture.Services.GetRequiredService<JwtService>();
    }

    [Fact]
    public void CanRoundTripUserAndToken()
    {
        // GIVEN a user
        var user = new LfUser("test@testeroolaboom.fun", LfId<User>.Parse("User:6359f8855e3dc273d4662f2a"),
            UserRole.User,
            new[] {
                new UserProjectRole("fun-language", ProjectRole.Manager),
                new UserProjectRole("spooky-language", ProjectRole.Contributor),
             });

        // WHEN generating a token
        var token = _jwtService.GenerateJwt(user);

        // And then converting it back to a WebUserContext
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var identity = new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken.Claims));
        var userContext = JwtService.BuildUserContext(identity);
        var newUser = userContext.User;

        // THEN the user is accurately preserved
        newUser.Id.ShouldBe(user.Id);
        newUser.Role.ShouldBe(user.Role);

        newUser.Projects[0].ShouldBe(user.Projects[0]);
        newUser.Projects[1].ShouldBe(user.Projects[1]);
    }
}
