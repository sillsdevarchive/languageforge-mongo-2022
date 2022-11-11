using LanguageForge.Api.Entities;
using LanguageForge.UnitTests.Fixtures;
using LanguageForge.WebApi.Dtos;
using LanguageForge.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

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

    [Fact]
    public async Task UpdateUserWithInvalidIdReturnsNull()
    {
        var userFound = await _userService.UpdateUser(new UserDto()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Username = "test1",
            Name = "hello",
            Email = "test@email.com",
            Role = UserRole.SystemAdmin,
            Active = false
        });
        userFound.ShouldBeNull();
    }

    [Fact]
    public async Task UpdateUserWorks()
    {
        // GIVEN an existing user
        var users = await _userService.ListUsers();
        var user = users.First();

        // WHEN the user is updated
        var newName = "UpdateUserWorks";
        user = user with { Name = newName };
        var updatedUser = await _userService.UpdateUser(user);

        // THEN the user should be updated
        updatedUser.Name.ShouldBe(newName);
        user = await _userService.FindUser(user.Id);
        user.Name.ShouldBe(newName);
    }
}
