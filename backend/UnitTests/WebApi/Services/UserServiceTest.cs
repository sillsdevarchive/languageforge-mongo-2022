using LanguageForge.Api.Entities;
using LanguageForge.UnitTests.Fixtures;
using LanguageForge.WebApi.Dtos;
using LanguageForge.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

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
        var user = await FirstUser();

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
            Id = LfId.FromDb<User>(ObjectId.GenerateNewId()),
            Username = "test1",
            Name = "hello",
            Email = "test@email.com",
            Role = UserRole.SystemAdmin,
            Active = false,
            DateCreated = DateTimeOffset.UtcNow
        });
        userFound.ShouldBeNull();
    }

    [Fact]
    public async Task UpdateUserWorks()
    {
        // GIVEN an existing user
        var user = await FirstUser();

        // WHEN the user is updated
        var newName = "UpdateUserWorks";
        user = user with { Name = newName };
        var updatedUser = await _userService.UpdateUser(user);

        // THEN the user should be updated
        updatedUser.Name.ShouldBe(newName);
        user = await _userService.FindUser(user.Id);
        user.Name.ShouldBe(newName);
    }

    [Fact]
    public async Task FindLfUserWorks()
    {
        var user = await FirstUser();
        var lfUser = await _userService.FindLfUser(user.Email);
        lfUser.ShouldNotBeNull();
    }

    private async Task<UserDto> FirstUser()
    {
        var users = await _userService.ListUsers();
        return users.First();
    }
}
