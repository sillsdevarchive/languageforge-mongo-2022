using LanguageForge.UnitTests.Fixtures;
using LanguageForge.WebApi.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageForge.UnitTests.WebApi.Auth;

public class AuthenticationServiceTest : IClassFixture<IntegrationTestFixture>
{
    private readonly AuthenticationService _authService;

    public AuthenticationServiceTest(IntegrationTestFixture iocFixture)
    {
        _authService = iocFixture.Services.GetRequiredService<AuthenticationService>();
    }

    [Fact]
    public async Task TestAuthenticationWithUsernameEmailAndPassword()
    {
        (await _authService.Authenticate("admin", "password")).ShouldNotBeNull();
        (await _authService.Authenticate("admin@example.com", "password")).ShouldNotBeNull();
        (await _authService.Authenticate("admin_", "password")).ShouldBeNull();
        (await _authService.Authenticate("admin", "password_")).ShouldBeNull();
    }

    [Fact]
    public async Task TestAuthenticationWithEmail()
    {
        (await _authService.Authenticate("admin@example.com")).ShouldNotBeNull();
        (await _authService.Authenticate("admin_@example.com")).ShouldBeNull();
    }
}
