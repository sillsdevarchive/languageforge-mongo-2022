using LanguageForge.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using UnitTests.Fixtures;

namespace LanguageForge.UnitTests.WebApi.Services;

public class UserServiceTest : IClassFixture<IocFixture>
{
    private readonly UserService _userService;

    public UserServiceTest(IocFixture iocFixture)
    {
        _userService = iocFixture.ServiceProvider.GetRequiredService<UserService>();
    }

    [Fact]
    public async Task CanDeleteUser()
    {
        // GIVEN an existing user
        var users = await _userService.ListUsers();
        var user = users.First();

        // WHEN the user is deleted
        var deleted = await _userService.DeleteUser(user.Id);

        // THEN the user should be gone
        deleted.ShouldBeTrue();
        user = await _userService.FindUser(user.Id);
        user.ShouldBeNull();
    }
}
