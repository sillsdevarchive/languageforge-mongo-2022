using LanguageForge.Api.Entities;

namespace UnitTests;

public class UnitTest1
{
    [Fact]
    public void NameIsSaved()
    {
        var project = new Project
        {
            Id = "6369cfe5c933ef623a020552",
            ProjectName = "test",
            ProjectCode = "test",
            AllowSharing = false,
            InputSystems = new Dictionary<string, InputSystem>()
        };

        project.ProjectName.ShouldBe("test");
    }
}
